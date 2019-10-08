using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DesktopBd.Service;
using DesktopBd.MVVM.View;
using DesktopBd.Repositories;

namespace DesktopBd
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            FileService.ReadAccountsInFile(); // TODO Перевод на базу данных.
            ViewManager.OpenNewWindowInShow(new AuthorizationView(), false);
             //ViewManager.OpenNewWindowInShow(new DesktopView(AccountRepositorie.GetAccount("Vinniko")), false);
        }
    }
}
