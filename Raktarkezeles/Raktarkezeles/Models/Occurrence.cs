using System;
using System.Collections.Generic;
using System.Text;

namespace Raktarkezeles.Models
{
    public class Occurrence
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public Part Part { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int Rack { get; set; }
        public int Shelf { get; set; }
        public int Quantity { get; set; }
    }
}
