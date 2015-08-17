using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace IntelAgent
{
    class DataBinding
    {
        public class myV : BindableBase
        {

            private ObservableCollection<string> _myVector;
            private List<string> _myLegend;
            private List<double> _items;
            private string _name;
            public ObservableCollection<string> myCollection
            {
                get { return this._myVector; }
                set { this.SetProperty(ref this._myVector, value); }
            }
            public List<string> myLegends
            {
                get { return this._myLegend; }
                set { this.SetProperty(ref this._myLegend, value); }
            }
            public List<double> items
            {
                get { return this._items; }
                set { this.SetProperty(ref this._items, value); }
            }
            public string name
            {
                get { return this._name; }
                set { this.SetProperty(ref this._name, value); }
            }
        }
        public sealed class getV
        {
            public static myV getVector(LinkedList<string> myStringArray)
            {
                myV tempV = new myV();
                List<string> list = (from i in myStringArray select i).ToList();
                tempV.myCollection = new ObservableCollection<string>(list);
                return tempV;
            }
            public static myV getLegend(List<double> itemsList,List<string> legendList,string n)
            {
                myV tempV = new myV();
                tempV.myLegends = legendList;
                tempV.name = n;
                tempV.items = itemsList;
                return tempV;
            }
        }
    }
}
