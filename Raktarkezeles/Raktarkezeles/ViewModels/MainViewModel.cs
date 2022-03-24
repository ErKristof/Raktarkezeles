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
using Raktarkezeles.MVVM;

namespace Raktarkezeles.ViewModels
{
    public class MainViewModel : BindableBase
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
                OnPropertyChanged();
            }
        }
        private Part selectedPart;
        public Part SelectedPart
        {
            get
            {
                return selectedPart;
            }
            set
            {
                selectedPart = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoToNewPartCommand { protected set; get; }
        public ICommand GoToDetailsCommand { protected set; get; }

        public MainViewModel()
        {
            GoToDetailsCommand = new Command(GoToDetailsCommandExecute);
            GoToNewPartCommand = new Command(GoToNewPartCommandExecute);
        }
        private async void GoToDetailsCommandExecute()
        {
            if (selectedPart != null)
            {
                DetailsViewModel detailsVM = new DetailsViewModel(selectedPart.Id);
                DetailsPage detailsPage = new DetailsPage();
                detailsPage.BindingContext = detailsVM;
                await Application.Current.MainPage.Navigation.PushAsync(detailsPage);
                SelectedPart = null;
            }
        }
        private async void GoToNewPartCommandExecute()
        {
            NewPartViewModel newVM = new NewPartViewModel();
            NewPartPage newPage = new NewPartPage();
            newPage.BindingContext = newVM;
            await Application.Current.MainPage.Navigation.PushAsync(newPage);
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            Parts = PartContext.GetParts();
            foreach (Part p in Parts)
            {
                int sum = 0;
                foreach (Occurrence o in p.Occurrences)
                {
                    sum += o.Quantity;
                }
                p.Quantity = sum;
            }
        }
    }
}
