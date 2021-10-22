using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot
{
    public class Bot
    {
        private static TelegramBotClient _bot;

        public Bot(string token)
        {
            _bot = new TelegramBotClient(token);
        }

        public void StartBot()

        {
            _bot.OnMessage += OnMessageReceived;
            _bot.StartReceiving();
            while (true)
            {
                Console.WriteLine("Bot is worked all right");
                Thread.Sleep(int.MaxValue);
            }
        }

        private async void OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            try
            {
                Message message = messageEventArgs.Message;
                Console.WriteLine(message.Text);

                switch (message.Text)
                {
                    case "/start":
                        var markup1 = new ReplyKeyboardMarkup(new[]
                        {
                            new KeyboardButton("/game"),
                            new KeyboardButton("/help"),
                        });

                        await _bot.SendTextMessageAsync(message.Chat.Id, message.Text, replyMarkup: markup1);
                        break;

                    case "/help":
                        message.Text =
                            "Победитель определяется по правилам: камень побеждает ножницы (камень затупляет ножницы)" +
                            "ножницы побеждают бумагу (ножницы разрезают бумагу)" +
                            "бумага побеждает камень (бумага заворачивает камень)" +
                            "ничья, если у всех игроков одновременно показан одинаковый знак";
                        await _bot.SendTextMessageAsync(message.Chat.Id, message.Text);
                        break;
                    case "/game":
                        var markup = new ReplyKeyboardMarkup(new[]
                        {
                            new KeyboardButton("rock"),
                            new KeyboardButton("paper"),
                            new KeyboardButton("scissors"),
                        });
                        markup.OneTimeKeyboard = true;
                        await _bot.SendTextMessageAsync(message.Chat.Id, message.Text, replyMarkup: markup);
                        break;
                    
                }

                if (message.Text == "rock" || message.Text == "paper" || message.Text == "scissors")
                {
                    var markup = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton("rock"),
                        new KeyboardButton("paper"),
                        new KeyboardButton("scissors"),
                    });
                    var listGame = new List<string>();
                    listGame.Add("rock");
                    listGame.Add("paper");
                    listGame.Add("scissors");
                    var random = new Random();
                    var element = listGame[random.Next(listGame.Count)];
                    await _bot.SendTextMessageAsync(message.Chat.Id,
                        RockPaperScissors(message.Text, element),
                        replyMarkup: markup);
                }
            }
            catch
                (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public static string RockPaperScissors(string first, string second)
        {
            string message = string.Empty;
            switch (first, second)
            {
                case ("rock", "paper"):
                    message = "rock is covered by paper. Paper wins.";
                    Console.WriteLine(message);
                    break;
                case ("rock", "scissors"):
                    message = "rock breaks scissors. Rock wins.";
                    Console.WriteLine(message);
                    break;
                case ("paper", "rock"):
                    message = "paper covers rock. Paper wins.";
                    Console.WriteLine(message);
                    break;
                case ("paper", "scissors"):
                    message = "paper is cut by scissors. Scissors wins.";
                    Console.WriteLine(message);
                    break;
                case ("scissors", "rock"):
                    message = "scissors is broken by rock. Rock wins.";
                    Console.WriteLine(message);
                    break;
                case ("scissors", "paper"):
                    message = "scissors cuts paper. Scissors wins.";
                    Console.WriteLine(message);
                    break;
            }

            return message;
        }
    }
}