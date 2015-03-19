namespace SupermarketsChainDB.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Vendor
    {
        private ICollection<Product> products;
        private ICollection<Expense> expenses;

        public Vendor()
        {
            this.products = new HashSet<Product>();
            this.expenses = new HashSet<Expense>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("Vendor Name")]
        public string VendorName { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
    }
}