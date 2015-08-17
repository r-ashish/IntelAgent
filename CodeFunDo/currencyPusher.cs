using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace IntelAgent
{
    class currencyPusher
    {
        static string[] myArray;
        public async static Task getList()
        {
            await readCountries();
            foreach (var item in myArray)
	        {
                settings.countriesList.AddLast(item.Trim()); 
	        }
        }
        public static async Task readCountries()
        {
            var folder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Data");
            var file = await folder.CreateFileAsync("countries.txt", CreationCollisionOption.OpenIfExists);
            var x = await FileIO.ReadTextAsync(file);
            string[] deli = {"\n"} ;
            myArray = x.Split(deli, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
