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
using Microsoft.Phone.Shell;

namespace Glicious
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        IsolatedStorageSettings appsettings = IsolatedStorageSettings.ApplicationSettings;
        public SettingsPage()
        {
            InitializeComponent();
            (App.Current as App).inverted = IsLightTheme;
            if ((App.Current as App).inverted)
            {
                gradStart.Color = Colors.White;
                gradStop.Color = Colors.Black;
                Glicious.Foreground = new SolidColorBrush(Colors.Black);
                PgTitle.Foreground = new SolidColorBrush(Colors.Black);
                textBlock1.Foreground = new SolidColorBrush(Colors.Black);
            }

            ovolactoBox.IsChecked = (App.Current as App).ovoFilter;
            veganBox.IsChecked = (App.Current as App).veganFilter; 
            gfBox.IsChecked = (App.Current as App).gfFilter;
            if ((App.Current as App).passover)
            {
                passoverBox.Visibility = Visibility.Visible;
                passoverBox.IsChecked = (App.Current as App).passoverFilter;
            }
            else
                passoverBox.Visibility = Visibility.Collapsed;
        }
        public bool IsLightTheme
        {
            get
            {
                return (Visibility)Resources["PhoneLightThemeVisibility"] == Visibility.Visible;
            }
        }
        void save_Click(object sender, EventArgs e)
        {
            if (appsettings.Contains("ovolacto"))
                appsettings.Remove("ovolacto"); 
            if (appsettings.Contains("vegan"))
                appsettings.Remove("vegan");
            if (appsettings.Contains("gf"))
                appsettings.Remove("gf");
            if (appsettings.Contains("passover"))
                appsettings.Remove("passover");
            appsettings.Add("ovolacto", ovolactoBox.IsChecked);
            appsettings.Add("vegan", veganBox.IsChecked);
            appsettings.Add("gf", gfBox.IsChecked);
            appsettings.Add("passover", passoverBox.IsChecked);
            (App.Current as App).ovoFilter = (bool)ovolactoBox.IsChecked;
            (App.Current as App).veganFilter = (bool)veganBox.IsChecked;
            (App.Current as App).gfFilter = (bool)gfBox.IsChecked;
            (App.Current as App).passoverFilter = (bool)passoverBox.IsChecked;
            appsettings.Save();
            NavigationService.GoBack();
        }

        void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}