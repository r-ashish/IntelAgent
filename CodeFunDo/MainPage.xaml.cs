using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Live;

namespace IntelAgent
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            fullScreen();
        }
        private async void fullScreen()
        {
            await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
        }
        public static int Balance;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.signInOrNot == "signIn")
            {
                signIn();
                App.signInOrNot = "no";
            }
            if(loadingPopup.IsOpen!=true) cB.Visibility = Windows.UI.Xaml.Visibility.Visible;
            await readBalance();
            balanceBlock.Text = App.symbol + " " + Balance;
            updateTile();
        }

        private void updateTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150ImageAndText02);
            XmlNodeList tileTextAttributes = tileXml.GetElementsByTagName("text");
            tileTextAttributes[0].InnerText = "";
            //var tileText = tileXml.GetElementsByTagName("text")[0] as XmlElement;
            //tileText.SetAttribute("text","Current balance is : " + Balance);
            var tileImage = tileXml.GetElementsByTagName("image")[0] as XmlElement;
            tileImage.SetAttribute("src", "ms-appx:///Assets/WideLogo.scale-240.png");
            var tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

            tileImage.SetAttribute("src", "ms-appx:///Assets/rupee (2).jpg");
            tileTextAttributes[0].InnerText = "Current balance is : " + App.symbol + " " + Balance;
            tileNotification = new TileNotification(tileXml);
            tileNotification.Tag = "myTag";
            //tileNotification.ExpirationTime = DateTimeOffset.UtcNow.AddSeconds(Int64.MaxValue);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

            var tileXml1 = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150PeekImageAndText04);
            var tileImage1 = tileXml1.GetElementsByTagName("image")[0] as XmlElement;
            tileImage1.SetAttribute("src", "ms-appx:///Assets/Square71x71Logo.scale-240.png");
            //tileImage1.SetAttribute("src", "ms-appx:///Assets/rupee (2).jpg");
            var tileNotification1 = new TileNotification(tileXml1);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification1);
            XmlNodeList tileTextAttributes1 = tileXml1.GetElementsByTagName("text");
            tileTextAttributes1[0].InnerText = "Current balance is : "+"\n" + App.symbol + " " + Balance;
            //tileTextAttributes1[1].InnerText = "Current balance is : " + "\n" + App.symbol + " " + Balance;
            tileNotification1 = new TileNotification(tileXml1);
            tileNotification1.Tag = "myTag";
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification1);

            //ShellTile.ActiveTiles.First().Update(
            //    new FlipTileData()
            //    {
            //        WideBackContent = WisdomText,
            //    });
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(detailsPage), "add");
        }

        private void minusButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(detailsPage), "minus");
        }
        public async Task readBalance()
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("Balance.txt",CreationCollisionOption.OpenIfExists);
            var x = await FileIO.ReadTextAsync(file);
            Balance = int.Parse(x);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(settings));
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            infoBlock.Text = "Loading Statistics...";
            mainG.Opacity = 0.2;
            loadingPopup.IsOpen = true;
            cB.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            addButton.IsEnabled = false;
            minusButton.IsEnabled = false;
            show.Begin();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            mainG.Opacity = 1;
            loadingPopup.IsOpen = false;
            addButton.IsEnabled = true;
            minusButton.IsEnabled = true;
        }

        private void show_Completed(object sender, object e)
        {
            this.Frame.Navigate(typeof(stats));
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {    
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(listDetailsPage));
        }

        private void AppBarButton_Click_4(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(aboutPAge));
        }

        private void sync_click(object sender, RoutedEventArgs e)
        {
            signIn();
        }
        bool signedIn = false;
        private async void signIn()
        {
            try
            {
                LiveAuthClient auth = new LiveAuthClient();
                mainG.Opacity = 0.2;
                loadingPopup.IsOpen = true;
                cB.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                addButton.IsEnabled = false;
                minusButton.IsEnabled = false;
                infoBlock.Text = "Signing in....";
                LiveLoginResult loginResult = await auth.LoginAsync(new string[] { "wl.signin", "wl.skydrive" });
                if (loginResult.Status == LiveConnectSessionStatus.Connected)
                {
                    signedIn = true;
                    show1.Begin();
                }
            }
            catch (LiveAuthException exception)
            {
                infoBlock.Text = "Error signing in : " + exception.Message;
                show1.Begin();
            }
            catch (NullReferenceException)
            {
                infoBlock.Text = "Error signing in!";
                show1.Begin();
            }
        }
        private void addToCB()
        {
            AppBarButton e = new AppBarButton();
            e.Label = "signed in";
            cB.SecondaryCommands.Add(e);
        }
        private void show1_Completed(object sender, object e)
        {
            if (signedIn)
            {
                infoBlock.Text = "Signed in.";
                addToCB();
            }
            show2.Begin();
        }

        private void show2_Completed(object sender, object e)
        {
            mainG.Opacity = 1;
            cB.Visibility = Windows.UI.Xaml.Visibility.Visible;
            loadingPopup.IsOpen = false;
            addButton.IsEnabled = true;
            minusButton.IsEnabled = true;
        }
    }
}
