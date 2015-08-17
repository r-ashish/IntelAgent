using System;
using System.Collections.Generic;
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

namespace IntelAgent
{
    public sealed partial class setPassword : Page
    {
        public setPassword()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
                this.Frame.Navigate(typeof(settings));
                e.Handled = true;
            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (Password.Password != "" && Password.Password == Confirm.Password)
            //{
            //    await writePass(Password.Password);
            //    App.currentPass = Password.Password;
            //    this.Frame.Navigate(typeof(settings));
            //}
            if (Password.Password == App.currentPass)
            {
                App.askOrNot = true;
                await writePass("true\n" + App.currentPass);
                this.Frame.Navigate(typeof(settings));
            }

        }
        private async Task writePass(string s)
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("password.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, s);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(settings));
        }
    }
}
