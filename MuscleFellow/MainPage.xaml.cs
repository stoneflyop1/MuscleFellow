using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MuscleFellow
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            Children.Add(new Views.ProductsPage());
            Children.Add(new Views.MyPage());
        }
    }
}
