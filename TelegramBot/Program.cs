using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models;
using TelegramBot.Repository.Request;
using TelegramBot.Repository.Response;

//Contacts contacts = new(new ResponseServices());
//var data = contacts.GetContactAsync().Result;
//Console.WriteLine(data.CompanyAdress);
//Console.WriteLine(data.CompanyEmail);
//Console.WriteLine(data.CompanyNumber);
//Console.WriteLine(data.CompanySocialNetworkFour);
//Console.WriteLine(data.CompanySocialNetworkTwo);

//Applications app = new Applications();
//app.PostAppAsync(new Application { NameClient = "Ivan Post", EmailClient = "minal@mail.ru", DescriptionApp = "вфцврицфр вофцмвпцм рпвфцмвс рпфцвмрцпфсмвпры" });

var client = new TelegramBotClient("6185901925:AAE5qxhMueMwahHhJmgr2-jsS3syBqgBTok");

client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);
Console.ReadKey();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
{
    var message = update.Message;


    if (update.Type == UpdateType.Message && update?.Message?.Text != null)
    {
        Console.WriteLine($"{DateTime.Now.ToString("G")} | {message.Chat.FirstName}");

        await HandleMessageMenu(botClient, update.Message);
        
        switch(message.Text)
        {
            case "Отправить заявку":
                await botClient.SendTextMessageAsync(message.Chat.Id, "Отправил заяввку");
                break;

            case "Проекты компании":
                await botClient.SendTextMessageAsync(message.Chat.Id, "Смотри проекты");
                break;
            case "Услуги компании":
                await GetOfficeAsync(botClient, message);
                break;

            case "Новостная лента":
                await botClient.SendTextMessageAsync(message.Chat.Id, "Смотри проекты");
                break;
            case "Контактная информация":
                await GetContactAsync(botClient, message);
                break;

            case "Перейти на сайт":
                await botClient.SendTextMessageAsync(message.Chat.Id, $"<strong>Новая волна</strong>" +
                                                                        $"\nФайл/" +
                                                                        $"\nВОФЫЛр твфшгцпвнфр нцвпфныовпцн вфцпрвнцфпво нвцфпвныфрв нцвпфоныпвнц", 
                                                                        parseMode: ParseMode.Html);
                break;
        }
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

async Task HandleMessageMenu(ITelegramBotClient botClient, Message message)
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

async Task GetProjectMessage(ITelegramBotClient botClient, Message message)
{
    Projects project = new(new ResponseServices());
    var projectsResponse = await project.GetProjectsAsync();
    foreach (var item in projectsResponse)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, 
                                            $"<strong>{item.Header}<strong>" +
                                            $"\n{item.File}" +
                                            $"\n{item.Description}",
                                            parseMode: ParseMode.Html);
    }
}

async Task GetOfficeAsync(ITelegramBotClient botClient, Message message)
{
    Offices office = new(new ResponseServices());
    var officesResponse = await office.GetOfficeAsync();
    foreach (var item in officesResponse)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                                            $"<strong>{item.header}</strong>" +
                                            $"\n{item.description}",
                                            parseMode: ParseMode.Html);
    }
}

async Task GetTidingAsync(ITelegramBotClient botClient, Message message)
{
    Tidings tiding = new(new ResponseServices());
    var tidingsResponse = await tiding.GetTidingsAsync();
    foreach (var item in tidingsResponse)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                                            $"{item.File}" +
                                            $"\nОпубликлванно - {item.DateTimePublication.ToString("G")}" +
                                            $"\n<strong>{item.Header}</strong>" +
                                            $"\n{item.Description}",
                                            parseMode: ParseMode.Html);
    }
}

///Получить контактную информацию
async Task GetContactAsync(ITelegramBotClient botClient, Message message)
{
    Contacts contact = new(new ResponseServices());
    var contactResponse = await contact.GetContactAsync();

    await botClient.SendContactAsync(message.Chat.Id,
                                        phoneNumber: $"{contactResponse.CompanyNumber}",
                                        firstName: "Mir");

    await botClient.SendTextMessageAsync(message.Chat.Id,
                                        $"<strong>Email:</strong> {contactResponse.CompanyEmail}",
                                        parseMode: ParseMode.Html);

    await botClient.SendVenueAsync(message.Chat.Id,
                                    latitude: 55.155659,
                                    longitude: 37.459864,
                                    title: "Адрес компании",
                                    address: $"{contactResponse.CompanyAdress}");
}