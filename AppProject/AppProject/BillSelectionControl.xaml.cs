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
    /// Interaction logic for BillSelectionControl.xaml
    /// </summary>
    public partial class BillSelectionControl : UserControl
    {
        public event EventHandler<EventArgs> Selected;
        public event EventHandler<EventArgs> Unselected;
        public Boolean selected = false;
        public Bill billLogic { get; set; }
        public string billName { get; set; }

        public BillSelectionControl(Bill owningBill)
        {
            InitializeComponent();
            billLogic = owningBill;
            billName = owningBill.billName;
            this.BillName.Text = billName;
        }
        
        public void Unselect()
        {
            selected = false;
            CheckmarkImage.Visibility = Visibility.Hidden;
        }

        private void BillSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("IS THIS EVENT FIRING?");
            if (selected)
            {
                selected = false;
                CheckmarkImage.Visibility = Visibility.Hidden;
                this.Unselected.Invoke(this, new EventArgs());
            }
            else
            {
                selected = true;
                CheckmarkImage.Visibility = Visibility.Visible;
                this.Selected.Invoke(this, new EventArgs());
            }
        }
    }
}
