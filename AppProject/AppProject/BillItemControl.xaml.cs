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
    /// Interaction logic for BillItem.xaml
    /// </summary>
    public partial class BillItemControl : UserControl
    {
        public BillControl owningBill { get; set; }
        public event EventHandler<BICEventArgs> Removed;
        //public event EventHandler<EventArgs> Dragged;
        //public event EventHandler<EventArgs> Released;
        public Boolean MovingEnabled { get; set; }
        public double itemPrice {get; set;}

        public Boolean itemSent = false;
        public string itemName { get; set; }

        //public FoodItem item { get; set; }
        public FoodItem originalItem { get; set; }


        //Default Constructor for creating the first instance of an billItemControl in a FoodItem
        public BillItemControl(FoodItem sourceItem)
        {
            originalItem = sourceItem;
            InitializeComponent();

            itemPrice = sourceItem.totalValue;
            string price = String.Format("{0:0.00}", itemPrice);
            this.ItemPrice.Text = price;

            itemName = sourceItem.name;
            this.ItemName.Text = itemName;

            MovingEnabled = false;
        }



        //Constructor used to allow different prices to be set.

        public BillItemControl(FoodItem sourceItem, double splitPrice)
        {
            originalItem = sourceItem;

            itemPrice = splitPrice;

            string price = String.Format("{0:0.00}", itemPrice);
            this.ItemPrice.Text = price;

            ItemName.Text = sourceItem.name;
        }

        public void Moved()
        {
            this.Removed.Invoke(this, new BICEventArgs() { bic = this });
        }
        
        public void ToggleCheckBoxVisibility()
        {
            if (this.ItemCheckBox.Visibility == Visibility.Visible)
            {
                this.ItemCheckBox.Visibility = Visibility.Hidden;
            } else
            {
                this.ItemCheckBox.Visibility = Visibility.Visible;
            }
        }

        public void ToggleMovingEnabled()
        {
            if (MovingEnabled)
            {
                MovingEnabled = false;
            } else
            { 
                MovingEnabled = true;
            }
        }

        public void ToggleCancelButtonVisibility()
        {;
            if (this.CancelButton.Visibility == Visibility.Visible)
            {
                this.CancelButton.Visibility = Visibility.Hidden;
            } else
            {
                if (!itemSent)
                {
                    this.CancelButton.Visibility = Visibility.Visible;
                }
            }
        }

        public void SendItem()
        {
            if (itemSent)
            {

            } else
            {
                itemSent = true;
                this.CancelButton.Visibility = Visibility.Hidden;
                this.BackgroundColor.Fill = Brushes.Black;
            }
        }

        public void ChangePrice(double newPrice)
        {
            itemPrice = newPrice;
            string price = String.Format("{0:0.00}", itemPrice);
            this.ItemPrice.Text = price;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MovingEnabled)
            {
                //this.Dragged.Invoke(this, new EventArgs());

                BillItemControl item = sender as BillItemControl;
                DragDrop.DoDragDrop(item, item, DragDropEffects.Move);

                //this.Released.Invoke(this, new EventArgs());
                /*
                 * Issue -- Allows items to be moved around from their own bill
                 * The Issue with above code is that when item is moved from one bill to another the starting bill will unsubscribe to this items events
                 * and the second bill will subscribe to item's events thereby causing both bills to have their droppability disabled.
                 */
            } else
            {
                //Do nothing
            }
           
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!itemSent)
            {
                this.Removed.Invoke(this, new BICEventArgs() { bic = this });
            }
        }
    }
}
