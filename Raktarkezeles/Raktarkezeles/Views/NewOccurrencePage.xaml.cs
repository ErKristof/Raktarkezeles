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
    public partial class NewOccurrencePage : ContentPage
    {
        public NewOccurrencePage(Part part)
        {
            InitializeComponent();
            this.BindingContext = new NewOccurrenceViewModel(part);
        }
    }
}