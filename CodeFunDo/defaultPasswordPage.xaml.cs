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
    public sealed partial class defaultPasswordPage : Page
    {
        public defaultPasswordPage()
        {
            this.InitializeComponent();
            fullScreen();
        }
        private async void fullScreen()
        {
            await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(getPassword.Password!="" && getPassword.Password == confirmPaswrd.Password)
            {
                App.currentPass = getPassword.Password;
                await writePass("true\n" + getPassword.Password);
                App.signInOrNot = "signIn";
                this.Frame.Navigate(typeof(MainPage));
            }
        }
        private async Task writePass(string s)
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("password.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file,s);
        }
    }
}
