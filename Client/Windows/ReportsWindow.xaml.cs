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
    public class MyItem
    {
        public int Id { get; set; }
        public string Time { get; set; }
    }

	/// <summary>
	/// Interaction logic for ReportsWindow.xaml
	/// </summary>
    /// 
	public partial class ReportsWindow : Window
	{
        private Dictionary<int, Report> _reports = new Dictionary<int, Report>();

		public ReportsWindow()
		{
            int i = 0;
			InitializeComponent();

            Reports reports = DataStorage.GetData<Reports>();

            if (reports.List == null)
            {
                return;
            }

            foreach (var report in reports.List)
            {
                i++;

                listView.Items.Add(new MyItem()
                {
                    Id = report.Id,
                    Time = report.Time.ToShortDateString() + " " + report.Time.ToShortTimeString()
                });

                _reports.Add(report.Id, report);
            }
		}

		private void openButton_Click(object sender, RoutedEventArgs e)
		{
            if (listView.SelectedIndex != -1)
            {
                MyItem item = (MyItem)listView.SelectedItem;
                ReportWindow window = new ReportWindow(_reports[item.Id]);
                window.ShowDialog();
            }
		}
	}
}
