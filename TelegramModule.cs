using CameraMicTelegram;
using System;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;

namespace TelegramController
{
    public class TelegramModule
    {
        private static TelegramBotClient botClient;
        private static ChatId FromChatId { get; set; }
        private static ChatId ToChatId { get; set; }
        private ChatId MyId { get; set; }
        private static Message MyMessage { get; set; }
        private static string picName { get; set; }
        private static string caption { get; set; }
        public TelegramModule()
        {
            //TelegramModule bot = new TelegramModule();
            botClient = new TelegramBotClient(Token.Get());
            using CancellationTokenSource cts = new();
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me}");
            Console.ReadLine();

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                // Only process Message updates: https://core.telegram.org/bots/api#message
                if (update.Message is not { } message)
                    return;
                // Only process text messages
                if (message.Text is not { } messageText)
                    return;

                FromChatId = message.Chat.Id;
                long chatId = message.Chat.Id;

                Console.WriteLine($"Received a '{messageText}' message in chat {FromChatId}.");
                
                await TeloSendMessage("Seni bulacam olum" + FromChatId.Username + "seni bulacam.");
                // Echo received message text


                switch (messageText)
                {
                    case "Get":
                        await SendPic(ToChatId).SendPic(MyId);
                        break;
                    case "Face":
                        await FaceSendPic(ToChatId);
                        break;
                    case "Watch":
                        await WatchAndGet(ToChatId);
                        break;
                    case "send":
                        await TeloSendMessage().TeloSendPic();
                        break;
                    default: break;
                }

            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }
        public async Task<TelegramModule> SendPic(ChatId chatId)
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

            return await this;
        }

        public async Task<TelegramModule> FaceSendPic(ChatId chatId = default)
        {
            string picName = DateTime.Now.ToString("/FACE:yyyy:MM:dd_HH:mm:ss");
            await CameraModule.FaceAndSave(picName);

            await using Stream stream = System.IO.File.OpenRead(Environment.CurrentDirectory + "/assets/" + picName + ".jpg");

            Message message = await botClient.SendPhotoAsync(
                chatId: ToChatId,
                photo: InputFile.FromStream(stream),
                caption: "Taken from comfuture",
                parseMode: ParseMode.Html,
                cancellationToken: default(CancellationToken));
            return await this;
        }

        public async Task<TelegramModule> WatchAndGet(ChatId chatId = default)
        {
            string picName = DateTime.Now.ToString("/FACE:yyyy:MM:dd_HH:mm:ss");
            await CameraModule.WatchAndSee(picName);

            return this;
        }

        public async Task<TelegramModule> TeloSendPic(string picName = default)
        {
            await using Stream stream = System.IO.File.OpenRead(Environment.CurrentDirectory + "/assets/" + picName + ".jpg");

            Message message = await botClient.SendPhotoAsync(
                chatId: ToChatId,
                photo: InputFile.FromStream(stream),
                caption: "Taken from comfuture",
                parseMode: ParseMode.Html,
                cancellationToken: default(CancellationToken));

            return this;
        }

        public async Task<TelegramModule> TeloSendMessage(string message = default)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: ToChatId,
                text: "Söyle ona sebastiyan:\n" + MyMessage.Text,
                cancellationToken: default(CancellationToken));
            return this;
        }
    }
}
