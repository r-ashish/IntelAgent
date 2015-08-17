using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace IntelAgent
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        public static string signInOrNot = "";
        public static string currentPass = "";
        public static bool dateEqual = false;
        public static int todayAmount = 0;
        public static DateTime todayDate = DateTime.Today.Date;
        private async Task readTodayData()
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("today.txt", CreationCollisionOption.OpenIfExists);
            string[] deli = { "\n" };
            string[] deli1 = { ":" };
            var x = await FileIO.ReadTextAsync(file);
            var tempArray = x.Split(deli, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                todayAmount = int.Parse(tempArray[1]);
                var fileDate = tempArray[0].Split(deli1, StringSplitOptions.RemoveEmptyEntries);
                if (int.Parse(fileDate[0]) == todayDate.Day && int.Parse(fileDate[1]) == todayDate.Month)
                {
                    dateEqual = true;
                }
                else
                {
                    todayAmount = 0;
                    var file1 = await folder.CreateFileAsync("dayWiseData.txt", CreationCollisionOption.OpenIfExists);
                    var x1 = await FileIO.ReadTextAsync(file1);
                    await FileIO.WriteTextAsync(file1, x1 + todayAmount + ";");

                    var file2 = await folder.CreateFileAsync("today.txt", CreationCollisionOption.OpenIfExists);
                    await FileIO.WriteTextAsync(file2, todayDate.Day + ":" + todayDate.Month +"\n"+todayAmount);
                }
            }
            catch { }
        }

        public static bool monthEqual = false;
        public static int thisMonthAmount = 0;
        private async Task readthisMonthData()
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("thisMonth.txt", CreationCollisionOption.OpenIfExists);
            string[] deli = { "\n" };
            var x = await FileIO.ReadTextAsync(file);
            var tempArray = x.Split(deli, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                thisMonthAmount = int.Parse(tempArray[1]);
                var fileMonth = int.Parse(tempArray[0]);
                if (fileMonth == todayDate.Month)
                {
                    monthEqual = true;
                }
                else
                {
                    thisMonthAmount = 0;
                    var file1 = await folder.CreateFileAsync("thisMonth.txt", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(file1, todayDate.Month + "/n" + 0);
                    var file2 = await folder.CreateFileAsync("MonthWiseData.txt", CreationCollisionOption.OpenIfExists);
                    var x1 = await FileIO.ReadTextAsync(file2);
                    await FileIO.WriteTextAsync(file2, x1 + thisMonthAmount + ";");
                }
            }
            catch { }
        }

        public static bool weekEqual = false;
        public static int thisWeekAmount = 0;

        private async Task readthisWeekData()
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("thisWeek.txt", CreationCollisionOption.OpenIfExists);
            string[] deli = { "\n" };
            var x = await FileIO.ReadTextAsync(file);
            var tempArray = x.Split(deli, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                Object index = DateTime.Now;
                int Week_num = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(Convert.ToDateTime(index), System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);

                thisWeekAmount = int.Parse(tempArray[1]);
                var fileWeek = int.Parse(tempArray[0]);
                if (fileWeek == Week_num)
                {
                    weekEqual = true;
                }
                else
                {
                    thisWeekAmount = 0;
                    var file1 = await folder.CreateFileAsync("thisWeek.txt", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(file1, Week_num + "/n" + 0);
                    var file2 = await folder.CreateFileAsync("WeekWiseData.txt", CreationCollisionOption.OpenIfExists);
                    var x1 = await FileIO.ReadTextAsync(file2);
                    await FileIO.WriteTextAsync(file2, x1 + thisWeekAmount + ";");
                }
            }
            catch { }
        }

        public static string detailsData = "";
        public static string t = "";
        private async Task readDetails()
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("details.txt", CreationCollisionOption.OpenIfExists);
            string[] deli = { "\n" };
            string[] deli1 = {";"};
            var x = await FileIO.ReadTextAsync(file);
            var tempArray = x.Split(deli, StringSplitOptions.RemoveEmptyEntries);
            if(tempArray.Length!=0 && tempArray[tempArray.Length-1].Split(deli1,StringSplitOptions.RemoveEmptyEntries)[0].Trim() == DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year)
            {
                detailsData = tempArray[tempArray.Length - 1];
                t = x;
            }
            else
            {
                
                if (tempArray.Length != 0)
                detailsData = "\n" + DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year + ";";
                else
                detailsData = DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year + ";";
                t += x+detailsData;
                await FileIO.WriteTextAsync(file,t);
            }
        }

        public static bool askOrNot;
        private async Task readPass()
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.GetFileAsync("password.txt");
            string[] deli = { "\n" };
            var x = await FileIO.ReadTextAsync(file);
            var tempArray = x.Split(deli,StringSplitOptions.RemoveEmptyEntries);
            try { currentPass = tempArray[1]; }
            catch(IndexOutOfRangeException) { }
            askOrNot = bool.Parse(tempArray[0]);
        }

        //private async Task writeCountry(string s)
        //{
        //    var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
        //    var file = await folder.CreateFileAsync("country.txt", CreationCollisionOption.ReplaceExisting);
        //    await FileIO.WriteTextAsync(file, s);
        //}
        public static string symbol;
        private async Task readCountry()
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("country.txt", CreationCollisionOption.OpenIfExists);
            var temp = await FileIO.ReadTextAsync(file);
            string[] deli = { "\n" };
            symbol = temp.Split(deli,StringSplitOptions.RemoveEmptyEntries)[2].Trim();
        }
        //SolidColorBrush cr = new SolidColorBrush(Colors.Green);
        private async Task createFiles()
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("Data");
            var file6 = await folder.CreateFileAsync("password.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file6, "true\n");
            var file = await folder.CreateFileAsync("Balance.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file,"0");
            var file1 = await folder.CreateFileAsync("categoryData.txt", CreationCollisionOption.OpenIfExists);
            var file2 = await folder.CreateFileAsync("countries.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file2, "India-₹\nUK-€\nUS-$");
            var file3 = await folder.CreateFileAsync("country.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file3, "--Choose a currency--\n0\n₹");
            var file4 = await folder.CreateFileAsync("currencies.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file4, "₹\n€\n$");
            var file5 = await folder.CreateFileAsync("MonthWiseData.txt", CreationCollisionOption.OpenIfExists);    
            var file7 = await folder.CreateFileAsync("thisMonth.txt", CreationCollisionOption.OpenIfExists);
            var file8 = await folder.CreateFileAsync("thisWeek.txt", CreationCollisionOption.OpenIfExists);
            var file9 = await folder.CreateFileAsync("today.txt", CreationCollisionOption.OpenIfExists);
            var file10 = await folder.CreateFileAsync("WeekWiseData.txt", CreationCollisionOption.OpenIfExists);
            var file11 = await folder.CreateFileAsync("dayWiseData.txt", CreationCollisionOption.OpenIfExists);
            var file12 = await folder.CreateFileAsync("details.txt", CreationCollisionOption.OpenIfExists);

        }
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                rootFrame.ContentTransitions = new TransitionCollection { new PaneThemeTransition { Edge = EdgeTransitionLocation.Top } };
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                bool excptn = false;
                try
                {
                    var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
                }
                catch
                {
                    excptn = true;
                }
                if(excptn)
                {
                    await createFiles();
                    excptn = false;
                }
                await readPass();
                await readCountry();
                await readTodayData();
                await readthisMonthData();
                await readDetails();
                await readthisWeekData();
                if (currentPass != "")
                {
                    if (askOrNot == false)
                    {
                        signInOrNot = "signIn";
                        if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }
                    else
                    {
                        if (!rootFrame.Navigate(typeof(passwordPage), e.Arguments))
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }
                }
                else
                {
                    if (!rootFrame.Navigate(typeof(defaultPasswordPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
        


        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}