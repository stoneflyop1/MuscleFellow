using Xamarin.Forms;

namespace MuscleFellow
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
        /// <summary>
        /// Test Host Address
        /// </summary>
        /// <value>The webapi host.</value>
        public static string Host { get; set; }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
