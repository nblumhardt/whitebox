using System.Windows;
using System.Windows.Input;

namespace Whitebox.Profiler.Controls
{
    /// <summary>
    /// Interaction logic for NavigationPanel.xaml
    /// </summary>
    public partial class NavigationPanel
    {
        public NavigationPanel()
        {
            InitializeComponent();
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(NavigationPanel));
    }
}
