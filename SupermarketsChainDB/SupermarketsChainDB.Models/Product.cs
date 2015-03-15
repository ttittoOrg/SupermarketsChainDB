namespace SupermarketsChainDB.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        private ICollection<Store> stores;
        private ICollection<Sale> sales;

        public Product()
        {
            this.stores = new HashSet<Store>();
            this.sales = new HashSet<Sale>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        //[ForeignKey("Vendor")]
        public int VendorID { get; set; }

        public virtual Vendor Vendor { get; set; }

        [Required]
        [Column("Product Name")]
        public string ProductName { get; set; }

        [Required]
        //[ForeignKey("Measure")]
        public int MeasureID { get; set; }

        public virtual Measure Measure { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public ProductType ProductType { get; set; }

        public virtual ICollection<Store> Stores { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}