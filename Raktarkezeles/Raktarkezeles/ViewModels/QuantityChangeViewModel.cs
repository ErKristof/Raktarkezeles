using System;
using Xamarin.Forms;
using Raktarkezeles.MVVM;
using Raktarkezeles.Models;
using System.Windows.Input;
using Raktarkezeles.Services;
using System.Net.Http;

namespace Raktarkezeles.ViewModels
{
    public class QuantityChangeViewModel : BindableBase
    {
        private IRaktarService service = new LocalRaktarkezelesService();
        private AlkatreszElofordulas elofordulas;
        public AlkatreszElofordulas Elofordulas
        {
            get { return elofordulas; }
            set { elofordulas = value; OnPropertyChanged(); }
        }
        private string mennyiseg;
        public string Mennyiseg
        {
            get { return mennyiseg; }
            set { mennyiseg = value; OnPropertyChanged(); }
        }
        private bool invalidMennyiseg = false;
        public bool InvalidMennyiseg { get { return invalidMennyiseg; } set { invalidMennyiseg = value; OnPropertyChanged(); } }
        public ICommand SubtractQuantityCommand { private set; get; }
        public ICommand AddQuantityCommand { private set; get; }
        public ICommand CancelCommand { private set; get; }
        public QuantityChangeViewModel(AlkatreszElofordulas elofordulas)
        {
            Elofordulas = elofordulas;
            SubtractQuantityCommand = new Command(SubtractQuantityCommndExecute);
            AddQuantityCommand = new Command(AddQuantityCommandExecute);
            CancelCommand = new Command(CancelCommandExecute);
        }
        private async void SubtractQuantityCommndExecute()
        {
            if (CheckValidation(true))
            {
                try
                {
                    await service.UpdateElofordulasQuantity(elofordulas.Id, Elofordulas.Mennyiseg - int.Parse(Mennyiseg));
                    elofordulas.Mennyiseg -= int.Parse(Mennyiseg);
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                catch (HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
                catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
            }
        }
        private async void AddQuantityCommandExecute()
        {
            if (CheckValidation(false))
            {
                try
                {
                    await service.UpdateElofordulasQuantity(elofordulas.Id, Elofordulas.Mennyiseg + int.Parse(Mennyiseg));
                    elofordulas.Mennyiseg += int.Parse(Mennyiseg);
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                catch (HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
                catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
            }
        }
        private async void CancelCommandExecute()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private bool CheckValidation(bool isSubtracting)
        {
            InvalidMennyiseg = !int.TryParse(Mennyiseg, out int result);
            if (isSubtracting && !InvalidMennyiseg)
            {
                InvalidMennyiseg = Elofordulas.Mennyiseg - result < 0;
            }
            else if(!isSubtracting && !InvalidMennyiseg)
            {
                InvalidMennyiseg = Elofordulas.Mennyiseg + result < 0;
            }
            return !InvalidMennyiseg;
        }
    }
}
