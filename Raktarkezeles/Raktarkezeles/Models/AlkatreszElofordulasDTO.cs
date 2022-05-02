using System;
using System.Collections.Generic;
using System.Text;

namespace Raktarkezeles.Models
{
    public class AlkatreszElofordulasDTO
    {
        public int Id { get; set; }
        public int AlkatreszId { get; set; }
        public int RaktarozasiHelyId { get; set; }
        public int Polc { get; set; }
        public int Szint { get; set; }
        public int Mennyiseg { get; set; }

        public AlkatreszElofordulasDTO() { }
        public AlkatreszElofordulasDTO(AlkatreszElofordulas alkatreszElofordulas)
        {
            Id = alkatreszElofordulas.Id;
            AlkatreszId = alkatreszElofordulas.AlkatreszId;
            RaktarozasiHelyId = alkatreszElofordulas.RaktarozasiHelyId;
            Polc = alkatreszElofordulas.Polc;
            Szint = alkatreszElofordulas.Szint;
            Mennyiseg = alkatreszElofordulas.Mennyiseg;
        }
    }
}
