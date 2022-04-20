using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Raktarkezeles.Views;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Raktarkezeles
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
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
