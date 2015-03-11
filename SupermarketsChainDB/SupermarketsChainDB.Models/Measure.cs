namespace SupermarketsChainDB.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Measure
    {
        private ICollection<Product> products;

        public Measure()
        {
            this.products = new HashSet<Product>();
        }

        [Key]
        public int ID { get; set; }

        [Column("Measure Name")]
        public string MeasureName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}