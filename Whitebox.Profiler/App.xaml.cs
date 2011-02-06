using System.Windows;
using Autofac;
using Autofac.Configuration;

namespace Whitebox.Profiler
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ConfigurationSettingsReader());
            var container = builder.Build();

            var mainWindow = container.Resolve<ProfilerWindowView>();
            mainWindow.Show();
        }
    }
}
