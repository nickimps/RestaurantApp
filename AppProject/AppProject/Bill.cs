using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppProject
{
    public class Bill
    {
        private List<FoodItem> billItems;
        private Boolean transactionCompleted { get; set; }
        public string billName { get; set; }
        public BillControl billView;
        private double total=0;

        public Bill(string identity)
        {
            billName = identity;
            billItems = new List<FoodItem>();
            transactionCompleted = false;
            CreateView();
        }

        public BillControl GetView()
        {
            return billView;
        }

        public void CreateView()
        {
            billView = new BillControl(this);
        }

        public void AddItem(FoodItem item) 
        {
            billItems.Add(item);
            billView.AddItem(item.billItemView);

            total += item.value;
            billView.TotalNumber.Text = total.ToString();
        }

        public void ToggleCheckBox()
        {
            if (billView.BillCheckBox.Visibility == Visibility.Visible)
            {
                billView.BillCheckBox.Visibility = Visibility.Hidden;
            }
            else
            {
                billView.BillCheckBox.Visibility = Visibility.Visible;
            }
        }

        public double ReturnTotal()
        {
            return total;
        }

        public void RemoveItem(int index)
        {
            total -= billItems[index].value;
            billItems.RemoveAt(index);
        }

        public void RemoveItem(FoodItem item)
        {
            total -= item.value;
            billItems.Remove(item);
        }
    }
}
