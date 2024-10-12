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

namespace AlgoBot.Logic
{
    public class UserRepository
    {
        private readonly BotDbContext _db;
        private readonly MainRepository _mainRepository;
        private Dictionary<string, int> UserRegisterSteps = new Dictionary<string, int>();

        public UserRepository(BotDbContext db, MainRepository main)
        {
            _db = db;
            _mainRepository = main;
        }
        public async Task RegisterUser(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (!UserRegisterSteps.ContainsKey(message.From.Username)) UserRegisterSteps.Add(message.From.Username, 0);
                var registerStep = UserRegisterSteps[message.From.Username];
                if (registerStep == 0) await bot.SendTextMessageAsync(message.Chat, "Для начала давайте познакомимся!\nКак вас зовут?");
                else if (registerStep == 1)
                {
                    await _db.Users.AddAsync(new BotUser { Username = message.From.Username, Firstname = message.Text, ChildAge = "", PhoneNumber = "", ChildName = "" });
                    await _db.SaveChangesAsync();
                    await bot.SendTextMessageAsync(message.Chat, "Ваш номер телефона:");
                }
                else if (registerStep == 2)
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == message.From.Username);
                    user.PhoneNumber = message.Text;
                    await _db.SaveChangesAsync();
                    await bot.SendTextMessageAsync(message.Chat, "Возраст ребёнка:");
                }
                else if (registerStep == 3)
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == message.From.Username);
                    user.ChildAge = message.Text;
                    await _db.SaveChangesAsync();
                    await bot.SendTextMessageAsync(message.Chat, "Имя ребёнка:");
                }
                else if (registerStep == 4)
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == message.From.Username);
                    user.ChildName = message.Text;
                    await _db.SaveChangesAsync();
                    await bot.SendTextMessageAsync(message.Chat, "Спасибо за регистрацию :)");
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
                UserRegisterSteps[message.From.Username]++;
            } 
        }
    }
}
