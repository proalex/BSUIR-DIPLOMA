using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
	/// <summary>
	/// Interaction logic for NewProfileWindow.xaml
	/// </summary>
	public partial class NewProfileWindow : Window
	{
		public NewProfileWindow()
		{
			InitializeComponent();
		}

        public NewProfileWindow(TaskData data, string name)
        {
            if (data == null)
            {
                throw new NullReferenceException("data is null");
            }

            if (name == null)
            {
                throw new NullReferenceException("name is null");
            }

            InitializeComponent();
            profileNameControl.Text = name;
            virtualUsersControl.Text = data.VirtualUsers.ToString();
            timeoutControl.Text = data.Timeout.ToString();
            betweenControl.Text = data.RequestDuration.ToString();
            durationControl.Text = data.Duration.ToString();
            strategyControl.SelectedIndex = (int)data.Strategy;

            foreach (var url in data._URLs)
            {
                urlListControl.Items.Add(url);
            }
        }

        private void virtualUsersControl_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!TextChecker.OnlyNumbers(e.Text))
			{
				e.Handled = true;
			}
		}

		private void timeoutControl_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!TextChecker.OnlyNumbers(e.Text))
			{
				e.Handled = true;
			}
		}

		private void betweenControl_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!TextChecker.OnlyNumbers(e.Text))
			{
				e.Handled = true;
			}
		}

		private void durationControl_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!TextChecker.OnlyNumbers(e.Text))
			{
				e.Handled = true;
			}
		}

		private void virtualUsersControl_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!TextChecker.OnlyNumbers(virtualUsersControl.Text))
			{
				virtualUsersControl.Text = "";
			}
		}

		private void timeoutControl_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!TextChecker.OnlyNumbers(virtualUsersControl.Text))
			{
				timeoutControl.Text = "";
			}
		}

		private void betweenControl_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!TextChecker.OnlyNumbers(virtualUsersControl.Text))
			{
				betweenControl.Text = "";
			}
		}

		private void durationControl_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!TextChecker.OnlyNumbers(virtualUsersControl.Text))
			{
				durationControl.Text = "";
			}
		}

		private void addUrlButton_Click(object sender, RoutedEventArgs e)
		{
			if (!TextChecker.URL(urlControl.Text))
			{
				MessageBox.Show("URL введен не верно.", "Ошибка");
				return;
			}

			if (urlListControl.Items.Contains(urlControl.Text))
			{
				MessageBox.Show("URL уже есть в списке.", "Ошибка");
				return;
			}

			urlListControl.Items.Add(urlControl.Text);
		}

		private async void saveProfileButton_Click(object sender, RoutedEventArgs e)
		{
            DisableButtons();

            try
            {
                List<string> URLs = new List<string>();
                int virtualUsers = int.Parse(virtualUsersControl.Text);
                int timeout = int.Parse(timeoutControl.Text);
                int requestDuration = int.Parse(betweenControl.Text);
                int duration = int.Parse(durationControl.Text); ;
                TestingStrategy strategy = (TestingStrategy)strategyControl.SelectedIndex;
                string name = profileNameControl.Text;

                foreach (var item in urlListControl.Items)
                {
                    URLs.Add((string)item);
                }

                if (URLs.Count == 0)
                {
                    MessageBox.Show("Список URL-адресов пустой.", "Ошибка");
                    EnableButtons();
                    return;
                }

                if (virtualUsers < 1 || timeout < 1 || requestDuration < 1 || duration < 1 || name.Length < 1)
                {
                    throw new FormatException();
                }

                TaskData data = new TaskData()
                {
                    _URLs = URLs,
                    VirtualUsers = virtualUsers,
                    Timeout = timeout,
                    RequestDuration = requestDuration,
                    Duration = duration,
                    Strategy = strategy
                };

                SaveProfileRequest request = new SaveProfileRequest()
                {
                    Name = name,
                    Data = data
                };

                await DataStorage.RequestData(request);

                var response = DataStorage.GetData<SaveProfileResponse>();

                if (response != null && response.Result == SaveProfileResult.NAME_EXISTS)
                {
                    MessageBox.Show("Введенное имя профиля уже существует.", "Ошибка");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Данные введены не верно.", "Ошибка");
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Сервер не ответил вовремя.", "Ошибка");
                Close();
                return;
            }
            catch (SocketException)
            {
                MessageBox.Show("Сервер не ответил вовремя.", "Ошибка");
                Close();
                return;
            }

            EnableButtons();
		}

		private async void beginTestingButton_Click(object sender, RoutedEventArgs e)
		{
            DisableButtons();

            try
            {
                List<string> URLs = new List<string>();
                int virtualUsers = int.Parse(virtualUsersControl.Text);
                int timeout = int.Parse(timeoutControl.Text);
                int requestDuration = int.Parse(betweenControl.Text);
                int duration = int.Parse(durationControl.Text); ;
                TestingStrategy strategy = (TestingStrategy)strategyControl.SelectedIndex;

                foreach (var item in urlListControl.Items)
                {
                    URLs.Add((string)item);
                }

                if (URLs.Count == 0)
                {
                    MessageBox.Show("Список URL-адресов пустой.", "Ошибка");
                    EnableButtons();
                    return;
                }

                if (virtualUsers < 1 || timeout < 1 || requestDuration < 1 || duration < 1)
                {
                    throw new FormatException();
                }

                TaskData data = new TaskData()
                {
                    _URLs = URLs,
                    VirtualUsers = virtualUsers,
                    Timeout = timeout,
                    RequestDuration = requestDuration,
                    Duration = duration,
                    Strategy = strategy
                };

                await DataStorage.RequestData(data);

                TaskCreationResult result = DataStorage.GetData<TaskCreationResult>();

                if (result != null && result.GroupNumber != 0)
                {
                    MonitoringWindow window = new MonitoringWindow();
                    window.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Сервер отказал в выполнении задания.", "Ошибка");
                }

                EnableButtons();
            }
            catch (FormatException)
            {
                MessageBox.Show("Данные введены не верно.", "Ошибка");
                EnableButtons();
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Сервер не ответил вовремя.", "Ошибка");
                Close();
                return;
            }
            catch (SocketException)
            {
                MessageBox.Show("Сервер не ответил вовремя.", "Ошибка");
                Close();
                return;
            }
        }

        public void EnableButtons()
        {
            saveProfileButton.IsEnabled = true;
            beginTestingButton.IsEnabled = true;
            addUrlButton.IsEnabled = true;
        }

        public void DisableButtons()
        {
            saveProfileButton.IsEnabled = false;
            beginTestingButton.IsEnabled = false;
            addUrlButton.IsEnabled = false;
        }
	}
}
