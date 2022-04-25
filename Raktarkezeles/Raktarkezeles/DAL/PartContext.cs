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
        public static ICollection<Manufacturer> manufacturers = new List<Manufacturer>();
        public static ICollection<Category> categories = new List<Category>();
        public static ICollection<Unit> units = new List<Unit>();
        public static ICollection<Warehouse> warehouses = new List<Warehouse>();
        public static ICollection<Part> parts = new ObservableCollection<Part>();
        public static ICollection<Occurrence> occurrences = new ObservableCollection<Occurrence>();
        public static int latestPartId = 21;
        public static int latestOccurrenceId = 1;

        static PartContext()
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
        //GET Alkatrészek
        //opcionális paraméter keresésre
        //válasznak id tömb a keresett paraméterekről!
        public static List<int> GetParts(string search = "")
        {
            List<int> partIds = new List<int>();
            if(search == "")
            {
                foreach(Part p in parts)
                {
                    partIds.Add(p.Id);
                }
            }
            else
            {
                search = search.ToUpper();
                var filteredParts = parts.Where(p => p.Name.ToUpper().Contains(search) || p.Category.Name.ToUpper().Contains(search) || p.Manufacturer.Name.ToUpper().Contains(search) || p.ItemNumber.ToUpper().Contains(search) || p.TypeNumber.ToUpper().Contains(search)).ToList();
                foreach (Part p in filteredParts)
                {
                    partIds.Add(p.Id);
                }
            }
            return partIds;
        }
        //GET Alkatrész id alapján
        //paraméter az alkatrész id-ja
        //visszatér a teljes alkatrésszel
        public static Part GetPart(int id)
        {
            Part selectedPart = parts.Where(x => x.Id == id).DefaultIfEmpty(null).First();
            if(selectedPart == null)
            {
                return null;
            }
            List<Occurrence> selectedOccurrences = occurrences.Where(x => x.PartId == id).ToList();
            selectedPart.Occurrences.Clear();
            foreach(Occurrence occ in selectedOccurrences)
            {
                selectedPart.Occurrences.Add(occ);
            }
            return selectedPart;
        }
        //POST Alkatrész
        //megkapja az alkatrészt és visszaadja azt ha sikeres a feltöltés
        public static Part AddPart(Part newPart)
        {
            newPart.Id = latestPartId;
            latestPartId++;
            parts.Add(newPart);
            return newPart;
        }
        //PUT Alkatrész
        //Megkapja a módosított alkatrész és visszaadja azt
        public static void EditPart(Part editedPart)
        {
            Part oldPart = parts.Where(x => x.Id == editedPart.Id).DefaultIfEmpty(null).First();
            if(oldPart == null)
            {
                return;
            }
            oldPart.ManufacturerId = editedPart.ManufacturerId;
            oldPart.CategoryId = editedPart.CategoryId;
            oldPart.UnitId = editedPart.UnitId;
            oldPart.Name = editedPart.Name;
            oldPart.ItemNumber = editedPart.ItemNumber;
            oldPart.TypeNumber = editedPart.TypeNumber;
            oldPart.Description = editedPart.Description;
        }
        //DELETE Alkatrész
        //Kitörli az megkapott id-jú alkatrészt
        public static void DeletePart(int partId)
        {
            var occurrencesToRemove = occurrences.Where(x => x.PartId == partId).ToList();
            foreach(var occ in occurrencesToRemove)
            {
                occurrences.Remove(occ);
            }
            var partToRemove = parts.Where(x => x.Id == partId).DefaultIfEmpty(null).First();
            if(partToRemove == null)
            {
                return;
            }
            parts.Remove(partToRemove);
        }
        //GET alkatrész kép
        //megkapja az alkatrész id-ját
        //visszaadja a képet
        public static byte[] GetPartPicture(int partId)
        {
            Part selectedPart = parts.Where(x => x.Id == partId).DefaultIfEmpty(null).First();
            if (selectedPart == null)
            {
                return null;
            }
            return selectedPart.Image;
        }
        //POST alkatrész kép
        //megkapja az alaktrész id-ját és a képet és frissíti azt
        public static void EditPartPicture(int partId, byte[] newImage)
        {
            Part selectedPart = parts.Where(x => x.Id == partId).DefaultIfEmpty(null).First();
            if (selectedPart == null)
            {
                return;
            }
            selectedPart.Image = newImage;
        }
        //GET előfordulás
        //megkapja az id-t 
        //visszaadja az előfordulást
        public static Occurrence GetOccurrence(int id)
        {
            var occurrence = occurrences.Where(x => x.Id == id).DefaultIfEmpty(null).First();
            return occurrence;
        }
        //POST előfordulás
        //megkapja az új előfordulási helyet és visszadaja azt
        public static void AddOccurrence(Occurrence occurrence)
        {
            occurrence.Id = latestOccurrenceId;
            latestOccurrenceId++;
            occurrences.Add(occurrence);
        }
        //PATCH előfordulás
        //megkapja az előfordulási hely id-ját és a mennyiséget, amire meg kéne változtatni
        public static void ChangeQuantity(int id, int quantity)
        {
            var occurrenceToChange = occurrences.Where(x => x.Id == id).DefaultIfEmpty(null).First();
            if(occurrenceToChange == null)
            {
                return;
            }
            occurrenceToChange.Quantity = quantity;
        }
        //DELETE előfordulás
        //megapott id-jú előfordulási helyet kitörli
        public static void DeleteOccurrence(int id)
        {
            var occurrenceToDelete = occurrences.Where(x => x.Id == id).DefaultIfEmpty(null).First();
            if(occurrenceToDelete == null)
            {
                return;
            }
            occurrences.Remove(occurrenceToDelete);
        }
        //GET raktározási helyek
        //visszaadja a raktározási helyek listáját
        public static ICollection<Warehouse> GetWarehouses()
        {
            return warehouses;
        }
        //GET gyártók
        //visszaadja a gyártók listáját
        public static ICollection<Manufacturer> GetManufacturers()
        {
            return manufacturers;
        }
        //GET kategóriák
        //visszaadja az összes kategóriát
        public static ICollection<Category> GetCategories()
        {
            return categories;
        }
        //GET mennyiségi egységek
        //visszaadja az összes mennyiségi egységet
        public static ICollection<Unit> GetUnits()
        {
            return units;
        }
    }
}
