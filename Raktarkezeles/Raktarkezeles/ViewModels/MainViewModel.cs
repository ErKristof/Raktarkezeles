using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
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
        private ObservableCollection<Part> parts = new ObservableCollection<Part>();
        public ObservableCollection<Part> Parts
        {
            get
            {
                return parts;
            }
            set
            {
                parts = value;
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
        private string searchText;
        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                if(searchText != value)
                {
                    searchText = value;
                    SearchPartsCommandExecute(searchText);
                    OnPropertyChanged();
                }
            }
        }

        public ICommand GoToNewPartCommand { protected set; get; }
        public ICommand GoToDetailsCommand { protected set; get; }

        public MainViewModel()
        {
            GoToDetailsCommand = new Command(GoToDetailsCommandExecute);
            GoToNewPartCommand = new Command(GoToNewPartCommandExecute);
            foreach(Part p in PartContext.GetParts())
            {
                Parts.Add(p);
            }
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
        private void SearchPartsCommandExecute(string input)
        {
            Parts.Clear();

            var containedlist = PartContext.GetFilteredList(input);
            foreach(Part p in containedlist)
            {
                Parts.Add(p);
            }
            OnPropertyChanged(nameof(Parts));
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            Parts.Clear();
            foreach(Part p in PartContext.GetParts())
            {
                Parts.Add(p);
            }
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
