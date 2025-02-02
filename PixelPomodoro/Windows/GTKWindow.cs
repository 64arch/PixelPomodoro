using System.Text.Json;
using Cairo;
using Gdk;
using Gtk;
using Pango;
using PixelPomodoro.Models;
using PixelPomodoro.Services;
using static Gtk.Orientation;
using Alignment = Gtk.Alignment;

namespace PixelPomodoro.Windows;

public class GTKWindow : Gtk.Window{
    public GTKWindow() : base("PixelPomodoro") {
        Resizable = false;
        SetSizeRequest(600,290);
        Icon = new Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icon.png"));
        Alignment alignment = new Alignment(0.5f, 0.5f, 0.8f, 1.0f);
        VBox vbox = new VBox(false, 5);

        var appdata = JSONService.GetJSONData<AppdataModel>(
            System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "appdata.json"));
        
        Label timeLabel = new Label($"{appdata.WorkMinutes:D2}:00");
        var timeFont = new Pango.FontDescription {
            Weight = Pango.Weight.Bold,
            Size = 36 * (int)Pango.Scale.PangoScale
        };
        timeLabel.ModifyFont(timeFont);
        timeLabel.Xalign = 0.5f;
        timeLabel.Yalign = 0.5f;
        vbox.PackStart(timeLabel, false, false, 15);

        Label statusLabel = new Label("Work period");
        var statusFont = new Pango.FontDescription {
            Weight = Pango.Weight.Bold,
            Size = 10 * (int)Pango.Scale.PangoScale
        };
        statusLabel.ModifyFont(statusFont);
        vbox.PackStart(statusLabel, false, false, 5);

        var pomodoro = new PomodoroTimer(timeLabel, statusLabel, appdata.WorkMinutes, appdata.BreakMinutes);
        
        Button startButton = new Button("Start");
        Button stopButton = new Button("Stop");
        stopButton.Sensitive = false;  
        
        startButton.Clicked += (sender, args) => 
        {
            pomodoro.Start();
            startButton.Sensitive = false; 
            stopButton.Sensitive = true;
        };
        
        stopButton.Clicked += (sender, args) => 
        {
            pomodoro.Stop();
            startButton.Sensitive = true;
            stopButton.Sensitive = false;
        };
        
        vbox.PackStart(startButton, false, false, 2);
        vbox.PackStart(stopButton, false, false, 2);
        
        Button settingsButton = new Button("Settings");
        settingsButton.Clicked += (sender, args) => {
            var settingsWindow = new GTKSettingsWindow(this);
            settingsWindow.ShowAll();
            settingsWindow.Present();
        };
        vbox.PackStart(settingsButton, false, false, 2);
        
        alignment.Add(vbox);
        Add(alignment);
        
        ShowAll();
    }

    protected override bool OnDeleteEvent(Event e) {
        Console.WriteLine("\nProcess finished with exit code 0.");
        Gtk.Application.Quit();
        return true;
    }
}