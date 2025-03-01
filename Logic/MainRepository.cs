using AlgoBot.EF;
using AlgoBot.EF;
using AlgoBot.EF.DAL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Bson;
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
                if (message.Text.StartsWith("/start"))
                {
                    var user = await _db.GetUser(message.From.Username);
                    if (user != null && user.StageReg == 5)
                    {
                        await bot.SendTextMessageAsync(
                            message.Chat.Id,
                            text: "Меню действий!",
                            replyMarkup: KeyboardMarkup.MainMenu);
                    }
                    else
                    {
                        var referal = "";
                        var referalName = message.Text.Split(' ');
                        if (referalName.Length == 2) referal = referalName[1].Split("referral_")[1];
                        await _db.CreateUser(message.From.Username, referal);
                        await bot.SendTextMessageAsync(message.Chat,
                                                text: "Здравствуйте! Я - бот помощник Алгоритмики :), давайте зарегистрируемся?",
                                                replyMarkup: KeyboardMarkup.StartMenu);
                    }
                }

                //else
                //{
                //    await bot.SendTextMessageAsync(message.Chat,
                //                                text: "Что я умею?",
                //                                replyMarkup: KeyboardMarkup.MainMenu);
                //}
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;
                if (callbackQuery.Data == "Register")
                {

                    DeleteMessage(bot, callbackQuery);
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
                    DeleteMessage(bot, callbackQuery);
                    var user = await _db.GetUserAsNoTracking(callbackQuery.From.Username);
                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: $"Имя: {user.FullName} \nНомер телефона: {user.PhoneNumber}\nИмя ребёнка: {user.ChildName} \nВозраст ребёнка: {user.ChildAge}",
                        replyMarkup: KeyboardMarkup.MainMenu);
                }
                else if (callbackQuery.Data == "ProfileWeb")
                {
                    DeleteMessage(bot, callbackQuery);
                    var user = await _db.GetUserAsNoTracking(callbackQuery.From.Username);
                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: $"Личный кабинет в https://algoref.ru/\nЛогин: {user.Login} \nПароль: {user.Password}",
                        replyMarkup: KeyboardMarkup.MainMenu);
                }
                else if (callbackQuery.Data == "GetRef")
                {
                    DeleteMessage(bot, callbackQuery);
                    var user = await _db.GetUserAsNoTracking(callbackQuery.From.Username);
                    string referralLink = $"https://t.me/{bot.GetMeAsync().Result.Username}?start=referral_{user.Login  }";
                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: $"Вот ваша реферальная ссылка:\n{referralLink}",
                        replyMarkup: KeyboardMarkup.MainMenu);
                }
                else if (callbackQuery.Data == "GetCashback")
                {
                    DeleteMessage(bot, callbackQuery);
                    var cashback = await _db.GetCashback(callbackQuery.From.Username);
                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: $"Ваш кэшбек:\n{cashback} рублей",
                        replyMarkup: KeyboardMarkup.MainMenu);
                }
                else if (callbackQuery.Data == "EndRegistration")
                {
                    DeleteMessage(bot, callbackQuery);
                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: $"Поздравляю с успешным прохождением регистрации!\nЧто я умею?",
                        replyMarkup: KeyboardMarkup.MainMenu);
                }
                else if(callbackQuery.Data == "Stats")
                {
                    DeleteMessage(bot, callbackQuery);
                    var users = await _db.GetReferals(callbackQuery.From.Username);

                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: $"Общее количество рефералов: {users.Count()}",
                        replyMarkup: KeyboardMarkup.MainMenu);
                }
            }
        }


        private void DeleteMessage(ITelegramBotClient bot, CallbackQuery callbackQuery)
        {
            try
            {
                bot.DeleteMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    callbackQuery.Message.MessageId);
            }
            catch (Exception e)
            {
            }
        }
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}
