using System.Windows;
using System.Windows.Controls;

namespace MementoPro.Views
{
    /// <summary>
    /// Логика взаимодействия для UserMenuView.xaml
    /// </summary>
    public partial class UserMenuView : UserControl
    {
        public UserMenuView()
        {
            InitializeComponent();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuBox.Visibility
                = MenuBox.Visibility == Visibility.Collapsed
                    ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
