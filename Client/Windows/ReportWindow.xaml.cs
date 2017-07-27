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

namespace Client
{
	/// <summary>
	/// Interaction logic for ReportWindow.xaml
	/// </summary>
	public partial class ReportWindow : Window
	{
		public ReportWindow(Report report)
		{
            if (report == null)
            {
                throw new NullReferenceException("report is null");
            }

            List<KeyValuePair<int, int>> _valueList = new List<KeyValuePair<int, int>>();

            InitializeComponent();
            durationControl.Text = report.Duration.ToString();
            virtualUsersControl.Text = report.VirtualUsers.ToString();
            timeoutControl.Text = report.Timeout.ToString();
            betweenControl.Text = report.RequestDuration.ToString();
            durationControl.Text = report.Duration.ToString();
            strategyControl.Text = report.Strategy == TestingStrategy.DECREASING ? 
                "Генерация убывающей нагрузки" : "Генерация возрастающей нагрузки";
            dateControl.Text = report.Time.ToShortDateString() + " " + report.Time.ToShortTimeString();

            foreach (var point in report.Points)
            {
                _valueList.Add(new KeyValuePair<int, int>(point.x, point.y));
            }

            lineChart.DataContext = _valueList;
        }
	}
}
