using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppProject
{
    /// <summary>
    /// Interaction logic for Bill.xaml
    /// </summary>
    public partial class BillControl : UserControl
    {
        public event EventHandler<EventArgs> BillItemListChange;
        public event EventHandler<BICEventArgs> SplitRequest;
        public event EventHandler<EventArgs> Deleted;

        public Bill billLogic { get; set; }

        public BillControl(Bill bill)
        {
            InitializeComponent();
            billLogic = bill;
            billLogic.billView = this;
            this.BillIdentifier.Text = bill.billName;
            string price = String.Format("{0:0.00}", bill.ReturnTotal().ToString());
            if (bill.ReturnTotal() == 0)
            {
                this.TotalNumber.Text = "$0.00";
            }
            else
            {
                this.TotalNumber.Text = "$" + price;
            }
            
        }

        public void AddItem(BillItemControl item)
        {
            this.ItemListGrid.Children.Add(item);
            item.billControl = this;
            //Subscribing to BillItemControl events -- Used for move drag and drop
            item.Removed += new EventHandler<BICEventArgs>(RemoveItem);
            item.Clicked += new EventHandler<BICEventArgs>(SplitSelection);
            item.Deleted += new EventHandler<BICEventArgs>(DeleteItem);

        }

        public void ItemListChanged()
        {
            this.BillItemListChange.Invoke(this, new EventArgs());
        }

        public void ToggleSplitMode()
        {

            if (billLogic.transactionCompleted)
            {

            } else
            {
                foreach (BillItemControl bic in this.ItemListGrid.Children)
                {
                    bic.ToggleSplitEnabled();
                }
            }
        }

        public void SendItems()
        {
            if (billLogic.transactionCompleted)
            {

            } else
            {
                foreach (BillItemControl bic in this.ItemListGrid.Children)
                {
                    bic.SendItem();
                }
            }
        }


        public void ToggleBillDeleteButton()
        {
            if (billLogic.transactionCompleted)
            {

            }
            else
            {
                if (this.RemoveBillButton.Visibility == Visibility.Visible)
                {
                    this.RemoveBillButton.Visibility = Visibility.Hidden;
                }
                else
                {
                    Boolean hasSentItem = false;
                    foreach (BillItemControl bic in this.ItemListGrid.Children)
                    {
                        if (bic.itemSent)
                        {
                            hasSentItem = true;
                            break;
                        }
                    }
                    if (!hasSentItem)
                    {
                        this.RemoveBillButton.Visibility = Visibility.Visible;
                    }

                }
            }
        }

        public void ToggleItemDraggability()
        {
            if (billLogic.transactionCompleted)
            {

            }
            else
            {
                foreach (BillItemControl bic in this.ItemListGrid.Children)
                {
                    bic.ToggleMovingEnabled();
                }
            }
        }

        public void ToggleItemDeletability()
        {
            if (billLogic.transactionCompleted)
            {

            }
            else
            {
                foreach (BillItemControl bic in this.ItemListGrid.Children)
                {
                    bic.ToggleCancelButtonVisibility();
                }
            }
        }

        private void ToggleDroppability(object sender, EventArgs e)
        {
            if (billLogic.transactionCompleted)
            {

            }
            else
            {
                if (this.ItemListGrid.AllowDrop)
                {
                    this.ItemListGrid.AllowDrop = false;
                }
                else
                {
                    this.ItemListGrid.AllowDrop = true;
                }
            }
        }

        private void ItemListGrid_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void ItemListGrid_Drop(object sender, DragEventArgs e)
        {
            BillItemControl itemC = (BillItemControl)e.Data.GetData("AppProject.BillItemControl");
            //Original Item is not in this current bill so add without issue

            BillItemControl bic = billLogic.GetRespectiveItem(itemC.originalItem);
            if (bic == null) {
                itemC.Moved();
                billLogic.AddExistingItem(itemC);
            } 
            //Checking if your dragging into the same bill.
            else if (itemC.billControl.Equals(bic.billControl))
            {
                //Nothing needs to be done in this instance
            }
            //Item was found to be in the same bill need to combine them and delete one.
            else
            {
                itemC.originalItem.Combine(bic, itemC);
                itemC.Delete();
            }
            
        }

        private void ItemListGrid_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("AppProject.BillItemControl"))
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void RemoveItem(object sender, BICEventArgs e)
        {
            this.ItemListGrid.Children.Remove(e.bic);
            this.BillItemListChange.Invoke(this, new EventArgs());
            //Unsubscribing from BillItemControl(BIC) Events for the BIC removed
            e.bic.Removed -= new EventHandler<BICEventArgs>(RemoveItem);
            e.bic.Clicked -= new EventHandler<BICEventArgs>(SplitSelection);
            e.bic.Deleted -= new EventHandler<BICEventArgs>(DeleteItem);

        }

        private void DeleteItem(object sender, BICEventArgs e)
        {
            this.ItemListGrid.Children.Remove(e.bic);
            e.bic.Removed -= new EventHandler<BICEventArgs>(RemoveItem);
            e.bic.Clicked -= new EventHandler<BICEventArgs>(SplitSelection);
            e.bic.Deleted -= new EventHandler<BICEventArgs>(DeleteItem);
            this.BillItemListChange.Invoke(this, new EventArgs());
            
        }

        private void SplitSelection(object sender, BICEventArgs e)
        {
            this.SplitRequest.Invoke(this, e);
        }

        private void RemoveBillButton_Click(object sender, RoutedEventArgs e)
        {
            this.Deleted.Invoke(this, new EventArgs());
        }

        /*
         * 
         * HERE IS STUFF
         * 
         * 
         * 
         */
        private void PaidForButton_Click(object sender, RoutedEventArgs e)
        {
            Boolean AllSent = true;

            foreach (BillItemControl bic in this.ItemListGrid.Children)
            {
                if (!bic.itemSent)
                {
                    AllSent = false;
                }
            }

            if (AllSent)
            {
                billLogic.ToggleTransactionStatus();
            } else
            {
               //Effect disable here if u can 
            }
        }

        private void BillIdentifier_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            billLogic.billName = BillIdentifier.Text;
            billLogic.UpdateIdentifiersInViews();

        }
    }
}
