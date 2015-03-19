namespace SupermarketsChainDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Vendor")]
        public int VendorId { get; set; }

        public virtual Vendor Vendor { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Total { get; set; }
    }
}
