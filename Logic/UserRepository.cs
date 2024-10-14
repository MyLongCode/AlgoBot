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

namespace AlgoBot.Logic
{
    public class UserRepository
    {
        private readonly DBMethods _db;
        private readonly MainRepository _mainRepository;
        private Dictionary<string, int> UserRegisterSteps = new Dictionary<string, int>();

        public UserRepository(DBMethods db, MainRepository main)
        {
            _db = db;
            _mainRepository = main;
        }
        public async Task RegisterUser(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                var stageReg = await _db.GetUserStageReg(message.From.Username);
                if (stageReg == 1) await _db.AddUserFirstName(message.From.Username, message.Text);
                if (stageReg == 2) await _db.AddUserPhoneNumber(message.From.Username, message.Text);
                if (stageReg == 3) await _db.AddUserChildAge(message.From.Username, message.Text);
                if (stageReg == 4) await _db.AddUserChildName(message.From.Username, message.Text);
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;
                if (callbackQuery.Data == "FirstName")
                {
                    await _db.EditStageReg(callbackQuery.Message.Chat.Username, 1);
                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: "Введите ФИО:");
                }
                else if (callbackQuery.Data == "PhoneNumber")
                {
                    await _db.EditStageReg(callbackQuery.Message.Chat.Username, 2);
                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: "Введите номер телефона:");
                }
                else if (callbackQuery.Data == "ChildAge")
                {
                    await _db.EditStageReg(callbackQuery.Message.Chat.Username, 3);
                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: "Введите возраст ребёнка:");
                }
                else if (callbackQuery.Data == "ChildName")
                {
                    await _db.EditStageReg(callbackQuery.Message.Chat.Username, 4);
                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: "Введите имя ребёнка:");
                }
            }
        }
    }
}
