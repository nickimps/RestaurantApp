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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace AppProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Bill selectedBill;
        List<Bill> bills = new List<Bill>();
        int numDinners = 0;
        List<FoodCategory> menu = new List<FoodCategory>();
        private Boolean addMode = false;
        FoodItem selectedItem;
    

        Grid selectedMenu = null;
        ScrollViewer selectedMenuItems = null;
        Rectangle selectedMenuCover;

        public MainWindow()
        {
            InitializeComponent();
            ReviewGrid.Visibility = Visibility.Hidden;
            DisplayMoreInfoGrid.Visibility = Visibility.Hidden;
            BillDisplayGrid.Visibility = Visibility.Hidden;
            ServerGrid.Visibility = Visibility.Hidden;
            HelpPromptGrid.Visibility = Visibility.Hidden;
            selectedMenu = M_MenuItemsGrid;
            selectedMenuItems = M_AppetizerScrollGrid;
            selectedMenuCover = M_AppetizerCover;

            HelpPromptGrid.Visibility = Visibility.Hidden;

            //Creating Menu from local file
            //string[] categories = System.IO.File.ReadAllLines(@"Menu\categories.txt");
            //foreach (string category in categories)
            //{
             //   menu.Add(new FoodCategory(category));
                //System.IO.File.ReadAllLines(@"Menu\"+category+)
            //}


            //Menu Screen Setup
            List<string> imageFileNames = HelperMethods481.
            AssemblyManager.GetAllEmbeddedResourceFilesEndingWith(".png", ".jpg");

            foreach (string fileName in imageFileNames)
            {
                Image image = HelperMethods481.AssemblyManager.GetImageFromEmbeddedResources(fileName);
                string itemName = fileName.Replace(".jpg", "").Split('.').Last();
                MenuItemsControl menuItems = new MenuItemsControl(image, itemName);

                menuItems.InformationRequest += new EventHandler<EventArgs>(M_DisplayMoreInfo);
                menuItems.AddRequest += new EventHandler<ItemEventArgs>(M_AddRequest);

                if (File.Exists(@"Images\Appetizers\" + itemName + ".jpg"))
                    this.Menu_items_uniform_gird.Children.Add(menuItems);
                else if (File.Exists(@"Images\Entrees\" + itemName + ".jpg"))
                    this.Menu_Entrees_uniform_grid.Children.Add(menuItems);
                else if (File.Exists(@"Images\Desserts\" + itemName + ".jpg"))
                    this.Menu_Dessert_uniform_grid.Children.Add(menuItems);
                else if (File.Exists(@"Images\Kids Menu\" + itemName + ".jpg"))
                    this.Menu_Kids_uniform_grid.Children.Add(menuItems);
                else if (File.Exists(@"Images\Drinks\" + itemName + ".jpg"))
                    this.Menu_Drink_uniform_grid.Children.Add(menuItems);
            }


            //Welcome Screen Setup
            W_StartButton.Opacity = 0.25;
            W_StartButton.IsEnabled = false;
            R_MoveButtonsGrid.Visibility = Visibility.Hidden;
        }

        private void M_DisplayMoreInfo(object sender, EventArgs e)
        {
            DisplayMoreInfoGrid.Visibility = Visibility.Visible;
            this.M_CategoryGrid.IsEnabled = false;
            this.selectedMenu.IsEnabled = false;
            this.selectedMenuItems.IsEnabled = false;
            this.selectedMenuCover.IsEnabled = false;

            BlurEffect myBlurEffect = new BlurEffect();
            myBlurEffect.Radius = 6;
            myBlurEffect.KernelType = KernelType.Gaussian;
            this.M_CategoryGrid.Effect = myBlurEffect;
            this.selectedMenu.Effect = myBlurEffect;
            this.selectedMenuItems.Effect = myBlurEffect;
            this.selectedMenuCover.Effect = myBlurEffect;

            MenuItemsControl mic = sender as MenuItemsControl;
            Image image = mic.ImageContent;
            string itemName = mic.FoodTitle.Text;
            MoreInfoControl moreInfo = new MoreInfoControl(image, itemName);
            this.DisplayMoreInfoGrid.Children.Clear();
            this.DisplayMoreInfoGrid.Children.Add(moreInfo);
            moreInfo.AddRequest += new EventHandler<ItemEventArgs>(M_AddRequest);
            moreInfo.DeBlur += new EventHandler<EventArgs>(M_DeBlur);
        }

        private void M_DeBlur(object sender, EventArgs e)
        {
            BlurEffect myDeBlurEffect = new BlurEffect();
            myDeBlurEffect.Radius = 0;
            myDeBlurEffect.KernelType = KernelType.Gaussian;
            this.M_CategoryGrid.Effect = myDeBlurEffect;
            this.selectedMenu.Effect = myDeBlurEffect;
            this.selectedMenuItems.Effect = myDeBlurEffect;
            this.selectedMenuCover.Effect = myDeBlurEffect;

            this.M_CategoryGrid.IsEnabled = true;
            this.selectedMenu.IsEnabled = true;
            this.selectedMenuItems.IsEnabled = true;
            this.selectedMenuCover.IsEnabled = true;
        }

        private void M_AddRequest(object sender, ItemEventArgs e)
        {
            this.M_CategoryGrid.Opacity = 0.3;
            this.M_CategoryGrid.IsEnabled = false;
            this.selectedMenu.Opacity = 0.3;
            this.selectedMenu.IsEnabled = false;
            this.selectedMenuItems.Opacity = 0.3;
            this.selectedMenuItems.IsEnabled = false;
            this.selectedMenuCover.Opacity = 0.3;
            this.selectedMenuCover.IsEnabled = false;
            this.DisplayMoreInfoGrid.Opacity = 0.5;
            this.DisplayMoreInfoGrid.IsEnabled = false;
            addMode = true;
            selectedItem = new FoodItem(e.item);
        }

        private void M_BillClick(object sender, EventArgs e)
        {
            Bill clickedBill = sender as Bill;
            if (addMode)
            {
                this.M_CategoryGrid.Opacity = 1;
                this.M_CategoryGrid.IsEnabled = true;
                this.selectedMenu.Opacity = 1;
                this.selectedMenu.IsEnabled = true;
                this.selectedMenuItems.Opacity = 1;
                this.selectedMenuItems.IsEnabled = true;
                this.selectedMenuCover.Opacity = 1;
                this.selectedMenuCover.IsEnabled = true;

                EventHandler<EventArgs> DeBlurThings = new EventHandler<EventArgs>(M_DeBlur);
                DeBlurThings.Invoke(this, new EventArgs());

                this.DisplayMoreInfoGrid.Visibility = Visibility.Hidden;
                this.DisplayMoreInfoGrid.Opacity = 1;
                this.DisplayMoreInfoGrid.IsEnabled = true;
                addMode = false;
                clickedBill.AddItem(selectedItem);
            } 
            else
            {
                this.BillDisplayGrid.Visibility = Visibility.Visible;
                BillControl bill = new BillControl(clickedBill);
                this.ContentGrid.Children.Add(bill);
            }
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

        //Method initializes bills and displays them onto their respective screens
        private void W_StartButton_Click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Visible;
            WelcomeScreen.Visibility = Visibility.Hidden;
            numDinners = (int) W_numberOfPeopleSlider.Value;

            for (int i = 0; i < numDinners; i++)
            {
                Bill cBill = new Bill((i + 1).ToString());
                bills.Add(cBill);
                R_BillUniformGrid.Children.Add(cBill.billView);
                M_BillUniformGrid.Children.Add(cBill.m_BillView);
                cBill.MenuBillClicked += new EventHandler<EventArgs>(M_BillClick);
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
                //Gotta Set it so that bills and items are dragable in this mode others disallow it
                
            }
            this.R_TransitionButtonGrid.Visibility = Visibility.Hidden;
            this.R_EditButtonsGrid.Visibility = Visibility.Hidden;
            this.R_MoveButtonsGrid.Visibility = Visibility.Visible;



          

        }

        private void R_CancelButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.ToggleCheckBox();
            }
            this.R_TransitionButtonGrid.Visibility = Visibility.Visible;
            this.R_EditButtonsGrid.Visibility = Visibility.Visible;
            this.R_MoveButtonsGrid.Visibility = Visibility.Hidden;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void R_CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.ServerGrid.Visibility = Visibility.Visible;
        }

        private void S_Dismiss_Click(object sender, RoutedEventArgs e)
        {
            this.ServerGrid.Visibility = Visibility.Hidden;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.BillDisplayGrid.Visibility = Visibility.Hidden;
        }

        private void M_AppetizersButton_Click(object sender, RoutedEventArgs e)
        {
            selectedMenu.Visibility = Visibility.Hidden;
            selectedMenuCover.Visibility = Visibility.Hidden;
            selectedMenuItems.Visibility = Visibility.Hidden;

            this.M_MenuItemsGrid.Visibility = Visibility.Visible;
            this.M_AppetizerCover.Visibility = Visibility.Visible;
            this.M_AppetizerScrollGrid.Visibility = Visibility.Visible;
            selectedMenu = this.M_MenuItemsGrid;
            selectedMenuCover = this.M_AppetizerCover;
            selectedMenuItems = this.M_AppetizerScrollGrid;
        }

        private void M_EntreesButton_Click(object sender, RoutedEventArgs e)
        {
            selectedMenu.Visibility = Visibility.Hidden;
            selectedMenuCover.Visibility = Visibility.Hidden;
            selectedMenuItems.Visibility = Visibility.Hidden;

            this.M_EntreeItemsGrid.Visibility = Visibility.Visible;
            this.M_EntreesCover.Visibility = Visibility.Visible;
            this.M_EntreeScrollGrid.Visibility = Visibility.Visible;
            selectedMenu = this.M_EntreeItemsGrid;
            selectedMenuCover = this.M_EntreesCover;
            selectedMenuItems = this.M_EntreeScrollGrid;
        }

        private void M_DessertsButton_Click(object sender, RoutedEventArgs e)
        {
            selectedMenu.Visibility = Visibility.Hidden;
            selectedMenuCover.Visibility = Visibility.Hidden;
            selectedMenuItems.Visibility = Visibility.Hidden;

            this.M_DessertItemGrid.Visibility = Visibility.Visible;
            this.M_DessertsCover.Visibility = Visibility.Visible;
            this.M_DessertScrollGrid.Visibility = Visibility.Visible;
            selectedMenu = this.M_DessertItemGrid;
            selectedMenuCover = this.M_DessertsCover;
            selectedMenuItems = this.M_DessertScrollGrid;
        }

        private void M_DrinksButton_Click(object sender, RoutedEventArgs e)
        {
            selectedMenu.Visibility = Visibility.Hidden;
            selectedMenuCover.Visibility = Visibility.Hidden;
            selectedMenuItems.Visibility = Visibility.Hidden;

            this.M_DrinkItemsGrid.Visibility = Visibility.Visible;
            this.M_DrinksCover.Visibility = Visibility.Visible;
            this.M_DrinkScrollGrid.Visibility = Visibility.Visible;
            selectedMenu = this.M_DrinkItemsGrid;
            selectedMenuCover = this.M_DrinksCover;
            selectedMenuItems = this.M_DrinkScrollGrid;
        }

        private void M_KidsMenuButton_Click(object sender, RoutedEventArgs e)
        {
            selectedMenu.Visibility = Visibility.Hidden;
            selectedMenuCover.Visibility = Visibility.Hidden;
            selectedMenuItems.Visibility = Visibility.Hidden;

            this.M_KidsItemGrid.Visibility = Visibility.Visible;
            this.M_KidsMenuCover.Visibility = Visibility.Visible;
            this.M_KidsScrollGrid.Visibility = Visibility.Visible;
            selectedMenu = this.M_KidsItemGrid;
            selectedMenuCover = this.M_KidsMenuCover;
            selectedMenuItems = this.M_KidsScrollGrid;
        }

        private void CallServerButton_Click(object sender, RoutedEventArgs e)
        {
            HelpPromptGrid.Visibility = Visibility.Visible;

            this.M_CategoryGrid.IsEnabled = false;
            this.selectedMenu.IsEnabled = false;
            this.selectedMenuItems.IsEnabled = false;
            this.selectedMenuCover.IsEnabled = false;

            BlurEffect myBlurEffect = new BlurEffect();
            myBlurEffect.Radius = 6;
            myBlurEffect.KernelType = KernelType.Gaussian;
            this.M_CategoryGrid.Effect = myBlurEffect;
            this.selectedMenu.Effect = myBlurEffect;
            this.selectedMenuItems.Effect = myBlurEffect;
            this.selectedMenuCover.Effect = myBlurEffect;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HelpPromptGrid.Visibility = Visibility.Hidden;

            BlurEffect myDeBlurEffect = new BlurEffect();
            myDeBlurEffect.Radius = 0;
            myDeBlurEffect.KernelType = KernelType.Gaussian;
            this.M_CategoryGrid.Effect = myDeBlurEffect;
            this.selectedMenu.Effect = myDeBlurEffect;
            this.selectedMenuItems.Effect = myDeBlurEffect;
            this.selectedMenuCover.Effect = myDeBlurEffect;

            this.M_CategoryGrid.IsEnabled = true;
            this.selectedMenu.IsEnabled = true;
            this.selectedMenuItems.IsEnabled = true;
            this.selectedMenuCover.IsEnabled = true;
        }
    }
}
