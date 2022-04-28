using System;
using System.Collections.Generic;

#nullable disable

namespace Raktarkezeles.API.Models
{
    public partial class MennyisegiEgyseg
    {
        public MennyisegiEgyseg()
        {
        }

        public int Id { get; set; }
        public string RovidNev { get; set; }
        public string TeljesNev { get; set; }
    }
}
