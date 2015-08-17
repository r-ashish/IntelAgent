using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace IntelAgent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class changePage : Page
    {
        public changePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
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
            this.Frame.Navigate(typeof(settings));
            e.Handled = true;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (newPaswrd.Password == confirmPaswrd.Password && getPassword.Password == App.currentPass)
            {
                var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                var file = await folder.CreateFileAsync("password.txt", CreationCollisionOption.OpenIfExists);
                var x = await FileIO.ReadTextAsync(file);
                string[] deliSemi = { "\n" };
                var data = x.Split(deliSemi, StringSplitOptions.RemoveEmptyEntries);
                App.currentPass = newPaswrd.Password;
                await FileIO.WriteTextAsync(file,data[0] + "\n" + newPaswrd.Password);
                this.Frame.Navigate(typeof(settings));
            }
            else
            {
                newPaswrd.Password = "";
                confirmPaswrd.Password = "";
                getPassword.Password = "";
            }
        }
    }
}
