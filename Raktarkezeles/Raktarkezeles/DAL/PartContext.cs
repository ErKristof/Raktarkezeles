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
        public static ICollection<Gyarto> manufacturers = new List<Gyarto>();
        public static ICollection<Kategoria> categories = new List<Kategoria>();
        public static ICollection<MennyisegiEgyseg> units = new List<MennyisegiEgyseg>();
        public static ICollection<RaktarozasiHely> warehouses = new List<RaktarozasiHely>();
        public static ICollection<Alkatresz> parts = new ObservableCollection<Alkatresz>();
        public static ICollection<AlkatreszElofordulas> occurrences = new ObservableCollection<AlkatreszElofordulas>();
        public static int latestPartId = 21;
        public static int latestOccurrenceId = 1;

        static PartContext()
        {
            manufacturers.Add(new Gyarto() { Id = 0, TeljesNev = "Siemens" });
            manufacturers.Add(new Gyarto() { Id = 1, TeljesNev = "Phoenix Contact" });
            manufacturers.Add(new Gyarto() { Id = 2, TeljesNev = "WAGO" });

            units.Add(new MennyisegiEgyseg() { Id = 0, TeljesNev = "Darab", RovidNev = "db" });
            units.Add(new MennyisegiEgyseg() { Id = 1, TeljesNev = "Méter", RovidNev = "m" });

            categories.Add(new Kategoria() { Id = 0, Nev = "Sorkapocs" });
            categories.Add(new Kategoria() { Id = 1, Nev = "Tápegység" });
            categories.Add(new Kategoria() { Id = 2, Nev = "Sorkapocs véglap" });
            categories.Add(new Kategoria() { Id = 3, Nev = "Egyéb" });

            warehouses.Add(new RaktarozasiHely() { Id = 0, Nev = "Iroda" });
            warehouses.Add(new RaktarozasiHely() { Id = 1, Nev = "Raktár" });

        }
        //GET Alkatrészek
        //opcionális paraméter keresésre
        //válasznak id tömb a keresett paraméterekről!
        public static List<int> GetParts(string search = "")
        {
            List<int> partIds = new List<int>();
            if(search == "")
            {
                foreach(Alkatresz p in parts)
                {
                    partIds.Add(p.Id);
                }
            }
            else
            {
                search = search.ToUpper();
                var filteredParts = parts.Where(p => p.Nev.ToUpper().Contains(search) || p.Kategoria.Nev.ToUpper().Contains(search) || p.Gyarto.TeljesNev.ToUpper().Contains(search) || p.Cikkszam.ToUpper().Contains(search) || p.Tipus.ToUpper().Contains(search)).ToList();
                foreach (Alkatresz p in filteredParts)
                {
                    partIds.Add(p.Id);
                }
            }
            return partIds;
        }
        //GET Alkatrész id alapján
        //paraméter az alkatrész id-ja
        //visszatér a teljes alkatrésszel
        public static Alkatresz GetPart(int id)
        {
            Alkatresz selectedPart = parts.Where(x => x.Id == id).DefaultIfEmpty(null).First();
            if(selectedPart == null)
            {
                return null;
            }
            List<AlkatreszElofordulas> selectedOccurrences = occurrences.Where(x => x.AlkatreszId == id).ToList();
            selectedPart.AlkatreszElofordulasok.Clear();
            foreach(AlkatreszElofordulas occ in selectedOccurrences)
            {
                selectedPart.AlkatreszElofordulasok.Add(occ);
            }
            return selectedPart;
        }
        //POST Alkatrész
        //megkapja az alkatrészt és visszaadja azt ha sikeres a feltöltés
        public static Alkatresz AddPart(Alkatresz newPart)
        {
            newPart.Id = latestPartId;
            latestPartId++;
            parts.Add(newPart);
            return newPart;
        }
        //PUT Alkatrész
        //Megkapja a módosított alkatrész és visszaadja azt
        public static void EditPart(Alkatresz editedPart)
        {
            Alkatresz oldPart = parts.Where(x => x.Id == editedPart.Id).DefaultIfEmpty(null).First();
            if(oldPart == null)
            {
                return;
            }
            oldPart.GyartoId = editedPart.GyartoId;
            oldPart.KategoriaId = editedPart.KategoriaId;
            oldPart.MennyisegiEgysegId = editedPart.MennyisegiEgysegId;
            oldPart.Nev = editedPart.Nev;
            oldPart.Cikkszam = editedPart.Cikkszam;
            oldPart.Tipus = editedPart.Tipus;
            oldPart.Leiras = editedPart.Leiras;
        }
        //DELETE Alkatrész
        //Kitörli az megkapott id-jú alkatrészt
        public static void DeletePart(int partId)
        {
            var occurrencesToRemove = occurrences.Where(x => x.AlkatreszId == partId).ToList();
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
        //public static byte[] GetPartPicture(int partId)
        //{
        //    Alkatresz selectedPart = parts.Where(x => x.Id == partId).DefaultIfEmpty(null).First();
        //    if (selectedPart == null)
        //    {
        //        return null;
        //    }
        //    return selectedPart.Foto;
        //}
        //POST alkatrész kép
        //megkapja az alaktrész id-ját és a képet és frissíti azt
        //public static void EditPartPicture(int partId, byte[] newImage)
        //{
        //    Alkatresz selectedPart = parts.Where(x => x.Id == partId).DefaultIfEmpty(null).First();
        //    if (selectedPart == null)
        //    {
        //        return;
        //    }
        //    selectedPart.Foto = newImage;
        //}
        //GET előfordulás
        //megkapja az id-t 
        //visszaadja az előfordulást
        public static AlkatreszElofordulas GetOccurrence(int id)
        {
            var occurrence = occurrences.Where(x => x.Id == id).DefaultIfEmpty(null).First();
            return occurrence;
        }
        //POST előfordulás
        //megkapja az új előfordulási helyet és visszadaja azt
        public static void AddOccurrence(AlkatreszElofordulas occurrence)
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
            occurrenceToChange.Mennyiseg = quantity;
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
        public static ICollection<RaktarozasiHely> GetWarehouses()
        {
            return warehouses;
        }
        //GET gyártók
        //visszaadja a gyártók listáját
        public static ICollection<Gyarto> GetManufacturers()
        {
            return manufacturers;
        }
        //GET kategóriák
        //visszaadja az összes kategóriát
        public static ICollection<Kategoria> GetCategories()
        {
            return categories;
        }
        //GET mennyiségi egységek
        //visszaadja az összes mennyiségi egységet
        public static ICollection<MennyisegiEgyseg> GetUnits()
        {
            return units;
        }
    }
}
