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
        //private List<FoodItem> billItems;
        private Boolean transactionCompleted { get; set; }
        public string billName { get; set; }
        public BillControl billView;
        public Menu_BillControl m_BillView;
        private double total = 0;

        public event EventHandler<EventArgs> MenuBillClicked;

        public Bill(string identity)
        {
            billName = identity;
            transactionCompleted = false;
            CreateView();
            CreateMenuView();
        }

        public BillControl GetView()
        {
            return billView;
        }

        private void CreateView()
        {
            billView = new BillControl(this);
            billView.BillItemListChange += new EventHandler<EventArgs>(RecalculateTotal);
        }

        private void CreateMenuView()
        {
            m_BillView = new Menu_BillControl(this);
            m_BillView.InteractionButton.Click += new RoutedEventHandler(MenuBillClickedEvent);
        }

        private void MenuBillClickedEvent(object sender, RoutedEventArgs e)
        {
            this.MenuBillClicked.Invoke(this, new EventArgs());
        }

        private void RecalculateTotal(object sender, EventArgs e)
        {
            UpdateTotalsInViews();
        }

        public void AddNewItem(FoodItem item)
        { 
            billView.AddItem(item.billItemView);

            UpdateTotalsInViews();
        }
        public void AddExistingItem(BillItemControl bic)
        {
            billView.AddItem(bic);
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

        public void ToggleItemDragging()
        {
            billView.ToggleItemDraggability();
        }
        /*
        public void RemoveItem(int index)
        {
            total -= billItems[index].totalValue;
            billItems.RemoveAt(index);
            UpdateTotalsInViews();
        }

        public void RemoveItem(FoodItem item)
        {
            total -= item.totalValue;
            billItems.Remove(item);
            UpdateTotalsInViews();
        }
        */
        private void UpdateTotalsInViews()
        {
            double newTotal = 0;

            foreach (BillItemControl bic in billView.ItemListGrid.Children)
            {
                newTotal += bic.itemPrice;
            }

            string price = String.Format("{0:0.00}", newTotal);
            m_BillView.TotalText.Text = price;
            billView.TotalNumber.Text = price;
        }

        private void UpdateIdentifiersInViews()
        {
            billView.BillIdentifier.Text = billName;
            m_BillView.IdentifierText.Text = billName;
        }

        public double ReturnTotal()
        {
            return total;
        }
    }
}
