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
            this.TotalText.Text = "$" + bill.ReturnTotal().ToString();
        }
    }
}
