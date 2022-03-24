using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Raktarkezeles.MVVM;

namespace Raktarkezeles.Models
{
    public class Part : BindableBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public string TypeNumber { get; set; }
        public string ItemNumber { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        private int quantity;
        public int Quantity {
            get
            {
                return quantity;
            }
            set
            {
                if(quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICollection<Occurrence> Occurrences { get; set; }
    }
}
