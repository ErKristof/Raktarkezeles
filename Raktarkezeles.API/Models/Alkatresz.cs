using System.Collections.Generic;

namespace Raktarkezeles.API.Models
{
    public partial class Alkatresz
    {
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
        public Alkatresz()
        {
            AlkatreszElofordulasok = new List<AlkatreszElofordulas>();
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
            AlkatreszElofordulasok = new List<AlkatreszElofordulas>();
        }
        public bool ShouldSerializeFoto() { return false; }
    }
}
