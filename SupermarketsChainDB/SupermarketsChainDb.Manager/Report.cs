using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketsChainDb.Manager
{
    public class Report
    {
        public Report()
        {
            this.ProductID = new List<int>();
            this.Quantity = new List<int>();
            this.UnitPrice = new List<decimal>();
            this.Sum = new List<decimal>();
        }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public List<int> ProductID { get; set; }

        public List<int> Quantity { get; set; }

        public List<decimal> UnitPrice { get; set; }

        public List<decimal> Sum { get; set; }
    }
}