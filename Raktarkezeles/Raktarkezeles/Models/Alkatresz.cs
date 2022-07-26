using Raktarkezeles.MVVM;
using System.Linq;
using System.Collections.ObjectModel;
using Raktarkezeles.DTOModels;

namespace Raktarkezeles.Models
{
    public class Alkatresz : BindableBase
    {
        private string nev;
        private Gyarto gyarto;
        private Kategoria kategoria;
        private MennyisegiEgyseg mennyisegiEgyseg;
        private string tipus;
        private string cikkszam;
        private string leiras;
        private byte[] foto;
        public int Id { get; set; }
        public int GyartoId { get; set; }
        public int KategoriaId { get; set; }
        public int MennyisegiEgysegId { get; set; }
        public string Nev { get { return nev; } set { nev = value; OnPropertyChanged(); } }
        public Gyarto Gyarto { get { return gyarto; } set { gyarto = value; OnPropertyChanged(); } }
        public Kategoria Kategoria { get { return kategoria; } set { kategoria = value; OnPropertyChanged(); } }
        public MennyisegiEgyseg MennyisegiEgyseg { get { return mennyisegiEgyseg; } set { mennyisegiEgyseg = value; OnPropertyChanged(); } }
        public string Tipus { get { return tipus; } set { tipus = value; OnPropertyChanged(); } }
        public string Cikkszam { get { return cikkszam; } set { cikkszam = value; OnPropertyChanged(); } }
        public string Leiras { get { return leiras; } set { leiras = value; OnPropertyChanged(); } }
        public byte[] Foto { get { return foto; } set { foto = value; OnPropertyChanged(); } }
        public int Mennyiseg => AlkatreszElofordulasok.Sum(x => x.Mennyiseg);
        public void MennyisegChanged() => OnPropertyChanged(nameof(Mennyiseg));
        public ObservableCollection<AlkatreszElofordulas> AlkatreszElofordulasok { get; set; }
        public Alkatresz()
        {
            Id = -1;
            nev = "";
            GyartoId = -1;
            gyarto = new Gyarto();
            tipus = "";
            cikkszam = "";
            KategoriaId = -1;
            kategoria = new Kategoria();
            MennyisegiEgysegId = -1;
            mennyisegiEgyseg = new MennyisegiEgyseg();
            leiras = "";
            AlkatreszElofordulasok = new ObservableCollection<AlkatreszElofordulas>();
        }
        public Alkatresz(AlkatreszDTO alkatreszDTO)
        {
            Id = alkatreszDTO.Id;
            nev = alkatreszDTO.Nev;
            GyartoId = alkatreszDTO.GyartoId;
            gyarto = new Gyarto();
            tipus = alkatreszDTO.Tipus;
            cikkszam = alkatreszDTO.Cikkszam;
            KategoriaId = alkatreszDTO.KategoriaId;
            kategoria = new Kategoria();
            MennyisegiEgysegId = alkatreszDTO.MennyisegiEgysegId;
            mennyisegiEgyseg = new MennyisegiEgyseg();
            leiras = alkatreszDTO.Leiras;
            AlkatreszElofordulasok = new ObservableCollection<AlkatreszElofordulas>();
        }
    }
}
