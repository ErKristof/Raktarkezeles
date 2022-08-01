using Raktarkezeles.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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