using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Glicious
{
    public class Menu
    {
        public Menu(Venue[] vens)
        {
            venues = vens;
        }
        public Venue[] venues { get; set; }
        public class Venue
        {
            public Venue (String nm, Dish[] dishAr)
            {
                name = nm;
                dishes = dishAr;
            }

            public String name { get; set; }
            public Dish[] dishes { get; set; }
            public class Dish
            {
                public Dish(String nm, bool hasN, bool ovo, bool veg, bool passO, bool hal, bool glutenF, float[] nutr)
                {
                    name = nm;
                    hasNutrition = hasN;
                    ovolacto = ovo;
                    vegan = veg;
                    passover = passO;
                    halal = hal;
                    gf = glutenF;
                    nutrition = nutr;
                }
                public Dish(String nm, bool hasN, bool ovo, bool veg, bool passO, bool hal, bool glutenF)
                {
                    name = nm;
                    hasNutrition = hasN;
                    ovolacto = ovo;
                    vegan = veg;
                    passover = passO;
                    halal = hal;
                    gf = glutenF;
                }
                public String name { get; set; }
                public bool hasNutrition { get; set; }
                public bool ovolacto { get; set; }
                public bool vegan { get; set; }
                public bool passover { get; set; }
                public bool halal { get; set; }
                public bool gf { get; set; }
                public float[] nutrition { get; set; }
            }
        }
    }
}
