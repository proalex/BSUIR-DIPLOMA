using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthenticateWindow : Window
    {
        public AuthenticateWindow()
        {
            InitializeComponent();
        }

        private async void AuthenticateAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                authenticateButton.IsEnabled = false;

                if (!DataStorage.Connected)
                {
                    await DataStorage.ConnectAsync();
                }

                await DataStorage.RequestData(ClientOpcode.SALT_REQUEST);

                byte[] salt = DataStorage.GetData<AuthSalt>().Salt;
                SHA512 hasher = SHA512.Create();
                byte[] password = hasher.ComputeHash(Encoding.UTF8.GetBytes(emailControl.Text + ":" + passwordControl.Password));
                byte[] concat = new byte[salt.Length + password.Length];

                password.CopyTo(concat, 0);
                salt.CopyTo(concat, password.Length);

                byte[] token = hasher.ComputeHash(concat);


                await DataStorage.RequestData(new AuthData()
                {
                    Email = emailControl.Text,
                    Token = token
                });

                AuthResponse response = DataStorage.GetData<AuthResponse>();

                if (response.Authenticated)
                {
                    Visibility = Visibility.Hidden;
                    authenticateButton.IsEnabled = true;

                    MainWindow window = new MainWindow();
                    window.ShowDialog();

                    if (window.ExitStatus)
                    {
                        Close();
                    }
                    else
                    {
                        Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (response.AttemptsLeft == 255)
                    {
                        MessageBox.Show("Не верный email.", "Ошибка");
                    }
                    else if (response.AttemptsLeft != 0)
                    {
                        MessageBox.Show("Не верный пароль. Осталось попыток для " + emailControl.Text + " " + response.AttemptsLeft + ".", "Ошибка");
                    }
                    else
                    {
                        MessageBox.Show("Аккаунт " + emailControl.Text + " заблокирован. Обратитесь к администратору для разблокировки.", "Ошибка");
                    }

                    authenticateButton.IsEnabled = true;
                }
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Сервер не ответил вовремя.", "Ошибка");
                authenticateButton.IsEnabled = true;
                return;
            }
            catch (SocketException)
            {
                MessageBox.Show("Не удалось установить соединение с сервером.", "Ошибка");
                authenticateButton.IsEnabled = true;
                return;
            }
        }
    }
}
