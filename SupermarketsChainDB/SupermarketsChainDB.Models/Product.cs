namespace SupermarketsChainDB.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        //[ForeignKey("Vendor")]
        public int VendorID { get; set; }

        public virtual Vendor Vendors { get; set; }

        [Required]
        [Column("Product Name")]
        public string ProductName { get; set; }

        [Required]
        //[ForeignKey("Measure")]
        public int MeasureID { get; set; }

        public virtual Measure Measures { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}