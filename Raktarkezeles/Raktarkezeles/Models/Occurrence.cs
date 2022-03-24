using System;
using System.Collections.Generic;
using System.Text;
using Raktarkezeles.MVVM;

namespace Raktarkezeles.Models
{
    public class Occurrence : BindableBase
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public Part Part { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int Rack { get; set; }
        public int Shelf { get; set; }
        private int quantity;
        public int Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
