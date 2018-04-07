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
        public BillSelectionControl s_BillView;
        private double total = 0;

        public event EventHandler<EventArgs> MenuBillClicked;

        public Bill(string identity)
        {
            billName = identity;
            transactionCompleted = false;
            CreateView();
            CreateMenuView();
            CreateSelectionView();
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
        
        private void CreateSelectionView()
        {
            s_BillView = new BillSelectionControl(this);
            
        }

        //This function returns an BillItemControl that was found to be in the item specified
        //Used to identify if a BillItemControl from an ordered FoodItem is in a BillControl already.
        //Returns null if not found else will return the respective BillItemControl
        public BillItemControl GetRespectiveItem(FoodItem item)
        {
            foreach (BillItemControl bic in this.billView.ItemListGrid.Children)
            {
                if (bic.originalItem.Equals(item))
                {
                    return bic;
                }
            }
            return null;
        }

        public void AddNewItem(FoodItem item)
        {
            billView.AddItem(item.viewList[0]);

            UpdateTotalsInViews();
        }

        public void AddExistingItem(BillItemControl bic)
        {
            billView.AddItem(bic);
            UpdateTotalsInViews();

        }

        public double ReturnTotal()
        {
            return total;
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

        private void MenuBillClickedEvent(object sender, RoutedEventArgs e)
        {
            this.MenuBillClicked.Invoke(this, new EventArgs());
        }

        private void RecalculateTotal(object sender, EventArgs e)
        {
            UpdateTotalsInViews();
        }

       
    }
}
