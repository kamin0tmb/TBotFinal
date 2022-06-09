using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TBotFinal.Configuration;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using TBotFinal.Services;

namespace TBotFinal.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {

            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            string choice = _memoryStorage.GetSession(message.Chat.Id).Choice;
            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Количество символов" , $"1"),
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел" , $"2")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот умеетсчитать количество символов в сообщении.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}А так же складывать числа, введенные через пробел.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    if (choice == "1")
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Длина сообщения: {message.Text.Length} знаков", cancellationToken: ct);
                    else
                    {
                        try
                        {
                            int summ = 0;
                            int[] numbers = message.Text.Split().Select(num => int.Parse(num)).ToArray();
                            foreach (int number in numbers)
                                summ = summ + number;
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма введенных чисел равна: {summ.ToString()}", cancellationToken: ct);
                        }

                        catch (Exception e)
                        {
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Произошла ошибка, введите числа через пробел еще раз.", cancellationToken: ct);
                        }

                    }

                    break;
            }

        }
    }    
}
