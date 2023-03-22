using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models;
using TelegramBot.Repository.Request;

//Contacts contacts = new(new ResponseServices());
//var data = contacts.GetContactAsync().Result;
//Console.WriteLine(data.CompanyAdress);
//Console.WriteLine(data.CompanyEmail);
//Console.WriteLine(data.CompanyNumber);
//Console.WriteLine(data.CompanySocialNetworkFour);
//Console.WriteLine(data.CompanySocialNetworkTwo);

Applications app = new Applications();
app.PostAppAsync(new Application { NameClient = "Ivan Post", EmailClient = "minal@mail.ru", DescriptionApp = "вфцврицфр вофцмвпцм рпвфцмвс рпфцвмрцпфсмвпры" });

Console.ReadLine();

var client = new TelegramBotClient("6185901925:AAE5qxhMueMwahHhJmgr2-jsS3syBqgBTok");

client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);
Console.ReadKey();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
{
    var message = update.Message;


    if (update.Type == UpdateType.Message && update?.Message?.Text != null)
    {
        Console.WriteLine($"{DateTime.Now.ToString("G")} | {message.Chat.FirstName}");

        await HandleMessage(botClient, update.Message);
        return;
    }
    return;
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception ex, CancellationToken token)
{
    var ErrorMessage = ex switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",_ => ex.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

async Task HandleMessage(ITelegramBotClient botClient, Message message)
{
    if (message.Text == "/start")
    {
        ReplyKeyboardMarkup keyboard = new(new[]
        {
            new KeyboardButton[] {"Отправить заявку"},
            new KeyboardButton[] {"Проекты компании"},
            new KeyboardButton[] {"Услуги компании"},
            new KeyboardButton[] {"Новостная лента"},
            new KeyboardButton[] {"Контактная информация"},
            new KeyboardButton[] {"Перейти на сайт"}
        })
        {
            ResizeKeyboard = true
        };
        await botClient.SendTextMessageAsync(message.Chat.Id, "Choose:", replyMarkup: keyboard);
        return;
    } 
}