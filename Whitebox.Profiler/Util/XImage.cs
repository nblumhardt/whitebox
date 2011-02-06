using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Whitebox.Profiler.Util
{
    class XImage : Image
    {
        public XImage()
        {
            Loaded += XImage_Loaded;
        }

        void XImage_Loaded(object sender, RoutedEventArgs e)
        {
            SetValue(SnapsToDevicePixelsProperty, true);
            SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var bitmapSource = (BitmapSource)Source;
            return new Size(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
        }
    }
}
