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
using System.IO;


namespace Glicious
{
    public partial class MainPage : PhoneApplicationPage
    {

        private string daysAvailableTXT;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            (App.Current as App).inverted = IsLightTheme;
            if ((App.Current as App).inverted)
            {
                LayoutRoot.Background = new SolidColorBrush(Colors.White);
                textBlock1.Foreground = new SolidColorBrush(Colors.Black);
                PgTitle.Foreground = new SolidColorBrush(Colors.Black);
                datePicker.Foreground = new SolidColorBrush(Colors.White);
                datePicker.Background = new SolidColorBrush(Colors.Black);
            }
            else
            {
                LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                textBlock1.Foreground = new SolidColorBrush(Colors.White);
                PgTitle.Foreground = new SolidColorBrush(Colors.White);
                datePicker.Foreground = new SolidColorBrush(Colors.Black);
                datePicker.Background = new SolidColorBrush(Colors.White);
            }
           
            hideAllButtons();
            textBlock1.Text = "Loading menus, please wait.";

            changeColors(bfastButton);
            changeColors(lunchButton);
            changeColors(dinnerButton);
            changeColors(outtakesButton);

            this.datePicker.ValueChanged += new EventHandler<DateTimeValueChangedEventArgs>(picker_ValueChanged);
        }

        private void changeColors(Button b)
        {
            if ((App.Current as App).inverted)
            {
                b.BorderBrush = new SolidColorBrush(Colors.Black);
                b.Background = new SolidColorBrush(Colors.White);
                b.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                b.BorderBrush = new SolidColorBrush(Colors.White);
                b.Background = new SolidColorBrush(Colors.Black);
                b.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        public bool IsLightTheme
        {
            get
            {
                return (Visibility)Resources["PhoneLightThemeVisibility"] == Visibility.Visible;
            }
        }

        private void hideAllButtons()
        {
            outtakesButton.Visibility = Visibility.Collapsed;
            dinnerButton.Visibility = Visibility.Collapsed;
            lunchButton.Visibility = Visibility.Collapsed;
            bfastButton.Visibility = Visibility.Collapsed;
            textBlock1.Visibility = Visibility.Visible;
        }

        private void checkButtons(DateTime date)
        {
            textBlock1.Visibility = Visibility.Collapsed;
            if (date.DayOfWeek == DayOfWeek.Sunday)
                bfastButton.Visibility = Visibility.Collapsed;
            else
                bfastButton.Visibility = Visibility.Visible;
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                outtakesButton.Visibility = Visibility.Collapsed;
            else
                outtakesButton.Visibility = Visibility.Visible;
            dinnerButton.Visibility = Visibility.Visible;
            lunchButton.Visibility = Visibility.Visible;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var webClient = new WebClient();
                webClient.OpenReadAsync(new Uri("http://tcdb.grinnell.edu/apps/glicious/available_days.php"));
                //webClient.OpenReadAsync(new Uri("http://www.cs.grinnell.edu/~tremblay/menu/available_days_FAKE.php"));
                webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            }
            catch (Exception except)
            {
                datePicker.Value = DateTime.Today;
                hideAllButtons();
                textBlock1.Text = "No menus are currently available.\nPlease check your network connection.";
            }
        }

        void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                using (var reader = new StreamReader(e.Result))
                {
                    daysAvailableTXT = reader.ReadToEnd();
                    DateTime newTime = (DateTime)datePicker.Value;
                    if (daysAvailableTXT.Equals("-1"))
                    {
                        datePicker.Value = DateTime.Today;
                        hideAllButtons();
                        textBlock1.Text = "No menus are currently available.";
                    }
                    else
                    {
                        String s;
                        if (newTime.Year != DateTime.Today.Year)
                        {
                            s = System.String.Format("No menus are available for the selected date.\nThere are only {0} days after today available.", daysAvailableTXT);
                            textBlock1.Text = s;
                        }
                        else
                        {
                            if (newTime.DayOfYear < DateTime.Today.DayOfYear)
                            {
                                newTime = DateTime.Today;
                                datePicker.Value = newTime;
                                checkButtons(newTime);
                            }
                            else if ((DateTime.Today.DayOfYear + Int32.Parse(daysAvailableTXT)) < newTime.DayOfYear)
                            {
                                hideAllButtons();
                                if (daysAvailableTXT.Equals("0"))
                                    s = "No menus are available for the selected date.\nToday's menu is the only available menu.";
                                else if (daysAvailableTXT.Equals("1"))
                                    s = "No menus are available for the selected date.\nThere is only 1 day after today available.";
                                else
                                    s = System.String.Format("No menus are available for the selected date.\nThere are only {0} days after today available.", daysAvailableTXT);
                                textBlock1.Text = s;
                            }
                            else
                                checkButtons(newTime);
                        }
                    }
                }
            }
            catch (Exception except)
            {
                datePicker.Value = DateTime.Today;
                hideAllButtons();
                textBlock1.Text = "No menus are currently available.\nPlease check your network connection.";
            }
        }
 
        void picker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            hideAllButtons();
            textBlock1.Text = "";
        }


        private void buttonClick()
        {
            datePicker.ValueStringFormat = "{0:D}";
            (App.Current as App).datePick = datePicker;
            NavigationService.Navigate(new Uri("/VenuesPage.xaml", UriKind.Relative));
        }

        private void bfastButton_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).mealString = "Breakfast";
            buttonClick();
        }

        private void lunchButton_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).mealString = "Lunch";
            buttonClick();
        }

        private void dinnerButton_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).mealString = "Dinner";
            buttonClick();
        }

        private void outtakesButton_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).mealString = "Outtakes";
            buttonClick();
        }
    }
}