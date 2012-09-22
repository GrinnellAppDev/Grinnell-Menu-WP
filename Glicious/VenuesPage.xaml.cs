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
        private bool datepickerBool, firstTime, settingsBool;

        public VenuesPage()
        {
            InitializeComponent();
            (App.Current as App).inverted = IsLightTheme;
            if ((App.Current as App).inverted)
            {
                gradStart.Color = Colors.White;
                gradStop.Color = Colors.Black;
                grad2start.Color = Colors.Black;
                grad2stop.Color = Colors.DarkGray;
                PgTitle.Foreground = new SolidColorBrush(Colors.Black);
                meal.Foreground = new SolidColorBrush(Colors.Black);
                date.Foreground = new SolidColorBrush(Colors.Black);
                datePicker.Foreground = new SolidColorBrush(Colors.Black);
                datePicker.Background = new SolidColorBrush(Colors.White);
                border1.BorderBrush = new SolidColorBrush(Colors.White);
            }
            ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton settings = new ApplicationBarIconButton();
            settings.IconUri = new Uri("/Images/appbar.feature.settings.rest.png", UriKind.Relative);
            settings.Text = "Settings";
            settings.Click += new EventHandler(settings_Click);    
            ApplicationBar.Buttons.Add(settings);

            datePicker.Value = DateTime.Now;
            datePicker.ValueStringFormat = "{0:D}";
            if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                ApplicationBarMenuItem bfastBar = new ApplicationBarMenuItem("Breakfast");
                bfastBar.Click += new EventHandler(bFast_Click);
                ApplicationBar.MenuItems.Add(bfastBar);
            }
            ApplicationBarMenuItem lunchBar = new ApplicationBarMenuItem("Lunch");
            ApplicationBarMenuItem dinnerBar = new ApplicationBarMenuItem("Dinner");
            lunchBar.Click += new EventHandler(lunch_Click);
            dinnerBar.Click += new EventHandler(dinner_Click);
            ApplicationBar.MenuItems.Add(lunchBar);
            ApplicationBar.MenuItems.Add(dinnerBar);
            if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday && DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
            {
                ApplicationBarMenuItem outtakesBar = new ApplicationBarMenuItem("Outtakes");
                outtakesBar.Click += new EventHandler(outtakes_Click);
                ApplicationBar.MenuItems.Add(outtakesBar);
            }

            if (appsettings.Contains("vegan"))
            {
                (App.Current as App).ovoFilter = (bool)appsettings["ovolacto"];
                (App.Current as App).veganFilter = (bool)appsettings["vegan"];
                (App.Current as App).passoverFilter = (bool)appsettings["passover"];
                (App.Current as App).gfFilter = (bool)appsettings["gf"];
            }
            this.datePicker.ValueChanged += new EventHandler<DateTimeValueChangedEventArgs>(picker_ValueChanged);
            firstTime = datepickerBool = true;
            settingsBool = false;
        }

        public bool IsLightTheme
        {
            get
            {
                return (Visibility)Resources["PhoneLightThemeVisibility"] == Visibility.Visible;
            }
        }

        private void OnNavigatedTo()
        {
            datepickerBool = true;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (firstTime || (!datepickerBool && settingsBool))
            {
                listBox.Items.Clear();
                textBlock1.Visibility = Visibility.Visible;
                textBlock1.Text = "Loading menu, please wait.";
                settingsBool = false;
                loadDataPrecursor();
            }
        }

        //Check if the selected date is valid, etc.
        void loadDataPrecursor()
        {
            listBox.Items.Clear();
            textBlock1.Visibility = Visibility.Visible;
            textBlock1.Text = "Loading menu, please wait.";
            ApplicationBar.MenuItems.Clear();
            DateTime dTime = (DateTime)datePicker.Value;
            if (dTime.DayOfWeek != DayOfWeek.Sunday)
            {
                ApplicationBarMenuItem bfastBar = new ApplicationBarMenuItem("Breakfast");
                bfastBar.Click += new EventHandler(bFast_Click);
                ApplicationBar.MenuItems.Add(bfastBar);
            }
            ApplicationBarMenuItem lunchBar = new ApplicationBarMenuItem("Lunch");
            ApplicationBarMenuItem dinnerBar = new ApplicationBarMenuItem("Dinner");
            lunchBar.Click += new EventHandler(lunch_Click);
            dinnerBar.Click += new EventHandler(dinner_Click);
            ApplicationBar.MenuItems.Add(lunchBar);
            ApplicationBar.MenuItems.Add(dinnerBar);
            if (dTime.DayOfWeek != DayOfWeek.Sunday && dTime.DayOfWeek != DayOfWeek.Saturday)
            {
                ApplicationBarMenuItem outtakesBar = new ApplicationBarMenuItem("Outtakes");
                outtakesBar.Click += new EventHandler(outtakes_Click);
                ApplicationBar.MenuItems.Add(outtakesBar);
            }
            try
            {
                var webClient = new WebClient();
                webClient.OpenReadAsync(new Uri("http://tcdb.grinnell.edu/apps/glicious/last_date.json"));
                webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(precursorCompleted);
            }
            catch (Exception except)
            {
                datePicker.Value = DateTime.Today;
                textBlock1.Text = "No menus are currently available.\nPlease check your network connection.";
                Console.WriteLine("Web exception: {0}", except);
            }
        }

        void precursorCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                using (var reader = new StreamReader(e.Result))
                {
                    String lastDateStr = reader.ReadToEnd();
                    lastDateStr = lastDateStr.Replace("{\"Last_Day\":\"", "");
                    lastDateStr = lastDateStr.Replace("\"}", "");
                    lastDateStr = lastDateStr.Replace("-", "/");
                    DateTime lastDate = DateTime.Parse(lastDateStr);
                    DateTime selectedTime = (DateTime)datePicker.Value;

                    if (DateTime.Compare(DateTime.Today, lastDate) > 0)
                    {
                        datePicker.Value = DateTime.Today;
                        textBlock1.Text = "No menus are currently available.";
                        textBlock1.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        String s;
                        if (DateTime.Compare(selectedTime, lastDate) > 0)
                        {
                            s = System.String.Format("No menus are available for the selected date.\n{0} is the \nlast date available.", lastDate.ToLongDateString());
                            textBlock1.Text = s;
                            textBlock1.Visibility = Visibility.Visible;
                        }
                        else if (DateTime.Compare(selectedTime, DateTime.Today) < 0)
                        {
                            datePicker.Value = DateTime.Today;
                            if (datepickerBool)
                                if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                                    meal.Text = "Lunch";
                                else
                                    meal.Text = "Breakfast";
                            datepickerBool = true;
                            textBlock1.Visibility = Visibility.Collapsed;
                            loadData();
                        }
                        else
                        {
                            if (firstTime)
                            {
                                firstTime = false;
                                if (DateTime.Now.Hour < 10 && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                                    meal.Text = "Breakfast";
                                else if (DateTime.Now.Hour < 1 || (DateTime.Now.Hour < 2 && DateTime.Now.Minute < 30))
                                    meal.Text = "Lunch";
                                else if (DateTime.Now.Hour < 19)
                                    meal.Text = "Dinner";
                                else if (DateTime.Now.Hour < 20 && DateTime.Now.DayOfWeek != DayOfWeek.Friday
                                    && DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                                    meal.Text = "Dinner";
                                else
                                {
                                    meal.Text = "Breakfast";
                                    datePicker.Value = DateTime.Now.AddDays(1);
                                }
                            }
                            else if (datepickerBool)
                                if (selectedTime.DayOfWeek == DayOfWeek.Sunday)
                                    meal.Text = "Lunch";
                                else
                                    meal.Text = "Breakfast";
                            datepickerBool = true;
                            textBlock1.Visibility = Visibility.Collapsed;
                            loadData();
                        }
                    }
                }
                datepickerBool = true;
            }
            catch (Exception except)
            {
                datepickerBool = true;
                datePicker.Value = DateTime.Today;
                textBlock1.Text = "No menus are currently available.\nPlease check your network connection.";
                textBlock1.Visibility = Visibility.Visible;
                Console.WriteLine("Parsing exception: {0}", except);
            }
        }

        //Get the data from the json into the listbox
        private void loadData()
        {
            try
            {
                listBox.Items.Clear();
                DateTime dTime = (DateTime)datePicker.Value;
                var webClient = new WebClient();
                String urlString = System.String.Format("http://tcdb.grinnell.edu/apps/glicious/{0}-{1}-{2}.json", dTime.Month, dTime.Day, dTime.Year);
                //String urlString = System.String.Format("http://www.cs.grinnell.edu/~tremblay/menu/{0}-{1}-{2}.json", dTime.Month, dTime.Day, dTime.Year);
                // System.Diagnostics.Debug.WriteLine(urlString);
                webClient.OpenReadAsync(new Uri(urlString));
                webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(loadCompleted);
            }
            catch (Exception except)
            {
                textBlock1.Visibility = Visibility.Collapsed;
                listBox.Items.Add(new Menu.Venue("No menu available \nfor selected meal.\n An error occurred.", null));
                Console.WriteLine("Web exception: {0}", except);
            }
        }

        void loadCompleted(object sender, OpenReadCompletedEventArgs e)
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
                    {
                        listBox.Items.Add(new Menu.Venue("No menu available \nfor selected meal.", null));
                        return;
                    }
                    menu = new Menu(tempVens);
                }

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
                textBlock1.Visibility = Visibility.Collapsed;
            }
            catch (Exception except)
            {
                textBlock1.Visibility = Visibility.Collapsed;
                listBox.Items.Add(new Menu.Venue("No menu available \nfor selected meal", null));
                Console.WriteLine("Parsing exception: {0}", except);
            }
        }

        void settings_Click(object sender, EventArgs e)
        {
            datepickerBool = false;
            settingsBool = true;
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        void bFast_Click(object sender, EventArgs e)
        {
            if (!meal.Text.Equals("Breakfast"))
            {
                meal.Text = "Breakfast";
                datepickerBool = false;
                loadDataPrecursor();
            }
        }

        void lunch_Click(object sender, EventArgs e)
        {
            if (!meal.Text.Equals("Lunch"))
            {
                meal.Text = "Lunch";
                datepickerBool = false;
                loadDataPrecursor();
            }
        }

        void dinner_Click(object sender, EventArgs e)
        {
            if (!meal.Text.Equals("Dinner"))
            {
                meal.Text = "Dinner";
                datepickerBool = false;
                loadDataPrecursor();
            }
        }

        void outtakes_Click(object sender, EventArgs e)
        {
            if (!meal.Text.Equals("Outtakes"))
            {
                meal.Text = "Outtakes";
                datepickerBool = false;
                loadDataPrecursor();
            }
        }

        private void picker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            loadDataPrecursor();
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
                    Menu.Venue.Dish dummy2 = (Menu.Venue.Dish)listBox.SelectedItem;
                    if (dummy2.hasNutrition)
                    {
                        (App.Current as App).nutrDish = (Menu.Venue.Dish)listBox.SelectedItem;
                        datepickerBool = false;
                        NavigationService.Navigate(new Uri("/NutritionPage.xaml", UriKind.Relative));
                    }
                }
                listBox.SelectedIndex = -1;
            }
        }
    }
}