using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using AlgoBot.EF;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using AlgoBot.Logic;

namespace TelegramBotExperiments
{

    public static class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("S");
        static RegisterRepository UserRepository;
        static MainRepository MainRepository;
       


        static void Main(string[] args)
        {
            var db = new DbContextFactory().CreateDbContext();
            var dbMethods = new DBMethods(db);
            MainRepository = new MainRepository(dbMethods);
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.Username);
            Console.WriteLine($"Всего пользователей: {db.Users.Count()}");

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            bot.StartReceiving(
                MainRepository.HandleUpdateAsync,
                MainRepository.HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}