using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace AlgoBot.Logic
{
    public class KeyboardMarkup
    {
        public static InlineKeyboardMarkup StartReg = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Ваше имя", callbackData: "FirstName"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Номер телефона", callbackData: "PhoneNumber"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Возраст ребёнка", callbackData: "ChildAge"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Имя ребёнка", callbackData: "ChildName"),
            },
        });

        public static InlineKeyboardMarkup MainMenu = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Мой профиль", callbackData: "Profile"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Статистика", callbackData: "Stats"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Создать реферальную ссылку", callbackData: "GetRef"),
            },
        });

        public static InlineKeyboardMarkup StartMenu = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Зарегистрироваться", callbackData: "Register"),
            },
        });
    }
}
