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

        public Bill billLogic { get; set; }

        public BillControl(Bill bill)
        {
            InitializeComponent();
            this.BillIdentifier.Text = bill.billName;
            this.TotalNumber.Text = bill.ReturnTotal().ToString();
            billLogic = bill;
        }

        public void AddItem(BillItemControl item)
        {
            this.ItemListGrid.Children.Add(item);
            item.billControl = this;
            //Subscribing to BillItemControl events -- Used for move drag and drop
            item.Removed += new EventHandler<BICEventArgs>(RemoveItem);
            item.Clicked += new EventHandler<BICEventArgs>(SplitSelection);
            item.Deleted += new EventHandler<BICEventArgs>(DeleteItem);
            //item.Released += new EventHandler<EventArgs>(ToggleDroppability);
            // item.Dragged += new EventHandler<EventArgs>(ToggleDroppability);
        }

        public void ItemListChanged()
        {
            this.BillItemListChange.Invoke(this, new EventArgs());
        }

        public void ToggleSplitMode()
        {
            foreach (BillItemControl bic in this.ItemListGrid.Children)
            {
                bic.ToggleSplitEnabled();
            }
        }

        public void SendItems()
        {
            foreach (BillItemControl bic in this.ItemListGrid.Children)
            {
                bic.SendItem();
            }
        }

        public void ToggleItemCheckBoxes()
        {
            foreach (BillItemControl bic in this.ItemListGrid.Children)
            {
                bic.ToggleCheckBoxVisibility();
            }
        }

        public void ToggleItemDraggability()
        {
            foreach (BillItemControl bic in this.ItemListGrid.Children)
            {
                bic.ToggleMovingEnabled();
            }
        }

        public void ToggleItemDeletability()
        {
            foreach (BillItemControl bic in this.ItemListGrid.Children)
            {
                bic.ToggleCancelButtonVisibility();
            }
        }

        private void ToggleDroppability(object sender, EventArgs e)
        {
            if (this.ItemListGrid.AllowDrop)
            {
                this.ItemListGrid.AllowDrop = false;
            } else
            {
                this.ItemListGrid.AllowDrop = true;
            }

            Console.WriteLine("Droppability = " + this.ItemListGrid.AllowDrop);
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
            //e.item.billItemView.Released -= new EventHandler<EventArgs>(ToggleDroppability);
            //e.item.billItemView.Dragged -= new EventHandler<EventArgs>(ToggleDroppability);
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
    }
}
