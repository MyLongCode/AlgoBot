using Algo96.EF;
using AlgoBot.EF.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotExperiments;

namespace AlgoBot.Logic
{
    public class MainRepository
    {
        private readonly BotDbContext _db;
        private readonly UserRepository _userRepository;
        private int RegisterStep = 0;
        public MainRepository(BotDbContext db)
        {
            _db = db;
            _userRepository = new UserRepository(db, this);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await bot.SendTextMessageAsync(message.Chat, "Здравствуйте! Я - бот помощник Алгоритмики :) Введите /register, чтобы зарегистрироваться.");
                }
                else if (message.Text.ToLower() == "/register")
                {
                    var receiverOptions = new ReceiverOptions
                    {
                        AllowedUpdates = { },
                    };
                    await bot.ReceiveAsync(
                        _userRepository.RegisterUser,
                        HandleErrorAsync,
                        receiverOptions,
                        cancellationToken
                    );
                }
                else
                {
                    await bot.SendTextMessageAsync(message.Chat, "Я тебя не понимаю :(, введи /help, чтобы узнать на что я способен!");
                }
            }
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}
