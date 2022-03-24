using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Raktarkezeles.Models;
using Raktarkezeles.DAL;
using System.Windows.Input;
using Raktarkezeles.MVVM;

namespace Raktarkezeles.ViewModels
{
    public class NewOccurrenceViewModel : BindableBase
    {
        private Part part;
        private List<Warehouse> warehouses;
        public List<Warehouse> Warehouses
        {
            get
            {
                return warehouses;
            }
        }
        private Warehouse warehouse;
        public Warehouse Warehouse
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
        public ICommand SaveOccurrenceCommand { protected set; get; }
        public ICommand CancelOccurrenceCommand { protected set; get; }
        public NewOccurrenceViewModel(int partId)
        {
            part = PartContext.GetPart(partId);
            warehouses = PartContext.GetWarehouses();
            SaveOccurrenceCommand = new Command(SaveOccurrenceCommandExecute);
            CancelOccurrenceCommand = new Command(CancelOccurrenceCommandExecute);
        }

        private async void SaveOccurrenceCommandExecute()
        {
            Occurrence occurrence = new Occurrence();
            occurrence.PartId = part.Id;
            occurrence.Part = part;
            occurrence.Warehouse = warehouse;
            occurrence.WarehouseId = occurrence.Warehouse.Id;
            occurrence.Rack = int.Parse(rack);
            occurrence.Shelf = int.Parse(shelf);
            occurrence.Quantity = int.Parse(quantity);
            PartContext.AddOccurrence(occurrence);
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private async void CancelOccurrenceCommandExecute()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
