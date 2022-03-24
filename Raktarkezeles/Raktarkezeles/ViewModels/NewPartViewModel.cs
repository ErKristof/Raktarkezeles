﻿using System;
using System.Collections.Generic;
using System.Text;
using Raktarkezeles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Raktarkezeles.DAL;
using Xamarin.Forms;
using System.Windows.Input;
using Raktarkezeles.MVVM;

namespace Raktarkezeles.ViewModels
{
    public class NewPartViewModel : BindableBase
    {
        private Part newPart = new Part();
        public Part NewPart
        {
            get
            {
                return newPart;
            }
            set
            {
                newPart = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPartCommand { protected set; get; }
 
        private List<Manufacturer> manufacturers;
        private List<Category> categories;
        private List<Unit> units;

        public List<Manufacturer> Manufacturers
        {
            get { return manufacturers; }
        }
        public List<Category> Categories
        {
            get { return categories; }
        }
        public List<Unit> Units
        {
            get { return units; }
        }

        public NewPartViewModel(Part newPart = null)
        {
            manufacturers = PartContext.GetManufacturers();
            categories = PartContext.GetCategories();
            units = PartContext.GetUnits();
            if (newPart != null)
            {
                this.newPart = newPart;
                AddPartCommand = new Command(SavePartCommandExecute);
            }
            else
            {
                AddPartCommand = new Command(AddPartCommandExecute);
            }
        }

        private async void AddPartCommandExecute()
        {
            newPart.CategoryId = newPart.Category.Id;
            newPart.ManufacturerId = newPart.Manufacturer.Id;
            newPart.UnitId = newPart.Unit.Id;
            newPart.Occurrences = new ObservableCollection<Occurrence>();
            newPart.Quantity = 0;
            PartContext.AddPart(newPart);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void SavePartCommandExecute()
        {
            newPart.CategoryId = newPart.Category.Id;
            newPart.ManufacturerId = newPart.Manufacturer.Id;
            newPart.UnitId = newPart.Unit.Id;
            PartContext.EditPart(newPart);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

    }
}
