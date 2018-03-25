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
        // This lets me add info to the object whenever it is initialized
        public MoreInfoControl(Image image, string imageName)
        {
            InitializeComponent();
            this.Food_Name.Text = imageName;
            this.Food_Image.BeginInit();
            this.Food_Image.Source = image.Source;
            this.Food_Image.EndInit();
            setPriceAndDescription(imageName);
        }


        private void setPriceAndDescription(string itemName)
        {
            int counter = 0;
            string line;
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(@"Descriptions\" + itemName + ".txt");
                while ((line = file.ReadLine()) != null)
                {
                    if (counter == 0)
                    {
                        this.Price.Text = line;
                    }
                    else
                        this.Description.Text = line;
                    counter++;
                }
            }
            catch (FileNotFoundException ex) { Console.WriteLine(ex); }
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            
        }
    }
}
