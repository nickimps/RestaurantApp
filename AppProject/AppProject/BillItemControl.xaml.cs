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
        public BillItemControl(FoodItem item)
        {
            InitializeComponent();
            this.ItemName.Text = item.name;
            this.ItemPrice.Text = item.value.ToString();
            Console.WriteLine("BEING CREATED");
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
    }
}
