using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
		private bool _exit;
        private DispatcherTimer _timer = new DispatcherTimer();
        private Profiles _profiles;
        private Dictionary<string, TaskData> _profilesData = new Dictionary<string, TaskData>();

        public bool ExitStatus { get { return _exit; } }

		public MainWindow()
		{
            InitializeComponent();
			_exit = true;
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var generators = DataProxy.Generators;
            int loadSum = 0;

            generatorsControl.Items.Clear();

            if (!DataStorage.Connected)
            {
                MessageBox.Show("Соединение с сервером потеряно.", "Ошибка");
                _exit = false;
                Close();
            }

            if (generators != null)
            {
                foreach (var generator in generators)
                {
                    generatorsControl.Items.Add(new
                    {
                        Number = generator.Number.ToString(),
                        Load = generator.Load.ToString()
                    });

                    loadSum += generator.Load;
                }

                if (generators.Count > 0)
                {
                    loadSum /= generators.Count;
                }

                infrastructureLoadingRate.Content = generators.Count == 0 ? "N/A" : loadSum.ToString() + "%";
            }
            else
            {
                infrastructureLoadingRate.Content = "N/A";
            }
            
            var tasks = DataStorage.GetData<ActiveTasks>();

            tasksConrtol.Items.Clear();

            if (tasks != null && tasks.Tasks != null)
            {
                foreach (var task in tasks.Tasks)
                {
                    tasksConrtol.Items.Add(new { User = task.Owner, Number = task.GroupNumber, Load = task.Loading });
                }
            }

            Profiles profiles = DataStorage.GetData<Profiles>();

            if (profiles != null && profiles != _profiles)
            {
                _profilesData.Clear();
                profileControl.Items.Clear();

                if (profiles.List != null)
                {
                    foreach (var profile in profiles.List)
                    {
                        _profilesData.Add(profile.Name, profile.Data);
                        profileControl.Items.Add(profile.Name);
                    }
                }

                _profiles = profiles;
            }
        }


        private async void changeUserButton_Click(object sender, RoutedEventArgs e)
		{
            DisableButtons();

            try
            {
                await DataStorage.RequestData(ClientOpcode.LOGOUT_REQUEST);
            }
            catch (SocketException)
            {

            }

            EnableButtons();
			_exit = false;
			Close();
		}

		private void createNewProfileButton_Click(object sender, RoutedEventArgs e)
		{
			NewProfileWindow window = new NewProfileWindow();
			window.ShowDialog();
		}

		private void chooseProfileButton_Click(object sender, RoutedEventArgs e)
		{
            string name = (string)profileControl.SelectedItem;

            if (name != null && _profilesData.ContainsKey(name))
            {
                TaskData data = _profilesData[name];
                NewProfileWindow window = new NewProfileWindow(data, name);
                window.ShowDialog();
            }
		}

		private void reportListButton_Click(object sender, RoutedEventArgs e)
		{
			ReportsWindow window = new ReportsWindow();
			window.ShowDialog();
		}

        private void EnableButtons()
        {
            changeUserButton.IsEnabled = true;
            chooseProfileButton.IsEnabled = true;
            createNewProfileButton.IsEnabled = true;
            deleteProfileButton.IsEnabled = true;
            reportListButton.IsEnabled = true;
        }

        private void DisableButtons()
        {
            changeUserButton.IsEnabled = false;
            chooseProfileButton.IsEnabled = false;
            createNewProfileButton.IsEnabled = false;
            deleteProfileButton.IsEnabled = false;
            reportListButton.IsEnabled = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _timer.Stop();
        }

        private async void deleteProfileButton_Click(object sender, RoutedEventArgs e)
        {
            string name = (string)profileControl.SelectedItem;

            DisableButtons();

            if (name != null && _profilesData.ContainsKey(name))
            {
                try
                {
                    await DataStorage.RequestData(new DeleteProfileRequest()
                    {
                        Name = name
                    });
                }
                catch (TimeoutException)
                {
                    MessageBox.Show("Сервер не ответил вовремя.", "Ошибка");
                    _exit = false;
                    Close();
                    return;
                }
                catch (SocketException)
                {
                    MessageBox.Show("Сервер не ответил вовремя.", "Ошибка");
                    _exit = false;
                    Close();
                    return;
                }
            }

            EnableButtons();
        }
    }
}
