using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Raktarkezeles.MVVM;
using Raktarkezeles.Models;
using System.Windows.Input;
using Raktarkezeles.DAL;

namespace Raktarkezeles.ViewModels
{
    public class QuantityChangeViewModel : BindableBase
    {
        private Occurrence occurrence;
        public Occurrence Occurrence
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
        public QuantityChangeViewModel(int occurrenceId)
        {
            Occurrence = PartContext.GetOccurrence(occurrenceId);
            SubtractQuantityCommand = new Command(SubtractQuantityCommndExecute);
            AddQuantityCommand = new Command(AddQuantityCommandExecute);
            CancelCommand = new Command(CancelCommandExecute);
        }
        private async void SubtractQuantityCommndExecute()
        {
            if (!CheckValidation(true))
            {
                PartContext.ChangeQuantity(occurrence.Id, -int.Parse(Quantity));
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
        private async void AddQuantityCommandExecute()
        {
            if (!CheckValidation(false))
            {
                PartContext.ChangeQuantity(occurrence.Id, int.Parse(Quantity));
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
                InvalidQuantity = Occurrence.Quantity - result < 0;
            }
            else if(!isSubtracting && !InvalidQuantity)
            {
                InvalidQuantity = Occurrence.Quantity + result < 0;
            }
            return InvalidQuantity;
        }
    }
}
