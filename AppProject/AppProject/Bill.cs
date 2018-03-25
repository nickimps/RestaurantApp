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
        public Menu_BillControl m_BillView;
        private int total = 0;

        public event EventHandler<EventArgs> MenuBillClicked;

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

        private void CreateView()
        {
            billView = new BillControl(this);
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

        private void UpdateTotalsInViews()
        {
            m_BillView.TotalText.Text = total.ToString();
            billView.TotalNumber.Text = total.ToString();
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
