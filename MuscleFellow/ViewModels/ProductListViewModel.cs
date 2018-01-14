using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MuscleFellow.ViewModels
{
    public class ProductListViewModel : BaseViewModel
    {
        public ProductListViewModel()
        {
            _isBusy = true;
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProductViewModel> _items;

        public ObservableCollection<ProductViewModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

    }
}
