using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable disable

namespace Raktarkezeles.API.Models
{
    public partial class Alkatresz
    {
        public Alkatresz()
        {
            AlkatreszElofordulasok = new HashSet<AlkatreszElofordulas>();
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
        public int GyartoId { get; set; }
        public int KategoriaId { get; set; }
        public int MennyisegiEgysegId { get; set; }
        public string Nev { get; set; }
        public string Tipus { get; set; }
        public string Cikkszam { get; set; }
        public byte[] Foto { get; set; }
        public string Leiras { get; set; }

        public virtual Gyarto Gyarto { get; set; }
        public virtual Kategoria Kategoria { get; set; }
        public virtual MennyisegiEgyseg MennyisegiEgyseg { get; set; }
        public virtual ICollection<AlkatreszElofordulas> AlkatreszElofordulasok { get; set; }

        public bool ShouldSerializeFoto() { return false; }
    }
}
