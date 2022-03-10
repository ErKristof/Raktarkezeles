using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Raktarkezeles.Models;
using Raktarkezeles.Views;
using Xamarin.Forms;
using System.Windows.Input;
using Raktarkezeles.DAL;

namespace Raktarkezeles.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Part> parts = PartContext.GetParts();

        public ObservableCollection<Part> Parts
        {
            get
            {
                return parts;
            }
            set
            {
                parts = value;
                OnPropertyChanged(nameof(Parts));
            }
        }

        public ICommand GoToNewPartCommand { protected set; get; }

        public MainViewModel(INavigation navigation) : base(navigation)
        {
            
            GoToNewPartCommand = new Command(GoToNewPartCommandExecute);
        }
        private async void GoToNewPartCommandExecute()
        {
            await Navigation.PushAsync(new NewPartPage(), true);
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            Parts = PartContext.GetParts();
        }
    }
}
