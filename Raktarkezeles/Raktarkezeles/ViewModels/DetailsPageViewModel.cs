using System;
using System.Collections.Generic;
using System.Text;
using Raktarkezeles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Raktarkezeles.ViewModels
{
    class DetailsPageViewModel : INotifyPropertyChanged
    {
        private Part part;

        public Part Part 
        { 
            get 
            { 
                return part; 
            } 
        }

        private ObservableCollection<Occurrence> occurrences = new ObservableCollection<Occurrence>();

        public ObservableCollection<Occurrence> Occurrences
        {
            get
            {
                return occurrences;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DetailsPageViewModel()
        {
            Manufacturer phoenix = new Manufacturer() { Id = 0, Name = "Phoenix Contact" };
            Category sorkapocs = new Category() { Id = 0, Name = "Sorkapocs" };
            Unit darab = new Unit() { Id = 0, FullName = "darab", ShortName = "db" };
            Warehouse iroda = new Warehouse() { Name = "Iroda" };
            Warehouse raktar = new Warehouse() { Name = "Raktárház" };
            Warehouse kulfold = new Warehouse() { Name = "Külföldi raktárház" };
            part = new Part() { Name = "Sorkapocs ", Manufacturer = phoenix, Category = sorkapocs, Unit = darab, Occurrences = occurrences, TypeNumber= "6EP1333-1LB00", ItemNumber= "6EP1555-1LB00", Description="valmamlvoiesngviudorebgiouerbgpeorngondfsogndfogndonfigodnfkonskofngoinregonisdfojnbgsdopujfnboadnbpfodunbosuipdhfognbdfouasnpgoisadfngpoindfpogsndfoipg" };
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 1, Shelf = 1, Quantity = 12 , Warehouse = iroda});
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 2, Shelf = 3, Quantity = 122, Warehouse = iroda });
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 2, Shelf = 3, Quantity = 122, Warehouse = iroda });
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 2, Shelf = 3, Quantity = 122, Warehouse = iroda });
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 2, Shelf = 3, Quantity = 122, Warehouse = iroda });
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 2, Shelf = 3, Quantity = 122, Warehouse = iroda });
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 3, Shelf = 4, Quantity = 96, Warehouse = raktar });
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 1, Shelf = 9, Quantity = 34 , Warehouse = raktar});
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 1, Shelf = 9, Quantity = 34 , Warehouse = raktar});
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 1, Shelf = 9, Quantity = 34 , Warehouse = raktar});
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 1, Shelf = 9, Quantity = 34 , Warehouse = raktar});
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 1, Shelf = 9, Quantity = 34 , Warehouse = raktar});
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 10, Shelf = 10, Quantity = 1233, Warehouse = kulfold });
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 10, Shelf = 10, Quantity = 1233, Warehouse = kulfold });
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 10, Shelf = 10, Quantity = 1233, Warehouse = kulfold });
            occurrences.Add(new Occurrence() { Id = 0, Part = part, Rack = 10, Shelf = 10, Quantity = 1233, Warehouse = kulfold });
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
