using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Raktarkezeles.Views;

namespace Raktarkezeles
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new DetailsPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
