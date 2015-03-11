namespace SupermarketsChainDB.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Vendor
    {
        private ICollection<Product> products;

        public Vendor()
        {
            this.products = new HashSet<Product>();
        }

        [Key]
        public int ID { get; set; }

        [Column("Vendor Name")]
        public string VendorName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}