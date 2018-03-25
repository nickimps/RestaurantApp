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

        public Bill billLogic { get; set; }

        public BillControl(Bill bill)
        {
            InitializeComponent();
            this.BillIdentifier.Text = bill.billName;
            this.TotalNumber.Text = bill.ReturnTotal().ToString();
            billLogic = bill;
        }

        public BillControl(BillControl clone)
        {
            this.BillIdentifier = clone.BillIdentifier;
            this.BillInfoGrid = clone.BillInfoGrid;
            this.TotalNumber = clone.TotalNumber;
        }

        public void AddItem(BillItemControl item)
        {
            this.ItemListGrid.Children.Add(item);

            //Subscribing to BillItemControl events -- Used for move drag and drop
            item.Removed += new EventHandler<ItemEventArgs>(RemoveItem);
            item.Released += new EventHandler<EventArgs>(ToggleDroppability);
            item.Dragged += new EventHandler<EventArgs>(ToggleDroppability);
        }

        public void ToggleItemCheckBoxes()
        {
            foreach (BillItemControl bc in this.ItemListGrid.Children)
            {
                bc.ToggleCheckBoxVisibility();
            }
        }

        public void ToggleItemDraggability()
        {
            foreach (BillItemControl bic in this.ItemListGrid.Children)
            {
                bic.ToggleMovingEnabled();
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
            itemC.Moved();
            billLogic.AddItem(itemC.item);
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

        private void RemoveItem(object sender, ItemEventArgs e)
        {
            //Unsubscribing from BillItemControl(BIC) Events for the BIC removed
            e.item.billItemView.Removed -= new EventHandler<ItemEventArgs>(RemoveItem);
            //e.item.billItemView.Released -= new EventHandler<EventArgs>(ToggleDroppability);
            //e.item.billItemView.Dragged -= new EventHandler<EventArgs>(ToggleDroppability);
            billLogic.RemoveItem(e.item);
            this.ItemListGrid.Children.Remove(e.item.billItemView);
        }
    }
}
