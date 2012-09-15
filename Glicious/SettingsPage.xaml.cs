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
                LayoutRoot.Background = new SolidColorBrush(Colors.White);
                Glicious.Foreground = new SolidColorBrush(Colors.Black);
                PgTitle.Foreground = new SolidColorBrush(Colors.Black);
                textBlock1.Foreground = new SolidColorBrush(Colors.Black);
                veganBox.Foreground = new SolidColorBrush(Colors.Black);
                ovolactoBox.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                LayoutRoot.Background = new SolidColorBrush(Colors.Black);
                Glicious.Foreground = new SolidColorBrush(Colors.White);
                PgTitle.Foreground = new SolidColorBrush(Colors.White);
                textBlock1.Foreground = new SolidColorBrush(Colors.White);
                veganBox.Foreground = new SolidColorBrush(Colors.White);
                ovolactoBox.Foreground = new SolidColorBrush(Colors.White);
            }
            ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Images/save.png", UriKind.Relative);
            save.Text = "Save";
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(save_Click);
            ApplicationBarIconButton cancel = new ApplicationBarIconButton();
            cancel.IconUri = new Uri("/Images/close.png", UriKind.Relative);
            cancel.Text = "Cancel";
            ApplicationBar.Buttons.Add(cancel);
            cancel.Click += new EventHandler(cancel_Click);

            ovolactoBox.IsChecked = (App.Current as App).ovoFilter;
            veganBox.IsChecked = (App.Current as App).veganFilter; 
            gfBox.IsChecked = (App.Current as App).gfFilter;
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
            appsettings.Add("ovolacto", ovolactoBox.IsChecked);
            appsettings.Add("vegan", veganBox.IsChecked);
            appsettings.Add("gf", gfBox.IsChecked);
            (App.Current as App).ovoFilter = (bool)ovolactoBox.IsChecked;
            (App.Current as App).veganFilter = (bool)veganBox.IsChecked;
            (App.Current as App).gfFilter = (bool)gfBox.IsChecked;
            appsettings.Save();
            NavigationService.GoBack();
        }

        void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}