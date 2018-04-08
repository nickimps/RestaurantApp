using System;
using System.IO;
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
        public List<Image> statIconList = new List<Image>();
        public List<Ellipse> statBackingList = new List<Ellipse>();

        public MenuItemsControl(Image image, string imageName)
        {
            InitializeComponent();
            PopulateSpecialIconList();

            GlutenFree_Icon.Visibility = Visibility.Hidden;
            GlutenFree_IconBacking.Visibility = Visibility.Hidden;
            Spicy_Icon.Visibility = Visibility.Hidden;
            Spicy_IconBacking.Visibility = Visibility.Hidden;
            Special_Icon.Visibility = Visibility.Hidden;
            Special_IconBacking.Visibility = Visibility.Hidden;
            Vegan_Icon.Visibility = Visibility.Hidden;
            Vegan_IconBacking.Visibility = Visibility.Hidden;
            Peanut_Icon.Visibility = Visibility.Hidden;
            Peanut_IconBacking.Visibility = Visibility.Hidden;


            this.FoodTitle.Text = imageName;
            this.ImageContent.BeginInit();
            this.ImageContent.Source = image.Source;
            this.ImageContent.EndInit();
            this.FoodPrice.Text = getFoodPrice(imageName);

            SetIconVisibilities(FoodTitle.Text);
        }

        public FoodItem ReturnFoodItem()
        {
            FoodItem item = new FoodItem(this.FoodTitle.Text, this.FoodPrice.Text);
            return item;
        }

        private void SetIconVisibilities(string itemName)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(@"Descriptions\" + itemName + ".txt");
                this.FoodPrice.Text = lines[0];
                if (lines.Length < 3)
                {
                    SetStatusVisibilities("00000");
                }
                else
                {
                    SetStatusVisibilities(lines[2]);
                }
            }
            catch (FileNotFoundException ex) { Console.WriteLine(ex); }
        }

        private void SetStatusVisibilities(string status)
        {
            for (int i = 0; i < statIconList.Count; i++)
            {
                if (status[i].Equals('1'))
                {
                    statIconList[i].Visibility = Visibility.Visible;
                    statBackingList[i].Visibility = Visibility.Visible;
                }
                else
                {
                    statIconList[i].Visibility = Visibility.Hidden;
                    statBackingList[i].Visibility = Visibility.Hidden;
                }
            }
        }
        private void PopulateSpecialIconList()
        {
            statIconList.Add(this.GlutenFree_Icon);
            statIconList.Add(this.Peanut_Icon);
            statIconList.Add(this.Spicy_Icon);
            statIconList.Add(this.Special_Icon);
            statIconList.Add(this.Vegan_Icon);

            statBackingList.Add(this.GlutenFree_IconBacking);
            statBackingList.Add(this.Peanut_IconBacking);
            statBackingList.Add(this.Spicy_IconBacking);
            statBackingList.Add(this.Special_IconBacking);
            statBackingList.Add(this.Vegan_IconBacking);
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
