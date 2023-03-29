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
                await PostApplicationAsync(botClient, message);
                break;

            case "Посмотерть информацию по заявке":
                await botClient.SendTextMessageAsync(message.Chat.Id, text: "Ведите /заявка №заявки");
                break;

            case "Проекты компании":
                await GetProjectMessage(botClient, message);
                break;
            case "Услуги компании":
                await GetOfficeAsync(botClient, message);
                break;

            case "Новостная лента":
                await GetTidingAsync(botClient, message);
                break;
            case "Контактная информация":
                await GetContactAsync(botClient, message);
                break;

            case "Перейти на сайт":
                await UrlButtonAsync(botClient, message);
                break;
        }

    }
    if (message.Text.Contains("/заявка"))
    {
        await GetApplicationAsync(botClient, message);
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
    if (message.Text.ToLower() == "/start")
    {
        ReplyKeyboardMarkup keyboard = new(new[]
        {
            new KeyboardButton[] {"Отправить заявку"},
             new KeyboardButton[] {"Посмотерть информацию по заявке"},
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

//Отправить заявку
async Task PostApplicationAsync(ITelegramBotClient botClient, Message message)
{
    InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            InlineKeyboardButton.WithUrl(
                text: "Отправить заявку",
                url: "https://skillbox.ru/")
        });

    await botClient.SendTextMessageAsync(message.Chat.Id,
                                         text: "Пройдите по ссылке ниже для отправки заявки",
                                         replyMarkup: inlineKeyboard);

    //Applications application = new();
    //Application app = new();

    //await botClient.SendTextMessageAsync(message.Chat.Id,
    //                                     text: "<strong>Ведите имя:</strong>",
    //                                     parseMode: ParseMode.Html);

    //app.NameClient = message.Text;

    //await botClient.SendTextMessageAsync(message.Chat.Id,
    //                                     text: "Ведите Emial",
    //                                     replyMarkup: new ForceReplyMarkup { Selective = true });       



    //if (message.ReplyToMessage != null && message.ReplyToMessage.Text != null)
    //{
    //    app.EmailClient = message.Text;
    //    await botClient.SendTextMessageAsync(message.Chat.Id,
    //                                        text: $"Name - {app.NameClient}, Email - {app.EmailClient}");
    //}   
}

//Получить информацию по заявке
async Task GetApplicationAsync(ITelegramBotClient botClient, Message message)
{
    ApplicationGet applicationGet = new(new ResponseServices());
    var number = message.Text.Remove(0, 7).Trim();
    //string number = "https://localhost:7297/home/bd9fe97a-70c9-477b-82ef-ae35b5a9cc8f";
    try
    {
        var appGet = await applicationGet.GetApplicationAsync(number);
            
        await botClient.SendTextMessageAsync(message.Chat.Id,
                                             $"<strong>Имя клиента</strong> - {appGet.NameClient}" +
                                             $"\n<strong>Дата создания заявки</strong> - {appGet.DateTimeCreatApp.ToString("g")}" +
                                             $"\n<strong>Статус заявки</strong> - {appGet.StatusApp}" +
                                             $"\n<strong>Email для обратной связи</strong> - {appGet.EmailClient}",
                                             parseMode: ParseMode.Html);
            
    }
    catch (Exception ex)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                                                    $"<strong>Ошибка загрузки сервиса</strong>" +
                                                    $"\nНе удалось загрузить данные",
                                                    parseMode: ParseMode.Html);
    }
    
}

//Получить список проектов
async Task GetProjectMessage(ITelegramBotClient botClient, Message message)
{
    Projects project = new(new ResponseServices());
    try
    {
        var projectsResponse = await project.GetProjectsAsync();
        foreach (var item in projectsResponse)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, 
                                                $"\n{item.File}" +
                                                $"<strong>{item.Header}<strong>" +
                                                $"\n{item.Description}",
                                                parseMode: ParseMode.Html);
        }
    }
    catch(Exception ex)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                                                $"<strong>Ошибка загрузки сервиса</strong>" +
                                                $"\nНе удалось загрузить данные",
                                                parseMode: ParseMode.Html);
    }
}

//Получить список услуг
async Task GetOfficeAsync(ITelegramBotClient botClient, Message message)
{
    Offices office = new(new ResponseServices());
    try
    {
        var officesResponse = await office.GetOfficeAsync();
        foreach (var item in officesResponse)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,
                                                $"<strong>{item.header}</strong>" +
                                                $"\n{item.description}",
                                                parseMode: ParseMode.Html);
        }
    }
    catch(Exception ex)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                                                $"<strong>Ошибка загрузки сервиса</strong>" +
                                                $"\nНе удалось загрузить данные",
                                                parseMode: ParseMode.Html);
    }
}

//Получить список новостей
async Task GetTidingAsync(ITelegramBotClient botClient, Message message)
{
    Tidings tiding = new(new ResponseServices());
    try
    {
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
    catch(Exception ex)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                                                $"<strong>Ошибка загрузки сервиса</strong>" +
                                                $"\nНе удалось загрузить данные",
                                                parseMode: ParseMode.Html);
    }
}

///Получить контактную информацию
async Task GetContactAsync(ITelegramBotClient botClient, Message message)
{
    Contacts contact = new(new ResponseServices());
    try
    {
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
    catch(Exception ex)
    {
        await botClient.SendTextMessageAsync(message.Chat.Id,
                                                $"<strong>Ошибка загрузки сервиса</strong>" +
                                                $"\nНе удалось загрузить данные",
                                                parseMode: ParseMode.Html);
    }
}

//Переход на сайт компании
async Task UrlButtonAsync(ITelegramBotClient botClient, Message message)
{
    InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            InlineKeyboardButton.WithUrl(
                text: "Перейти на сайт",
                url: "https://skillbox.ru/")
        });

    await botClient.SendTextMessageAsync(message.Chat.Id,
                                         text: "Пройдите по ссылке ниже",
                                         replyMarkup: inlineKeyboard);
}
                                        