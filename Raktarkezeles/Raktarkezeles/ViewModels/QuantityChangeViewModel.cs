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
        private int quantity;
        public int Quantity
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
            if (occurrence.Quantity - quantity >= 0)
            {
                PartContext.ChangeQuantity(occurrence.Id, -quantity);
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {

            }
        }
        private async void AddQuantityCommandExecute()
        {
            PartContext.ChangeQuantity(occurrence.Id, quantity);
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private async void CancelCommandExecute()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
