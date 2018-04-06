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
        public double totalValue { get; set; }
        public string description { get; set; }
        private int splitCount = 0;
        public Boolean evenSplit = true;
        public string additionalInfo { get; set; }
        public BillItemControl billItemView;
        public List<BillItemControl> viewList;

        public FoodItem(string itemName, string itemValue)
        {
            name = itemName;
            totalValue = convertPriceToDouble(itemValue);
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
            totalValue = clone.totalValue;
            description = clone.description;
            additionalInfo = clone.additionalInfo;
            CreateView();
        }

        //So far the control is undefined so work on this method later.
        public void SplitOrderEvenly(List<BillControl> billsAffected)
        {
            //Split Logic goes here
            splitCount += billsAffected.Count;
            List<double> prices = calculatePrices();

            //This loop assumes that it is always a new bill upon addition.
            for (int i=0; i<splitCount; i++)
            {
                if (i >= viewList.Count)
                {
                    //Adding it to a the new bill.
                    BillItemControl nBIC = new BillItemControl(this, prices[i]);
                    
                    //Need to add it to billcontrols
                    billsAffected[i - viewList.Count].AddItem(nBIC);

                    //Adding into self array
                    viewList.Add(nBIC);
                } else
                {
                    viewList[i].ChangePrice(prices[i]);
                }
            }

        }

        //Method gets split prices
        private List<double> calculatePrices()
        {
            List<double> prices = new List<double>();
            int tempVal = (int) totalValue * 100;
            int remainder = tempVal % splitCount;
            double splitPrice = (double)tempVal / 100 / splitCount; 
            for (int i=0; i<splitCount; i++)
            {
                if (i < remainder)
                {
                    prices.Add(splitPrice + 0.01);
                }
                else
                {
                    prices.Add(splitPrice);
                }
            }
            return prices;

        }

        public void SplitUnEvenly()
        {
            //Split Unevenly logic goes here
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
