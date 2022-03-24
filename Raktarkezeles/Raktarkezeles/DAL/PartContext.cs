using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Raktarkezeles.Models;
using System.Linq;

namespace Raktarkezeles.DAL
{
    public static class PartContext
    {
        public static List<Manufacturer> manufacturers = new List<Manufacturer>();
        public static List<Category> categories = new List<Category>();
        public static List<Unit> units = new List<Unit>();
        public static List<Warehouse> warehouses = new List<Warehouse>();
        public static ObservableCollection<Part> parts = new ObservableCollection<Part>();
        public static int latestPartId = 2;
        public static int latestOccurrenceId = 3;

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

            parts.Add(new Part() { Id = 0, ManufacturerId = 1, Manufacturer = manufacturers[1], CategoryId = 0, Category = categories[0], UnitId = 0, Unit = units[0], Name = "Sorkapocs", TypeNumber = "8WA1011-1DF11", ItemNumber = "8WA1011-1DF11", Quantity = 0, Description = "Through-type terminal thermoplast Screw terminal on both sides Single terminal, 6mm, Sz. 2.5 " });
            parts.Add(new Part() { Id = 1, ManufacturerId = 0, Manufacturer = manufacturers[0], CategoryId = 1, Category = categories[1], UnitId = 1, Unit = units[1], Name = "Sorkapocs véglap", TypeNumber = "8WA1011-1DF13", ItemNumber = "8WA1011-1DF13", Quantity = 0, Description = "" });

            parts[0].Occurrences = new ObservableCollection<Occurrence>();
            parts[0].Occurrences.Add(new Occurrence() { Id = 0, PartId = 0, Part = parts[0], WarehouseId = 0, Warehouse = warehouses[0], Rack = 1, Shelf = 2, Quantity = 10 });
            parts[0].Occurrences.Add(new Occurrence() { Id = 1, PartId = 0, Part = parts[0], WarehouseId = 1, Warehouse = warehouses[1], Rack = 3, Shelf = 8, Quantity = 8 });
            parts[0].Occurrences.Add(new Occurrence() { Id = 2, PartId = 0, Part = parts[0], WarehouseId = 0, Warehouse = warehouses[0], Rack = 1, Shelf = 3, Quantity = 14 });
            parts[1].Occurrences = new ObservableCollection<Occurrence>();
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
        public static List<Warehouse> GetWarehouses()
        {
            return warehouses;
        }
        public static void AddPart(Part newPart)
        {
            newPart.Id = latestPartId;
            latestPartId++;
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
            foreach(Part p in parts)
            {
                p.Occurrences.Remove(p.Occurrences.Where(o => o.Id == id).FirstOrDefault());
            }
        }
        public static void DeletePart(Part part)
        {
            parts.Remove(part);
        }
        public static void AddOccurrence(Occurrence occurrence)
        {
            occurrence.Id = latestOccurrenceId;
            latestOccurrenceId++;
            foreach(Part part in parts)
            {
                if(part.Id == occurrence.PartId)
                {
                    if(part.Occurrences == null) 
                    {
                        part.Occurrences = new ObservableCollection<Occurrence>();
                    }
                    part.Occurrences.Add(occurrence);
                }
            }
        }
        public static void TransferQuantity(int from, int to, int quantity)
        {
            foreach (Part part in parts)
            {
                foreach (Occurrence occurrence in part.Occurrences)
                {
                    if (occurrence.Id == from)
                    {
                        occurrence.Quantity -= quantity;
                    }
                    if (occurrence.Id == to)
                    {
                        occurrence.Quantity += quantity;
                    }
                }
            }
        }
        public static void ChangeQuantity(int id, int quantity)
        {
            foreach (Part part in parts)
            {
                foreach (Occurrence occurrence in part.Occurrences)
                {
                    if(occurrence.Id == id)
                    {
                        occurrence.Quantity += quantity;
                    }
                }
            }
        }
        public static Occurrence GetOccurrence(int id)
        {
            foreach(Part p in parts)
            {
                Occurrence occ = p.Occurrences.Where(o => o.Id == id).FirstOrDefault();
                if(occ != null)
                {
                    return occ;
                }
            }
            return null;
        }
        public static ICollection<Occurrence> GetOccurrences(int id)
        {
            return parts.Where(p => p.Id == id).FirstOrDefault().Occurrences;
        }
    }
}
