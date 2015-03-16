namespace JSONReportsMongoDB
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Newtonsoft.Json;

    public class SalesReport
    {
        [JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [JsonProperty("product-id")]
        [BsonElement("product-id")]
        public int ProductId { get; set; }

        [JsonProperty("product-name")]
        [BsonElement("product-name")]
        public string ProductName { get; set; }

        [JsonProperty("vendor-name")]
        [BsonElement("vendor-name")]
        public string VendorName { get; set; }

        [JsonProperty("total-quantity-sold")]
        [BsonElement("total-quantity-sold")]
        public decimal TotalQuantitySold { get; set; }

        [JsonProperty("total-incomes")]
        [BsonElement("total-incomes")]
        public decimal TotalIncome { get; set; }

        // this is for testing purposes only TO BE DELETED!
        public override string ToString()
        {
            return string.Format("Id: {0}\nName: {1}\nVendor:{2}\nTotal quantity: {3}\nTotal income: {4}",
                this.ProductId,
                this.ProductName,
                this.VendorName,
                this.TotalQuantitySold,
                this.TotalIncome);
        }
    }
}
