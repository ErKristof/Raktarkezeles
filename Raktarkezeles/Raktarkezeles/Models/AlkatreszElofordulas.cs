using Raktarkezeles.DTOModels;
using Raktarkezeles.MVVM;

namespace Raktarkezeles.Models
{
    public class AlkatreszElofordulas : BindableBase
    {
        private int mennyiseg;
        public int Id { get; set; }
        public Alkatresz Alkatresz { get; set; }
        public int AlkatreszId { get; set; }
        public int RaktarozasiHelyId { get; set; }
        public RaktarozasiHely RaktarozasiHely { get; set; }
        public int Polc { get; set; }
        public int Szint { get; set; }
        public int Mennyiseg { get { return mennyiseg; } set { if (mennyiseg != value) { mennyiseg = value; OnPropertyChanged(); } } }
        public AlkatreszElofordulas()
        {
            Id = -1;
            AlkatreszId = -1;
            RaktarozasiHelyId = -1;
            RaktarozasiHely = new RaktarozasiHely();
            Polc = 0;
            Szint = 0;
            Mennyiseg = 0;
        }
        public AlkatreszElofordulas(AlkatreszElofordulasDTO alkatreszElofordulasDTO)
        {
            Id = alkatreszElofordulasDTO.Id;
            AlkatreszId = alkatreszElofordulasDTO.AlkatreszId;
            RaktarozasiHelyId = alkatreszElofordulasDTO.RaktarozasiHelyId;
            RaktarozasiHely = new RaktarozasiHely();
            Polc = alkatreszElofordulasDTO.Polc;
            Szint = alkatreszElofordulasDTO.Szint;
            Mennyiseg = alkatreszElofordulasDTO.Mennyiseg;
        }
        public override string ToString()
        {
            return RaktarozasiHely.Nev + " " + Polc + ". polc " + Szint + ". szint";
        }
    }
}
