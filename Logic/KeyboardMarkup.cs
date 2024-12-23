﻿using System;
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
                InlineKeyboardButton.WithCallbackData(text: "Начать регистрацию!", callbackData: "StartRegistration"),
            }
        });
        public static InlineKeyboardMarkup EndReg = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Закончить регистрацию", callbackData: "EndRegistration"),
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
                InlineKeyboardButton.WithCallbackData(text: "Данные для входа в сервис", callbackData: "ProfileWeb"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Создать реферальную ссылку", callbackData: "GetRef"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Посмотреть кэшбек", callbackData: "GetCashback"),
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
