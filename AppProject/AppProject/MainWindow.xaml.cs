﻿using System;
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
using System.Windows.Media.Animation;

namespace AppProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Bill selectedBill;
        List<Bill> bills = new List<Bill>();
        List<FoodItem> orderedFoods = new List<FoodItem>();
        List<Bill> selectedBills = new List<Bill>();

        private int numDinners = 0;
        private int billPosition;
 
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
            AddItemsPromptGrid.Visibility = Visibility.Hidden;
            selectedMenu = M_MenuItemsGrid;
            selectedMenuItems = M_AppetizerScrollGrid;
            selectedMenuCover = M_AppetizerCover;
            this.M_AppetizersButton.FontWeight = FontWeights.Bold;

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

        private void RemoveItemOrder(object sender, ItemEventArgs e) 
        {
            e.item.Empty -= new EventHandler<ItemEventArgs>(RemoveItemOrder);
            orderedFoods.Remove(e.item);
        }

        /**********************************************************
         *************WELCOME SCREEN BUTTTON FUNCTIONS*************
         **********************************************************/
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
            this.W_StartButton.IsEnabled = false;
            numDinners = (int)W_numberOfPeopleSlider.Value;

            for (int i = 0; i < numDinners; i++)
            {
                Bill cBill = new Bill("Bill #" + (i + 1).ToString());
                bills.Add(cBill);
                R_BillUniformGrid.Children.Add(cBill.billView);
                M_BillUniformGrid.Children.Add(cBill.m_BillView);
                S_BillUniformGrid.Children.Add(cBill.s_BillView);
                cBill.s_BillView.Selected += new EventHandler<EventArgs>(S_AddToSplitList);
                cBill.s_BillView.Unselected += new EventHandler<EventArgs>(S_RemoveFromSplitList);
                cBill.MenuBillClicked += new EventHandler<EventArgs>(M_BillClick);
                cBill.billView.SplitRequest += new EventHandler<BICEventArgs>(R_DisplaySelections);
            }
            Storyboard sb = this.FindResource("CloseWelcomeScreen") as Storyboard;
            sb.Completed += OnWelcomeScreenStoryboardCompleted;
        }

        private void OnWelcomeScreenStoryboardCompleted(object sender, EventArgs e)
        {
            MenuGrid.Visibility = Visibility.Visible;
            WelcomeScreen.Visibility = Visibility.Hidden;
        }

        /**********************************************************
        ***************MENU SCREEN BUTTTON FUNCTIONS***************
        **********************************************************/
        private void M_AppetizersButton_Click(object sender, RoutedEventArgs e)
        {
            selectedMenu.Visibility = Visibility.Hidden;
            selectedMenuCover.Visibility = Visibility.Hidden;
            selectedMenuItems.Visibility = Visibility.Hidden;

            this.M_AppetizersButton.FontWeight = FontWeights.Bold;
            this.M_EntreesButton.FontWeight = FontWeights.Regular;
            this.M_DessertsButton.FontWeight = FontWeights.Regular;
            this.M_DrinksButton.FontWeight = FontWeights.Regular;
            this.M_KidsMenuButton.FontWeight = FontWeights.Regular;

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

            this.M_EntreesButton.FontWeight = FontWeights.Bold;
            this.M_AppetizersButton.FontWeight = FontWeights.Regular;
            this.M_DessertsButton.FontWeight = FontWeights.Regular;
            this.M_DrinksButton.FontWeight = FontWeights.Regular;
            this.M_KidsMenuButton.FontWeight = FontWeights.Regular;

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

            this.M_DessertsButton.FontWeight = FontWeights.Bold;
            this.M_AppetizersButton.FontWeight = FontWeights.Regular;
            this.M_EntreesButton.FontWeight = FontWeights.Regular;
            this.M_DrinksButton.FontWeight = FontWeights.Regular;
            this.M_KidsMenuButton.FontWeight = FontWeights.Regular;

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

            this.M_DrinksButton.FontWeight = FontWeights.Bold;
            this.M_AppetizersButton.FontWeight = FontWeights.Regular;
            this.M_EntreesButton.FontWeight = FontWeights.Regular;
            this.M_DessertsButton.FontWeight = FontWeights.Regular;
            this.M_KidsMenuButton.FontWeight = FontWeights.Regular;

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

            this.M_KidsMenuButton.FontWeight = FontWeights.Bold;
            this.M_AppetizersButton.FontWeight = FontWeights.Regular;
            this.M_EntreesButton.FontWeight = FontWeights.Regular;
            this.M_DessertsButton.FontWeight = FontWeights.Regular;
            this.M_DrinksButton.FontWeight = FontWeights.Regular;

            this.M_KidsItemGrid.Visibility = Visibility.Visible;
            this.M_KidsMenuCover.Visibility = Visibility.Visible;
            this.M_KidsScrollGrid.Visibility = Visibility.Visible;
            selectedMenu = this.M_KidsItemGrid;
            selectedMenuCover = this.M_KidsMenuCover;
            selectedMenuItems = this.M_KidsScrollGrid;
        }

        private void M_DisplayMoreInfo(object sender, EventArgs e)
        {
            DisplayMoreInfoGrid.Visibility = Visibility.Visible;

            BlurEffect myBlurEffect = new BlurEffect
            {
                Radius = 10,
                KernelType = KernelType.Gaussian
            };
            this.M_CategoryGrid.Effect = myBlurEffect;
            this.selectedMenu.Effect = myBlurEffect;
            this.selectedMenuItems.Effect = myBlurEffect;
            this.selectedMenuCover.Effect = myBlurEffect;
            this.M_ReviewOrderButton.Effect = myBlurEffect;

            this.M_ReviewOrderButton.IsEnabled = false;
            this.M_CategoryGrid.IsEnabled = false;
            this.selectedMenu.IsEnabled = false;
            this.selectedMenuItems.IsEnabled = false;
            this.selectedMenuCover.IsEnabled = false;

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
            BlurEffect myDeBlurEffect = new BlurEffect
            {
                Radius = 0,
                KernelType = KernelType.Gaussian
            };
            this.M_CategoryGrid.Effect = myDeBlurEffect;
            this.DisplayMoreInfoGrid.Effect = myDeBlurEffect;
            this.selectedMenu.Effect = myDeBlurEffect;
            this.selectedMenuItems.Effect = myDeBlurEffect;
            this.selectedMenuCover.Effect = myDeBlurEffect;
            this.M_ReviewOrderButton.Effect = myDeBlurEffect;

            this.M_ReviewOrderButton.IsEnabled = true;
            this.M_CategoryGrid.IsEnabled = true;
            this.DisplayMoreInfoGrid.IsEnabled = true;
            this.selectedMenu.IsEnabled = true;
            this.selectedMenuItems.IsEnabled = true;
            this.selectedMenuCover.IsEnabled = true;
        }

        private void M_AddRequest(object sender, ItemEventArgs e)
        {
            AddItemsPromptGrid.Visibility = Visibility.Visible;

            BlurEffect myBlurEffect = new BlurEffect
            {
                Radius = 10,
                KernelType = KernelType.Gaussian
            };
            this.M_CategoryGrid.Effect = myBlurEffect;
            this.DisplayMoreInfoGrid.Effect = myBlurEffect;
            this.selectedMenu.Effect = myBlurEffect;
            this.selectedMenuItems.Effect = myBlurEffect;
            this.selectedMenuCover.Effect = myBlurEffect;
            this.M_ReviewOrderButton.Effect = myBlurEffect;

            this.M_ReviewOrderButton.IsEnabled = false;
            this.M_CategoryGrid.IsEnabled = false;
            this.selectedMenu.IsEnabled = false;
            this.selectedMenuItems.IsEnabled = false;
            this.selectedMenuCover.IsEnabled = false;

            DropShadowEffect myDropShadow = new DropShadowEffect
            {
                BlurRadius = 10,
                Direction = 315,
                ShadowDepth = 5,
                Color = Colors.Black
            };
            this.M_BillUniformGrid.Effect = myDropShadow;

            addMode = true;
            selectedItem = new FoodItem(e.item);
        }

        //Deals with MENU interface bills clicking has two seperate modes
        //When in adding mode will add item. When not will display the bill.
        private void M_BillClick(object sender, EventArgs e)
        {
            Bill clickedBill = sender as Bill;
            if (addMode)
            {
                EventHandler<EventArgs> DeBlurThings = new EventHandler<EventArgs>(M_DeBlur);
                DeBlurThings.Invoke(this, new EventArgs());
                
                this.DisplayMoreInfoGrid.IsEnabled = true;

                AddItemsPromptGrid.Visibility = Visibility.Hidden;

                DropShadowEffect myUnDropShadow = new DropShadowEffect
                {
                    BlurRadius = 0,
                    Direction = 0,
                    ShadowDepth = 0,
                    Color = Colors.White
                };
                this.M_BillUniformGrid.Effect = myUnDropShadow;

                addMode = false;
                orderedFoods.Add(selectedItem);
                selectedItem.Empty += new EventHandler<ItemEventArgs>(RemoveItemOrder);
                clickedBill.AddNewItem(selectedItem);
            }
            //Display Bill
            else
            {
                this.BillDisplayGrid.Visibility = Visibility.Visible;
                billPosition = this.R_BillUniformGrid.Children.IndexOf(clickedBill.billView);
                this.R_BillUniformGrid.Children.Remove(clickedBill.billView);
                this.ContentGrid.Children.Add(clickedBill.billView);

                //This is a temporary solution for Menu Bill interactive when one is clicked
                this.M_BillUniformGrid.IsEnabled = false;

            }
        }

        //Function responsible to reseting bill display accordingly
        private void Close_BillDisplayGrid(object sender, RoutedEventArgs e)
        {
            this.BillDisplayGrid.Visibility = Visibility.Hidden;
            BillControl bc = this.ContentGrid.Children[1] as BillControl;
            this.ContentGrid.Children.Remove(bc);
            if (billPosition == BillDisplayGrid.Children.Count)
            {
                this.R_BillUniformGrid.Children.Add(bc);
            } else
            {
                this.R_BillUniformGrid.Children.Insert(billPosition, bc);
            }

            //This is a temporary solution for Menu Bill interactive when one is clicked
            this.M_BillUniformGrid.IsEnabled = true;
        }

        private void OnHideReviewStoryboardCompleted(object sender, EventArgs e)
        {
            ReviewGrid.Visibility = Visibility.Hidden;
        }

        private void M_ReviewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            ReviewGrid.Visibility = Visibility.Visible;
            R_ReviewTitle.Text = "Review Bills";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HelpPromptGrid.Visibility = Visibility.Hidden;

            BlurEffect myDeBlurEffect = new BlurEffect
            {
                Radius = 0,
                KernelType = KernelType.Gaussian
            };
            this.M_CategoryGrid.Effect = myDeBlurEffect;
            this.DisplayMoreInfoGrid.Effect = myDeBlurEffect;
            this.selectedMenu.Effect = myDeBlurEffect;
            this.selectedMenuItems.Effect = myDeBlurEffect;
            this.selectedMenuCover.Effect = myDeBlurEffect;
            this.M_ReviewOrderButton.Effect = myDeBlurEffect;

            this.M_ReviewOrderButton.IsEnabled = true;
            this.M_CategoryGrid.IsEnabled = true;
            this.DisplayMoreInfoGrid.IsEnabled = true;
            this.selectedMenu.IsEnabled = true;
            this.selectedMenuItems.IsEnabled = true;
            this.selectedMenuCover.IsEnabled = true;
        }

        private void AddItems_Click(object sender, RoutedEventArgs e)
        {
            EventHandler<EventArgs> DeBlurThings = new EventHandler<EventArgs>(M_DeBlur);
            DeBlurThings.Invoke(this, new EventArgs());

            DropShadowEffect myUnDropShadow = new DropShadowEffect
            {
                BlurRadius = 0,
                Direction = 0,
                ShadowDepth = 0,
                Color = Colors.White
            };
            this.M_BillUniformGrid.Effect = myUnDropShadow;

            addMode = false;
            AddItemsPromptGrid.Visibility = Visibility.Hidden;
        }

        /**********************************************************
        **************REVIEW SCREEN BUTTTON FUNCTIONS**************
        **********************************************************/
        private void R_MoveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.ToggleItemDragging();
                bill.billView.ToggleItemDeletability();
            }
            this.R_TransitionButtonGrid.Visibility = Visibility.Hidden;
            this.R_EditButtonsGrid.Visibility = Visibility.Hidden;
            this.R_MoveButtonsGrid.Visibility = Visibility.Visible;
            R_ReviewTitle.Text = "Drag items to organize bills";
        }

        private void R_DoneButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.ToggleItemDragging();
                bill.billView.ToggleItemDeletability();
            }

            this.R_TransitionButtonGrid.Visibility = Visibility.Visible;
            this.R_EditButtonsGrid.Visibility = Visibility.Visible;
            this.R_MoveButtonsGrid.Visibility = Visibility.Hidden;
            R_ReviewTitle.Text = "Review Bills";
        }

        private void R_SplitButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.billView.ToggleSplitMode();
                bill.billView.ToggleItemDeletability();
            }
            this.R_TransitionButtonGrid.Visibility = Visibility.Hidden;
            this.R_EditButtonsGrid.Visibility = Visibility.Hidden;
            this.R_SplitButtonsGrid.Visibility = Visibility.Visible;
            R_ReviewTitle.Text = "Click which food item to split";
        }

        private void R_FinishSplitButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.billView.ToggleSplitMode();
                bill.billView.ToggleItemDeletability();
            }
            this.R_TransitionButtonGrid.Visibility = Visibility.Visible;
            this.R_EditButtonsGrid.Visibility = Visibility.Visible;
            this.R_SplitButtonsGrid.Visibility = Visibility.Hidden;
            R_ReviewTitle.Text = "Review Bills";
        }

        private void R_SendButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.billView.SendItems();
            }
            this.CommunicationGrid.Visibility = Visibility.Visible;
            this.ReviewGrid.IsEnabled = false;
            //EFFECTS HERE?

        }

        private void R_CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.ServerGrid.Visibility = Visibility.Visible;
        }

        private void R_ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = this.FindResource("ShrinkReviewScreen") as Storyboard;
            sb.Completed += OnHideReviewStoryboardCompleted;
        }

        private void R_DisplaySelections(object sender, BICEventArgs e)
        {
            BillItemControl bic = e.bic;
            billPosition = bills.IndexOf(bic.billControl.billLogic);
            this.S_BillUniformGrid.Children.RemoveAt(billPosition);
            this.BillSelectionGrid.Visibility = Visibility.Visible;
            selectedItem = bic.originalItem;
            
            //Temporary Solutions to disable too many instances
            this.ReviewGrid.IsEnabled = false;
            this.R_ReviewTitle.Text = "Split " + bic.itemName +  " with which bills?";

        }

        /**********************************************************
        *************SERVER RELATTED BUTTTON FUNCTIONS*************
        **********************************************************/

        private void S_Dismiss_Click(object sender, RoutedEventArgs e)
        {
            this.ServerGrid.Visibility = Visibility.Hidden;
        }

        private void CallServerButton_Click(object sender, RoutedEventArgs e)
        {
            HelpPromptGrid.Visibility = Visibility.Visible;

            this.M_CategoryGrid.IsEnabled = false;
            this.DisplayMoreInfoGrid.IsEnabled = false;
            this.selectedMenu.IsEnabled = false;
            this.selectedMenuItems.IsEnabled = false;
            this.selectedMenuCover.IsEnabled = false;
            this.M_ReviewOrderButton.IsEnabled = false;

            BlurEffect myBlurEffect = new BlurEffect
            {
                Radius = 10,
                KernelType = KernelType.Gaussian
            };
            this.M_CategoryGrid.Effect = myBlurEffect;
            this.DisplayMoreInfoGrid.Effect = myBlurEffect;
            this.selectedMenu.Effect = myBlurEffect;
            this.selectedMenuItems.Effect = myBlurEffect;
            this.selectedMenuCover.Effect = myBlurEffect;
            this.M_ReviewOrderButton.Effect = myBlurEffect;
        }

        /**********************************************************
        **************COMMUNICATION BUTTTON FUNCTIONS**************
        **********************************************************/

        private void C_DismissButton_Click(object sender, RoutedEventArgs e)
        {
            this.CommunicationGrid.Visibility = Visibility.Hidden;
            this.ReviewGrid.IsEnabled = true;
        }

        /**********************************************************
        ********************SPLITTING FUNCTIONS********************
        **********************************************************/
        private void S_AddToSplitList(object sender, EventArgs e)
        {
            BillSelectionControl bSC = sender as BillSelectionControl;
            selectedBills.Add(bSC.billLogic);
        }

        private void S_RemoveFromSplitList(object sender, EventArgs e)
        {
            BillSelectionControl bSC = sender as BillSelectionControl;
            selectedBills.Remove(bSC.billLogic);
        }

        private void S_CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.BillSelectionGrid.Visibility = Visibility.Hidden;
            this.ReviewGrid.IsEnabled = true;
            this.R_ReviewTitle.Text = "Click which food item to split";
            foreach (Bill bill in selectedBills)
            {
                bill.s_BillView.Unselect();
            }
            this.S_BillUniformGrid.Children.Insert(billPosition, bills[billPosition].s_BillView);
            selectedBills.Clear();
        }

        private void S_ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.BillSelectionGrid.Visibility = Visibility.Hidden;
            this.ReviewGrid.IsEnabled = true;
            this.R_ReviewTitle.Text = "Click which food item to split";


            //Mandatory unselection for clean up purposes
            foreach (Bill bill in selectedBills)
            {
                bill.s_BillView.Unselect();
            }

            this.S_BillUniformGrid.Children.Insert(billPosition, bills[billPosition].s_BillView);
            selectedBills.Clear();
        }
    }
}