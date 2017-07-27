using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Interaction logic for MonitoringWindow.xaml
    /// </summary>
    public partial class MonitoringWindow : Window
    {
        private DispatcherTimer _timer = new DispatcherTimer();
        private bool _finished = false;

        public MonitoringWindow()
        {
            InitializeComponent();
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
            statusLabel.Content = "Ожидание информации от сервера...";
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            if (_finished)
            {
                return;
            }

            try
            {
                TaskCreationResult result = DataStorage.GetData<TaskCreationResult>();
                List<KeyValuePair<int, int>> _valueList = new List<KeyValuePair<int, int>>();

                await DataStorage.RequestData(new RequestTaskInfo()
                {
                    TaskGroup = result.GroupNumber
                });

                TaskInfoClient task = DataStorage.GetData<TaskInfoClient>();

                if (task != null)
                {
                    durationControl.Text = task.Duration.ToString();
                    betweenControl.Text = task.RequestDuration.ToString();
                    timeoutControl.Text = task.Timeout.ToString();
                    virtualUsersControl.Text = task.VirtualUsers.ToString();

                    switch (task.Strategy)
                    {
                        case TestingStrategy.DECREASING:
                            strategyControl.Text = "Генерация убывающей нагрузки";
                            break;
                        case TestingStrategy.INCREASING:
                            strategyControl.Text = "Генерация возрастающей нагрузки";
                            break;
                    }

                    switch (task.State)
                    {
                        case TaskState.ABORTED:
                            statusLabel.Content = "Выполнение задания было прервано сервером.";
                            _finished = true;
                            break;
                        case TaskState.CREATED:
                            statusLabel.Content = "Задание создано. Ожидание начала выполнения.";
                            break;
                        case TaskState.RUNNING:
                            statusLabel.Content = "Задание выполняется.";
                            break;
                        case TaskState.FINISHED:
                            statusLabel.Content = "Задание завершено.";
                            _finished = true;
                            break;
                    }

                    if (task.Points != null)
                    {
                        foreach (var point in task.Points)
                        {
                            _valueList.Add(new KeyValuePair<int, int>(point.x, point.y));
                        }

                        lineChart.DataContext = _valueList;
                    }
                }
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Сервер не ответил вовремя.", "Ошибка");
                Close();
            }
            catch (SocketException)
            {
                MessageBox.Show("Соединение с сервером потеряно.", "Ошибка");
                Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _timer.Stop();
        }
    }
}
