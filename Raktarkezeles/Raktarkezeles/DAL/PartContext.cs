using System;
using System.Collections.Generic;
using System.Text;
using Raktarkezeles.Models;

namespace Raktarkezeles.DAL
{
    public class PartContext
    {
        public List<Manufacturer> manufacturers = new List<Manufacturer>();
        public List<Category> categories = new List<Category>();
        public List<Unit> units = new List<Unit>();
        public List<Warehouse> warehouses = new List<Warehouse>();
        public List<Part> parts = new List<Part>();
        public List<Occurrence> occurrences = new List<Occurrence>();

        public PartContext()
        {
            manufacturers.Add(new Manufacturer() { Id = 0, Name = "Siemens" });
            manufacturers.Add(new Manufacturer() { Id = 1, Name = "Phoenix Contact" });
            manufacturers.Add(new Manufacturer() { Id = 2, Name = "WAGO" });

            units.Add(new Unit() { Id = 0, FullName = "Darab", ShortName = "db" });
            units.Add(new Unit() { Id = 1, FullName = "Méter", ShortName = "m" });

            categories.Add(new Category() { Id = 0, Name = "Sorkapocs" });
            categories.Add(new Category() { Id = 1, Name = "Tápegység" });
            categories.Add(new Category() { Id = 2, Name = "Sorkapocs véglap" });
            categories.Add(new Category() { Id = 3, Name = "Egyéb" });
        }

        public List<Part> GetParts()
        {
            return parts;
        }
        public List<Manufacturer> GetManufacturers()
        {
            return manufacturers;
        }
        public List<Unit> GetUnits()
        {
            return units;
        }
        public List<Category> GetCategories()
        {
            return categories;
        }
    }
}
