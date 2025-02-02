using System.Diagnostics;
using Gdk;
using Gtk;
using PixelPomodoro.Models;
using PixelPomodoro.Services;
using Switch = Gtk.Switch;

namespace PixelPomodoro.Windows;

public class GTKSettingsWindow : Gtk.Window {
    private HBox telegramApiTokenBox;
    private HBox userTelegramIdBox;
    public GTKSettingsWindow(Gtk.Window parent) : base("Settings") {
        Resizable = false;
        SetSizeRequest(600, 340);
        Icon = new Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icon.png"));

        parent.Sensitive = false;
        DeleteEvent += (sender, args) => { parent.Sensitive = true; };

        var appdata = JSONService.GetJSONData<AppdataModel>(
            System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "appdata.json"));

        Alignment alignment = new Alignment(0.5f, 0.5f, 0.8f, 1.0f);
        VBox vbox = new VBox(false, 5);


        HBox switchBox = new HBox(false, 10);
        var sendNotifySwitch = new Switch();
        sendNotifySwitch.Active = appdata.SendTelegramNotifications;
        sendNotifySwitch.StateChanged += (o, args) => {

            bool isActive = sendNotifySwitch.Active;
            telegramApiTokenBox.Sensitive = isActive;
            userTelegramIdBox.Sensitive = isActive;
        };
        
        Label switchTextLabel = new Label("Enable telegram notifications");
        switchBox.PackStart(switchTextLabel, false, false, 0);
        switchBox.PackStart(sendNotifySwitch, false, false, 0);
        vbox.PackStart(switchBox, false, false, 20);

        var telegramApiTokenEntry = new Entry();
        var userTelegramIdEntry = new Entry();

        telegramApiTokenBox = new HBox(false, 10);
        telegramApiTokenEntry = new Entry();
        telegramApiTokenEntry.Text = appdata.TelegramAPIToken;
        Label telegramApiTokenLabel = new Label("Telegram bot token");
        telegramApiTokenBox.PackStart(telegramApiTokenLabel, false, false, 0);
        telegramApiTokenBox.PackStart(telegramApiTokenEntry, true, true, 0);
        vbox.PackStart(telegramApiTokenBox, false, false, 10);

        userTelegramIdBox = new HBox(false, 10);
        userTelegramIdEntry = new Entry();
        userTelegramIdEntry.Text = appdata.UserTelegramID;
        Label userTelegramIdLabel = new Label("Telegram user id");
        userTelegramIdBox.PackStart(userTelegramIdLabel, false, false, 0);
        userTelegramIdBox.PackStart(userTelegramIdEntry, true, true, 0);
        vbox.PackStart(userTelegramIdBox, false, false, 10);
        
        HBox workMinutesBox = new HBox(false, 10);
        var workMinutesEntry = new Entry();
        workMinutesEntry.Text = appdata.WorkMinutes.ToString();
        Label workMinutesLabel = new Label("Work minutes");
        workMinutesBox.PackStart(workMinutesLabel, false, false, 0);
        workMinutesBox.PackStart(workMinutesEntry, true, true, 0);
        vbox.PackStart(workMinutesBox, false, false, 10);

        HBox breakMinutesBox = new HBox(false, 10);
        var breakMinutesEntry = new Entry();
        breakMinutesEntry.Text = appdata.BreakMinutes.ToString();
        Label breakMinutesLabel = new Label("Break minutes");
        breakMinutesBox.PackStart(breakMinutesLabel, false, false, 0);
        breakMinutesBox.PackStart(breakMinutesEntry, true, true, 0);
        vbox.PackStart(breakMinutesBox, false, false, 10);

        var saveButton = new Button("Save");
        saveButton.Clicked += (sender, args) => {
            var saveData = new AppdataModel() {
                SendTelegramNotifications = sendNotifySwitch.Active,
                TelegramAPIToken = telegramApiTokenEntry.Text,
                UserTelegramID = userTelegramIdEntry.Text,
                WorkMinutes = int.Parse(workMinutesEntry.Text),
                BreakMinutes = int.Parse(breakMinutesEntry.Text)
            };
            JSONService.SaveJSONData(saveData,
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "appdata.json"));

            Process.Start("notify-send", "\"PixelPomodoro\" \"Settings saved! Restart app.\"");
        };
        vbox.PackStart(saveButton, false, false, 10);

        alignment.Add(vbox);
        Add(alignment);

        ShowAll();
    }
}