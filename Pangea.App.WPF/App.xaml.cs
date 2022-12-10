using Esri.ArcGISRuntime.UI.Controls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pangea.Adapters.Esri;
using Pangea.App.ViewModels;
using Pangea.App.ViewModels.Services;
using Pangea.Core.UI;
using System;
using System.Windows;

namespace Pangea.App.WPF
{
    public partial class App : Application
    {
        private IHost _host;

        public new static App Current => (App)Application.Current;

        public App()
        {
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            SetEsriLicense();

            BuildHost();
            await _host.StartAsync();

            var shellView = _host.Services.GetService<ShellView>();
            shellView.Show();
        }

        private const string _environment = "Development";

        private void BuildHost()
        {
            _host = new HostBuilder()
                .ConfigureHostConfiguration(configBuilder =>
                {
                    configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    //configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    //        .AddJsonFile($"appsettings.{_environment}.json", optional: false, reloadOnChange: true);
                })
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    configBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<Settings>(context.Configuration);
                    RegisterServices(services);
                    RegisterViewModels(services);
                }).Build();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services
                .AddSingleton<IFileService, FileService>()
                .AddSingleton<IMapService, MapService>()
                .AddSingleton<IMessageService, MessageService>()
                .AddSingleton<IMap2D, Map2D>()
                .AddSingleton<MapView>();
        }

        private void RegisterViewModels(IServiceCollection services)
        {
            services
                .AddTransient<ShellView>()
                .AddSingleton<IShellVM, ShellVM>()
                .AddSingleton<IMainWindowVM, MainWindowVM>()
                .AddSingleton<IMapVM, MapVM>()
                .AddSingleton<ISceneVM, SceneVM>()
                .AddSingleton<IMessagesVM, MessagesVM>();
        }


        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }
        }

        private void SetEsriLicense()
        {
            // TODO: fetch key from DB through service
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey =
                "AAPK66c614bd2831461cb18f6bf0cbd2d1b97yRoME6IQpnATHwYmbUrOt5p6F-EwkJUQY7n4kNT5W4zjHh3xHvtsTLsDzEfX_VU";
        }

    }
}
