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
        List<Bill> bills=new List<Bill>();
        int numDinners = 0;

        public MainWindow()
        {
            InitializeComponent();
            DisplayMoreInfoGrid.Visibility = Visibility.Hidden;

            List<string> imageFileNames = HelperMethods481.
            AssemblyManager.GetAllEmbeddedResourceFilesEndingWith(".png", ".jpg");

            foreach (string fileName in imageFileNames)
            {
                Image image = HelperMethods481.AssemblyManager.GetImageFromEmbeddedResources(fileName);
                string itemName = fileName.Replace(".jpg", "").Split('.').Last();
                MenuItemsControl menuItems = new MenuItemsControl(image, itemName);
                menuItems.MouseDown += new MouseButtonEventHandler(item_more_info_MouseDown);
                this.Menu_items_uniform_gird.Children.Add(menuItems);
            }
            W_StartButton.Opacity = 0.25;
            W_StartButton.IsEnabled = false;
        }

        private void item_more_info_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DisplayMoreInfoGrid.Visibility = Visibility.Visible;
            
            MenuItemsControl mic = sender as MenuItemsControl;
            Image image = mic.ImageContent;
            string itemName = mic.FoodTitle.Text;
            MoreInfoControl moreInfo = new MoreInfoControl(image, itemName);
            this.DisplayMoreInfoGrid.Children.Clear();
            this.DisplayMoreInfoGrid.Children.Add(moreInfo);
            
                
        }

        private void W_numberOfPeopleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (W_numberOfPeopleSlider.Value > 0)
            {
                W_StartButton.Opacity = 1;
                W_StartButton.IsEnabled = true;
            }
            else
            {
                W_StartButton.Opacity = 0.25;
                W_StartButton.IsEnabled = false;
            }
        }

        private void W_StartButton_Click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Visible;
            WelcomeScreen.Visibility = Visibility.Hidden;
            numDinners = (int) W_numberOfPeopleSlider.Value;

            for (int i=0; i<numDinners; i++)
            {
                Bill cBill = new Bill(i + 1);
                this.R_BillUnicormGrid.Children.Add(cBill);
                bills.Add(cBill);
            }

        }

        private void R_ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Visible;
            ReviewGrid.Visibility = Visibility.Hidden;
        }

        private void M_ReviewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Hidden;
            ReviewGrid.Visibility = Visibility.Visible;
        }

        private void R_MoveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.ToggleCheckBoxVisibility();
            }
            this.R_TransitionButtonGrid.Visibility = Visibility.Hidden;
            this.R_EditButtonsGrid.Visibility = Visibility.Hidden;
            this.R_MoveButtonsGrid.Visibility = Visibility.Visible;
        }

        private void R_CancelButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.ToggleCheckBoxVisibility();
            }
            this.R_TransitionButtonGrid.Visibility = Visibility.Visible;
            this.R_EditButtonsGrid.Visibility = Visibility.Visible;
            this.R_MoveButtonsGrid.Visibility = Visibility.Hidden;
        }
    }
}
