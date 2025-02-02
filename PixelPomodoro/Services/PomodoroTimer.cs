using System.Timers;
using Gtk;

namespace PixelPomodoro.Services;

public class PomodoroTimer {
    private readonly TelegramNotifier _telegramNotifier = new(); 
    
    private readonly System.Timers.Timer _timer;
    private int _secondsLeft;
    private bool _isWorkPhase = true;
    private Label _timeLabel = new();
    private Label _statusLabel = new();
    private int _workMinutes = 0;
    private int _breakMinutes = 0;
    private bool _phaseChanged = false;

    public PomodoroTimer(Label timeLabel, Label statusLabel, int workMinutes = 25, int breakMinutes = 5) {
        _workMinutes = workMinutes;
        _breakMinutes = breakMinutes;
        _timeLabel = timeLabel;
        _statusLabel = statusLabel;
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += TimerElapsed;
    }

    private void StartTimer(int minutes) {
        _secondsLeft = minutes * 60;
        _phaseChanged = false;
        UpdateDisplay();
        _timer.Start();
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e) {
        if (_secondsLeft > 0) {
            _secondsLeft--;
            Application.Invoke((sender, args) => UpdateDisplay());
        }
        else {
            _timer.Stop();
            _isWorkPhase = !_isWorkPhase;
            _phaseChanged = false;
            StartTimer(_isWorkPhase ? _workMinutes : _breakMinutes);
        }
    }

    private void UpdateDisplay() {
        _timeLabel.Text = $"{_secondsLeft / 60:D2}:{_secondsLeft % 60:D2}";
        if (!_phaseChanged) {
            if (_isWorkPhase) {
                _telegramNotifier.SendNotification("The clock is ticking... let's get to work!");
                _statusLabel.Text = "Work period";
            }
            else {
                _telegramNotifier.SendNotification("Take a breather, you've earned it!");
                _statusLabel.Text = "Break";
            }
            _phaseChanged = true;
        }
    }

    public void Start() {
        StartTimer(_workMinutes);
    }
    
    public void Stop() {
        _secondsLeft = _isWorkPhase ? _workMinutes * 60 : _breakMinutes * 60;
        _timeLabel.Text = $"{_secondsLeft / 60:D2}:{_secondsLeft % 60:D2}";
        _timer.Stop();
        UpdateDisplay();
    }

    public bool isTick() {
        return _timer.Enabled;
    }
}