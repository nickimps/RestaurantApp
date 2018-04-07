using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProject
{
    public class FoodItem
    {
        public event EventHandler<ItemEventArgs> Empty;

        public string name { get; set; }
        public double totalValue { get; set; }
        public string description { get; set; }
        public Boolean evenSplit = true;
        public string additionalInfo { get; set; }
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
        public void SplitOrderEvenly(List<Bill> billsAffected, BillItemControl bic)
        {
            int numberOfBills = 1 + billsAffected.Count;
      
            //Calculate the prices of an even split between affected parties
            List<double> prices = calculatePrices(bic.itemPrice, numberOfBills);

            //This loop looks for items inside existing bills or creates new bills
            bic.ChangePrice(prices[0]);
            int index = 1;
            foreach (Bill bill in billsAffected)
            {
                BillItemControl associatedBIC = bill.GetRespectiveItem(this);
                if (associatedBIC == null)
                {
                    //New Bill must create it 
                    //Must retain properties of old bill item
                    associatedBIC = new BillItemControl(this, prices[index]);
                    if (bic.itemSent)
                    {
                        associatedBIC.itemSent = true;
                    }

                    viewList.Add(associatedBIC);
                    associatedBIC.Deleted += new EventHandler<BICEventArgs>(HandleViewDeletion);
                    bill.AddExistingItem(associatedBIC);

                } else
                {
                    //If it is not a new bill then the price is the old price + new price split into it
                    associatedBIC.ChangePrice(prices[index] + associatedBIC.itemPrice);
                }
                index++;
            }

        }

        //Combines two seperate BillItemControls 
        // *** TO BE USED ONLY ON BILLITEMCONTROLS WITHIN THE SAME BILLLOGIC ***
        public void Combine(BillItemControl combined, BillItemControl destroyed)
        {
            combined.ChangePrice(combined.itemPrice + destroyed.itemPrice);
            //Destroy the second item
            destroyed.ChangePrice(0);
            
        }

        //Determines if there are duplicates between the bills viewList children and the list of bills provided
        /*                                                DEPRACTED
        private int DuplicateCount(List<Bill> bills)
        {
            int count = 0;
            foreach (Bill bill in bills)
            {
                Boolean duplicate = false;
                foreach (BillItemControl bic in viewList)
                {
                    if (bic.billControl.billLogic.Equals(bill))
                    {
                        duplicate = true;
                        break;
                    }
                }
                if (!duplicate)
                {
                    count++;
                }
            }
            return count;
        }
        */

        //Method gets split prices
        private List<double> calculatePrices(double price, int bills)
        {
            List<double> prices = new List<double>();
            int tempVal =  (int) (price * 100);
            int remainder = tempVal % bills;
            int splitPrice = tempVal/bills; 
            for (int i=0; i<bills; i++)
            {
                if (i < remainder)
                {
                    prices.Add((double) (splitPrice + 1)/100);
                }
                else
                {
                    prices.Add((double) splitPrice/100);
                }
            }
            return prices;

        }

        public void SplitUnEvenly()
        {
            //Split Unevenly logic goes here
        }

        public void CreateView()
        {
            viewList = new List<BillItemControl>();
            BillItemControl billItemView = new BillItemControl(this);
            viewList.Add(billItemView);
            billItemView.Deleted += new EventHandler<BICEventArgs>(HandleViewDeletion);
        }

        private void HandleViewDeletion(object sender, BICEventArgs e)
        {
            viewList.Remove(e.bic);
            e.bic.Deleted -= new EventHandler<BICEventArgs>(HandleViewDeletion);
            if (viewList.Count == 0)
            { 
                this.Empty.Invoke(this, new ItemEventArgs() {item = this});
            }
           
            //If the price is set to 0 then its been handled by something else so fase to deleted completely
            else if (e.bic.itemPrice == 0)
            {
                //e.bic = null;
            }
            
            //Need to recalculate other bills
            //Mechanism, going to default evenly allocate the price of specific split to other bills.
            //Will not perfectly evenly distribute bills.
            else
            {
                double priceToBeDistributed = e.bic.itemPrice;
                List<double> prices = calculatePrices(priceToBeDistributed, viewList.Count);
                int index = 0;
                foreach (BillItemControl bic in viewList)
                {
                    bic.ChangePrice(bic.itemPrice + prices[index]);
                    index++;
                }
            }
        }
    }
}
