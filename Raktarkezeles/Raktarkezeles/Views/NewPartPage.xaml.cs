using Raktarkezeles.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Raktarkezeles.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPartPage : ContentPage
    {
        public NewPartPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            bool result = (BindingContext as NewPartViewModel).OnBackButtonPressed();
            base.OnBackButtonPressed();
            return result;
        }
    }
}