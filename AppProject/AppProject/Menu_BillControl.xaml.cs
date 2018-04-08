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
    /// Interaction logic for Menu_BillControl.xaml
    /// </summary>
    public partial class Menu_BillControl : UserControl
    {

        //public event EventHandler<EventArgs> ButtonClicked;

        public Menu_BillControl(Bill bill)
        {
            InitializeComponent();
            this.IdentifierText.Text = bill.billName;
            string price = String.Format("{0:0.00}", bill.ReturnTotal().ToString());

            if (price.Equals("0"))
            {
                this.TotalText.Text = "$0.00";
            }
            else
            {
                this.TotalText.Text = "$" + price;
            }
        }
    }
}
