namespace SupermarketsChainDB.Models
{
    using System;

    public class Sale
    {
        public int Id { get; set; }

        public int StoreId { get; set; }

        public virtual Store Store { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal SinglePrice { get; set; }

        public decimal Sum { get; set; }

        public DateTime Date { get; set; }
    }
}