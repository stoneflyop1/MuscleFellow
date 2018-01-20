using System;
using System.Collections.Generic;
using MuscleFellow.ViewModels;
using Xamarin.Forms;

namespace MuscleFellow.Views
{
    public partial class MyPage : ContentPage
    {
        public MyPage()
        {
            InitializeComponent();
            BindingContext = new MyViewModel();
        }
    }
}
