using Algo96.EF;
using AlgoBot.EF;
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
        private readonly DBMethods _db;
        private readonly UserRepository _userRepository;
        private int RegisterStep = 0;
        public MainRepository(DBMethods db)
        {
            _db = db;
            _userRepository = new UserRepository(db, this);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            var callbackQuery = update.CallbackQuery;
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await bot.SendTextMessageAsync(message.Chat, "Здравствуйте! Я - бот помощник Алгоритмики :) Введите /register, чтобы зарегистрироваться.");
                }
                else if (message.Text.ToLower() == "/register")
                {
                    _db.CreateUser(message.From.Username);
                    await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        text: "Для начала давайте познакомимся!\nКак вас зовут?\nНажмите на кнопку для ввода данных",
                        replyMarkup: KeyboardMarkup.StartReg);
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
                else if (message.Text.ToLower() == "/profile")
                {
                    var user = await _db.GetUser(message.From.Username);
                    if (user != null) await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        text: $"Имя: {user.Firstname} \nНомер телефона: {user.PhoneNumber}\nИмя ребёнка: {user.ChildName} \nВозраст ребёнка: {user.ChildAge}");
                    else await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        text: "Вы ещё не зарегистрированы у нас :(, вы можете исправить это с помощью команды /register");

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
