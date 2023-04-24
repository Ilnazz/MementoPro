using MementoPro.ViewModels;
using MementoPro.ViewModels.Base;
using MementoPro.Views.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MementoPro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
            => new WindowView(new AuthVM()).Show();
    }
}
