using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProject
{
    public class FoodItem
    {
        public string name;
        public double value;
        public string descripition;
        public FoodItem(string itemName, double itemValue)
        {
            name = itemName;
            value = itemValue;
        }
    }
}
