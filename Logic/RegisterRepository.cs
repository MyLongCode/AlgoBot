
using AlgoBot.EF;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AlgoBot.Logic
{
    public class RegisterRepository
    {
        private readonly DBMethods _db;
        private readonly MainRepository _mainRepository;

        public RegisterRepository(DBMethods db, MainRepository main)
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
                if (stageReg == 1)
                {
                    await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        text: "Ваш номер телефона:");
                    await _db.EditStageReg(message.Chat.Username, 2);
                    await _db.AddUserFirstName(message.From.Username, message.Text);
                }
                if (stageReg == 2)
                {
                    await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        text: "Возраст ребёнка:");
                    await _db.EditStageReg(message.Chat.Username, 3);
                    await _db.AddUserPhoneNumber(message.From.Username, message.Text);
                } 
                if (stageReg == 3)
                {
                    await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        text: "Имя ребёнка:");
                    await _db.EditStageReg(message.Chat.Username, 4);
                    await _db.AddUserChildAge(message.From.Username, message.Text);
                }
                if (stageReg == 4)
                {
                    await bot.SendTextMessageAsync(
                        message.Chat.Id,
                        text: "Замечательно! Вы ввели все нужные данные :).",
                        replyMarkup: KeyboardMarkup.EndReg);
                    await _db.EditStageReg(message.Chat.Username, 5);
                    await _db.AddUserChildName(message.From.Username, message.Text);
                    await bot.ReceiveAsync(
                        _mainRepository.HandleUpdateAsync,
                        _mainRepository.HandleErrorAsync,
                        new ReceiverOptions
                        {
                            AllowedUpdates = { },
                        },
                        cancellationToken
                    );
                }
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;
                if (callbackQuery.Data == "StartRegistration")
                {
                    await bot.DeleteMessageAsync(
                            callbackQuery.Message.Chat.Id,
                            callbackQuery.Message.MessageId);
                    
                    await bot.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: "Введите ФИО:");

                    await _db.EditStageReg(callbackQuery.Message.Chat.Username, 1);
                }
            }
        }
    }
}
