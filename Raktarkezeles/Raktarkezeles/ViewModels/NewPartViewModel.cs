using System;
using System.Collections.Generic;
using System.Text;
using Raktarkezeles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Raktarkezeles.DAL;
using Xamarin.Forms;

namespace Raktarkezeles.ViewModels
{
    public class NewPartViewModel : ViewModelBase
    {
        private PartContext partContext = new PartContext();

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

        public NewPartViewModel(INavigation navigation) : base(navigation)
        {
            manufacturers = partContext.GetManufacturers();
            categories = partContext.GetCategories();
            units = partContext.GetUnits();
        }

    }
}
