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
        // This lets me add info to the object whenever it is initialized
        public MoreInfoControl(Image image, string imageName)
        {
            InitializeComponent();
            this.Food_Name.Text = imageName;
            this.Description.Text = readDescription(imageName);
            this.Food_Image.BeginInit();
            this.Food_Image.Source = image.Source;
            this.Food_Image.EndInit();
        }


        private string readDescription(string itemName)
        {
            string text = "Didn't find a description";
            try
            {
                
                text = File.ReadAllText(@"Descriptions\" + itemName + ".txt");
                Console.WriteLine(text);  
            }
            catch (FileNotFoundException ex){ Console.WriteLine(ex); }
            
            return text;
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            this.DeBlur.Invoke(this, new EventArgs());
        }

        public FoodItem ReturnFoodItem()
        {
            FoodItem item = new FoodItem(this.Food_Name.Text, 2.00);
            return item;
        }

        private void MI_add_to_bill_button_Click(object sender, RoutedEventArgs e)
        {
            this.AddRequest.Invoke(this, new ItemEventArgs() { item = this.ReturnFoodItem() });
        }
    }
}
