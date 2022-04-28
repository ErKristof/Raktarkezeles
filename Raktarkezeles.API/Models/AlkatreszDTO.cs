using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raktarkezeles.API.Models
{
    public class AlkatreszDTO
    {
        public int Id { get; set; }
        public int GyartoId { get; set; }
        public int KategoriaId { get; set; }
        public int MennyisegiEgysegId { get; set; }
        public string Nev { get; set; }
        public string Tipus { get; set; }
        public string Cikkszam { get; set; }
        public string Leiras { get; set; }

        public AlkatreszDTO() { }
        public AlkatreszDTO(Alkatresz alkatresz)
        {
            Id = alkatresz.Id;
            GyartoId = alkatresz.GyartoId;
            KategoriaId = alkatresz.KategoriaId;
            MennyisegiEgysegId = alkatresz.MennyisegiEgysegId;
            Nev = alkatresz.Nev;
            Tipus = alkatresz.Tipus;
            Cikkszam = alkatresz.Cikkszam;
            Leiras = alkatresz.Leiras;
        }
    }
}
