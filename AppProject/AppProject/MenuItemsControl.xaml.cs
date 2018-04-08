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
    /// Interaction logic for MenuItemsControl.xaml
    /// </summary>
    public partial class MenuItemsControl : UserControl
    {
        public event EventHandler<EventArgs> InformationRequest;
        public event EventHandler<ItemEventArgs> AddRequest;
        public MenuItemsControl(Image image, string imageName)
        {
            InitializeComponent();


            this.FoodTitle.Text = imageName;
            this.ImageContent.BeginInit();
            this.ImageContent.Source = image.Source;
            this.ImageContent.EndInit();
            this.FoodPrice.Text = getFoodPrice(imageName);
        }

        public FoodItem ReturnFoodItem()
        {
            FoodItem item = new FoodItem(this.FoodTitle.Text, this.FoodPrice.Text);
            return item;
        }


        // read description file for the price of the food item
        private string getFoodPrice(string item)
        {
            int counter = 0;
            string price = "";
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(@"Descriptions\" + item + ".txt");
                while ((price = file.ReadLine()) != null)
                {
                    if (counter == 0)
                    {
                        return price;
                    }
                    else
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return price;
        }

        private void M_more_info_button_Click(object sender, RoutedEventArgs e)
        {
            this.InformationRequest.Invoke(this, new EventArgs());
        }

        private void M_add_to_bill_button_Click(object sender, RoutedEventArgs e)
        {
            this.AddRequest.Invoke(this, new ItemEventArgs() { item = this.ReturnFoodItem() });
        }
    }
}
