using ShopNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShopNetwork
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void ForgotPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Свяжитесь с разработчиком для восстановления пароля!",
                "Забыли пароль?",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void DemoVersion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UserName.Text = "regular";
            Password.Password = "1234567890";

            PerformLogin();
        }

        private void PerformLogin()
        {
            string username = UserName.Text;
            string password = Password.Password;

            using (ShopNetworkContext _db = new ShopNetworkContext())
            {
                var user = _db.Users.FirstOrDefault(u => u.Username == username && u.UserPassword == password);

                if (user != null)
                {
                    MainWindow mainWindow = new MainWindow(user.Username);
                    this.Close();
                    mainWindow.ShowDialog();
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UserName.Text;
            string password = Password.Password;

            using (ShopNetworkContext _db = new ShopNetworkContext())
            {
                var user = _db.Users.FirstOrDefault(u => u.Username == username && u.UserPassword == password);

                if (user != null)
                {
                    MainWindow mainWindow = new MainWindow();
                    this.Close();
                    mainWindow.ShowDialog();
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Вы ввели неверный логин или пароль!",
                        "Неверные данные",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
    }
}
