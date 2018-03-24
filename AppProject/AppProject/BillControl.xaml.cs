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

        public List<BillItemControl> items = new List<BillItemControl>();
        public BillControl(Bill bill)
        {
            InitializeComponent();
            this.BillIdentifier.Text = bill.billName;
            this.TotalNumber.Text = bill.ReturnTotal().ToString();
        }


        public void AddItem(BillItemControl item)
        {
            this.ItemListGrid.Children.Add(item); 
            items.Add(item);
        }

        public void ToggleItemCheckBoxes()
        {
            foreach (BillItemControl item in items)
            {
                item.ToggleCheckBoxVisibility();
            }
        }
    }
}
