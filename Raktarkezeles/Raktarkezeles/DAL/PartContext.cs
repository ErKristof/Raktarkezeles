using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Raktarkezeles.Models;

namespace Raktarkezeles.DAL
{
    public static class PartContext
    {
        public static List<Manufacturer> manufacturers = new List<Manufacturer>();
        public static List<Category> categories = new List<Category>();
        public static List<Unit> units = new List<Unit>();
        public static List<Warehouse> warehouses = new List<Warehouse>();
        public static ObservableCollection<Part> parts = new ObservableCollection<Part>();
        public static List<Occurrence> occurrences = new List<Occurrence>();
        public static int latestId = 0;

        static  PartContext()
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

            warehouses.Add(new Warehouse() { Id = 0, Name = "Iroda" });
            warehouses.Add(new Warehouse() { Id = 1, Name = "Raktár" });

            parts.Add(new Part() { Id = 0, ManufacturerId = 0, Manufacturer = manufacturers[0], CategoryId = 0, Category = categories[0], UnitId = 0, Unit = units[0], Name = "Sorkapocs", TypeNumber = "8WA1011-1DF11", ItemNumber = "8WA1011-1DF11", Quantity = 32, Description = "Through-type terminal thermoplast Screw terminal on both sides Single terminal, 6mm, Sz. 2.5 " });

            occurrences.Add(new Occurrence() { Id = 0, PartId = 0, Part = parts[0], WarehouseId = 0, Warehouse = warehouses[0], Rack = 1, Shelf = 2, Quantity = 10 });
            occurrences.Add(new Occurrence() { Id = 1, PartId = 0, Part = parts[0], WarehouseId = 1, Warehouse = warehouses[1], Rack = 3, Shelf = 8, Quantity = 8 });
            occurrences.Add(new Occurrence() { Id = 2, PartId = 0, Part = parts[0], WarehouseId = 0, Warehouse = warehouses[0], Rack = 1, Shelf = 3, Quantity = 14 });

            parts[0].Occurrences = new List<Occurrence>();
            parts[0].Occurrences.Add(occurrences[0]);
            parts[0].Occurrences.Add(occurrences[1]);
            parts[0].Occurrences.Add(occurrences[2]);
        }

        public static ObservableCollection<Part> GetParts()
        {
            return parts;
        }
        public static List<Manufacturer> GetManufacturers()
        {
            return manufacturers;
        }
        public static List<Unit> GetUnits()
        {
            return units;
        }
        public static List<Category> GetCategories()
        {
            return categories;
        }
        public static void AddPart(Part newPart)
        {
            parts.Add(newPart);
        }
        public static void EditPart(Part editedPart)
        {
            int editedIndex = -1;
            foreach(Part part in parts)
            {
                if(part.Id == editedPart.Id)
                {
                    editedIndex = parts.IndexOf(part);
                }
            }
            parts[editedIndex] = editedPart;
        }
        public static Part GetPart(int id)
        {
            foreach(Part part in parts)
            {
                if(part.Id == id)
                {
                    return part;
                }
            }
            return null;
        }
        public static void DeleteOccurrence(int id)
        {
            foreach(Occurrence occurrence in occurrences)
            {
                if(occurrence.Id == id)
                {
                    occurrences.Remove(occurrence);
                    break;
                }
            }
        }
        public static void DeletePart(Part part)
        {
            if (part.Occurrences != null)
            {
                foreach (Occurrence occurrence in part.Occurrences)
                {
                    if (occurrence.PartId == part.Id)
                    {
                        occurrences.Remove(occurrence);
                    }
                }
            }
            parts.Remove(part);
        }
    }
}
