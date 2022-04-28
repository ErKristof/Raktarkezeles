using System;
using System.Collections.Generic;

#nullable disable

namespace Raktarkezeles.API.Models
{
    public partial class Gyarto
    {
        public Gyarto()
        {
        }

        public int Id { get; set; }
        public string RovidNev { get; set; }
        public string TeljesNev { get; set; }
    }
}
