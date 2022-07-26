using Raktarkezeles.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Raktarkezeles.Services
{
    public class LocalRaktarkezelesService : IRaktarService
    {
        private static readonly List<Alkatresz> alkatreszek = new List<Alkatresz>();
        private static readonly List<AlkatreszElofordulas> elofordulasok = new List<AlkatreszElofordulas>();
        private static readonly ObservableCollection<Gyarto> gyartok = new ObservableCollection<Gyarto>();
        private static readonly ObservableCollection<Kategoria> kategoriak = new ObservableCollection<Kategoria>();
        private static readonly ObservableCollection<MennyisegiEgyseg> mennyisegiEgysegek = new ObservableCollection<MennyisegiEgyseg>();
        private static readonly ObservableCollection<RaktarozasiHely> raktarak = new ObservableCollection<RaktarozasiHely>();
        private static int alkatreszId = 2000;
        private static int elofordulasId = 2000;

        static LocalRaktarkezelesService()
        {
            gyartok.Add(new Gyarto { Id = 1, RovidNev = "SIE", TeljesNev = "Siemens" });
            gyartok.Add(new Gyarto { Id = 2, RovidNev = "PHC", TeljesNev = "Phoenix Contact" });
            gyartok.Add(new Gyarto { Id = 3, RovidNev = "WAGO", TeljesNev = "WAGO" });
            kategoriak.Add(new Kategoria { Id = 1, Nev = "Sorkapocs" });
            kategoriak.Add(new Kategoria { Id = 2, Nev = "Sorkapocs véglap" });
            kategoriak.Add(new Kategoria { Id = 3, Nev = "Tápegység" });
            kategoriak.Add(new Kategoria { Id = 4, Nev = "Egyéb" });
            mennyisegiEgysegek.Add(new MennyisegiEgyseg { Id = 1, TeljesNev = "Darab", RovidNev = "db" });
            mennyisegiEgysegek.Add(new MennyisegiEgyseg { Id = 2, TeljesNev = "Méter", RovidNev = "m" });
            raktarak.Add(new RaktarozasiHely { Id = 1, Nev = "Iroda" });
            raktarak.Add(new RaktarozasiHely { Id = 2, Nev = "Raktár" });
            raktarak.Add(new RaktarozasiHely { Id = 3, Nev = "Asztal" });
            for(int i = 1; i < 101; i++)
            {
                alkatreszek.Add(new Alkatresz { Id = i, Nev = "Alkatrész " + i });
            }
        }

        public Task DeleteAlkatresz(int id)
        {
            var torlendo = alkatreszek.Find(x => x.Id == id);
            if(torlendo == null)
            {
                throw new HttpRequestException("Not Found");
            }
            elofordulasok.RemoveAll(x => x.AlkatreszId == id);
            alkatreszek.Remove(torlendo);
            return Task.CompletedTask;
        }

        public Task DeleteElofordulas(int id)
        {
            var torlendo = elofordulasok.Find(x => x.Id == id);
            if (torlendo == null)
            {
                throw new HttpRequestException("Not Found");
            }
            elofordulasok.Remove(torlendo);
            return Task.CompletedTask;
        }

        public Task<Alkatresz> GetAlkatresz(int id)
        {
            var keresett = alkatreszek.Find(x => x.Id == id);
            if(keresett == null)
            {
                throw new HttpRequestException("Not Found");
            }
            var copy = CopyAlkatresz(keresett);
            var keresettElofordulasok = elofordulasok.Where(x => x.AlkatreszId == id).ToList();
            foreach(var e in keresettElofordulasok)
            {
                copy.AlkatreszElofordulasok.Add(CopyElofordulas(e));
            }
            return Task.FromResult(copy);
        }

        public Task<List<int>> GetAlkatreszek(string input = "")
        {
            string kereses = input.ToUpper();
            List<int> keresettAlkatreszek = new List<int>();
            keresettAlkatreszek = alkatreszek
               .Where(x => x.Nev.ToUpper().Contains(kereses) || x.Gyarto.TeljesNev.ToUpper().Contains(kereses) || x.Kategoria.Nev.ToUpper().Contains(kereses) || x.Tipus.ToUpper().Contains(kereses) || x.Cikkszam.ToUpper().Contains(kereses) || x.Leiras.ToUpper().Contains(kereses))
               .Select(x => x.Id)
               .ToList();
            return Task.FromResult(keresettAlkatreszek);
        }

        public Task<byte[]> GetFoto(int id)
        {
            var keresett = alkatreszek.Find(x => x.Id == id);
            if(keresett == null || keresett.Foto == null)
            {
                throw new HttpRequestException("Not Found");
            }
            return Task.FromResult(keresett.Foto);
        }

        public Task<ObservableCollection<Gyarto>> GetGyartok()
        {
            return Task.FromResult(gyartok);
        }

        public Task<ObservableCollection<Kategoria>> GetKategoriak()
        {
            return Task.FromResult(kategoriak);
        }

        public Task<ObservableCollection<MennyisegiEgyseg>> GetMennyisegiEgysegek()
        {
            return Task.FromResult(mennyisegiEgysegek);
        }

        public Task<ObservableCollection<RaktarozasiHely>> GetRaktarozasiHelyek()
        {
            return Task.FromResult(raktarak);
        }

        public Task<Alkatresz> PostAlkatresz(Alkatresz alkatresz)
        {
            if (!AlkatreszValid(alkatresz))
            {
                throw new HttpRequestException("Bad Request");
            }
            alkatresz.Id = alkatreszId;
            alkatreszId++;
            var newAlkatresz = CopyAlkatresz(alkatresz);
            alkatreszek.Add(newAlkatresz);
            return Task.FromResult(alkatresz);
        }

        public Task<AlkatreszElofordulas> PostElofodulas(AlkatreszElofordulas elofordulas)
        {
            if (!AlkatreszElofordulasValid(elofordulas))
            {
                throw new HttpRequestException("Bad Request");
            }
            elofordulas.Id = elofordulasId;
            elofordulasId++;
            var newElofordulas = CopyElofordulas(elofordulas);
            elofordulasok.Add(newElofordulas);
            return Task.FromResult(elofordulas);
        }

        public Task PostFoto(int id, byte[] foto)
        {
            var alkatresz = alkatreszek.Find(x => x.Id == id);
            if(alkatresz == null)
            {
                throw new HttpRequestException("Not Found");
            }
            alkatresz.Foto = new byte[foto.Length];
            foto.CopyTo(alkatresz.Foto, 0);
            return Task.CompletedTask;
        }

        public Task<Alkatresz> UpdateAlkatresz(Alkatresz alkatresz)
        {
            if (!AlkatreszValid(alkatresz))
            {
                throw new HttpRequestException("Bad Request");
            }
            var keresett = alkatreszek.Find(x => x.Id == alkatresz.Id);
            if(keresett == null)
            {
                throw new HttpRequestException("Not Found");
            }
            keresett.GyartoId = alkatresz.GyartoId;
            keresett.Gyarto = alkatresz.Gyarto;
            keresett.KategoriaId = alkatresz.KategoriaId;
            keresett.Kategoria = alkatresz.Kategoria;
            keresett.MennyisegiEgysegId = alkatresz.MennyisegiEgysegId;
            keresett.MennyisegiEgyseg = alkatresz.MennyisegiEgyseg;
            keresett.Nev = alkatresz.Nev;
            keresett.Cikkszam = alkatresz.Cikkszam;
            keresett.Tipus = alkatresz.Tipus;
            keresett.Foto = alkatresz.Foto;
            keresett.Leiras = alkatresz.Leiras;
            return Task.FromResult(alkatresz);
        }

        public Task UpdateElofordulasQuantity(int id, int quantity)
        {
            var keresett = elofordulasok.Find(x => x.Id == id);
            if(keresett == null)
            {
                throw new HttpRequestException("Not Found");
            }
            keresett.Mennyiseg = quantity;
            return Task.CompletedTask;
        }

        private Alkatresz CopyAlkatresz(Alkatresz original)
        {
            Alkatresz copy = new Alkatresz
            {
                Id = original.Id,
                GyartoId = original.GyartoId,
                Gyarto = original.Gyarto,
                KategoriaId = original.KategoriaId,
                Kategoria = original.Kategoria,
                MennyisegiEgysegId = original.MennyisegiEgysegId,
                MennyisegiEgyseg = original.MennyisegiEgyseg,
                Nev = original.Nev,
                Cikkszam = original.Cikkszam,
                Tipus = original.Tipus,
                Foto = original.Foto,
                Leiras = original.Leiras
            };
            return copy;
        }
        private AlkatreszElofordulas CopyElofordulas(AlkatreszElofordulas original)
        {
            AlkatreszElofordulas copy = new AlkatreszElofordulas
            {
                Id = original.Id,
                AlkatreszId = original.AlkatreszId,
                RaktarozasiHelyId = original.RaktarozasiHelyId,
                RaktarozasiHely = original.RaktarozasiHely,
                Mennyiseg = original.Mennyiseg,
                Polc = original.Polc,
                Szint = original.Szint
            };
            return copy;
        }
        private bool AlkatreszValid(Alkatresz alkatresz)
        {
            bool gyartoHelyes = gyartok.Any(x => x.Id == alkatresz.GyartoId);
            bool kategoriaHelyes = kategoriak.Any(x => x.Id == alkatresz.KategoriaId);
            bool egysegHelyes = mennyisegiEgysegek.Any(x => x.Id == alkatresz.MennyisegiEgysegId);
            bool alkatreszHelyes = !string.IsNullOrWhiteSpace(alkatresz.Nev) && !string.IsNullOrWhiteSpace(alkatresz.Tipus) && !string.IsNullOrWhiteSpace(alkatresz.Cikkszam);
            return gyartoHelyes && kategoriaHelyes && egysegHelyes && alkatreszHelyes;
        }
        private bool AlkatreszElofordulasValid(AlkatreszElofordulas alkatreszElofordulas)
        {
            bool alkatreszHelyes = alkatreszek.Any(x => x.Id == alkatreszElofordulas.AlkatreszId);
            bool raktarHelyes = raktarak.Any(x => x.Id == alkatreszElofordulas.RaktarozasiHelyId);
            bool elofordulasHelyes = alkatreszElofordulas.Mennyiseg >= 0;
            return alkatreszHelyes && raktarHelyes && elofordulasHelyes;
        }
    }
}
