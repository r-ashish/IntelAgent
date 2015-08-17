using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace IntelAgent
{
    public sealed partial class passwordPage : Page
    {
        public passwordPage()
        {
            this.InitializeComponent();
            fullScreen();
        }
        private int wrongCount = 0;
        private async void fullScreen()
        {
            await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
           // WinRTXamlToolkit.Controls.DataVisualization.Charting.
        }
        int x = 7;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(int.TryParse(e.Parameter.ToString(),out x) == true)
            {
                wrongCount = int.Parse(e.Parameter.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            show.Stop();
            if(passBox.Password == App.currentPass)
            {
                App.signInOrNot = "signIn";
                this.Frame.Navigate(typeof(MainPage));
            }
            else if (passBox.Password != "")
            {
                wrongCount += 1;
                passBox.Password = "";
                wrongPopup.IsOpen = true;
                if(wrongCount <= 3) show.Begin();
                else
                {
                    wrongPopup.Opacity = 1;
                    wrongPopup.IsOpen = true;
                    wrongText.Text = "Looks like you don't remember the password.";
                    forgot.Text = "Forgot Password?";
                }
            }
        }

        private void show_Completed(object sender, object e)
        {
            hide.Begin();
        }

        private void hide_Completed(object sender, object e)
        {
            wrongPopup.IsOpen = false;
        }

        private void forgot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ResetPasswordPage));
        }

        private void passBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.Equals(Windows.System.VirtualKey.Enter))
            {
                  goButton.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }
    }
}
