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
using System.IO.IsolatedStorage;

namespace Glicious
{
    public partial class NutritionPage : PhoneApplicationPage
    {
        private Menu.Venue.Dish dish;
        public NutritionPage()
        {
            InitializeComponent();
            (App.Current as App).inverted = IsLightTheme;
            if ((App.Current as App).inverted)
            {
                gradStart.Color = Colors.White;
                gradStop.Color = Colors.Black;
                grad2start.Color = Colors.Black;
                grad2stop.Color = Colors.DarkGray;
                Glicious.Foreground = new SolidColorBrush(Colors.Black);
                PgTitle.Foreground = new SolidColorBrush(Colors.Black);
                dishName.Foreground = new SolidColorBrush(Colors.Black);
            }
            
            dish = (App.Current as App).nutrDish;
            dishName.Visibility = Visibility.Visible;
            dishName.Text = dish.name;
            if (dish.hasNutrition)
            {
                nutrTxt.Text = "Calories\nSaturated Fat\nTrans Fat\nCholesterol\nSodium\nDietary Fiber\nSugar\nProtein";
              
                String temp = System.String.Format("{0}\n{1}g\n{2}g\n{3}mg\n{4}mg\n{5}g\n{6}g\n{7}g",
                    dish.nutrition[0], dish.nutrition[4], dish.nutrition[14], dish.nutrition[7], dish.nutrition[11], dish.nutrition[8], dish.nutrition[19], dish.nutrition[3]);
                nutrVals.Text = temp;
                nutrVals.FontSize = nutrTxt.FontSize = 35;
            }
            else
            {
                nutrTxt.FontSize = 44;
                nutrTxt.Text = "No nutritional \ninformation is \ncurrently available \nfor this dish.";
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
    }
}