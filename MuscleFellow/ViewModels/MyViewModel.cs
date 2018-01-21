using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MuscleFellow.Utils;
using Xamarin.Forms;

namespace MuscleFellow.ViewModels
{
    public class MyViewModel : BaseViewModel
    {
        public MyViewModel()
        {
            _login = new Command(async obj =>
            {
                await LoginAsync();
            }, CanExecute);
        }

        private async Task LoginAsync()
        {
            var err = await ApiClient.Default.LoginAsync(UserName, Password);
            if (String.IsNullOrEmpty(err))
            {
                LoggedIn = true;
                _login.ChangeCanExecute();
            }
        }

        private bool CanExecute(object obj)
        {
            return !LoggedIn;
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

        private Command _login;

        public ICommand Login
        {
            get { return _login; }
        }

        private bool _loggedIn;

        public bool LoggedIn
        {
            get { return _loggedIn; }
            set
            {
                _loggedIn = value;
                OnPropertyChanged();
            }
        }
    }
}
