using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Raktarkezeles.Models;
using System.Windows.Input;
using Raktarkezeles.MVVM;
using Raktarkezeles.Services;
using System.Collections.ObjectModel;

namespace Raktarkezeles.ViewModels
{
    public class NewOccurrenceViewModel : BindableBase
    {
        private RaktarkezelesService service = new RaktarkezelesService();
        private Alkatresz part;
        private ObservableCollection<RaktarozasiHely> warehouses = new ObservableCollection<RaktarozasiHely>();
        public ObservableCollection<RaktarozasiHely> Warehouses
        {
            get
            {
                return warehouses;
            }
            set
            {
                warehouses = value;
            }
        }
        private RaktarozasiHely warehouse;
        public RaktarozasiHely Warehouse
        {
            get { return warehouse; }
            set { warehouse = value; OnPropertyChanged(); }
        }
        private string rack;
        public string Rack
        {
            get { return rack; }
            set { rack = value; OnPropertyChanged(); }
        }
        private string shelf;
        public string Shelf
        {
            get { return shelf; }
            set { shelf = value; OnPropertyChanged(); }
        }
        private string quantity;
        public string Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged(); }
        }
        private bool invalidWarehouse = false;
        public bool InvalidWarehouse { get { return invalidWarehouse; } set { invalidWarehouse = value; OnPropertyChanged(); } }
        private bool invalidRack = false;
        public bool InvalidRack { get { return invalidRack; } set { invalidRack = value; OnPropertyChanged(); } }
        private bool invalidShelf = false;
        public bool InvalidShelf { get { return invalidShelf; } set { invalidShelf = value; OnPropertyChanged(); } }
        private bool invalidQuantity = false;
        public bool InvalidQuantity { get { return invalidQuantity; } set { invalidQuantity = value; OnPropertyChanged(); } }
        public ICommand SaveOccurrenceCommand { protected set; get; }
        public ICommand CancelOccurrenceCommand { protected set; get; }
        public NewOccurrenceViewModel(Alkatresz part)
        {
            this.part = part;
            GetWarehouses();
            SaveOccurrenceCommand = new Command(SaveOccurrenceCommandExecute);
            CancelOccurrenceCommand = new Command(CancelOccurrenceCommandExecute);
        }
        private async void GetWarehouses()
        {
            foreach(var w in await service.GetRaktarozasiHelyek())
            {
                Warehouses.Add(w);
            }
        }

        private async void SaveOccurrenceCommandExecute()
        {
            if (!CheckValidation())
            {
                AlkatreszElofordulas occurrence = new AlkatreszElofordulas();
                occurrence.AlkatreszId = part.Id;
                occurrence.Alkatresz = part;
                occurrence.RaktarozasiHely = warehouse;
                occurrence.RaktarozasiHelyId = occurrence.RaktarozasiHely.Id;
                occurrence.Polc = int.Parse(rack);
                occurrence.Szint = int.Parse(shelf);
                occurrence.Mennyiseg = int.Parse(quantity);
                occurrence = await service.PostElofodulas(occurrence);
                MessagingCenter.Send(this, "NewOccurrence", occurrence);
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
        private async void CancelOccurrenceCommandExecute()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private bool CheckValidation()
        {
            InvalidWarehouse = Warehouse == null;
            int result;
            InvalidRack = !int.TryParse(Rack, out result);
            InvalidShelf = !int.TryParse(Shelf, out result);
            InvalidQuantity = !int.TryParse(Quantity, out result);
            return InvalidWarehouse || InvalidRack || InvalidShelf || InvalidQuantity;
        }
    }
}
