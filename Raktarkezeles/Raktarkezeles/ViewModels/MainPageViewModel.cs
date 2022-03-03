﻿using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Raktarkezeles.Models;

namespace Raktarkezeles.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Part> parts = new ObservableCollection<Part>();

        public ObservableCollection<Part> Parts
        {
            get
            {
                return parts;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            Manufacturer siemens = new Manufacturer() { Id = 0, Name = "Siemens" };
            Manufacturer wago = new Manufacturer() { Id = 0, Name = "WAGO" };
            Manufacturer phoenix = new Manufacturer() { Id = 0, Name = "Phoenix Contact" };
            Category sorkapocs = new Category() { Id = 0, Name = "Sorkapocs" };
            Category tapegyseg = new Category() { Id = 0, Name = "Tápegység" };
            Category sorkapocsveg = new Category() { Id = 0, Name = "Sorkapocs véglap" };
            Unit darab = new Unit() { Id = 0, FullName = "darab", ShortName = "db" };
            Unit meter = new Unit() { Id = 0, FullName = "méter", ShortName = "m" };

            parts.Add(new Part() { Name = "Sorkapocs 4es", Manufacturer = siemens, Category = sorkapocs, Unit = darab, Quantity = 54 });
            parts.Add(new Part() { Name = "Tápegység", Manufacturer = wago, Category = tapegyseg, Unit = darab, Quantity = 100 });
            parts.Add(new Part() { Name = "Sorkapocs ", Manufacturer = phoenix, Category = sorkapocs, Unit = meter, Quantity = 13 });
            parts.Add(new Part() { Name = "Sorkapocs véglap", Manufacturer = siemens, Category = sorkapocsveg, Unit = meter, Quantity = 987 });
            parts.Add(new Part() { Name = "Sorkapocs 4es", Manufacturer = siemens, Category = sorkapocs, Unit = darab, Quantity = 546 });
            parts.Add(new Part() { Name = "Tápegység", Manufacturer = wago, Category = tapegyseg, Unit = darab, Quantity = 1234 });
            parts.Add(new Part() { Name = "Sorkapocs ", Manufacturer = phoenix, Category = sorkapocs, Unit = meter, Quantity = 74 });
            parts.Add(new Part() { Name = "Sorkapocs véglap", Manufacturer = siemens, Category = sorkapocsveg, Unit = meter, Quantity = 1 });
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
