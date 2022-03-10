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
        public DetailsPage(Part part)
        {
            InitializeComponent();
            this.BindingContext = new DetailsViewModel(Navigation, part);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as DetailsViewModel).OnAppearing();
        }
    }
}