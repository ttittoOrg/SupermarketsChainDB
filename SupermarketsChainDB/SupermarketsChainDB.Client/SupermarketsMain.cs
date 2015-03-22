namespace SupermarketsChainDB.Client
{
    using System;
    using System.Data.Entity;

    using GeneratePDFSalesReports;
    using JSONReportsMongoDB;

    using SupermarketsChainDB.Data.Migrations;
    using SupermarketsChainDB.Data;
    using SupermarketsChainDb.Manager;

    public class SupermarketsMain
    {
        private const string reportsFile = "../../../Input/Sales-Reports.zip";

        static void Main()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SupermarketSystemDbContext, Configuration>());
            var data = new SupermarketSystemData();

            //SalesReportsMigrator reportsMigrator = new SalesReportsMigrator(reportsFile);
            //reportsMigrator.MigrateSalesReport();

            //XMLFromToMSSQL xmlParser = new XMLFromToMSSQL(data, @"../../../Input/Sample-Vendor-Expenses.xml");
            //xmlParser.SaveExpenses();
            

            //SalesReportHandler reportsHandler = new SalesReportHandler(data, @"../../../Output/Json-Reports");
            //reportsHandler.SaveReportsToFiles(new DateTime(2014, 7, 20), new DateTime(2014, 7, 22));

            //string localhost = "localhost";
            //string cloud = "cloud";

            //reportsHandler.SaveReportsToMongoDb(localhost);
            //reportsHandler.SaveReportsToMongoDb(cloud);

            //PdfReportHandler pdfReport = new PdfReportHandler(data, @"../../../Output/Sales-Reports");
            //pdfReport.CreateReport(new DateTime(2000, 7, 20), new DateTime(2014, 7, 22));

            //var measures = data.Measures.All().ToList();

            //String ConnStr = "Server=(localdb)v11.0;Database=SupermarketSystem;Integrated Security=True;";

            //String database = "192.168.137.156";

            //ConnStr = String.Format(ConnStr, database);

            //SqlManagerConnection sqlManager = new SqlManagerConnection(ConnStr);


            //DateTime fromDate = DateTime.ParseExact("20.07.2014", "dd.MM.yyyy", CultureInfo.InvariantCulture);
            //DateTime toDate = DateTime.ParseExact("22.07.2014", "dd.MM.yyyy", CultureInfo.InvariantCulture);
            //List<Product> p = sqlManager.GetProducts(fromDate, toDate);


            //foreach (Product item in p)
            //{
            //    Console.WriteLine(item.Sum);
            //    Console.WriteLine(item.Quantity);
            //}

            //OracleToSqlDb.MigrateToSql();
            //xmlParser.GenerateSalesByVendorReport(@"../../../Output/Sales-by-Vendors-Report.xml", new DateTime(2014, 07, 01), new DateTime(2014, 07, 31));

            MsSqlToMySql.MigrateToMySql();
        }
    }
}