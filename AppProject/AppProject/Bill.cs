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
        public Menu_BillControl mBillView;
        private double total = 0;

        public Bill(string identity)
        {
            billName = identity;
            billItems = new List<FoodItem>();
            transactionCompleted = false;
            CreateView();
            CreateMenuView();
        }

        public BillControl GetView()
        {
            return billView;
        }

        public void CreateView()
        {
            billView = new BillControl(this);
        }

        public void CreateMenuView()
        {
            mBillView = new Menu_BillControl(this);
        }

        public void AddItem(FoodItem item)
        {
            billItems.Add(item);
            billView.AddItem(item.billItemView);

            total += item.value;

            UpdateTotalsInViews();
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

        public void RemoveItem(int index)
        {
            total -= billItems[index].value;
            billItems.RemoveAt(index);
            UpdateTotalsInViews();
        }

        public void RemoveItem(FoodItem item)
        {
            total -= item.value;
            billItems.Remove(item);
            UpdateTotalsInViews();
        }

        public void UpdateTotalsInViews()
        {
            mBillView.TotalText.Text = total.ToString();
            billView.TotalNumber.Text = total.ToString();
        }

        public void UpdateIdentifiersInViews()
        {
            billView.BillIdentifier.Text = billName;
            mBillView.IdentifierText.Text = billName;
        }

        public double ReturnTotal()
        {
            return total;
        }
    }
}
