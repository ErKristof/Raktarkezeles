using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Raktarkezeles.Models;
using Raktarkezeles.DAL;
using System.Windows.Input;

namespace Raktarkezeles.ViewModels
{
    public class TransferQuantityViewModel : ViewModelBase
    {
        public Occurrence FromOccurrence { get; set; }
        private ObservableCollection<Occurrence> occurrences;
        public ObservableCollection<Occurrence> Occurrences
        {
            get { return occurrences; }
            set { occurrences = value; OnPropertyChanged();}
        }
        private Occurrence pickedOccurrence;
        public Occurrence PickedOccurrence
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
        
        public ICommand SaveTransferCommand { protected set; get; }
        public ICommand CancelTransferCommand { protected set; get; }
        public TransferQuantityViewModel()
        {
            SaveTransferCommand = new Command(SaveTransferCommandExecute);
            CancelTransferCommand = new Command(CancelTransferCommandExecute);
        }
        private async void SaveTransferCommandExecute()
        {
            PartContext.TransferQuantity(FromOccurrence.Id, pickedOccurrence.Id, int.Parse(quantity));
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private async void CancelTransferCommandExecute()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

    }
}
