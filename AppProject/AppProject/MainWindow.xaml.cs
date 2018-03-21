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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<string> imageFileNames = HelperMethods481.
            AssemblyManager.GetAllEmbeddedResourceFilesEndingWith(".png", ".jpg");

            foreach (string fileName in imageFileNames)
            {
                Image image = HelperMethods481.AssemblyManager.GetImageFromEmbeddedResources(fileName);
                string itemName = fileName.Replace(".jpg", "").Split('.').Last();
                MoreInfoControl moreInfo = new MoreInfoControl(image, itemName);
                this.DisplayMoreInfoGrid.Children.Add(moreInfo);
            }
            W_StartButton.Opacity = 0.25;
        }

        private void W_numberOfPeopleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (W_numberOfPeopleSlider.Value > 0)
            {
                W_StartButton.Opacity = 1;
            }
            else
            {
                W_StartButton.Opacity = 0.25;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.MenuGrid.Visibility = Visibility.Hidden;
            this.ReviewWindow.Visibility = Visibility.Visible;
        }
    }
}
