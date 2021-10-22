using System;
using System.Collections.Generic;
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
        private readonly TelegramBotClient _bot;

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
                    case "/help":
                        message.Text = String.Format(
                            "Победитель определяется по правилам: камень побеждает ножницы (камень затупляет ножницы)" +
                            "ножницы побеждают бумагу (ножницы разрезают бумагу)" +
                            "бумага побеждает камень (бумага заворачивает камень)" +
                            "ничья, если у всех игроков одновременно показан одинаковый знак");
                        break;
                    case "/game":
                        message.Text = "Выберите камень, ножницы или бумагу";
                        var listGame = new List<string>();
                        listGame.Add("rock");
                        listGame.Add("paper");
                        listGame.Add("scissors");
                        var random = new Random();
                        var item = listGame.OrderBy(s => random.NextDouble()).First();
                        RockPaperScissors(message.Text, item);
                        break;
                }

                if (message.Text == "/game")
                {
                    var markup = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton("rock"),
                        new KeyboardButton("paper"),
                        new KeyboardButton("scissors"),
                    });

                    markup.OneTimeKeyboard = true;
                    await _bot.SendTextMessageAsync(message.Chat.Id, message.Text, replyMarkup: markup);
                }
                else
                {
                    var markup = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton("/game"),
                        new KeyboardButton("/help"),
                        
                    });
                    
                    await _bot.SendTextMessageAsync(message.Chat.Id, message.Text, replyMarkup: markup);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public static string RockPaperScissors(string first, string second)
            => (first, second) switch

            {
                ("rock", "paper") => "rock is covered by paper. Paper wins.",
                ("rock", "scissors") => "rock breaks scissors. Rock wins.",
                ("paper", "rock") => "paper covers rock. Paper wins.",
                ("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
                ("scissors", "rock") => "scissors is broken by rock. Rock wins.",
                ("scissors", "paper") => "scissors cuts paper. Scissors wins.",
                (_, _) => "tie"
            };
    }
}