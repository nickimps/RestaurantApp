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
        public event EventHandler<ItemEventArgs> Removed;
        //public event EventHandler<EventArgs> Dragged;
        //public event EventHandler<EventArgs> Released;
        public Boolean MovingEnabled { get; set; }
        public FoodItem item { get; set; }
        public FoodItem originalItem { get; set; }

        public BillItemControl(FoodItem sourceItem)
        {
            item = sourceItem;
            originalItem = sourceItem;
            InitializeComponent();

            this.ItemPrice.Text = item.value.ToString();

            string price = String.Format("{0:0.00}", item.value);
            this.ItemPrice.Text = price;
            this.ItemName.Text = sourceItem.name;
            MovingEnabled = false;
        }

        public BillItemControl(FoodItem sourceItem, string price)
        {
            item = sourceItem;
            originalItem = sourceItem;
            ItemPrice.Text = item.value.ToString();
            ItemName.Text = sourceItem.name;
        }

        public void Moved()
        {
            this.Removed.Invoke(this, new ItemEventArgs() { item = item });
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
            //this.Removed.Invoke(this, new ItemControlEventArgs() { bic = this });
        }
    }
}
