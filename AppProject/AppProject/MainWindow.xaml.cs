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
using System.Windows.Media.Animation;

namespace AppProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //List of active bills
        List<Bill> bills = new List<Bill>();
        
        //List of ordered foods (Must be present on a bill to in this list
        List<FoodItem> orderedFoods = new List<FoodItem>();

        //This is for bill splitting. Will List all bills selected (does not include original bill that has BillItemControl to be split)
        List<Bill> selectedBills = new List<Bill>();

        private int numDinners = 0;
        private int billPosition;
 
        private Boolean addMode = false;
        private Boolean serverMode = false;
        FoodItem selectedItem;
        BillItemControl selectedBIC;
        Grid selectedMenu = null;
        ScrollViewer selectedMenuItems = null;
        Rectangle selectedMenuCover;
       

        public MainWindow()
        {
            InitializeComponent();
            serverMode = false;
            ReviewGrid.Visibility = Visibility.Hidden;
            DisplayMoreInfoGrid.Visibility = Visibility.Hidden;
            BillDisplayGrid.Visibility = Visibility.Hidden;
            ServerGrid.Visibility = Visibility.Hidden;
            HelpPromptGrid.Visibility = Visibility.Hidden;
            AddItemsPromptGrid.Visibility = Visibility.Hidden;
            BillSelectionGrid.Visibility = Visibility.Hidden;
            


            //Set the opening menu category to appetizers
            selectedMenu = M_MenuItemsGrid;
            selectedMenuItems = M_AppetizerScrollGrid;
            selectedMenuCover = M_AppetizerCover;

            this.M_AppetizersButton.FontWeight = FontWeights.Bold;
            this.M_EntreesButton.FontWeight = FontWeights.Regular;
            this.M_DessertsButton.FontWeight = FontWeights.Regular;
            this.M_DrinksButton.FontWeight = FontWeights.Regular;
            this.M_KidsMenuButton.FontWeight = FontWeights.Regular;

            this.M_MenuItemsGrid.Visibility = Visibility.Visible;
            this.M_AppetizerCover.Visibility = Visibility.Visible;
            this.M_AppetizerScrollGrid.Visibility = Visibility.Visible;

            HelpPromptGrid.Visibility = Visibility.Hidden;

            //Menu Screen Setup
            List<string> imageFileNames = HelperMethods481.
            AssemblyManager.GetAllEmbeddedResourceFilesEndingWith(".png", ".jpg");

            foreach (string fileName in imageFileNames)
            {
                Image image = HelperMethods481.AssemblyManager.GetImageFromEmbeddedResources(fileName);

                string itemName = "";
                if (fileName.Contains(".jpg"))
                {
                    itemName = fileName.Replace(".jpg", "").Split('.').Last();
                }
                else if (fileName.Contains(".png"))
                {
                    itemName = fileName.Replace(".png", "").Split('.').Last();
                }

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
               else if (File.Exists(@"Images\Appetizers\" + itemName + ".png"))
                    this.Menu_items_uniform_gird.Children.Add(menuItems);
                else if (File.Exists(@"Images\Entrees\" + itemName + ".png"))
                    this.Menu_Entrees_uniform_grid.Children.Add(menuItems);
                else if (File.Exists(@"Images\Desserts\" + itemName + ".png"))
                    this.Menu_Dessert_uniform_grid.Children.Add(menuItems);
                else if (File.Exists(@"Images\Kids Menu\" + itemName + ".png"))
                    this.Menu_Kids_uniform_grid.Children.Add(menuItems);
                else if (File.Exists(@"Images\Drinks\" + itemName + ".png"))
                    this.Menu_Drink_uniform_grid.Children.Add(menuItems);
            }


            //Welcome Screen Setup
            W_StartButton.Opacity = 0.25;
            W_StartButton.IsEnabled = false;
            R_MoveButtonsGrid.Visibility = Visibility.Hidden;
            this.R_UpdateSendButtonStatus();
        }

        private void RemoveItemOrder(object sender, ItemEventArgs e) 
        {
            e.item.Empty -= new EventHandler<ItemEventArgs>(RemoveItemOrder);
            orderedFoods.Remove(e.item);
            this.R_UpdateSendButtonStatus();
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
                cBill.billView.Deleted += new EventHandler<EventArgs>(R_HandleBillDeletion);
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
                DisplayMoreInfoGrid.Visibility = Visibility.Hidden;

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
                this.R_UpdateSendButtonStatus();
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
            R_SplitButtonsGrid.Visibility = Visibility.Hidden;
            S_clear_table_button.Visibility = Visibility.Hidden;
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

        public void R_UpdateSendButtonStatus()
        {
            Boolean UnSentItems = false;
            foreach (FoodItem item in orderedFoods)
            {
                if (!item.itemSent)
                {
                    UnSentItems = true;
                    break;
                }
            }
            if (UnSentItems)
            {
                R_SendButton.Opacity = 1;
                R_SendButton.IsEnabled = true;
            } else
            {
                R_SendButton.Opacity = 0.5;
                R_SendButton.IsEnabled = false;
            }
        }

        private void R_MoveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.ToggleItemDragging();
                bill.billView.ToggleItemDeletability();
            }       
            
            this.R_TransitionButtonGrid.Visibility = Visibility.Hidden;
            this.R_EditButtonsGrid.Visibility = Visibility.Hidden;
            this.R_BillA_DGrid.Visibility = Visibility.Hidden;
            this.R_MoveButtonsGrid.Visibility = Visibility.Visible;
            this.S_clear_table_button.Visibility = Visibility.Hidden;
            this.S_exit_server_mode.Visibility = Visibility.Hidden;
            R_ReviewTitle.Text = "Drag items to organize bills";
        }

        private void R_DoneButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.ToggleItemDragging();
                bill.billView.ToggleItemDeletability();
            }

            if (serverMode == false)
            {
                this.R_TransitionButtonGrid.Visibility = Visibility.Visible;
                S_clear_table_button.Visibility = Visibility.Hidden;
                S_exit_server_mode.Visibility = Visibility.Hidden;
                R_ReviewTitle.Text = "Review Bills";
            }
            else
            {
                this.R_ReviewTitle.Text = "Server View";
                this.S_clear_table_button.Visibility = Visibility.Visible;
                this.S_exit_server_mode.Visibility = Visibility.Visible;
            }
            this.R_EditButtonsGrid.Visibility = Visibility.Visible;
                this.R_BillA_DGrid.Visibility = Visibility.Visible;
                this.R_MoveButtonsGrid.Visibility = Visibility.Hidden;
                             
        }

        private void R_SplitButton_Click(object sender, RoutedEventArgs e)
        {
            this.R_TransitionButtonGrid.Visibility = Visibility.Hidden;
            this.R_EditButtonsGrid.Visibility = Visibility.Hidden;
            this.R_BillA_DGrid.Visibility = Visibility.Hidden;
            this.R_SplitButtonsGrid.Visibility = Visibility.Visible;
            this.S_clear_table_button.Visibility = Visibility.Hidden;
            this.S_exit_server_mode.Visibility = Visibility.Hidden;
            R_ReviewTitle.Text = "Click which food item to split";
            foreach (Bill bill in bills)
            {
                bill.billView.ToggleSplitMode();
                bill.billView.ToggleItemDeletability();
            }
            
        }

        private void R_FinishSplitButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.billView.ToggleSplitMode();
                bill.billView.ToggleItemDeletability();
            }
            if (serverMode == false)
            {
                this.R_TransitionButtonGrid.Visibility = Visibility.Visible;
                S_clear_table_button.Visibility = Visibility.Hidden;
                S_exit_server_mode.Visibility = Visibility.Hidden;
                R_ReviewTitle.Text = "Review Bills";
            }
            else
            {
                this.R_ReviewTitle.Text = "Server View";
                this.S_clear_table_button.Visibility = Visibility.Visible;
                this.S_exit_server_mode.Visibility = Visibility.Visible;
            }

            this.R_EditButtonsGrid.Visibility = Visibility.Visible;
            this.R_BillA_DGrid.Visibility = Visibility.Visible;
            this.R_SplitButtonsGrid.Visibility = Visibility.Hidden;
            
        }

        private void R_SendButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.billView.SendItems();
            }
            foreach (FoodItem item in orderedFoods)
            {
                if (item.itemSent)
                {
                }
                else
                {
                    item.itemSent = true;
                }
            }

            this.CommunicationGrid.Visibility = Visibility.Visible;

            this.R_CheckoutButton.Opacity = 1;
            this.R_CheckoutButton.IsEnabled = true;

            BlurEffect myBlurEffect = new BlurEffect
            {
                Radius = 10,
                KernelType = KernelType.Gaussian
            };
            this.ReviewGrid.Effect = myBlurEffect;

            this.ReviewGrid.IsEnabled = false;
            this.R_UpdateSendButtonStatus();
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
            selectedBIC = e.bic;
            billPosition = bills.IndexOf(selectedBIC.billControl.billLogic);
            this.S_BillUniformGrid.Children.RemoveAt(billPosition);
            this.BillSelectionGrid.Visibility = Visibility.Visible;

            //Temporary Solutions to disable too many instances

            BlurEffect myBlurEffect = new BlurEffect
            {
                Radius = 10,
                KernelType = KernelType.Gaussian
            };
            this.R_BillScroller.Effect = myBlurEffect;
            this.R_BillScroller.IsEnabled = false;
            //this.ReviewGrid.Effect = myBlurEffect;

           // this.ReviewGrid.IsEnabled = false;

            DropShadowEffect myDropShadow = new DropShadowEffect
            {
                BlurRadius = 10,
                Direction = 315,
                ShadowDepth = 5,
                Color = Colors.Black
            };
            this.S_BillUniformGrid.Effect = myDropShadow;
            
            this.R_ReviewTitle.Text = "Split " + selectedBIC.itemName +  " with...";

        }

        private void R_DeleteBillButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.billView.ToggleBillDeleteButton();
                bill.billView.ToggleItemDeletability();
            }
            this.R_EditButtonsGrid.Visibility = Visibility.Hidden;
            this.R_TransitionButtonGrid.Visibility = Visibility.Hidden;
            this.R_BillA_DGrid.Visibility = Visibility.Hidden;
            this.R_DButtonsGrid.Visibility = Visibility.Visible;
            this.S_clear_table_button.Visibility = Visibility.Hidden;
            this.S_exit_server_mode.Visibility = Visibility.Hidden;
            this.R_ReviewTitle.Text = "Click the 'x' to delete bill(s)";
        }

        private void R_DoneDeleting_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bill bill in bills)
            {
                bill.billView.ToggleBillDeleteButton();
                bill.billView.ToggleItemDeletability();
            }
            if (serverMode == false)
            {
                this.R_TransitionButtonGrid.Visibility = Visibility.Visible;
                S_clear_table_button.Visibility = Visibility.Hidden;
                S_exit_server_mode.Visibility = Visibility.Hidden;      
                this.R_ReviewTitle.Text = "Review Bills";
            }
            else
            {
                this.R_ReviewTitle.Text = "Server View";
                this.S_clear_table_button.Visibility = Visibility.Visible;
                this.S_exit_server_mode.Visibility = Visibility.Visible;
            }
            this.R_EditButtonsGrid.Visibility = Visibility.Visible;     
            this.R_BillA_DGrid.Visibility = Visibility.Visible;
            this.R_DButtonsGrid.Visibility = Visibility.Hidden;
            
        }

        //Add New Bill button
        private void R_AddBillButton_Click(object sender, RoutedEventArgs e)
        {
            numDinners++;
            Bill cBill = new Bill("New Bill");
            bills.Add(cBill);
            R_BillUniformGrid.Children.Add(cBill.billView);
            M_BillUniformGrid.Children.Add(cBill.m_BillView);
            S_BillUniformGrid.Children.Add(cBill.s_BillView);

            cBill.s_BillView.Selected += new EventHandler<EventArgs>(S_AddToSplitList);
            cBill.s_BillView.Unselected += new EventHandler<EventArgs>(S_RemoveFromSplitList);
            cBill.MenuBillClicked += new EventHandler<EventArgs>(M_BillClick);
            cBill.billView.Deleted += new EventHandler<EventArgs>(R_HandleBillDeletion);
            cBill.billView.SplitRequest += new EventHandler<BICEventArgs>(R_DisplaySelections);
        }

        //Handling of bill deletion
        private void R_HandleBillDeletion(object sender, EventArgs e)
        {
            Bill deletedBill = (sender as BillControl).billLogic;

            //Just Delete all FoodItems inside the bill 
            while (deletedBill.billView.ItemListGrid.Children.Count != 0)
            {
                BillItemControl bic = deletedBill.billView.ItemListGrid.Children[0] as BillItemControl;
                bic.Delete();
            }

            //Removing subscriptions and assignments
            numDinners--;
            bills.Remove(deletedBill);

            R_BillUniformGrid.Children.Remove(deletedBill.billView);
            M_BillUniformGrid.Children.Remove(deletedBill.m_BillView);
            S_BillUniformGrid.Children.Remove(deletedBill.s_BillView);

            deletedBill.s_BillView.Selected -= new EventHandler<EventArgs>(S_AddToSplitList);
            deletedBill.s_BillView.Unselected -= new EventHandler<EventArgs>(S_RemoveFromSplitList);
            deletedBill.MenuBillClicked -= new EventHandler<EventArgs>(M_BillClick);
            deletedBill.billView.Deleted -= new EventHandler<EventArgs>(R_HandleBillDeletion);
            deletedBill.billView.SplitRequest -= new EventHandler<BICEventArgs>(R_DisplaySelections);

            deletedBill = null;
        }

        /**********************************************************
        *************SERVER RELATTED BUTTTON FUNCTIONS*************
        **********************************************************/

        private void S_Dismiss_Click(object sender, RoutedEventArgs e)
        {
            S_Password.Clear();
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

        private void S_enter_click(object sender, RoutedEventArgs e)
        {
            if (S_Password.Password != "1234")
            {
                statusText.Text = "Incorrect Password";
            }
            else if (S_Password.Password == "1234")
            {
                statusText.Text = string.Empty;
                S_Password.Clear();
                ServerGrid.Visibility = Visibility.Hidden;
                enterServerMode();
            }
            else
            {
                statusText.Text = string.Empty;
            }
        }

        private void S_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            statusText.Text = string.Empty;
        }

        private void enterServerMode()
        {
            this.serverMode = true;
            R_TransitionButtonGrid.Visibility = Visibility.Hidden;
            this.R_ReviewTitle.Text = "Server View";
            S_exit_server_mode.Visibility = Visibility.Visible;
            foreach (Bill bill in bills)
            {
                bill.billView.PaidForButton.Visibility = Visibility.Visible;
                if (bill.transactionCompleted)
                {
                    R_BillUniformGrid.Children.Add(bill.billView);
                    M_BillUniformGrid.Children.Add(bill.m_BillView);
                    S_BillUniformGrid.Children.Add(bill.s_BillView);
                }
            }
            S_clear_table_button.Visibility = Visibility.Visible;

        }

        private void s_exit_server_mode(object sender, RoutedEventArgs e)
        {
            this.serverMode = false;
            R_TransitionButtonGrid.Visibility = Visibility.Visible;
            this.R_ReviewTitle.Text = "Review Bills";
            S_exit_server_mode.Visibility = Visibility.Hidden;

            foreach (Bill bill in bills)
            {
                bill.billView.PaidForButton.Visibility = Visibility.Hidden;
                if (bill.transactionCompleted)
                {
                    R_BillUniformGrid.Children.Remove(bill.billView);
                    M_BillUniformGrid.Children.Remove(bill.m_BillView);
                    S_BillUniformGrid.Children.Remove(bill.s_BillView);
                }
            }
            S_clear_table_button.Visibility = Visibility.Hidden;

        }

        /**********************************************************
        **************COMMUNICATION BUTTTON FUNCTIONS**************
        **********************************************************/

        private void C_DismissButton_Click(object sender, RoutedEventArgs e)
        {
            this.CommunicationGrid.Visibility = Visibility.Hidden;
            this.ReviewGrid.IsEnabled = true;

            BlurEffect myDeBlurEffect = new BlurEffect
            {
                Radius = 0,
                KernelType = KernelType.Gaussian
            };
            this.ReviewGrid.Effect = myDeBlurEffect;

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

            BlurEffect myDeBlurEffect = new BlurEffect
            {
                Radius = 0,
                KernelType = KernelType.Gaussian
            };
            this.R_BillScroller.Effect = myDeBlurEffect;
            this.R_BillScroller.IsEnabled = true;

            DropShadowEffect myUnDropShadow = new DropShadowEffect
            {
                BlurRadius = 0,
                Direction = 0,
                ShadowDepth = 0,
                Color = Colors.Black
            };
            this.S_BillUniformGrid.Effect = myUnDropShadow;

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

            BlurEffect myDeBlurEffect = new BlurEffect
            {
                Radius = 0,
                KernelType = KernelType.Gaussian
            };
            this.R_BillScroller.Effect = myDeBlurEffect;
            this.R_BillScroller.IsEnabled = true;

            DropShadowEffect myUnDropShadow = new DropShadowEffect
            {
                BlurRadius = 0,
                Direction = 0,
                ShadowDepth = 0,
                Color = Colors.Black
            };
            this.S_BillUniformGrid.Effect = myUnDropShadow;
            
            this.R_ReviewTitle.Text = "Click which food item to split";

            //Spliting Logic here
            selectedBIC.originalItem.SplitOrderEvenly(selectedBills, selectedBIC);

            //Mandatory unselection for clean up purposes
            foreach (Bill bill in selectedBills)
            {
                bill.s_BillView.Unselect();
            }

            this.S_BillUniformGrid.Children.Insert(billPosition, bills[billPosition].s_BillView);
            selectedBills.Clear();
        }

        /**********************************************************
        ********************Scrolling******************************
        **********************************************************/

        // Used when manually scrolling.
        private Point scrollStartPoint;
        private Point scrollStartOffset;
        private Boolean MenuBillScroller = false;
        //private Boolean ReviewScroller = false;
        private Boolean MenuScroller = false;
        private Boolean SplitItemsScroller = false;

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            MenuBillScroller = false;
            //ReviewScroller = false;
            MenuScroller = false;
            SplitItemsScroller = false;

            /*if (R_BillScroller.IsMouseOver)
            {
                ReviewScroller = true;
                // Save starting point, used later when determining how much to scroll.
                scrollStartPoint = e.GetPosition(this);
                scrollStartOffset.X = R_BillScroller.HorizontalOffset;
                scrollStartOffset.Y = R_BillScroller.VerticalOffset;

                // Update the cursor if can scroll or not.
                this.Cursor = (R_BillScroller.ExtentWidth > R_BillScroller.ViewportWidth) ||
                    (R_BillScroller.ExtentHeight > R_BillScroller.ViewportHeight) ?
                    Cursors.ScrollAll : Cursors.Arrow;

                this.CaptureMouse();
            }
            else if (!ReviewGrid.IsVisible && selectedMenuItems.IsMouseDirectlyOver)*/
            if (!ReviewGrid.IsVisible && selectedMenuItems.IsMouseDirectlyOver)
            {
                MenuScroller = true;
                scrollStartPoint = e.GetPosition(this);
                scrollStartOffset.X = selectedMenuItems.HorizontalOffset;
                scrollStartOffset.Y = selectedMenuItems.VerticalOffset;
                
                this.Cursor = (selectedMenuItems.ExtentWidth > selectedMenuItems.ViewportWidth) ||
                    (selectedMenuItems.ExtentHeight > selectedMenuItems.ViewportHeight) ?
                    Cursors.ScrollAll : Cursors.Arrow;

                this.CaptureMouse();
            }
            else if (!ReviewGrid.IsVisible && M_BillScroller.IsMouseDirectlyOver)
            {
                MenuBillScroller = true;
                scrollStartPoint = e.GetPosition(this);
                scrollStartOffset.X = M_BillScroller.HorizontalOffset;
                scrollStartOffset.Y = M_BillScroller.VerticalOffset;

                this.Cursor = (M_BillScroller.ExtentWidth > M_BillScroller.ViewportWidth) ||
                    (M_BillScroller.ExtentHeight > M_BillScroller.ViewportHeight) ?
                    Cursors.ScrollAll : Cursors.Arrow;

                this.CaptureMouse();
            }
            else if (BillSelectionGrid.IsVisible && S_Scroller.IsMouseDirectlyOver)
            {
                SplitItemsScroller = true;
                scrollStartPoint = e.GetPosition(this);
                scrollStartOffset.X = S_Scroller.HorizontalOffset;
                scrollStartOffset.Y = S_Scroller.VerticalOffset;

                this.Cursor = (S_Scroller.ExtentWidth > S_Scroller.ViewportWidth) ||
                    (S_Scroller.ExtentHeight > S_Scroller.ViewportHeight) ?
                    Cursors.ScrollAll : Cursors.Arrow;

                this.CaptureMouse();
            }


            base.OnPreviewMouseDown(e);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                // Get the new scroll position.
                Point point = e.GetPosition(this);

                // Determine the new amount to scroll.
                Point delta = new Point(
                    (point.X > this.scrollStartPoint.X) ?
                        -(point.X - this.scrollStartPoint.X) :
                        (this.scrollStartPoint.X - point.X),

                    (point.Y > this.scrollStartPoint.Y) ?
                        -(point.Y - this.scrollStartPoint.Y) :
                        (this.scrollStartPoint.Y - point.Y));

                /*if (ReviewScroller)
                {
                    // Scroll to the new position.
                    R_BillScroller.ScrollToHorizontalOffset(this.scrollStartOffset.X + delta.X);
                    R_BillScroller.ScrollToVerticalOffset(this.scrollStartOffset.Y + delta.Y);
                }
                else if (MenuScroller)
                */if (MenuScroller)
                {
                    selectedMenuItems.ScrollToHorizontalOffset(this.scrollStartOffset.X + delta.X);
                    selectedMenuItems.ScrollToVerticalOffset(this.scrollStartOffset.Y + delta.Y);
                }
                else if (MenuBillScroller)
                {
                    M_BillScroller.ScrollToHorizontalOffset(this.scrollStartOffset.X + delta.X);
                    M_BillScroller.ScrollToVerticalOffset(this.scrollStartOffset.Y + delta.Y);
                }
                else if (SplitItemsScroller)
                {
                    S_Scroller.ScrollToHorizontalOffset(this.scrollStartOffset.X + delta.X);
                    S_Scroller.ScrollToVerticalOffset(this.scrollStartOffset.Y + delta.Y);
                }

            }

            base.OnPreviewMouseMove(e);
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                this.Cursor = Cursors.Arrow;
                this.ReleaseMouseCapture();
            }

            base.OnPreviewMouseUp(e);
        }

        public void MurderAndReplace(MainWindow murdered)
        {
            murdered.Close();
            murdered = null;
        }

        private void ResetStateButton_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow nWindow = new MainWindow();
            nWindow.Show();
            nWindow.MurderAndReplace(this);
        }
    }
}