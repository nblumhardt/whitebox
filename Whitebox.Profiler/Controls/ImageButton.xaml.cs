using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Whitebox.Profiler.Controls
{
    /// <summary>
    /// Interaction logic for ImageButton.xaml
    /// </summary>
    public partial class ImageButton : Button
    {
        public ImageButton()
        {
            InitializeComponent();
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageSource));
    }
}
