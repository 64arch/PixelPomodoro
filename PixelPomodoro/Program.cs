using PixelPomodoro.Services;
using PixelPomodoro.Windows;

namespace PixelPomodoro;

class Program {
    static void Main(string[] args) {
        Console.WriteLine("Application started");
        Gtk.Application.Init();
        GTKWindow w = new GTKWindow();
        Gtk.Application.Run();
    }
}