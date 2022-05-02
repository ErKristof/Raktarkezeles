using System;
using System.Collections.Generic;
using System.Text;
using Raktarkezeles.MVVM;

namespace Raktarkezeles.Models
{
    public class AlkatreszElofordulas : BindableBase
    {
        public AlkatreszElofordulas() { }
        public AlkatreszElofordulas(AlkatreszElofordulasDTO alkatreszElofordulasDTO)
        {
            AlkatreszId = alkatreszElofordulasDTO.AlkatreszId;
            RaktarozasiHelyId = alkatreszElofordulasDTO.RaktarozasiHelyId;
            Polc = alkatreszElofordulasDTO.Polc;
            Szint = alkatreszElofordulasDTO.Szint;
            Mennyiseg = alkatreszElofordulasDTO.Mennyiseg;
        }
        public int Id { get; set; }
        public int AlkatreszId { get; set; }
        public Alkatresz Alkatresz { get; set; }
        public int RaktarozasiHelyId { get; set; }
        public RaktarozasiHely RaktarozasiHely { get; set; }
        public int Polc { get; set; }
        public int Szint { get; set; }
        private int mennyiseg;
        public int Mennyiseg
        {
            get
            {
                return mennyiseg;
            }
            set
            {
                if (mennyiseg != value)
                {
                    mennyiseg = value;
                    OnPropertyChanged();
                }
            }
        }
        public override string ToString()
        {
            return RaktarozasiHely != null ? RaktarozasiHely.Nev + " " + Polc + ". polc " + Szint + ". szint" : base.ToString();
        }
    }
}
