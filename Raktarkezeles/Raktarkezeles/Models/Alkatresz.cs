using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Raktarkezeles.MVVM;
using System.Linq;
using System.Collections.ObjectModel;

namespace Raktarkezeles.Models
{
    public class Alkatresz : BindableBase
    {
        public Alkatresz()
        {
            AlkatreszElofordulasok = new ObservableCollection<AlkatreszElofordulas>();
        }
        public Alkatresz(AlkatreszDTO alkatreszDTO)
        {
            GyartoId = alkatreszDTO.GyartoId;
            KategoriaId = alkatreszDTO.KategoriaId;
            MennyisegiEgysegId = alkatreszDTO.MennyisegiEgysegId;
            Nev = alkatreszDTO.Nev;
            Tipus = alkatreszDTO.Tipus;
            Cikkszam = alkatreszDTO.Cikkszam;
            Leiras = alkatreszDTO.Leiras;
        }
        public int Id { get; set; }
        private string nev;
        public string Nev { get { return nev; } set { nev = value; OnPropertyChanged(); } }
        public int GyartoId { get; set; }
        private Gyarto gyarto;
        public Gyarto Gyarto { get { return gyarto; } set { gyarto = value; OnPropertyChanged(); } }
        private string tipus;
        public string Tipus { get { return tipus; } set { tipus = value; OnPropertyChanged(); } }
        private string cikkszam;
        public string Cikkszam { get { return cikkszam; } set { cikkszam = value; OnPropertyChanged(); } }
        public int KategoriaId { get; set; }
        private Kategoria kategoria;
        public Kategoria Kategoria { get { return kategoria; } set { kategoria = value; OnPropertyChanged(); } }
        public int MennyisegiEgysegId { get; set; }
        private MennyisegiEgyseg mennyisegiEgyseg;
        public MennyisegiEgyseg MennyisegiEgyseg { get { return mennyisegiEgyseg; } set { mennyisegiEgyseg = value; OnPropertyChanged(); } }
        private string leiras;
        public string Leiras { get { return leiras; } set { leiras = value; OnPropertyChanged(); } }
        public byte[] Foto { get; set; }
        public int Mennyiseg => AlkatreszElofordulasok.Sum(x => x.Mennyiseg);
        public void MennyisegChanged() => OnPropertyChanged(nameof(Mennyiseg));
        public ObservableCollection<AlkatreszElofordulas> AlkatreszElofordulasok { get; set; }
    }
}
