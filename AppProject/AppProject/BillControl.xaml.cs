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

        public List<BillItem> items = new List<BillItem>();
        public BillControl(int billNumber)
        {
            InitializeComponent();
            this.BillNumber.Text = billNumber.ToString();
        }


        public void AddItem(BillItem item)
        {
            this.ItemListGrid.Children.Add(item); 
            items.Add(item);
        }

        public void ToggleCheckBoxVisibility()
        {
            if (this.BillCheckBox.Visibility == Visibility.Visible)
            {
                this.BillCheckBox.Visibility = Visibility.Hidden;
            }
            else
            {
                this.BillCheckBox.Visibility = Visibility.Visible;
            }
        }

        public void ToggleItemCheckBoxes()
        {
            foreach (BillItem item in items)
            {
                item.ToggleCheckBoxVisibility();
            }
        }
    }
}
