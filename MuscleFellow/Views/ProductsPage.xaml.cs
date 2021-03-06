﻿using System.Threading.Tasks;
using MuscleFellow.ViewModels;
using Xamarin.Forms;

namespace MuscleFellow.Views
{
    public partial class ProductsPage : ContentPage
    {
        public ProductsPage()
        {
            InitializeComponent();
            var vm = new ProductListViewModel();
            BindingContext = vm;
            Task.Run(async () =>
            {
                vm.Items = new System.Collections.ObjectModel.ObservableCollection<ProductViewModel>(await ApiClient.Default.GetProductsAsync());
                vm.IsBusy = false;
            });
        }
    }
}
