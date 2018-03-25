using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProject
{
    public class FoodItem
    {
        public string name { get; set; }
        public double value { get; set; }
        public string description { get; set; }
        public string additionalInfo { get; set; }
        public BillItemControl billItemView;

        public FoodItem(string itemName, string itemValue)
        {
            name = itemName;
            value = convertPriceToDouble(itemValue);
            description = "";
            additionalInfo = "";
        }

        public double convertPriceToDouble(string itemValue)
        {
            string price = itemValue.Substring(1);
            double x = Convert.ToDouble(price);
            return x;
        }
        
        public FoodItem(FoodItem clone)
        {
            name = clone.name;
            value = clone.value;
            description = clone.description;
            additionalInfo = clone.additionalInfo;
            CreateView();
        }

        public BillItemControl GetView()
        {
            return billItemView;
        }

        public void CreateView()
        {
            billItemView = new BillItemControl(this);
        }
    }
}
