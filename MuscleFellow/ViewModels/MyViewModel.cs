using System;
namespace MuscleFellow.ViewModels
{
    public class MyViewModel : BaseViewModel
    {
        public MyViewModel()
        {
        }

        private string _userName { get; set; }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
    }
}
