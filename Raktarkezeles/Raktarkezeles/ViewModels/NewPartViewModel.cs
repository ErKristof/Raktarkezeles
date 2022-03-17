using System;
using System.Collections.Generic;
using System.Text;
using Raktarkezeles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Raktarkezeles.DAL;
using Xamarin.Forms;
using System.Windows.Input;

namespace Raktarkezeles.ViewModels
{
    public class NewPartViewModel : ViewModelBase
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

        public NewPartViewModel(INavigation navigation, Part _newPart = null) : base(navigation)
        {
            manufacturers = PartContext.GetManufacturers();
            categories = PartContext.GetCategories();
            units = PartContext.GetUnits();
            if(_newPart != null)
            {
                newPart = _newPart;
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
            PartContext.AddPart(newPart);
            await Navigation.PopAsync(true);
        }

        private async void SavePartCommandExecute()
        {
            newPart.CategoryId = newPart.Category.Id;
            newPart.ManufacturerId = newPart.Manufacturer.Id;
            newPart.UnitId = newPart.Unit.Id;
            PartContext.EditPart(newPart);
            await Navigation.PopAsync(true);
        }

    }
}
