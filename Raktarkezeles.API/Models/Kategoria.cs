using System;
using System.Collections.Generic;

#nullable disable

namespace Raktarkezeles.API.Models
{
    public partial class Kategoria
    {
        public Kategoria()
        {
        }

        public int Id { get; set; }
        public string Nev { get; set; }
    }
}
