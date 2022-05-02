using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Raktarkezeles.Models;
using Raktarkezeles.DAL;
using System.Windows.Input;
using Raktarkezeles.MVVM;
using System.Linq;
using Raktarkezeles.Services;

namespace Raktarkezeles.ViewModels
{
    public class TransferQuantityViewModel : BindableBase
    {
        private RaktarkezelesService service = new RaktarkezelesService();
        public AlkatreszElofordulas FromOccurrence { get; set; }
        private List<AlkatreszElofordulas> occurrences;
        public List<AlkatreszElofordulas> Occurrences
        {
            get { return occurrences; }
            set { occurrences = value; OnPropertyChanged();}
        }
        private AlkatreszElofordulas pickedOccurrence;
        public AlkatreszElofordulas PickedOccurrence
        {
            get { return pickedOccurrence; }
            set { pickedOccurrence = value; OnPropertyChanged(); }
        }
        private string quantity;
        public string Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged(); }
        }
        private bool invalidQuantity = false;
        public bool InvalidQuantity { get { return invalidQuantity; } set { invalidQuantity = value; OnPropertyChanged(); } }
        public ICommand SaveTransferCommand { protected set; get; }
        public ICommand CancelTransferCommand { protected set; get; }
        public TransferQuantityViewModel(int occurrenceId, Alkatresz part)
        {
            FromOccurrence = part.AlkatreszElofordulasok.Where(x => x.Id == occurrenceId).DefaultIfEmpty(null).First();
            Occurrences = part.AlkatreszElofordulasok.Where(p => p.Id != occurrenceId).ToList();
            SaveTransferCommand = new Command(SaveTransferCommandExecute);
            CancelTransferCommand = new Command(CancelTransferCommandExecute);
        }
        private async void SaveTransferCommandExecute()
        {
            if (!CheckValidation())
            {
                int mennyiseg = int.Parse(quantity);
                if (await service.ChangeQuantity(FromOccurrence.Id, FromOccurrence.Mennyiseg - mennyiseg))
                {
                    if (await service.ChangeQuantity(pickedOccurrence.Id, pickedOccurrence.Mennyiseg + mennyiseg))
                    {
                        FromOccurrence.Mennyiseg -= mennyiseg;
                        pickedOccurrence.Mennyiseg += mennyiseg;
                    }
                }
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
        private async void CancelTransferCommandExecute()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private bool CheckValidation()
        {
            int result;
            InvalidQuantity = !int.TryParse(Quantity, out result);
            if (!InvalidQuantity)
            {
                InvalidQuantity = FromOccurrence.Mennyiseg - result < 0 || result < 0;
            }
            
            return InvalidQuantity;
        }
    }
}
