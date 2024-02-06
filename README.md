## Throttle ScrollBar

As I understand it, the problem occurs:

> after scrolling fast or slow many times...

There are a couple of ways you could consider throttling a trackbar or scrollbar. Here, when the Throttle function is enabled it reduces the frequency of "progress" updates for visual feedback (loading thumbnail perhaps, or something else). As a separate strategy, with the `WatchDog` timer, you only get a "load" after a period of inactivity. I wonder if either of these things might help?

Also, when you 'do' perform any kind of update while the scrolling is taking place (whether at normal or reduced rate), it's probably best not to block the scrolling event handler to do it so try `BeginInvoke` and post the update instead.

___

The test condition has the max set to 3000 and I pull the handle down as quick as I can.

[![visual comparison][1]][1]

```csharp
public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        enableThrottle.Location = new Point(textBox.Width - 80, textBox.Height - 80);
        textBox.Controls.Add(enableThrottle);

        vScrollBar.Scroll += async (sender, e) =>
        {
            if (enableThrottle.Checked)
            {
                if (_isScrollBusy.Wait(0))
                {
                    try
                    {
                        localUpdate();
                        await Task.Delay(25);
                    }
                    finally
                    {
                        _isScrollBusy.Release();
                    }
                }
            }
            else localUpdate();

            void localUpdate()
            {
                BeginInvoke(() =>
                {
                    textBox.AppendText($"{e.NewValue}{Environment.NewLine}");
                    textBox.ScrollToCaret();
                    _count++;
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
                BeginInvoke(() => MessageBox.Show($@"
{_count} updates in {_stopwatch.ElapsedMilliseconds} ms.
Load image #{vScrollBar.Value}".Trim())
                );
            });
        };
        enableThrottle.CheckedChanged += (sender, e) =>
        {
            enableThrottle.BackColor = enableThrottle.Checked ? Color.DarkBlue : Color.RoyalBlue;
            textBox.Clear();
        };
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
}
```


  [1]: https://i.stack.imgur.com/BObIq.png