using System;
using Xamarin.Forms;
using Raktarkezeles.Models;
using System.Windows.Input;
using Raktarkezeles.MVVM;
using Raktarkezeles.Services;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace Raktarkezeles.ViewModels
{
    public class NewOccurrenceViewModel : BindableBase
    {
        private IRaktarService service = new LocalRaktarkezelesService();
        private Alkatresz alkatresz;
        public ObservableCollection<RaktarozasiHely> RaktarozasiHelyek { get; set; } = new ObservableCollection<RaktarozasiHely>();
        private RaktarozasiHely raktarozasiHely;
        public RaktarozasiHely RaktarozasiHely
        {
            get { return raktarozasiHely; }
            set { raktarozasiHely = value; OnPropertyChanged(); }
        }
        private string polc;
        public string Polc
        {
            get { return polc; }
            set { polc = value; OnPropertyChanged(); }
        }
        private string szint;
        public string Szint
        {
            get { return szint; }
            set { szint = value; OnPropertyChanged(); }
        }
        private string mennyiseg;
        public string Mennyiseg
        {
            get { return mennyiseg; }
            set { mennyiseg = value; OnPropertyChanged(); }
        }
        private bool invalidRaktarozasiHely = false;
        public bool InvalidRaktarozasiHely
        {
            get { return invalidRaktarozasiHely; }
            set { invalidRaktarozasiHely = value; OnPropertyChanged(); }
        }
        private bool invalidPolc = false;
        public bool InvalidPolc
        {
            get { return invalidPolc; }
            set { invalidPolc = value; OnPropertyChanged(); }
        }
        private bool invalidSzint = false;
        public bool InvalidSzint
        {
            get { return invalidSzint; }
            set { invalidSzint = value; OnPropertyChanged(); }
        }
        private bool invalidMennyiseg = false;
        public bool InvalidMennyiseg
        {
            get { return invalidMennyiseg; }
            set { invalidMennyiseg = value; OnPropertyChanged(); }
        }
        public ICommand SaveOccurrenceCommand { protected set; get; }
        public ICommand CancelOccurrenceCommand { protected set; get; }
        public NewOccurrenceViewModel(Alkatresz alkatresz)
        {
            this.alkatresz = alkatresz;
            GetWarehouses();
            SaveOccurrenceCommand = new Command(SaveOccurrenceCommandExecute);
            CancelOccurrenceCommand = new Command(CancelOccurrenceCommandExecute);
        }
        private async void GetWarehouses()
        {
            try
            {
                RaktarozasiHelyek = await service.GetRaktarozasiHelyek();
            }
            catch (HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
            catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
        }

        private async void SaveOccurrenceCommandExecute()
        {
            if (CheckValidation())
            {
                AlkatreszElofordulas elofordulas = new AlkatreszElofordulas();
                elofordulas.AlkatreszId = alkatresz.Id;
                elofordulas.Alkatresz = alkatresz;
                elofordulas.RaktarozasiHely = raktarozasiHely;
                elofordulas.RaktarozasiHelyId = elofordulas.RaktarozasiHely.Id;
                elofordulas.Polc = int.Parse(polc);
                elofordulas.Szint = int.Parse(szint);
                elofordulas.Mennyiseg = int.Parse(mennyiseg);
                try
                {
                    elofordulas = await service.PostElofodulas(elofordulas);
                    MessagingCenter.Send(this, "NewOccurrence", elofordulas);
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                catch (HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
                catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
            }
        }
        private async void CancelOccurrenceCommandExecute()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private bool CheckValidation()
        {
            InvalidRaktarozasiHely = RaktarozasiHely.Id == -1;
            InvalidPolc = !int.TryParse(Polc, out _);
            InvalidSzint = !int.TryParse(Szint, out _);
            InvalidMennyiseg = !int.TryParse(Mennyiseg, out _);
            return !(InvalidRaktarozasiHely || InvalidPolc || InvalidSzint || InvalidMennyiseg);
        }
    }
}
