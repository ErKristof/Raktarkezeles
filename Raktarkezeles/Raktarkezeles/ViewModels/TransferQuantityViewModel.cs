using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Raktarkezeles.Models;
using Raktarkezeles.DAL;
using System.Windows.Input;

namespace Raktarkezeles.ViewModels
{
    public class TransferQuantityViewModel : ViewModelBase
    {
        private Occurrence occurrence = new Occurrence();
        private List<Occurrence> occurrences;
        public List<Occurrence> Occurrences
        {
            get { return occurrences; }
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
        public TransferQuantityViewModel(INavigation navigation) : base(navigation)
        {
            //this.occurrence = occurrence;
            //occurrences = (List<Occurrence>)occurrence.Part.Occurrences;
            SaveTransferCommand = new Command(SaveTransferCommandExecute);
            CancelTransferCommand = new Command(CancelTransferCommandExecute);
        }
        private async void SaveTransferCommandExecute()
        {
            PartContext.TransferQuantity(occurrence, pickedOccurrence, int.Parse(quantity));
            await Navigation.PopModalAsync();
        }
        private async void CancelTransferCommandExecute()
        {
            await Navigation.PopModalAsync();
        }

    }
}
