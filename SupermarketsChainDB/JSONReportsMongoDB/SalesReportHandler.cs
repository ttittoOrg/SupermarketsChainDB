namespace JSONReportsMongoDB
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    //  using MongoDB.Driver;
    using SupermarketsChainDB.Models;
    using SupermarketsChainDB.Data;
  

    public class SalesReportHandler
    {
        private SupermarketSystemData data;
        private string reportsFilePath;

        public SalesReportHandler(SupermarketSystemData data, string reportsFilePath)
        {
            this.data = data;
            this.reportsFilePath = reportsFilePath;
        }

        public void SaveReportsToFiles(DateTime startDate, DateTime endDate)
        {
            var salesReports = this.GenerateReports(startDate, endDate);
            foreach (var report in salesReports)
            {
                string fileName = report.ProductId.ToString() + ".json";
                var filePath = this.reportsFilePath + "/" + fileName;
                var serializedReport = JsonConvert.SerializeObject(report, Formatting.Indented);
                File.WriteAllText(filePath, serializedReport);
            }

            Console.WriteLine(string.Format("{0} reports were generated.", salesReports.Count));
        }

        private List<SalesReport> GenerateReports(DateTime startDate, DateTime endDate)
        {
            var salesReports = new List<SalesReport>();
            var sales = this.data.Sales
                .Search(s => s.Date >= startDate && s.Date <= endDate)
                .Include(s => s.Product)
                .Include(s => s.Product.Vendor)
                .Select(s => new 
                {
                    productId = s.ProductId,
                    productName = s.Product.ProductName,
                    vendorName = s.Product.Vendor.VendorName,
                    quantitySold = s.Quantity,
                    income = s.Sum
                })
                .ToList();

            if (sales.Count == 0)
            {
                throw new ArgumentNullException("statDate/endDate", string.Format("There are now sales in between {0} and  {1}",
                    startDate.ToShortDateString(),
                    endDate.ToShortDateString()));
            }

            var groupedSales = sales.GroupBy(s => s.productId);
            foreach (var salesGroup in groupedSales)
            {
                SalesReport currentReport;
                var currentSalesGroup = salesGroup.ToList();
                string productName = currentSalesGroup.FirstOrDefault().productName;
                string vendorName = currentSalesGroup.FirstOrDefault().vendorName;
                decimal totalQuantitySold = 0.0M;
                decimal totalIncome = 0.0M;

                foreach (var sale in currentSalesGroup)
	            {
                    totalQuantitySold += sale.quantitySold;
                    totalIncome += sale.income;
	            }

                currentReport = new SalesReport
                {
                    ProductId = salesGroup.Key,
                    ProductName = productName,
                    VendorName = vendorName,
                    TotalQuantitySold = totalQuantitySold,
                    TotalIncome = totalIncome,
                };

                salesReports.Add(currentReport);
            }

            return salesReports;
        }

        // private MongoDatabase monogoDb = GetDatabase(mongoDbConnectionString);

        // private method for connecting to the mongo server and getting the database
        //private static MongoDatabase GetDatabase(string[] mongoDbConnectionString)
        //{
        //    var mongoClient = new MongoClient(mongoDbConnectionString[0]);
        //    var server = mongoClient.GetServer();
        //    return server.GetDatabase(mongoDbConnectionString[1]);
        //}
    }
}
