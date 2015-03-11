using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLManager
{
    public class Product
    {
        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string Location { get; set; }

        public decimal Sum { get; set; }

        public DateTime LoadingDate { get; set; }

        public decimal Total { get; set; }
    }
}
