using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Raktarkezeles.MVVM;
using Raktarkezeles.Models;
using System.Windows.Input;
using Raktarkezeles.DAL;
using Raktarkezeles.Services;

namespace Raktarkezeles.ViewModels
{
    public class QuantityChangeViewModel : BindableBase
    {
        private RaktarkezelesService service = new RaktarkezelesService();
        private AlkatreszElofordulas occurrence;
        public AlkatreszElofordulas Occurrence
        {
            get
            {
                return occurrence;
            }
            set
            {
                occurrence = value;
                OnPropertyChanged();
            }
        }
        private string quantity;
        public string Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
                OnPropertyChanged();
            }
        }
        private bool invalidQuantity = false;
        public bool InvalidQuantity { get { return invalidQuantity; } set { invalidQuantity = value; OnPropertyChanged(); } }
        public ICommand SubtractQuantityCommand { private set; get; }
        public ICommand AddQuantityCommand { private set; get; }
        public ICommand CancelCommand { private set; get; }
        public QuantityChangeViewModel(AlkatreszElofordulas elofordulas)
        {
            Occurrence = elofordulas;
            SubtractQuantityCommand = new Command(SubtractQuantityCommndExecute);
            AddQuantityCommand = new Command(AddQuantityCommandExecute);
            CancelCommand = new Command(CancelCommandExecute);
        }
        private async void SubtractQuantityCommndExecute()
        {
            if (!CheckValidation(true))
            {
                //PartContext.ChangeQuantity(occurrence.Id, Occurrence.Mennyiseg - int.Parse(Quantity));
                await service.ChangeQuantity(occurrence.Id, Occurrence.Mennyiseg - int.Parse(Quantity));
                occurrence.Mennyiseg -= int.Parse(Quantity);
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
        private async void AddQuantityCommandExecute()
        {
            if (!CheckValidation(false))
            {
                //PartContext.ChangeQuantity(occurrence.Id, Occurrence.Mennyiseg + int.Parse(Quantity));
                await service.ChangeQuantity(occurrence.Id, Occurrence.Mennyiseg + int.Parse(Quantity));
                occurrence.Mennyiseg += int.Parse(Quantity);
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
        private async void CancelCommandExecute()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private bool CheckValidation(bool isSubtracting)
        {
            int result;
            InvalidQuantity = !int.TryParse(Quantity, out result);
            if (isSubtracting && !InvalidQuantity)
            {
                InvalidQuantity = Occurrence.Mennyiseg - result < 0;
            }
            else if(!isSubtracting && !InvalidQuantity)
            {
                InvalidQuantity = Occurrence.Mennyiseg + result < 0;
            }
            return InvalidQuantity;
        }
    }
}
