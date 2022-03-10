using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Raktarkezeles.ViewModels;
using Raktarkezeles.Models;

namespace Raktarkezeles.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel(Navigation);
        }

        private void PartList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            Navigation.PushAsync(new DetailsPage((Part)e.Item));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as MainViewModel).OnAppearing();
        }
    }
}