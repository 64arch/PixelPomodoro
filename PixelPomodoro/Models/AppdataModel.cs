namespace PixelPomodoro.Models;

public class AppdataModel {
    public bool SendTelegramNotifications { get; set; }
    public string TelegramAPIToken { get; set; }
    public string UserTelegramID { get; set; }
    public int WorkMinutes { get; set; }
    public int BreakMinutes { get; set; }
}