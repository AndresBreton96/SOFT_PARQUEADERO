using Datos.Contratos;
using Datos.Contratos.Rates;
using Datos.Dapper.RatesRepositories;
using Datos.Dapper.Users;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Negocio.Contratos.Rates;
using Negocio.Contratos.Users;
using Negocio.General.Rates;
using Negocio.General.Users;
using Presentacion.WPF.State.Accounts;
using Presentacion.WPF.State.Authenticators;
using Presentacion.WPF.State.Navigators;
using Presentacion.WPF.ViewModels;
using Presentacion.WPF.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Presentacion.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Presentacion.WPF.dll");
            var config = ConfigurationManager.OpenExeConfiguration(path);
            var section = config.GetSection("connectionStrings");

            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                config.Save();
            }

            Window window = serviceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            #region Dependency Layers
            #region Business Layer
            services.AddSingleton<IUsersAdministrator, UsersAdministrator>();
            services.AddSingleton<IRatesAdministrator, RatesAdministrator>();

            #endregion

            #region Data Layer
            services.AddSingleton<IUsersRepository, UserRepository>();
            services.AddSingleton<IRatesRepository, RatesRepository>();

            services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            #endregion

            #endregion

            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddSingleton<IViewModelFactory, ViewModelFactory>();

            services.AddSingleton<CreateViewModel<HomeViewModel>>(services =>
            {
                return () => new HomeViewModel(
                    services.GetRequiredService<IAccountStore>());
            });

            services.AddSingleton<CreateViewModel<ModifyPricesViewModel>>(services =>
            {
                return () => new ModifyPricesViewModel(
                    services.GetRequiredService<IRatesAdministrator>());
            });

            services.AddSingleton<ViewModelDelegateRenavigator<HomeViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<ModifyPricesViewModel>>();
            services.AddSingleton<CreateViewModel<LoginViewModel>>(services =>
            {
                return () => new LoginViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<ViewModelDelegateRenavigator<HomeViewModel>>());
            });

            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IAccountStore, AccountStore>();
            services.AddScoped<MainViewModel>();
            services.AddScoped<HomeViewModel>();
            services.AddScoped<ModifyPricesViewModel>();


            services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));

            return services.BuildServiceProvider();
        }
    }
}
