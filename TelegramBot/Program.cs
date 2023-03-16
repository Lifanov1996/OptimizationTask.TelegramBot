using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

var client = new TelegramBotClient("6185901925:AAE5qxhMueMwahHhJmgr2-jsS3syBqgBTok");

client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync);
Console.ReadKey();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
{
    var message = update.Message;
    if(message.Text != null)
    {
        Console.WriteLine($"{DateTime.Now.ToString("G")} | {message.Chat.FirstName}");

        if (message.Text.ToLower().Trim().Contains("привет"))
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Ну привет");
            return;
        }
    }
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