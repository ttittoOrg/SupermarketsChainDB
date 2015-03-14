namespace SupermarketsChainDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Store
    {
        private ICollection<Product> products;
        private ICollection<Sale> sales;

        public Store()
        {
            this.products = new HashSet<Product>();
            this.sales = new HashSet<Sale>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}