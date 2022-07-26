using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Raktarkezeles.Models;
using System.Windows.Input;
using Raktarkezeles.MVVM;
using System.Linq;
using Raktarkezeles.Services;
using System.Net.Http;

namespace Raktarkezeles.ViewModels
{
    public class TransferQuantityViewModel : BindableBase
    {
        private IRaktarService service = new LocalRaktarkezelesService();
        public AlkatreszElofordulas FromElofordulas { get; set; }
        private List<AlkatreszElofordulas> elofordulasok;
        public List<AlkatreszElofordulas> Elofordulasok
        {
            get { return elofordulasok; }
            set { elofordulasok = value; OnPropertyChanged(); }
        }
        private AlkatreszElofordulas toElofordulas;
        public AlkatreszElofordulas ToElofordulas
        {
            get { return toElofordulas; }
            set { toElofordulas = value; OnPropertyChanged(); }
        }
        private string mennyiseg;
        public string Mennyiseg
        {
            get { return mennyiseg; }
            set { mennyiseg = value; OnPropertyChanged(); }
        }
        private bool invalidMennyiseg = false;
        public bool InvalidMennyiseg { get { return invalidMennyiseg; } set { invalidMennyiseg = value; OnPropertyChanged(); } }
        public ICommand SaveTransferCommand { protected set; get; }
        public ICommand CancelTransferCommand { protected set; get; }
        public TransferQuantityViewModel(int occurrenceId, Alkatresz part)
        {
            FromElofordulas = part.AlkatreszElofordulasok.First(x => x.Id == occurrenceId);
            Elofordulasok = part.AlkatreszElofordulasok.Where(p => p.Id != occurrenceId).ToList();
            SaveTransferCommand = new Command(SaveTransferCommandExecute);
            CancelTransferCommand = new Command(CancelTransferCommandExecute);
        }
        private async void SaveTransferCommandExecute()
        {
            if (CheckValidation())
            {
                int mennyiseg = int.Parse(this.mennyiseg);
                try
                {
                    await service.UpdateElofordulasQuantity(FromElofordulas.Id, FromElofordulas.Mennyiseg - mennyiseg);
                    await service.UpdateElofordulasQuantity(toElofordulas.Id, toElofordulas.Mennyiseg + mennyiseg);
                    FromElofordulas.Mennyiseg -= mennyiseg;
                    ToElofordulas.Mennyiseg += mennyiseg;
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                catch (HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
                catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
            }
        }
        private async void CancelTransferCommandExecute()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private bool CheckValidation()
        {
            InvalidMennyiseg = !int.TryParse(Mennyiseg, out int result);
            if (!InvalidMennyiseg)
            {
                InvalidMennyiseg = FromElofordulas.Mennyiseg - result < 0 || result < 0;
            }
            return !InvalidMennyiseg;
        }
    }
}
