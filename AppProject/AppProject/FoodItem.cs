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

        public FoodItem(string itemName, double itemValue)
        {
            name = itemName;
            value = itemValue;
            description = "";
            additionalInfo = "";
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
