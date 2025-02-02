using PixelPomodoro.Models;
using Telegram.Bot;

namespace PixelPomodoro.Services;

public class TelegramNotifier {
    private AppdataModel _appdata = new();
    private string _botToken = string.Empty;
    
    public TelegramNotifier() {
        _appdata = JSONService.GetJSONData<AppdataModel>(
            System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "appdata.json"));
       _botToken = _appdata.TelegramAPIToken;
       Console.WriteLine($"BotToken parsed! TOKEN={_botToken}");
    }

    public async Task SendNotification(string message) {
        if (_appdata.SendTelegramNotifications) {
            var bot = new TelegramBotClient(_botToken);
            await bot.SendTextMessageAsync(_appdata.UserTelegramID, message);
            Console.WriteLine($"Notification sended");   
        }
    }
}