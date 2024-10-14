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
using Telegram.Bots;
using Telegram.Bots.Http;
using TelegramBotExperiments;

namespace AlgoBot.Logic
{
    public class MainRepository
    {
        private readonly DBMethods _db;
        private readonly RegisterRepository _userRepository;
        private int RegisterStep = 0;
        public MainRepository(DBMethods db)
        {
            _db = db;
            _userRepository = new RegisterRepository(db, this);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    var user = await _db.GetUser(message.From.Username);
                    if (user != null)
                    {
                        await bot.SendTextMessageAsync(
                            message.Chat.Id,
                            text: "Здравствуйте!",
                            replyMarkup: KeyboardMarkup.MainMenu);
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(message.Chat,
                                                text: "Здравствуйте! Я - бот помощник Алгоритмики :), давайте зарегистрируемся?",
                                                replyMarkup: KeyboardMarkup.StartMenu);
                    }
                }
                
                else
                {
                    await bot.SendTextMessageAsync(message.Chat, "Я тебя не понимаю :(, введи /help, чтобы узнать на что я способен!");
                }
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;
                if (callbackQuery.Data == "Register")
                {
                    await bot.DeleteMessageAsync(
                            callbackQuery.Message.Chat.Id,
                            callbackQuery.Message.MessageId);
                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: "Для начала давайте познакомимся!\nНажмите на кнопку для ввода данных",
                        replyMarkup: KeyboardMarkup.StartReg);
                    await bot.ReceiveAsync(
                        _userRepository.RegisterUser,
                        HandleErrorAsync,
                        new ReceiverOptions
                        {
                            AllowedUpdates = { },
                        },
                        cancellationToken
                    );
                }
                else if (callbackQuery.Data== "Profile")
                {
                    var user = await _db.GetUser(callbackQuery.Message.From.Username);
                    if (user != null) await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: $"Имя: {user.Firstname} \nНомер телефона: {user.PhoneNumber}\nИмя ребёнка: {user.ChildName} \nВозраст ребёнка: {user.ChildAge}");
                    else await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: "Вы ещё не зарегистрированы у нас :(, вы можете исправить это с помощью команды /register");

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
