using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace IntelAgent
{
    public sealed partial class detailsPage : Page
    {
        public detailsPage()
        {
            this.InitializeComponent();
        }

        string addOrMinus;
        //public string[] 
        string[] category = { "Food", "Travel", "Entertainment", "Daily Needs", "Clothes", "Others" };
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            foreach (var item in category)
            {
                ComboBoxItem i1 = new ComboBoxItem();
                i1.Content = item;
                //i1.Foreground = new SolidColorBrush(Colors.Green);
                categoryList.Items.Add(i1);
            }
           
            if(e.Parameter.ToString() == "add")
            {
                categoryList.IsEnabled = false;
                amount.Text = "Enter amount to add";
                addOrMinus = "add";
                actionButton.Content = "Add Amount";
                headBlock.Text = "Add Amount";
                categoryList.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                categoryBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                Thickness margin = Date.Margin;
                margin.Top = 197;
                margin.Right = 20;
                Date.Margin = margin;

                Thickness margin1 = dateBlock.Margin;
                margin1.Top = 207;
                margin1.Left = 20;
                dateBlock.Margin = margin1;
            }
            else if (e.Parameter.ToString() == "minus")
            {
                categoryList.IsEnabled = true;
                amount.Text = "Enter expense amount";
                addOrMinus = "minus";
                actionButton.Content = "Add Expense";
                headBlock.Text = "Add Expense";
            }
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

        private void amount_GotFocus(object sender, RoutedEventArgs e)
        {
            if(amount.Text == "Enter amount to add" || amount.Text == "Enter expense amount")
            {
                amount.Text = "";
            }
            else
            {
                amount.Text = retrieveAmount(amount.Text);
            }
        }

        private void amount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (amount.Text == "")
            {
                if (addOrMinus == "add") amount.Text = "Enter amount to add";
                else amount.Text = "Enter expense amount";
            }
            else
            {
                amount.Text = formatAmount(amount.Text);
            }
        }

        public string formatAmount(string amt)
        {
            return String.Format("{1} {0}", amt,App.symbol);
        }
        public string retrieveAmount(string amt)
        {
            return amt.Replace(string.Format("{0} ",App.symbol), "");
        }

        private void descriptionBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (descriptionBox.Text == "Optional") descriptionBox.Text = "";
        }

        private void descriptionBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (descriptionBox.Text == "") descriptionBox.Text = "Optional";
        }

        private async void actionButton_Click(object sender, RoutedEventArgs e)
        {
            bool navigate = false;
            if(addOrMinus == "add" && amount.Text != "Enter amount to add")
            {
                MainPage.Balance += int.Parse(retrieveAmount(amount.Text).Trim());
                navigate = true;
                if (descriptionBox.Text != "Optional") writeDetails("Credit", descriptionBox.Text, int.Parse(retrieveAmount(amount.Text).Trim()));
                else writeDetails("Credit","", int.Parse(retrieveAmount(amount.Text).Trim()));
            }
            else if (addOrMinus == "minus" && amount.Text != "Enter expense amount")
            {
                if (categoryList.SelectedIndex != -1)
                {
                    MainPage.Balance -= int.Parse(retrieveAmount(amount.Text).Trim());
                    navigate = true;
                    writeTodayData(int.Parse(retrieveAmount(amount.Text).Trim()));
                    writethisMonthData(int.Parse(retrieveAmount(amount.Text).Trim()));
                    writethisWeekData(int.Parse(retrieveAmount(amount.Text).Trim()));
                    if (descriptionBox.Text != "Optional") writeDetails("Debit", descriptionBox.Text, int.Parse(retrieveAmount(amount.Text).Trim()));
                    else writeDetails("Debit", category.ElementAt(categoryList.SelectedIndex), int.Parse(retrieveAmount(amount.Text).Trim()));
                    addToPieData(category.ElementAt(categoryList.SelectedIndex), retrieveAmount(amount.Text).Trim());
                }
                else
                {
                    categoryList.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
            try
            {
                var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                var file = await folder.CreateFileAsync("Balance.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file,MainPage.Balance.ToString());
    
            }
            catch(UnauthorizedAccessException)
            {
                
            }
            if(navigate) this.Frame.Navigate(typeof(MainPage),"");
        }

        private async void writeTodayData(int i)
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("today.txt", CreationCollisionOption.ReplaceExisting);
            string[] deli = { "\n" };
            string[] deli1 = { ":" };

            if (App.dateEqual)
            {
                i += App.todayAmount;
            }
            else
            {
                App.dateEqual = true;
            }
            App.todayAmount = i;
            await FileIO.WriteTextAsync(file, DateTime.Today.Date.Day + ":" + DateTime.Today.Date.Month + "\n" + i);

            var fileDay = await folder.CreateFileAsync("dayWiseData.txt", CreationCollisionOption.OpenIfExists);
            var dayContent = await FileIO.ReadTextAsync(fileDay);
            string[] deliSemi = { ";" };
            var dailyDataList = dayContent.Split(deliSemi, StringSplitOptions.RemoveEmptyEntries);
            string s = "";
            if (dailyDataList.Length != 0)
            {
                dailyDataList[dailyDataList.Length - 1] = App.todayAmount.ToString();
                foreach (var item in dailyDataList)
                {
                    s += item + ";";
                }
                
            }
            else
            {
                s = App.todayAmount.ToString() + ";";
            }
            await FileIO.WriteTextAsync(fileDay, s);
        }
        private async void writethisMonthData(int i)
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");

            string[] deli = { "\n" };
            //string[] deli1 = { ":" };
            if (App.monthEqual)
            {
                i += App.thisMonthAmount;
            }
            else
            {
                App.monthEqual = true;
            }

            App.thisMonthAmount = i;
            var file = await folder.CreateFileAsync("thisMonth.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, DateTime.Today.Date.Month + "\n" + App.thisMonthAmount);
            var fileMonth = await folder.CreateFileAsync("MonthWiseData.txt", CreationCollisionOption.OpenIfExists);
            var MonthContent = await FileIO.ReadTextAsync(fileMonth);
            string[] deliSemi = { ";" };
            var MonthlyDataList = MonthContent.Split(deliSemi, StringSplitOptions.RemoveEmptyEntries);
            string s = "";
            if (MonthlyDataList.Length != 0)
            {
                MonthlyDataList[MonthlyDataList.Length - 1] = App.thisMonthAmount.ToString();
                foreach (var item in MonthlyDataList)
                {
                    s += item + ";";
                }
            }
            else
            {
                s = App.thisMonthAmount.ToString() + ";";
            }
            var file2 = await folder.CreateFileAsync("MonthWiseData.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(fileMonth, s);
        }

        private async void writethisWeekData(int i)
        {
            Object index = DateTime.Now;
            int Week_num = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(Convert.ToDateTime(index), System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            string[] deli = { "\n" };
            //string[] deli1 = { ":" };
            if (App.weekEqual)
            {
                i += App.thisWeekAmount;
            }
            else
            {
                App.weekEqual = true;
            }
            App.thisWeekAmount = i;
            var file = await folder.CreateFileAsync("thisWeek.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, Week_num + "\n" + App.thisWeekAmount);
            var fileWeek = await folder.CreateFileAsync("WeekWiseData.txt", CreationCollisionOption.OpenIfExists);
            var WeekContent = await FileIO.ReadTextAsync(fileWeek);
            string[] deliSemi = { ";" };
            var WeeklyDataList = WeekContent.Split(deliSemi, StringSplitOptions.RemoveEmptyEntries);
            string s = "";
            if (WeeklyDataList.Length != 0)
            {
                WeeklyDataList[WeeklyDataList.Length - 1] = App.thisWeekAmount.ToString();
                
                foreach (var item in WeeklyDataList)
                {
                    s += item + ";";
                }
            }
            else
            {
                s = App.thisWeekAmount.ToString() + ";";
            }
            var file2 = await folder.CreateFileAsync("WeekWiseData.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(fileWeek, s);
        }

        public async void addToPieData(string head,string amount)
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("categoryData.txt", CreationCollisionOption.OpenIfExists);
            var s = await FileIO.ReadTextAsync(file);
            string[] deli = { "\n" };
            string[] deli1 = { ";" };
            string[] lines = s.Split(deli,StringSplitOptions.RemoveEmptyEntries);
            int i = 0; 
            while(i < lines.Length)
            {
                if(lines[i].Contains(head))
                {
                    var a = lines[i].Split(deli1,StringSplitOptions.RemoveEmptyEntries);
                    int x = int.Parse(a[1]) + int.Parse(amount);
                    lines[i] = head + ";" + x;
                    break;
                }
                i++;
            }
            
            var file1 = await folder.CreateFileAsync("categoryData.txt", CreationCollisionOption.ReplaceExisting);
            string myLines = "";
            if (i!=0 && i == lines.Length)
            {
                myLines = (s + "\n" + head + ";" + amount);
            }
            else if (i != 0)
            {
                foreach (var item in lines)
                {
                    myLines += item;
                    if (item != lines[lines.Length - 1]) myLines +=  "\n";
                }
            }
            else
            {
                myLines = head + ";" + amount+"\n";
            }
            await FileIO.WriteTextAsync(file1, myLines);
        }

        private async void writeDetails(string type,string details,int amt)
        {
            string[] deli = { "\n" };
            string empt = "";
            var intel1 = App.t.Split(deli,StringSplitOptions.RemoveEmptyEntries);
            if (details != "") intel1[intel1.Length - 1] = App.detailsData + type + " : " + details + " : " + App.symbol + amt + ";";
            else intel1[intel1.Length - 1] = App.detailsData + type + " : " + App.symbol + amt + ";";
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("details.txt", CreationCollisionOption.ReplaceExisting);
            foreach (var item in intel1)
            {
                empt += item + "\n";
            }
            App.t = empt;
            if(details!="") App.detailsData += type + " : " + details + " : " + App.symbol + amt + ";";
            else App.detailsData += type + " : " + App.symbol + amt + ";";
            await FileIO.WriteTextAsync(file,App.t);
        }

        private void categoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoryList.BorderBrush = new SolidColorBrush(Colors.Green);
        }
    }
}
