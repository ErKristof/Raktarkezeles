using Raktarkezeles.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Raktarkezeles.Models;

namespace Raktarkezeles.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as DetailsViewModel).OnAppearing();
        }
        protected override void OnDisappearing()
        {
            (BindingContext as DetailsViewModel).OnDisappearing();
            base.OnDisappearing();
        }
    }
}