using IVSoftware.Portable;
using System.Diagnostics;

namespace throttle_scroll_bar
{
    enum DisposeModeForTest
    {
        None,
        ThrowsException,
        WorksButUnnecessary
    }
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            vScrollBar.Scroll += (sender, e) =>
            {
                if (_isScrollBusy.Wait(0))
                {
                    BeginInvoke(() =>
                    {
                        try
                        {
                            localReload();
                        }
                        finally
                        {
                            _isScrollBusy.Release();
                        }
                    });
                }
                _wdtLoad.StartOrRestart(
                initialAction: () =>
                {
                    _stopwatch.Restart();
                    _count = 0;
                },
                completeAction: () =>
                {
                    _stopwatch.Stop();
                    BeginInvoke(() =>
                    {
                        localReload();
                    });
                });
            };
            enableThrottle.CheckedChanged += (sender, e) =>
            {
                enableThrottle.BackColor = enableThrottle.Checked ? Color.DarkBlue : Color.RoyalBlue;
            };
            string localGetImagePath()
            {
                while(true)
                {
                    var image = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Images",
                        $"Image.{_random.Next(1, 5):D2}.jpg");
                    if(image != _prevImage)
                    {
                        _prevImage = image;
                        return image;
                    }
                }
            }
            async void localReload()
            {
                var testMode = DisposeModeForTest.None;

                switch (testMode)
                {
                    case DisposeModeForTest.None:
                        pictureBox.ImageLocation = localGetImagePath();
                        break;
                    case DisposeModeForTest.ThrowsException:
                        // Doesn't work, period.
                        pictureBox.Image?.Dispose();
                        pictureBox.ImageLocation = localGetImagePath();
                        break;
                    case DisposeModeForTest.WorksButUnnecessary:
                        var toDispose = pictureBox.Image;
                        pictureBox.ImageLocation = localGetImagePath();

                        // If you delay the automatic disposal by capturing a
                        // reference to the previous image, the "average" memory
                        // use increases because GC is delayed.
                        await Task.Delay(1000);
                        toDispose?.Dispose();
                        break;
                }
            }
        }
        CheckBox enableThrottle = new CheckBox
        {
            BackColor = Color.RoyalBlue,
            Appearance = Appearance.Button,
            Height = 75,
            Width = 125,
            Text = "Throttle",
            ForeColor = Color.White,
        };
        SemaphoreSlim _isScrollBusy = new SemaphoreSlim(1,1);

        // <PackageReference Include = "IVSoftware.Portable.WatchdogTimer" Version="1.2.1" />
        WatchdogTimer _wdtLoad = new WatchdogTimer { Interval = TimeSpan.FromSeconds(0.5) };

        Stopwatch _stopwatch = new Stopwatch();
        int _count = 0;
        string _prevImage = null;
        Random _random = new Random();
    }
}
