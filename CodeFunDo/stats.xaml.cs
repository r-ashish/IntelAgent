using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
//using System.Windows.Controls.DataVisualization.Toolkit;
//using System.Windows.Controls.Toolkit;
//using System.Windows.Controls.Toolkit.Internals;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace IntelAgent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class stats : Page
    {
        public stats()
        {
            this.InitializeComponent();
        }

        public class graphData
        {
            public string Name { get; set; }
            public double Amount { get; set; }
        }

        private async Task SetChartData()
        {
            await LoadChartContents("Daily");
        }
        
        private async Task LoadChartContents(string option)
        {
            List<graphData> data = new List<graphData>();
            List<graphData> pieData = new List<graphData>();
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");

            if(option == "Daily")
            {
                tB2.Text = "";
                tB3.Text = "";
                var fileDay = await folder.CreateFileAsync("dayWiseData.txt", CreationCollisionOption.OpenIfExists);
                var dayContent = await FileIO.ReadTextAsync(fileDay);
                string[] deliSemi = { ";" };
                var dailyDataList = dayContent.Split(deliSemi, StringSplitOptions.RemoveEmptyEntries);
                DateTime id = DateTime.Today.Date;
                int i = id.Day - dailyDataList.Length + 1;
                foreach (var item in dailyDataList)
                {
                    pieData.Add(new graphData() { Name = i + "-" + id.Month, Amount = double.Parse(item) });
                    i += 1;
                }
            }
            else if (option == "Weekly")
            {
                tB2.Text = "";
                tB3.Text = "";
                var fileWeek = await folder.CreateFileAsync("WeekWiseData.txt", CreationCollisionOption.OpenIfExists);
                var WeekContent = await FileIO.ReadTextAsync(fileWeek);
                string[] deliSemi = { ";" };
                var WeeklyDataList = WeekContent.Split(deliSemi, StringSplitOptions.RemoveEmptyEntries);
                Object index = DateTime.Now;
                int Week_num = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(Convert.ToDateTime(index), System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);

                int i = 0;
                for (int x = WeeklyDataList.Length; x > 0; x--)
                {
                    string sd;
                    int ct = i + 1;
                    if (ct == 5) sd = "Current Week";
                    else sd = "Week " + ct;
                    pieData.Add(new graphData() { Name = sd, Amount = double.Parse(WeeklyDataList[WeeklyDataList.Length - x]) });
                    i += 1;
                }

            }
            else
            {
                tB2.Text = "";
                tB3.Text = "";
                var fileMonth = await folder.CreateFileAsync("MonthWiseData.txt", CreationCollisionOption.OpenIfExists);
                var MonthContent = await FileIO.ReadTextAsync(fileMonth);
                string[] deliSemi = { ";" };
                var MonthlyDataList = MonthContent.Split(deliSemi, StringSplitOptions.RemoveEmptyEntries);
                DateTime id = DateTime.Today.Date;
                int i = id.Month;
                i = i - MonthlyDataList.Length;
                if (i < 0)
                {
                    i += 12;
                }
                string[] Montth = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                foreach (var item in MonthlyDataList)
                {
                    //var itm = item.Split(deli1, StringSplitOptions.RemoveEmptyEntries);
                    pieData.Add(new graphData() { Name = Montth[i % 12], Amount = double.Parse(item) });
                    i += 1;
                }
            }

            var file = await folder.CreateFileAsync("categoryData.txt", CreationCollisionOption.OpenIfExists);
            var s = await FileIO.ReadTextAsync(file);
            string[] deli = { "\n" };
            string[] deli1 = { ";" };

            var dataList = s.Split(deli, StringSplitOptions.RemoveEmptyEntries);
            float sum = 0;
            if(dataList.Length !=0 ) sum = mySum(dataList);
            foreach (var item in dataList)
            {
                var itm = item.Split(deli1, StringSplitOptions.RemoveEmptyEntries);
                float percnt = (float.Parse(itm[1])/sum)*100;
                var nm = percnt.ToString();
                if (nm.Length > 4) nm = nm.Substring(0, 4);
                data.Add(new graphData() { Name = itm[0] + "(" + nm+"%)", Amount = double.Parse(itm[1]) });
            }
            ((PieSeries)PieChart.Series[0]).ItemsSource = data;
            ((LineSeries)LineChart.Series[0]).ItemsSource = pieData;
            ((BarSeries)BarChart.Series[0]).ItemsSource = pieData;
            if (pieData.Capacity == 0)
            {
                tB2.Text = "No data to display";
                tB3.Text = "No data to display";
            }
            if (data.Capacity == 0)
            {
                tB1.Text = "No data to display";
            }
        }
        private float mySum(String[] list)
        {
            float sum = 0;
            string[] deli1 = { ";" };
            foreach (var item in list)
            {
                sum += float.Parse(item.Split(deli1, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
            return sum;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await SetChartData();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += Hardwarebuttons_BackPressed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= Hardwarebuttons_BackPressed;
        }
        private void Hardwarebuttons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (this.Frame == null) return;
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                e.Handled = true;
            }
            else
            {
                this.Frame.Navigate(typeof(MainPage));
                e.Handled = true;
            }
        }

        private void ComboBox_SelectionChanged(object sender, object e)
        {
            string[] options = { "Daily", "Weekly", "Monthly" };
            var obj = sender as ComboBox;
            refreshPlot(options.ElementAt(obj.SelectedIndex));
        }

        private async void refreshPlot(string s)
        {
            try
            {
                await LoadChartContents(s);
            }
            catch(NullReferenceException)
            {

            }
        }
    }
}
