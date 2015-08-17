using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
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
    public sealed partial class listDetailsPage : Page
    {
        public listDetailsPage()
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
            //textBlocksCreator(20, "17-01-2014", "Yuvraj's b'day : $100");
            displayer();
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
                this.Frame.Navigate(typeof(MainPage),"");
                e.Handled = true;
            }
        }


        private void textBlocksCreator(int upMargin,string date,string data)
        {
            TextBlock headTextBlock = new TextBlock();
            headTextBlock.Text = date;
            headTextBlock.FontSize = 25;
            headTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            headTextBlock.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            headTextBlock.FontWeight = FontWeights.Bold;
            Thickness margin = headTextBlock.Margin;
            margin.Left = 20;
            margin.Top = upMargin;
            margin.Right = 10;
            headTextBlock.Margin = margin;
            myGrid.Children.Add(headTextBlock);


            Grid gridNew = new Grid();
            gridNew.Background = new SolidColorBrush(Colors.LightGreen);
            gridNew.MaxHeight = 120;
            gridNew.Height = Double.NaN;
            Thickness margin2 = gridNew.Margin;
            margin2.Left = 40;
            margin2.Top = upMargin + 40;
            margin2.Right = 10;
            gridNew.Margin = margin2;
            gridNew.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;

            ScrollViewer myViewer = new ScrollViewer();

            TextBlock contentTextBlock = new TextBlock();
            contentTextBlock.Text = date;
            contentTextBlock.Height = Double.NaN;
            contentTextBlock.FontSize = 20;
            contentTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            contentTextBlock.TextWrapping = TextWrapping.Wrap;
            contentTextBlock.Text = data;
            contentTextBlock.Width = 360;
            contentTextBlock.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            contentTextBlock.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            myViewer.Content = contentTextBlock;
            gridNew.Children.Add(myViewer);
            myGrid.Children.Add(gridNew);
        }

        private async void displayer()
        {
            int margin = 20;
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var fileDetails = await folder.CreateFileAsync("details.txt", CreationCollisionOption.OpenIfExists);
            var DetailsContent = await FileIO.ReadTextAsync(fileDetails);
            string[] deliSemi = { ";" };
            string[] deli1 = { "\n" };
            var lines = DetailsContent.Split(deli1, StringSplitOptions.RemoveEmptyEntries);
            int n = lines.Length-1;
            for (int i = n; i >= 0; i--)
            {
                string temp = lines[i];
                var line = temp.Split(deliSemi, StringSplitOptions.RemoveEmptyEntries);
                string dates = line[0];
                string s = "";
                int len = line.Length;
                for (int j = 1; j < len; j++)
                {
                    s += line[j] + "\n";
                }
                textBlocksCreator(margin,dates,s);
                margin += 160;
            }
        }

    }
}
