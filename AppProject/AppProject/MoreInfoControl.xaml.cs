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
    /// Interaction logic for MoreInfoControl.xaml
    /// </summary>
    public partial class MoreInfoControl : UserControl
    {
        public event EventHandler<ItemEventArgs> AddRequest;
        public event EventHandler<EventArgs> DeBlur;
        public List<Button> statButtonList = new List<Button>();

        // This lets me add info to the object whenever it is initialized
        public MoreInfoControl(Image image, string imageName)
        {
            InitializeComponent();
            PopulateSpecialButtonList();

            this.Food_Name.Text = imageName;
            this.Food_Image.BeginInit();
            this.Food_Image.Source = image.Source;
            this.Food_Image.EndInit();
            setPriceAndDescription(imageName);

            GF_Text.Visibility = Visibility.Hidden;
            Nuts_Text.Visibility = Visibility.Hidden;
            Special_Text.Visibility = Visibility.Hidden;
            Vegan_Text.Visibility = Visibility.Hidden;
            Spicy_Text.Visibility = Visibility.Hidden;
        }

        private void setPriceAndDescription(string itemName)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(@"Descriptions\" + itemName + ".txt");
                this.Price.Text = lines[0];
                this.Description.Text = lines[1];
                if (lines.Length < 3)
                {
                    SetStatusVisibilities("00000");
                } else
                {
                    SetStatusVisibilities(lines[2]);
                }
            }
            catch (FileNotFoundException ex) { Console.WriteLine(ex); }
        }

        private void SetStatusVisibilities(string status)
        {
            for (int i = 0; i < statButtonList.Count; i++)
            {
                if (status[i].Equals('1'))
                {
                    statButtonList[i].Visibility = Visibility.Visible;
                }
                else
                {
                    statButtonList[i].Visibility = Visibility.Hidden;
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            this.DeBlur.Invoke(this, new EventArgs());
        }

        public FoodItem ReturnFoodItem()
        {
            FoodItem item = new FoodItem(this.Food_Name.Text, this.Price.Text);
            return item;
        }

        private void MI_add_to_bill_button_Click(object sender, RoutedEventArgs e)
        {
            this.AddRequest.Invoke(this, new ItemEventArgs() { item = this.ReturnFoodItem() });
        }

        private void PopulateSpecialButtonList()
        {
            statButtonList.Add(this.GlutenFreeIconButton);
            statButtonList.Add(this.NutsIconButton);
            statButtonList.Add(this.SpicyIconButton);
            statButtonList.Add(this.SpecialIconButton);
            statButtonList.Add(this.VeganIconButton);
        }

        private void GlutenFreeIconButton_Click(object sender, RoutedEventArgs e)
        {
            if (GF_Text.IsVisible)
            {
                GF_Text.Visibility = Visibility.Hidden;
                Nuts_Text.Visibility = Visibility.Hidden;
                Special_Text.Visibility = Visibility.Hidden;
                Vegan_Text.Visibility = Visibility.Hidden;
                Spicy_Text.Visibility = Visibility.Hidden;
            }
            else
            {
                GF_Text.Visibility = Visibility.Visible;
                Nuts_Text.Visibility = Visibility.Hidden;
                Special_Text.Visibility = Visibility.Hidden;
                Vegan_Text.Visibility = Visibility.Hidden;
                Spicy_Text.Visibility = Visibility.Hidden;
            }
        }

        private void NutsIconButton_Click(object sender, RoutedEventArgs e)
        {
            if (Nuts_Text.IsVisible)
            {
                GF_Text.Visibility = Visibility.Hidden;
                Nuts_Text.Visibility = Visibility.Hidden;
                Special_Text.Visibility = Visibility.Hidden;
                Vegan_Text.Visibility = Visibility.Hidden;
                Spicy_Text.Visibility = Visibility.Hidden;
            }
            else
            {
                GF_Text.Visibility = Visibility.Hidden;
                Nuts_Text.Visibility = Visibility.Visible;
                Special_Text.Visibility = Visibility.Hidden;
                Vegan_Text.Visibility = Visibility.Hidden;
                Spicy_Text.Visibility = Visibility.Hidden;
            }
        }

        private void SpicyIconButton_Click(object sender, RoutedEventArgs e)
        {
            if (Spicy_Text.IsVisible)
            {
                GF_Text.Visibility = Visibility.Hidden;
                Nuts_Text.Visibility = Visibility.Hidden;
                Special_Text.Visibility = Visibility.Hidden;
                Vegan_Text.Visibility = Visibility.Hidden;
                Spicy_Text.Visibility = Visibility.Hidden;
            }
            else
            {
                GF_Text.Visibility = Visibility.Hidden;
                Nuts_Text.Visibility = Visibility.Hidden;
                Special_Text.Visibility = Visibility.Hidden;
                Vegan_Text.Visibility = Visibility.Hidden;
                Spicy_Text.Visibility = Visibility.Visible;
            }
        }

        private void SpecialIconButton_Click(object sender, RoutedEventArgs e)
        {
            if (Special_Text.IsVisible)
            {
                GF_Text.Visibility = Visibility.Hidden;
                Nuts_Text.Visibility = Visibility.Hidden;
                Special_Text.Visibility = Visibility.Hidden;
                Vegan_Text.Visibility = Visibility.Hidden;
                Spicy_Text.Visibility = Visibility.Hidden;
            }
            else
            {
                GF_Text.Visibility = Visibility.Hidden;
                Nuts_Text.Visibility = Visibility.Hidden;
                Special_Text.Visibility = Visibility.Visible;
                Vegan_Text.Visibility = Visibility.Hidden;
                Spicy_Text.Visibility = Visibility.Hidden;
            }
        }

        private void VeganIconButton_Click(object sender, RoutedEventArgs e)
        {
            if (Vegan_Text.IsVisible)
            {
                GF_Text.Visibility = Visibility.Hidden;
                Nuts_Text.Visibility = Visibility.Hidden;
                Special_Text.Visibility = Visibility.Hidden;
                Vegan_Text.Visibility = Visibility.Hidden;
                Spicy_Text.Visibility = Visibility.Hidden;
            }
            else
            {
                GF_Text.Visibility = Visibility.Hidden;
                Nuts_Text.Visibility = Visibility.Hidden;
                Special_Text.Visibility = Visibility.Hidden;
                Vegan_Text.Visibility = Visibility.Visible;
                Spicy_Text.Visibility = Visibility.Hidden;
            }
        }
    }
}
