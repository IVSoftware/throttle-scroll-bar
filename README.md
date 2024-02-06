## Throttle ScrollBar

I thought this might need throttling so I made a semaphore to prevent a reload if one was already in progress. **I was wrong!** Apparently, this is all baked into the `vScrollBar.Scroll` event because the debug line never once executed no matter how fast or long the scrollbar was pulled.

[![mockup][1]][1]

[![memory monitor][2]][2]
```
namespace throttle_scroll_bar
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            vScrollBar.Scroll += (sender, e) =>
            {
                if (_isScrollBusy.Wait(0))
                {
                    try
                    {
                        var image = RotateImageByExifOrientationData(localGetImagePath());
                        pictureBox.Image?.Dispose();
                        pictureBox.Image = image;
                        pictureBox.Refresh();
                    }
                    finally
                    {
                        _isScrollBusy.Release();
                    }
                }
                else
                {
                    Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} Busy");
                }
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
        }
        SemaphoreSlim _isScrollBusy = new SemaphoreSlim(1, 1);
        string? _prevImage = null;
        Random _random = new Random();
        // EXIF orientation tag
        const int EXIF_ORIENTATION_TAG = 0x0112;

        private Image RotateImageByExifOrientationData(string imageLocation)
        {
            Image img = Image.FromFile(imageLocation);
            if (img.PropertyIdList.FirstOrDefault(_=>_ == EXIF_ORIENTATION_TAG) is int id)
            {
                img.RotateFlip(ToRotateFlipType(img.GetPropertyItem(id)!.Value!.First()));
            }
            return img;
        }
        private RotateFlipType ToRotateFlipType(int orientation)
        {
            switch (orientation)
            {
                case 1: return RotateFlipType.RotateNoneFlipNone;
                case 2: return RotateFlipType.RotateNoneFlipX;
                case 3: return RotateFlipType.Rotate180FlipNone;
                case 4: return RotateFlipType.Rotate180FlipX;
                case 5: return RotateFlipType.Rotate90FlipX;
                case 6: return RotateFlipType.Rotate90FlipNone;
                case 7: return RotateFlipType.Rotate270FlipX;
                case 8: return RotateFlipType.Rotate270FlipNone;
                default: return RotateFlipType.RotateNoneFlipNone;
            }
        }
    }
}
```


  [1]: https://i.stack.imgur.com/J9NiE.png
  [2]: https://i.stack.imgur.com/vKmuc.png