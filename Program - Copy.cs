using System;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using System.Threading;


#region TELEGRAM_SETTINGS

//var botClient = new TelegramBotClient("6847713222:AAEMHAaQYs-XD42abzz3UbeNWTTDskMt35c");

//using CancellationTokenSource cts = new();

//// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
//ReceiverOptions receiverOptions = new()
//{
//    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
//};

//botClient.StartReceiving(
//    updateHandler: HandleUpdateAsync,
//    pollingErrorHandler: HandlePollingErrorAsync,
//    receiverOptions: receiverOptions,
//    cancellationToken: cts.Token
//);

//var me = await botClient.GetMeAsync();

//Console.WriteLine($"Start listening for @{me.Username}");
//Console.ReadLine();

//// Send cancellation request to stop bot
//cts.Cancel();

//async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
//{
//    // Only process Message updates: https://core.telegram.org/bots/api#message
//    if (update.Message is not { } message)
//        return;
//    // Only process text messages
//    if (message.Text is not { } messageText)
//        return;

//    var chatId = message.Chat.Id;

//    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

//    // Echo received message text
//    Message sentMessage = await botClient.SendTextMessageAsync(
//        chatId: chatId,
//        text: "You said:\n" + messageText,
//        cancellationToken: cancellationToken);

//    await using Stream stream = System.IO.File.OpenRead( Environment.CurrentDirectory + "/assets/a.jpg");

//    Message Photomessage = await botClient.SendPhotoAsync(
//    chatId: chatId,
//    photo: InputFile.FromStream(stream),
//    //FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg"),
//    caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
//    parseMode: ParseMode.Html,
//    cancellationToken: default(CancellationToken));
//}

//Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
//{
//    var ErrorMessage = exception switch
//    {
//        ApiRequestException apiRequestException
//            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
//        _ => exception.ToString()
//    };

//    Console.WriteLine(ErrorMessage);
//    return Task.CompletedTask;
//}
#endregion TELEGRAM_SETTINGS

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using Emgu.CV.Reg;
using CameraMicTelegram;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Configuration;

namespace CameraMicTelegram
{
    //public class Program
    //{
    //    //private static IWaveIn waveIn;
    //    private static TelegramBotClient botClient;
    //    private static ChatId FromChatId { get ; set; }
    //    private static ChatId ToChatId { get ; set; }
    //    private static Message MyMessage {  get; set; }
    //    private static string picName {  get; set; }
    //    private static string caption { get; set; }
    //    static async Task Main(string[] args)
    //    {
    //        // 6847713222:AAEMHAaQYs-XD42abzz3UbeNWTTDskMt35c

    //        Program bot = new Program();
    //        botClient = new TelegramBotClient(Token.Get());
    //        using CancellationTokenSource cts = new();
    //        ReceiverOptions receiverOptions = new()
    //        {
    //            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
    //        };

    //        botClient.StartReceiving(
    //            updateHandler: HandleUpdateAsync,
    //            pollingErrorHandler: HandlePollingErrorAsync,
    //            receiverOptions: receiverOptions,
    //            cancellationToken: cts.Token
    //        );

    //        var me = await botClient.GetMeAsync();

    //        Console.WriteLine($"Start listening for @{me.Username}");
    //        Console.ReadLine();

    //        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    //        {
    //            // Only process Message updates: https://core.telegram.org/bots/api#message
    //            if (update.Message is not { } message)
    //                return;
    //            // Only process text messages
    //            if (message.Text is not { } messageText)
    //                return;

    //            FromChatId = message.Chat.Id;

    //            Console.WriteLine($"Received a '{messageText}' message in chat {FromChatId}.");

    //            await TeloSendMessage();
    //            // Echo received message text
    //            Message sentMessage = await botClient.SendTextMessageAsync(
    //                chatId: ToChatId,
    //                text: "You said:\n" + MyMessage.Caption,
    //                cancellationToken: cancellationToken);

    //            switch (messageText)
    //            {
    //                case "Get":
    //                    await SendPic(chatId);
    //                    break;
    //                case "Face":
    //                    await FaceSendPic(chatId);
    //                    break;
    //                case "Watch":
    //                    await WatchAndGet(chatId);
    //                    break;
    //                default: break;
    //            }

    //        }

    //        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    //        {
    //            var ErrorMessage = exception switch
    //            {
    //                ApiRequestException apiRequestException
    //                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
    //                _ => exception.ToString()
    //            };

    //            Console.WriteLine(ErrorMessage);
    //            return Task.CompletedTask;
    //        }

    //        static async Task<Program> SendPic(long chatId)
    //        {
    //            picName = DateTime.Now.ToString("yyyy:MM:dd_HH:mm:ss");
    //            await CameraModule.TakeSavePic(picName);
    //            await using Stream stream = System.IO.File.OpenRead(Environment.CurrentDirectory + "/assets/" + picName +".jpg");

    //            Message message = await botClient.SendPhotoAsync(
    //                chatId: chatId,
    //                photo: InputFile.FromStream(stream),
    //                caption: "Taken from comfuture",
    //                parseMode: ParseMode.Html,
    //                cancellationToken: default(CancellationToken));

    //            return ;
    //        }

    //        static async Task FaceSendPic(long chatId)
    //        {
    //            string picName = DateTime.Now.ToString("/FACE:yyyy:MM:dd_HH:mm:ss");
    //            await CameraModule.FaceAndSave(picName);

    //            await using Stream stream = System.IO.File.OpenRead(Environment.CurrentDirectory + "/assets/" + picName + ".jpg");

    //            Message message = await botClient.SendPhotoAsync(
    //                chatId: chatId,
    //                photo: InputFile.FromStream(stream),
    //                caption: "Taken from comfuture",
    //                parseMode: ParseMode.Html,
    //                cancellationToken: default(CancellationToken));
    //        }

    //        static async Task WatchAndGet(long chatId) 
    //        {
    //            string picName = DateTime.Now.ToString("/FACE:yyyy:MM:dd_HH:mm:ss");
    //            await CameraModule.WatchAndSee(picName);


    //        }

    //        static async Task TeloSendPic(string picName) 
    //        {
    //            await using Stream stream = System.IO.File.OpenRead(Environment.CurrentDirectory + "/assets/" + picName + ".jpg");

    //            Message message = await botClient.SendPhotoAsync(
    //                chatId: ToChatId,
    //                photo: InputFile.FromStream(stream),
    //                caption: "Taken from comfuture",
    //                parseMode: ParseMode.Html,
    //                cancellationToken: default(CancellationToken));
    //        }

    //        static async Task TeloSendMessage(string message)
    //        {
    //            Message sentMessage = await botClient.SendTextMessageAsync(
    //                chatId: ToChatId,
    //                text: "Söyle ona sebastiyan:\n" + MyMessage.Text,
    //                cancellationToken: default(CancellationToken));
    //        }
    //    }
    //}

    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Exceptions;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Polling;

    namespace CameraMicTelegram
    {
        public class Program
        {
            private static TelegramBotClient botClient;
            private static ChatId FromChatId { get; set; }
            private static ChatId ToChatId { get; set; }
            private static Message MyMessage { get; set; }
            private static string picName { get; set; }
            private static string caption { get; set; }

            static async Task Main(string[] args)
            {
                Program bot = new Program();
                botClient = new TelegramBotClient(Token.Get());
                using CancellationTokenSource cts = new();
                ReceiverOptions receiverOptions = new()
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                };

                botClient.StartReceiving(
                    updateHandler: HandleUpdateAsync,
                    pollingErrorHandler: HandlePollingErrorAsync,
                    receiverOptions: receiverOptions,
                    cancellationToken: cts.Token
                );

                var me = await botClient.GetMeAsync();

                Console.WriteLine($"Start listening for @{me.Username}");
                Console.ReadLine();

                async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
                {
                    if (update.Message is not { } message)
                        return;

                    FromChatId = message.Chat.Id;
                    Console.WriteLine($"Received a '{message.Text}' message in chat {FromChatId}.");

                    await TeloSendMessage().TeloSendPic().WatchAndGet();
                }

                Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
                {
                    var errorMessage = exception switch
                    {
                        ApiRequestException apiRequestException
                            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                        _ => exception.ToString()
                    };

                    Console.WriteLine(errorMessage);
                    return Task.CompletedTask;
                }

                // Other methods remain unchanged
                static Program SendPic(long chatId)
                {
                    picName = DateTime.Now.ToString("yyyy:MM:dd_HH:mm:ss");
                    await CameraModule.TakeSavePic(picName);
                    await using Stream stream = System.IO.File.OpenRead(Environment.CurrentDirectory + "/assets/" + picName + ".jpg");

                    Message message = await botClient.SendPhotoAsync(
                        chatId: chatId,
                        photo: InputFile.FromStream(stream),
                        caption: "Taken from comfuture",
                        parseMode: ParseMode.Html,
                        cancellationToken: default(CancellationToken));

                    return;
                }

                static Program FaceSendPic(long chatId)
                {
                    string picName = DateTime.Now.ToString("/FACE:yyyy:MM:dd_HH:mm:ss");
                    await CameraModule.FaceAndSave(picName);

                    await using Stream stream = System.IO.File.OpenRead(Environment.CurrentDirectory + "/assets/" + picName + ".jpg");

                    Message message = await botClient.SendPhotoAsync(
                        chatId: chatId,
                        photo: InputFile.FromStream(stream),
                        caption: "Taken from comfuture",
                        parseMode: ParseMode.Html,
                        cancellationToken: default(CancellationToken));
                    return bot;
                }

                static Program WatchAndGet(long chatId)
                {
                    string picName = DateTime.Now.ToString("/FACE:yyyy:MM:dd_HH:mm:ss");
                    await CameraModule.WatchAndSee(picName);


                }

                static Program TeloSendPic(string picName)
                {
                    await using Stream stream = System.IO.File.OpenRead(Environment.CurrentDirectory + "/assets/" + picName + ".jpg");

                    Message message = await botClient.SendPhotoAsync(
                        chatId: ToChatId,
                        photo: InputFile.FromStream(stream),
                        caption: "Taken from comfuture",
                        parseMode: ParseMode.Html,
                        cancellationToken: default(CancellationToken));
                }

                static Program TeloSendMessage(string message)
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: ToChatId,
                        text: "Söyle ona sebastiyan:\n" + MyMessage.Text,
                        cancellationToken: default(CancellationToken));
                }

                static Program TeloSendMessage()
                {
                    // Modify this method as needed
                    // Example code for demonstration purposes:
                    // MyMessage = await botClient.SendTextMessageAsync(...);
                    return bot;
                }

                static Program TeloSendPic()
                {
                    // Modify this method as needed
                    // Example code for demonstration purposes:
                    // await using Stream stream = System.IO.File.OpenRead(...);
                    // MyMessage = await botClient.SendPhotoAsync(...);
                    return bot;
                }

                static Program WatchAndGet()
                {
                    // Modify this method as needed
                    // Example code for demonstration purposes:
                    // await CameraModule.WatchAndSee(...);
                    return bot;
                }
            }
        }
    }
}