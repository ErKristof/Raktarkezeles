using System;
using System.Collections.Generic;

#nullable disable

namespace Raktarkezeles.API.Models
{
    public partial class AlkatreszElofordulas
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
        public int RaktarozasiHelyId { get; set; }
        public short Polc { get; set; }
        public short Szint { get; set; }
        public int Mennyiseg { get; set; }

        public virtual RaktarozasiHely RaktarozasiHely { get; set; }
    }
}
