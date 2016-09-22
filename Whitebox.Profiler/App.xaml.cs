using System.Windows;
using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;

namespace Whitebox.Profiler
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var config = new ConfigurationBuilder();
            config.AddJsonFile("WhiteboxProfilerAutofac.json");
            var module = new ConfigurationModule(config.Build());

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var mainWindow = container.Resolve<ProfilerWindowView>();
            mainWindow.Show();
        }
    }
}
