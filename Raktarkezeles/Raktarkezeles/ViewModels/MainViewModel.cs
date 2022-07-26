using System;
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
using System.Net.Http;

namespace Raktarkezeles.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private IRaktarService service = new LocalRaktarkezelesService();
        private List<int> alkatreszIdk = new List<int>();
        private static readonly int LOADING_VALUE = 20;
        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                }
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Alkatresz> Alkatreszek { get; set; } = new ObservableCollection<Alkatresz>();
        private Alkatresz selectedAlkatresz;
        public Alkatresz SelectedAlkatresz
        {
            get
            {
                return selectedAlkatresz;
            }
            set
            {
                selectedAlkatresz = value;
                OnPropertyChanged();
            }
        }
        private string searchWord = "";
        public string SearchWord
        {
            get { return searchWord; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    searchWord = "";
                }
                else
                {
                    searchWord = value;
                }
                OnPropertyChanged();
            }
        }
        private volatile bool IsBusy = false;
        private readonly object syncObject = new object();
        public ICommand GoToNewPartCommand { private set; get; }
        public ICommand GoToDetailsCommand { private set; get; }
        public ICommand ScanBarcodeCommand { private set; get; }
        public ICommand LoadItemsCommand { private set; get; }
        public ICommand RefreshPartsCommand { private set; get; }
        public ICommand SearchCommand { private set; get; }
        public MainViewModel()
        {
            GoToDetailsCommand = new Command(GoToDetailsCommandExecute);
            GoToNewPartCommand = new Command(GoToNewPartCommandExecute);
            ScanBarcodeCommand = new Command(ScanBarcodeCommandExecute);
            LoadItemsCommand = new Command(LoadItemsCommandExecute);
            RefreshPartsCommand = new Command(RefreshPartsCommandExecute);
            SearchCommand = new Command<string>(SearchCommandExecute);
            MessagingCenter.Subscribe<DetailsViewModel, Alkatresz>(this, "Updated", (vm, changedPart) =>
            {
                changedPart.MennyisegChanged();
            });
            MessagingCenter.Subscribe<DetailsViewModel, int>(this, "Deleted", (vm, id) =>
            {
                lock (syncObject)
                {
                    alkatreszIdk.Remove(id);
                    Alkatreszek.Remove(Alkatreszek.First(x => x.Id == id));
                }
            });
            MessagingCenter.Subscribe<NewPartViewModel, Alkatresz>(this, "Added", (vm, newPart) =>
            {
                lock (syncObject)
                {
                    alkatreszIdk.Insert(0, newPart.Id);
                    Alkatreszek.Insert(0, newPart);
                }
            });
        }
        private async void GoToDetailsCommandExecute()
        {
            if (selectedAlkatresz != null)
            {
                DetailsViewModel detailsVM = new DetailsViewModel(selectedAlkatresz);
                DetailsPage detailsPage = new DetailsPage();
                detailsPage.BindingContext = detailsVM;
                await Application.Current.MainPage.Navigation.PushAsync(detailsPage);
                SelectedAlkatresz = null;
            }
        }
        private async void GoToNewPartCommandExecute()
        {
            NewPartViewModel newVM = new NewPartViewModel();
            NewPartPage newPage = new NewPartPage();
            newPage.BindingContext = newVM;
            await Application.Current.MainPage.Navigation.PushAsync(newPage);
        }
        private async void SearchCommandExecute(string input)
        {
            List<int> ids = await LoadIds(input);
            int partsToLoad = ids.Count > LOADING_VALUE ? LOADING_VALUE : ids.Count;
            List<Alkatresz> alkatreszek = await LoadAlkatreszek(ids.GetRange(0, partsToLoad));
            lock (syncObject)
            {
                Alkatreszek.Clear();
                alkatreszIdk.Clear();
                alkatreszIdk.AddRange(ids);
                foreach (Alkatresz a in alkatreszek)
                {
                    Alkatreszek.Add(a);
                }
            }
        }
        private async void ScanBarcodeCommandExecute()
        {
            MobileBarcodeScanner scanner = new MobileBarcodeScanner();
            Result result = await scanner.Scan();
            if (result == null)
            {
                return;
            }
            var matchingPartIds = await LoadIds(result.Text);
            if (matchingPartIds.Count == 1)
            {
                List<Alkatresz> alkatreszek = await LoadAlkatreszek(matchingPartIds);
                SelectedAlkatresz = alkatreszek[0];
            }
            else
            {
                SearchWord = result.Text;
            }
        }
        private async void LoadItemsCommandExecute()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            List<Alkatresz> newAlkatreszek = await LoadAlkatreszek(GetNextIds());
            lock (syncObject)
            {
                foreach (Alkatresz a in newAlkatreszek)
                {
                    Alkatreszek.Add(a);
                }
            }
            IsBusy = false;
        }
        private async void RefreshPartsCommandExecute()
        {
            List<int> ids = await LoadIds(SearchWord);
            int numberOfItemsToLoad = ids.Count > LOADING_VALUE ? LOADING_VALUE : ids.Count;
            List<Alkatresz> alkatreszek = await LoadAlkatreszek(ids.GetRange(0, numberOfItemsToLoad));
            lock (syncObject)
            {
                Alkatreszek.Clear();
                alkatreszIdk.Clear();
                alkatreszIdk.AddRange(ids);
                foreach (Alkatresz a in alkatreszek)
                {
                    Alkatreszek.Add(a);
                }
            }
            IsRefreshing = false;
        }
        private async Task<List<Alkatresz>> LoadAlkatreszek(List<int> ids)
        {
            List<Alkatresz> newAlkatreszek = new List<Alkatresz>();
            foreach (int id in ids)
            {
                try
                {
                    newAlkatreszek.Add(await service.GetAlkatresz(id));
                }
                catch (HttpRequestException HREx)
                {
                    DependencyService.Get<IAlertService>().LongAlert(HREx.Message);
                }
                catch (TimeoutException TEx)
                {
                    DependencyService.Get<IAlertService>().LongAlert(TEx.Message);
                }
            }
            foreach (Alkatresz a in newAlkatreszek)
            {
                try
                {
                    a.Foto = await service.GetFoto(a.Id);
                }
                catch (HttpRequestException) {}
                catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
            }
            return newAlkatreszek;
        }
        private async Task<List<int>> LoadIds(string searchWord = "")
        {
            List<int> newIds = new List<int>();
            try
            {
                newIds.AddRange(await service.GetAlkatreszek(searchWord));
            }
            catch (HttpRequestException HREx)
            {
                DependencyService.Get<IAlertService>().LongAlert(HREx.Message);
            }
            catch (TimeoutException TEx)
            {
                DependencyService.Get<IAlertService>().LongAlert(TEx.Message);
            }
            return newIds;
        }
        private List<int> GetNextIds()
        {
            List<int> loadedIds = new List<int>();
            foreach(Alkatresz a in Alkatreszek)
            {
                loadedIds.Add(a.Id);
            }
            List<int> idsToLoad = alkatreszIdk.Where(x => !loadedIds.Contains(x)).ToList();
            if(idsToLoad.Count < 20)
            {
                return idsToLoad;
            }
            return idsToLoad.GetRange(0, 20);
        }
    }
}
