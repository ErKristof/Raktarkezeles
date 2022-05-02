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
using Raktarkezeles.MVVM;
using ZXing.Mobile;
using ZXing;
using Raktarkezeles.Services;
using System.Threading.Tasks;

namespace Raktarkezeles.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private RaktarkezelesService service = new RaktarkezelesService();
        private List<int> partIds = new List<int>();
        private int partsLoading = 0;
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
        private ObservableCollection<Alkatresz> parts = new ObservableCollection<Alkatresz>();
        public ObservableCollection<Alkatresz> Parts
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
        private Alkatresz selectedPart;
        public Alkatresz SelectedPart
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
            GetPartIds();
            MessagingCenter.Subscribe<DetailsViewModel, Alkatresz>(this, "Updated", (vm, changedPart) =>
            {
                changedPart.MennyisegChanged();
            });
            MessagingCenter.Subscribe<DetailsViewModel, int>(this, "Deleted", (vm, id) =>
            {
                partIds.Remove(id);
                Parts.Remove(Parts.First(x => x.Id == id));
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
            
        }
        private async void ScanBarcodeCommandExecute()
        {
            MobileBarcodeScanner scanner = new MobileBarcodeScanner();
            Result result = await scanner.Scan();
            if(result is null)
            {
                return;
            }
            var filteredParts = parts.Where(p => p.Cikkszam.ToUpper().Contains(result.Text.ToUpper())).ToList();
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
        private async void LoadItemsCommandExecute()
        {
            await LoadItems();
        }
        private void RefreshPartsCommandExecute()
        {
            IsRefreshing = true;
            partIds.Clear();
            Parts.Clear();
            partsLoading = 0;
            GetPartIds();
            IsRefreshing = false;
        }
        private async Task LoadItems()
        {
            int loadingValue = 20;
            int partsToLoadTo = 0;
            if(partsLoading == 0)
            {
                partsToLoadTo = partIds.Count < loadingValue ? partIds.Count : loadingValue;
            }
            else if(parts.Count < partsLoading || partsLoading == partIds.Count)
            {
                return;
            }
            else
            {
                partsToLoadTo = partIds.Count < partsLoading + loadingValue ? partIds.Count : partsLoading + loadingValue;
            }
            partsLoading = partsToLoadTo;
            for (int i = Parts.Count; i < partsToLoadTo; i++)
            {
                Alkatresz newAlkatresz = await service.GetAlkatresz(partIds[i]);
                Parts.Add(newAlkatresz);
            }

        }
        private async void GetPartIds(string kereses = "")
        {
            partIds = await service.GetAlkatreszek(kereses);
            await LoadItems();
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
