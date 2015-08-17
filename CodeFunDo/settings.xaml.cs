using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
//using Microsoft.Live;
//using Microsoft.Live.Controls;
namespace IntelAgent
{
    public sealed partial class settings : Page
    {

        public settings()
        {
            this.InitializeComponent();
        }
        string selectedCountry;
        int index;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await readCountry();
            string[] deli = { "\n" };
            string[] array = selectedCountry.Split(deli,StringSplitOptions.RemoveEmptyEntries);
            currencySelect.Content = array[0].Trim();
            index = int.Parse(array[1].Trim());
            if (App.askOrNot == true)
            {
                toggle.IsOn = true;
            }
            else
            {
                toggle.IsOn = false;
                changePass.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            toggle.Toggled += ToggleSwitch_Toggled;
            //try
            //{
            //    toggle.Tapped += ToggleSwitch_Toggled;
            //}
            //catch { }
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += Hardwarebuttons_BackPressed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= Hardwarebuttons_BackPressed;
        }
        private void Hardwarebuttons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (this.Frame == null) return;
            this.Frame.Navigate(typeof(MainPage),"");
            e.Handled = true;
        }
        public static LinkedList<string> countriesList;
        bool loadData = true;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (loadData == true)
            {
                countriesList = new LinkedList<string>();
                await currencyPusher.getList();
                this.DataContext = DataBinding.getV.getVector(countriesList);
                myPicker.SelectedIndex = index;
                loadData = false;
            }
        }
        string[] currencyList;
        private async Task loadCurrencies()
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("currencies.txt", CreationCollisionOption.OpenIfExists);
            var temp = await FileIO.ReadTextAsync(file);
            string[] deli = {"\n"};
            currencyList = temp.Split(deli,StringSplitOptions.RemoveEmptyEntries);
        }
        private async void ListPickerFlyout_ItemsPicked(ListPickerFlyout sender, ItemsPickedEventArgs args)
        {
            await loadCurrencies();
            var str = countriesList.ElementAt(sender.SelectedIndex);
            currencySelect.Content = str;
            App.symbol = currencyList.ElementAt(sender.SelectedIndex).Trim();
            await writeCountry(str + "\n" + sender.SelectedIndex + "\n" + App.symbol);
        }
        private async Task writeCountry(string s)
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("country.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file,s);
        }
        private async Task readCountry()
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("country.txt", CreationCollisionOption.OpenIfExists);
            selectedCountry = await FileIO.ReadTextAsync(file);
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var obj = sender as ToggleSwitch;
            if(obj.IsOn == true)
            {
                this.Frame.Navigate(typeof(setPassword));
            }
            else
            {
                this.Frame.Navigate(typeof(confirmPass));
            }
        }

        private void changePass_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(changePage));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            restFly.Hide();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(passCheck.Text == App.currentPass)
            {
                var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                var file = await folder.CreateFileAsync("Balance.txt", CreationCollisionOption.ReplaceExisting);
                var file1 = await folder.CreateFileAsync("categoryData.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file,"0");
                await FileIO.WriteTextAsync(file1, "");
                var file2 = await folder.CreateFileAsync("country.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file2, "--Choose a currency--" + "\n" + "0" + "\n" + "₹");
                var file3 = await folder.CreateFileAsync("dayWiseData.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file3, "");
                var file4 = await folder.CreateFileAsync("details.txt", CreationCollisionOption.ReplaceExisting);
                var file5 = await folder.CreateFileAsync("MonthWiseData.txt", CreationCollisionOption.ReplaceExisting);
                var file6 = await folder.CreateFileAsync("thisMonth.txt", CreationCollisionOption.ReplaceExisting);
                var file7 = await folder.CreateFileAsync("today.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file4,"");
                await FileIO.WriteTextAsync(file5, "");
                await FileIO.WriteTextAsync(file6, "");
                await FileIO.WriteTextAsync(file7, "");
                var file8 = await folder.CreateFileAsync("thisWeek.txt", CreationCollisionOption.ReplaceExisting);
                var file9 = await folder.CreateFileAsync("WeekWiseData.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file8, "");
                await FileIO.WriteTextAsync(file9, "");
                var file10 = await folder.CreateFileAsync("password.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file10, "true\n");

                Application.Current.Exit();
            }

        }

        private void passCheck_GotFocus(object sender, RoutedEventArgs e)
        {
            if (passCheck.Text == "Enter password to continue") passCheck.Text = "";
        }

        private void passCheck_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passCheck.Text == "") passCheck.Text = "Enter password to continue";
        }

        private void toggle_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        
    }
}
