using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Windows.Controls.Primitives;
using System.IO.IsolatedStorage;

namespace Glicious
{
    public partial class VenuesPage : PhoneApplicationPage
    {
        IsolatedStorageSettings appsettings = IsolatedStorageSettings.ApplicationSettings;
        public Menu menu;
        private DatePicker dPicker;
        Popup mealChange = new Popup();
       
        public VenuesPage()
        {
            InitializeComponent();

            (App.Current as App).inverted = IsLightTheme;
            if ((App.Current as App).inverted)
            {
                LayoutRoot.Background = new SolidColorBrush(Colors.White);
                textBlock1.Foreground = new SolidColorBrush(Colors.Black);
                PgTitle.Foreground = new SolidColorBrush(Colors.Black);
                meal.Foreground = new SolidColorBrush(Colors.Black);
                date.Foreground = new SolidColorBrush(Colors.Black);
                listBox.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                textBlock1.Foreground = new SolidColorBrush(Colors.White);
                PgTitle.Foreground = new SolidColorBrush(Colors.White);
                meal.Foreground = new SolidColorBrush(Colors.White);
                date.Foreground = new SolidColorBrush(Colors.White);
                listBox.Foreground = new SolidColorBrush(Colors.White);
            }


            mealChange.IsOpen = false;
            textBlock1.Visibility = Visibility.Visible;
            textBlock1.Text = "Loading menu, please wait.";
            ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton settings = new ApplicationBarIconButton();
            settings.IconUri = new Uri("/Images/settings.png", UriKind.Relative);
            settings.Text = "Settings";
            settings.Click += new EventHandler(settings_Click);
            ApplicationBarIconButton changeMeal = new ApplicationBarIconButton();
            changeMeal.IconUri = new Uri("/Images/change.png", UriKind.Relative);
            changeMeal.Text = "Change";
            changeMeal.Click += new EventHandler(changeMeal_Click);
            ApplicationBar.Buttons.Add(changeMeal);
            ApplicationBar.Buttons.Add(settings);

            dPicker = (App.Current as App).datePick;
            dPicker.ValueStringFormat = "{0:D}";
            date.Text = dPicker.ValueString;
            meal.Text = (App.Current as App).mealString;

            if (appsettings.Contains("vegan"))
            {
                (App.Current as App).ovoFilter = (bool)appsettings["ovolacto"];
                (App.Current as App).veganFilter = (bool)appsettings["vegan"];
            }

        }
        public bool IsLightTheme
        {
            get
            {
                return (Visibility)Resources["PhoneLightThemeVisibility"]
                    == Visibility.Visible;
            }
        }
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ignore scenarios when we navigate back to this page and clear what was previously selected
            if (listBox.SelectedItem != null)
            {
                Menu.Venue.Dish dummy = new Menu.Venue.Dish("", false, false, false, false, false, false);
                Type type = listBox.SelectedItem.GetType();
                Type type2 = dummy.GetType();
                if (type.FullName.Equals(type2.FullName)) 
                {
                    //Menu.Venue.Dish dummy2 = (Menu.Venue.Dish)listBox.SelectedItem;
                    //if (dummy2.hasNutrition)
                    //{
                    (App.Current as App).nutrDish = (Menu.Venue.Dish)listBox.SelectedItem;
                    NavigationService.Navigate(new Uri("/NutritionPage.xaml", UriKind.Relative));
                    //}
                }
                listBox.SelectedIndex = -1;
            }
        }

        private void OnNavigatedTo()
        {
            loadData();
        }


        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            try
            {
                listBox.Items.Clear();
                DateTime dTime = (DateTime)dPicker.Value;
                var webClient = new WebClient();
                String urlString = System.String.Format("http://tcdb.grinnell.edu/apps/glicious/{0}-{1}-{2}.json", dTime.Month, dTime.Day, dTime.Year);
                //String urlString = System.String.Format("http://www.cs.grinnell.edu/~tremblay/menu/{0}-{1}-{2}.json", dTime.Month, dTime.Day, dTime.Year);
                // System.Diagnostics.Debug.WriteLine(urlString);
                webClient.OpenReadAsync(new Uri(urlString));
                webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            }
            catch (Exception except)
            {
                textBlock1.Visibility = Visibility.Collapsed;
                listBox.Items.Add(new Menu.Venue("No menu available \nfor selected meal", null));
                Console.WriteLine("Web exception: {0}", except);
            }
        }

        void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                using (var reader = new StreamReader(e.Result))
                {
                    String json = reader.ReadToEnd();
                    JObject o = JObject.Parse(json);
                    Menu.Venue[] tempVens = new Menu.Venue[30];
                    int i = 0;
                    
                    String pass = (String)o["PASSOVER"];
                    if (pass.Equals("true"))
                        (App.Current as App).passover = true;
                    else
                        (App.Current as App).passover = false;


                    if (o[meal.Text.ToUpper()] != null && o[meal.Text.ToUpper()].HasValues)
                        foreach (JToken venue in o[meal.Text.ToUpper()].Children())
                        {
                            //TODO allocate smarter
                            Menu.Venue.Dish[] tempDishes = new Menu.Venue.Dish[60];
                            String temp = venue.ToString().Substring(1, 40);
                            String venName = temp.Remove(temp.IndexOf("\""));
                            int j = 0;
                            foreach (JToken dish in venue.Children().Children())
                            {
                                bool ovolacto, vegan, halal, passover, hasNutrition, gf;

                                String name = (String)dish["name"];

                                String ovo = (String)dish["ovolacto"];
                                if (ovo.Equals("true"))
                                    ovolacto = true;
                                else
                                    ovolacto = false;

                                String veg = (String)dish["vegan"];
                                if (veg.Equals("true"))
                                    vegan = true;
                                else
                                    vegan = false;

                                if ((App.Current as App).passover)
                                {
                                    String passO = (String)dish["passover"];
                                    if (passO.Equals("true"))
                                        passover = true;
                                    else
                                        passover = false;
                                }
                                else
                                    passover = false;

                                String hal = (String)dish["halal"];
                                if (hal.Equals("true"))
                                    halal = true;
                                else
                                    halal = false;

                                String glutenF = (String)dish["gluten_free"];
                                if (glutenF.Equals("true"))
                                    gf = true;
                                else
                                    gf = false;

                                Newtonsoft.Json.Linq.JValue dummy = new Newtonsoft.Json.Linq.JValue(false);
                                if (dummy.GetType().FullName.Equals(dish["nutrition"].GetType().FullName))
                                {
                                    hasNutrition = false;
                                    tempDishes[j++] = new Menu.Venue.Dish(name, hasNutrition, ovolacto, vegan, passover, halal, gf);
                                }
                                else
                                {
                                    hasNutrition = true;
                                    float[] nutrition = new float[20];
                                    int k = 0;
                                    foreach (JToken child in dish["nutrition"])
                                        nutrition[k++] = (float)child;
                                    tempDishes[j++] = new Menu.Venue.Dish(name, hasNutrition, ovolacto, vegan, passover, halal, gf, nutrition);
                                }

                            }
                            tempVens[i++] = new Menu.Venue(venName, tempDishes);
                        }
                    else
                        listBox.Items.Add(new Menu.Venue("No menu available \nfor selected meal", null));
                    menu = new Menu(tempVens);
                }
                textBlock1.Visibility = Visibility.Collapsed;

                
            bool pFlag, oFlag, vFlag, gfFlag, compositeBool;
            if ((App.Current as App).passover && (App.Current as App).passoverFilter)
                pFlag = true;
            else
                pFlag = false;
            if ((App.Current as App).ovoFilter)
                oFlag = true;
            else
                oFlag = false;
            if ((App.Current as App).veganFilter)
                vFlag = true;
            else
                vFlag = false;
            if ((App.Current as App).gfFilter)
                gfFlag = true;
            else
                gfFlag = false;

                foreach (Menu.Venue ven in menu.venues)
                    if (ven != null)
                    {
                        bool added = false;
                        listBox.Items.Add(ven);
                        foreach (Menu.Venue.Dish dish in ven.dishes)
                        {
                            if (dish != null)
                            {
                                if (pFlag)
                                    if (gfFlag && oFlag)
                                        compositeBool = (dish.passover && (dish.vegan || dish.ovolacto) && dish.gf);
                                    else if (gfFlag && vFlag)
                                        compositeBool = (dish.passover && dish.vegan && dish.gf);
                                    else if (oFlag)
                                        compositeBool = (dish.passover && (dish.vegan || dish.ovolacto));
                                    else if (vFlag)
                                        compositeBool = (dish.passover && dish.vegan);
                                    else if (gfFlag)
                                        compositeBool = (dish.passover && dish.gf);
                                    else
                                        compositeBool = dish.passover;
                                else
                                    if (gfFlag && oFlag)
                                        compositeBool = ((dish.vegan || dish.ovolacto) && dish.gf);
                                    else if (gfFlag && vFlag)
                                        compositeBool = (dish.vegan && dish.gf);
                                    else if (oFlag)
                                        compositeBool = (dish.vegan || dish.ovolacto);
                                    else if (vFlag)
                                        compositeBool = dish.vegan;
                                    else if (gfFlag)
                                        compositeBool = dish.gf;
                                    else
                                        compositeBool = true;

                                if (compositeBool)
                                {
                                    listBox.Items.Add(dish);
                                    added = true;
                                }
                            }
                        }
                        if (!added)
                            listBox.Items.Remove(ven);
                        else
                            listBox.Items.Add(new Menu.Venue("\t", null));
                    }
                   
            }
            catch (Exception except)
            {
                textBlock1.Visibility = Visibility.Collapsed;
                listBox.Items.Add(new Menu.Venue("No menu available \nfor selected meal", null));
                Console.WriteLine("Parsing exception: {0}", except);
            }
        }

        void filter()
        {
        }


        void settings_Click(object sender, EventArgs e)
        {
             NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        void popupEnd()
        {
            meal.Opacity = PgTitle.Opacity = date.Opacity = .9;
            listBox.Opacity = 1;
            mealChange.IsOpen = false;
        }

        void popupCancel_Click(object sender, EventArgs e)
        {
            popupEnd();   
        }

        void popupBFast_Click(object sender, EventArgs e)
        {
            if (!(App.Current as App).mealString.Equals("Breakfast"))
            {
                meal.Text = "Breakfast";
                (App.Current as App).mealString = "Breakfast"; 
                loadData();
            }
            popupEnd(); 
        }

        void popupLunch_Click(object sender, EventArgs e)
        {
            if (!(App.Current as App).mealString.Equals("Lunch"))
            {
                meal.Text = "Lunch";
                (App.Current as App).mealString = "Lunch"; 
                loadData();
            }
            popupEnd(); 
        }

        void popupDinner_Click(object sender, EventArgs e)
        {
            if (!(App.Current as App).mealString.Equals("Dinner"))
            {
                meal.Text = "Dinner";
                (App.Current as App).mealString = "Dinner"; 
                loadData();
            }
            popupEnd(); 
        }

        void popupOuttakes_Click(object sender, EventArgs e)
        {
            if (!(App.Current as App).mealString.Equals("Outtakes"))
            {
                meal.Text = "Outtakes";
                (App.Current as App).mealString = "Outtakes";
                loadData();
            }
            popupEnd(); 
        }

        void changeMeal_Click(object sender, EventArgs e)
        {
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.White);
            border.BorderThickness = new Thickness(2.0);

            StackPanel panel1 = new StackPanel();
            if ((App.Current as App).inverted)
                panel1.Background = new SolidColorBrush(Colors.LightGray);
            else
                panel1.Background = new SolidColorBrush(Colors.Gray);
            Button cancel = new Button();
            cancel.Content = "Cancel";
            cancel.Margin = new Thickness(0);
            cancel.Click += new RoutedEventHandler(popupCancel_Click);
            Button bFast = new Button();
            bFast.Content = "Breakfast";
            bFast.Margin = new Thickness(0);
            bFast.Click += new RoutedEventHandler(popupBFast_Click);
            Button lunch = new Button();
            lunch.Content = "Lunch";
            lunch.Margin = new Thickness(0);
            lunch.Click += new RoutedEventHandler(popupLunch_Click);
            Button dinner = new Button();
            dinner.Content = "Dinner";
            dinner.Margin = new Thickness(0);
            dinner.Click += new RoutedEventHandler(popupDinner_Click);
            Button outtakes = new Button();
            outtakes.Content = "Outtakes";
            outtakes.Margin = new Thickness(0);
            outtakes.Click += new RoutedEventHandler(popupOuttakes_Click);
            TextBlock textblock1 = new TextBlock();
            textblock1.Text = " Select meal:";
            textblock1.FontSize = 24;
            textblock1.Margin = new Thickness(10.0);
            panel1.Children.Add(textblock1);
            DateTime dTime = (DateTime)dPicker.Value;
            if ((dTime.DayOfWeek == DayOfWeek.Saturday) || (dTime.DayOfWeek == DayOfWeek.Sunday))
                if (dTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    panel1.Children.Add(lunch);
                    panel1.Children.Add(dinner);
                    panel1.Children.Add(cancel);
                }
                else
                {
                    panel1.Children.Add(bFast);
                    panel1.Children.Add(lunch);
                    panel1.Children.Add(dinner);
                    panel1.Children.Add(cancel);
                }
            else
            {
                panel1.Children.Add(bFast);
                panel1.Children.Add(lunch);
                panel1.Children.Add(dinner);
                panel1.Children.Add(outtakes);
                panel1.Children.Add(cancel);
            }
            border.Child = panel1;
            // Set the Child property of Popup to the border 
            // which contains a stackpanel, textblock and button.
            mealChange.Child = border;

            // Set where the popup will show up on the screen.
            mealChange.VerticalOffset = 200;
            mealChange.HorizontalOffset = 150;
            meal.Opacity = PgTitle.Opacity = date.Opacity = .75;
            listBox.Opacity = .25;
            // Open the popup.
            mealChange.IsOpen = true;
        }
    }
}