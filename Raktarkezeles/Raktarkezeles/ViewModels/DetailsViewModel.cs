using System;
using System.Collections.ObjectModel;
using Raktarkezeles.Models;
using Xamarin.Forms;
using System.Windows.Input;
using Raktarkezeles.Views;
using Raktarkezeles.MVVM;
using System.Linq;
using Raktarkezeles.Services;
using System.Net.Http;

namespace Raktarkezeles.ViewModels
{
    public class DetailsViewModel : BindableBase
    {
        private IRaktarService service = new LocalRaktarkezelesService();
        private Alkatresz alkatresz;
        private byte[] foto;
        public byte[] Foto
        {
            get
            {
                return foto;
            }
            set
            {
                if (foto != value)
                {
                    foto = value;
                    OnPropertyChanged();
                }
            }
        }
        private string nev;
        public string Nev
        {
            get
            {
                return nev;
            }
            set
            {
                if (nev != value)
                {
                    nev = value;
                    OnPropertyChanged();
                }
            }
        }
        private Gyarto gyarto;
        public Gyarto Gyarto
        {
            get
            {
                return gyarto;
            }
            set
            {
                if (gyarto != value)
                {
                    gyarto = value;
                    OnPropertyChanged();
                }
            }
        }
        private string cikkszam;
        public string Cikkszam
        {
            get
            {
                return cikkszam;
            }
            set
            {
                if (cikkszam != value)
                {
                    cikkszam = value;
                    OnPropertyChanged();
                }
            }
        }
        private string tipus;
        public string Tipus
        {
            get
            {
                return tipus;
            }
            set
            {
                if (tipus != value)
                {
                    tipus = value;
                    OnPropertyChanged();
                }
            }
        }
        private string leiras;
        public string Leiras
        {
            get
            {
                return leiras;
            }
            set
            {
                if (leiras != value)
                {
                    leiras = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<AlkatreszElofordulas> Elofordulasok
        {
            get
            {
                return alkatresz?.AlkatreszElofordulasok;
            }
        }

        public ICommand EditPartCommand { private set; get; }
        public ICommand DeletePartCommand { private set; get; }
        public ICommand NewOccurrenceCommand { private set; get; }
        public ICommand TransferQuantityCommand { private set; get; }
        public ICommand MinusOneCommand { private set; get; }
        public ICommand PlusOneCommand { private set; get; }
        public ICommand ChangeQuantityCommand { private set; get; }
        public ICommand DeleteOccurrenceCommnad { private set; get; }
        public DetailsViewModel(Alkatresz alkatresz)
        {
            this.alkatresz = alkatresz;
            UpdatePage();
            EditPartCommand = new Command(EditPartCommandExecute);
            DeletePartCommand = new Command(DeletePartCommandExecute);
            NewOccurrenceCommand = new Command(NewOccurrenceCommandExecute);
            TransferQuantityCommand = new Command<int>(TransferQuantityCommandExecute);
            MinusOneCommand = new Command<int>(MinusOneCommandExecute);
            PlusOneCommand = new Command<int>(PlusOneCommandExecute);
            DeleteOccurrenceCommnad = new Command<int>(DeleteOccurrenceCommandExecute);
            ChangeQuantityCommand = new Command<int>(ChangeQuantityCommandExecute);
        }
        private async void EditPartCommandExecute()
        {
            NewPartViewModel newPartVM = new NewPartViewModel(alkatresz);
            NewPartPage newPartPage = new NewPartPage();
            newPartPage.BindingContext = newPartVM;
            await Application.Current.MainPage.Navigation.PushAsync(newPartPage);
        }
        private async void DeletePartCommandExecute()
        {
            try
            {
                await service.DeleteAlkatresz(alkatresz.Id);
                MessagingCenter.Send(this, "Deleted", alkatresz.Id);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch(HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
            catch(TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
        }
        private async void NewOccurrenceCommandExecute()
        {
            MessagingCenter.Subscribe<NewOccurrenceViewModel, AlkatreszElofordulas>(this, "NewOccurrence", (vm, newOccurrence) =>
            {
                Elofordulasok.Add(newOccurrence);
                MessagingCenter.Unsubscribe<NewOccurrenceViewModel, AlkatreszElofordulas>(this, "NewOccurrence");
            });
            NewOccurrenceViewModel newOccurrenceVM = new NewOccurrenceViewModel(alkatresz);
            NewOccurrencePage newOccurrencePage = new NewOccurrencePage();
            newOccurrencePage.BindingContext = newOccurrenceVM;
            await Application.Current.MainPage.Navigation.PushModalAsync(newOccurrencePage);
        }
        private async void TransferQuantityCommandExecute(int id)
        {
            TransferQuantityViewModel transferQuantityVM = new TransferQuantityViewModel(id, alkatresz);
            TransferQuantityPage transferQuantityPage = new TransferQuantityPage();
            transferQuantityPage.BindingContext = transferQuantityVM;
            await Application.Current.MainPage.Navigation.PushModalAsync(transferQuantityPage);
        }
        private async void ChangeQuantityCommandExecute(int id)
        {
            QuantityChangeViewModel quantityChangeVM = new QuantityChangeViewModel(Elofordulasok.FirstOrDefault(o => o.Id == id));
            QuantityChangePage quantityChangePage = new QuantityChangePage();
            quantityChangePage.BindingContext = quantityChangeVM;
            await Application.Current.MainPage.Navigation.PushModalAsync(quantityChangePage);
        }
        private async void MinusOneCommandExecute(int id)
        {
            AlkatreszElofordulas changedElofordulas = Elofordulasok.First(o => o.Id == id);
            if (changedElofordulas.Mennyiseg > 0)
            {
                try
                {
                    await service.UpdateElofordulasQuantity(id, changedElofordulas.Mennyiseg - 1);
                    changedElofordulas.Mennyiseg--;
                }
                catch (HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
                catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
            }
        }
        private async void PlusOneCommandExecute(int id)
        {
            AlkatreszElofordulas changedElofordulas = Elofordulasok.First(o => o.Id == id);
            try
            {
                await service.UpdateElofordulasQuantity(id, changedElofordulas.Mennyiseg + 1);
                changedElofordulas.Mennyiseg++;
            }
            catch (HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
            catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
        }
        private async void DeleteOccurrenceCommandExecute(int id)
        {
            try
            {
                await service.DeleteElofordulas(id);
                Elofordulasok.Remove(Elofordulasok.First(x => x.Id == id));
            }
            catch (HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
            catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
        }
        public override void OnAppearing()
        {
            UpdatePage();
        }
        public void OnDisappearing()
        {
            MessagingCenter.Send(this, "Updated", alkatresz);
        }
        private void UpdatePage()
        {
            Foto = alkatresz.Foto;
            Nev = alkatresz.Nev;
            Gyarto = alkatresz.Gyarto;
            Cikkszam = alkatresz.Cikkszam;
            Tipus = alkatresz.Tipus;
            Leiras = alkatresz.Leiras;
        }
    }
}
