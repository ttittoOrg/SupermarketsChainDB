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

        public static void Main()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SupermarketSystemDbContext, Configuration>());
            var data = new SupermarketSystemData();

            Console.WriteLine("----------------------------------------------------------------------------");
            // Task 1 - Migrate from Oracle to MSSQL SERVER
            Console.WriteLine("Migrating data from ORACLE to MS SQL SERVER");
            OracleToSqlDb.MigrateToSql();
            Console.WriteLine(" ... migration finished\nPress any key to proceed with loading Excel reports to SQL SERVER");
            Console.ReadLine();

            Console.WriteLine("----------------------------------------------------------------------------");
            // Task 2 - load excell reports from zip
            Console.WriteLine("\nLoading Excel reports to SQL SERVER");
            SalesReportsMigrator reportsMigrator = new SalesReportsMigrator(reportsFile);
            reportsMigrator.MigrateSalesReport();
            Console.WriteLine("... migrations finished.\nPress any key to proceed withh generating pdf reports");
            Console.ReadLine();

            Console.WriteLine("----------------------------------------------------------------------------");
            // Task 3 - Generate pdf reports 
            Console.WriteLine("\nGenerating PDF reports");
            PdfReportHandler pdfReport = new PdfReportHandler(data, @"../../../Output/Sales-Reports");
            pdfReport.CreateReport(new DateTime(2000, 7, 20), new DateTime(2014, 7, 22));
            Console.WriteLine("Genrating PDF reports finished.\nPress any key to proceed withh generating XML reports");
            Console.ReadLine();

            Console.WriteLine("----------------------------------------------------------------------------");
            // Task 4 - Generate XML Sales by Vendor Report
            Console.WriteLine("\nGenerating XML reports");
            XMLFromToMSSQL xmlParser = new XMLFromToMSSQL(data, @"../../../Input/Sample-Vendor-Expenses.xml");
            xmlParser.GenerateSalesByVendorReport(@"../../../Output/Sales-by-Vendors-Report.xml",
                new DateTime(2014, 07, 01),
                new DateTime(2014, 07, 31));
            Console.WriteLine("Genrating XML reports finished.\nPress any key to proceed with generating JSON repots and uploding to MongoDB");
            Console.ReadLine();

            Console.WriteLine("----------------------------------------------------------------------------");
            // Task 5 -JSON Reports in MongoDB
            SalesReportHandler reportsHandler = new SalesReportHandler(data, @"../../../Output/Json-Reports");
            reportsHandler.SaveReportsToFiles(new DateTime(2014, 7, 20), new DateTime(2014, 7, 22));
            string localhost = "localhost";
            string cloud = "cloud";
            reportsHandler.SaveReportsToMongoDb(localhost);
            reportsHandler.SaveReportsToMongoDb(cloud);
            Console.WriteLine(" ... finished.\nPress any key to proceed with loading expenses data from XML");
            Console.ReadLine();

            Console.WriteLine("----------------------------------------------------------------------------");
            // Task 6 – Load Expense Data from XML
            Console.WriteLine("\nLoading expense data from XML");
            xmlParser.SaveExpenses();
            Console.WriteLine(" ... finished.\nPress any key to proceed with migrating data to MySQL");
            Console.ReadLine();

            Console.WriteLine("----------------------------------------------------------------------------");
            // Task 7 - Load Data to MySQL
            Console.WriteLine("Migrating data to MySQL...");
            MsSqlToMySql.MigrateToMySql();
            Console.WriteLine(" ... finished.\nEnough already.");
            Console.ReadLine();

        }
    }
}