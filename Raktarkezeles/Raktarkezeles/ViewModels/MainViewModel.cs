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
using ZXing.Mobile;
using ZXing;

namespace Raktarkezeles.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private List<int> partIds = new List<int>();
        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                if(isRefreshing != value)
                {
                    isRefreshing = value;
                }
                OnPropertyChanged();
            }
        }
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
        private string searchText = "";
        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    SearchPartsCommandExecute(searchText);
                    OnPropertyChanged();
                }
            }
        }

        public ICommand GoToNewPartCommand { private set; get; }
        public ICommand GoToDetailsCommand { private set; get; }
        public ICommand ScanBarcodeCommand { private set; get; }
        public ICommand LoadItemsCommand { private set; get; }
        public ICommand RefreshPartsCommand { private set; get; }
        public MainViewModel()
        {
            GoToDetailsCommand = new Command(GoToDetailsCommandExecute);
            GoToNewPartCommand = new Command(GoToNewPartCommandExecute);
            ScanBarcodeCommand = new Command(ScanBarcodeCommandExecute);
            LoadItemsCommand = new Command(LoadItemsCommandExecute);
            RefreshPartsCommand = new Command(RefreshPartsCommandExecute);
            partIds = PartContext.GetParts();
            LoadItems();
            MessagingCenter.Subscribe<DetailsViewModel, Part>(this, "Updated", (vm, changedPart) =>
            {
                for (int i = 0; i < parts.Count; i++)
                {
                    if (Parts[i].Id == changedPart.Id)
                    {
                        Parts[i] = changedPart;
                    }
                }
            });
            MessagingCenter.Subscribe<DetailsViewModel, int>(this, "Deleted", (vm, id) =>
            {
                partIds.Remove(id);
                Parts.Remove(Parts.First(x => x.Id == id));
            });
            MessagingCenter.Subscribe<NewPartViewModel, Part>(this, "New", (vm, newPart) =>
            {
                partIds.Add(newPart.Id);
                if(Parts.Count + 20 > partIds.Count)
                {
                    LoadItems();
                }
            });
        }
        private async void GoToDetailsCommandExecute()
        {
            if (selectedPart != null)
            {
                DetailsViewModel detailsVM = new DetailsViewModel(selectedPart);
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
            partIds = PartContext.GetParts(input);
            foreach(var id in partIds)
            {
                Parts.Add(PartContext.GetPart(id));
            }
        }
        private async void ScanBarcodeCommandExecute()
        {
            MobileBarcodeScanner scanner = new MobileBarcodeScanner();
            Result result = await scanner.Scan();
            if(result is null)
            {
                return;
            }
            var filteredParts = parts.Where(p => p.ItemNumber.ToUpper().Contains(result.Text.ToUpper())).ToList();
            if(filteredParts.Count == 1)
            {
                SelectedPart = filteredParts[0];
                SearchText = "";
            }
            else
            {
                SearchText = result.Text;
            }
        }
        private void LoadItemsCommandExecute()
        {
            LoadItems();
        }
        private void RefreshPartsCommandExecute()
        {
            IsRefreshing = true;
            partIds.Clear();
            Parts.Clear();
            partIds = PartContext.GetParts(SearchText);
            LoadItems();
            IsRefreshing = false;
        }
        private void LoadItems()
        {
            int partsToLoadTo = partIds.Count < Parts.Count + 20 ? partIds.Count : Parts.Count + 20;
            for (int i = Parts.Count; i < partsToLoadTo; i++)
            {
                Part newPart = PartContext.GetPart(partIds[i]);
                newPart.Image = PartContext.GetPartPicture(partIds[i]);
                Parts.Add(newPart);
            }
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
