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
        public static int latestPartId = 0;
        public static int latestOccurrenceId = 0;

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
        public static ICollection<Part> GetFilteredList(string input)
        {
            input = input.ToUpper();
            var filteredList = new List<Part>();
            return parts.Where(p => p.Name.ToUpper().Contains(input) || p.Category.Name.ToUpper().Contains(input) || p.Manufacturer.Name.ToUpper().Contains(input) || p.ItemNumber.ToUpper().Contains(input) || p.TypeNumber.ToUpper().Contains(input)).ToList();
        }
    }
}
