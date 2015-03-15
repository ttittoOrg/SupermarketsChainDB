namespace SupermarketsChainDB.Client
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SQLManager;
    using System.Data.Entity;
    using SupermarketsChainDB.Data.Migrations;
    using SupermarketsChainDB.Data;
    using SupermarketsChainDb.Manager;

    public class SupermarketsMain
    {
        private const string reportsFile = "../../../SalesInfo/Sales-Reports.zip";

        static void Main(string[] args)
        {
            
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SupermarketSystemDbContext, Configuration>());
            var data = new SupermarketSystemData();

            OracleToSqlDb.MigrateToSql();

            SalesReportsMigrator reportsMigrator = new SalesReportsMigrator(reportsFile);
            reportsMigrator.ExtractReports();
            reportsMigrator.GetAllReports();
            reportsMigrator.FillTable();
            reportsMigrator.DeleteReports();

            

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
        }
    }
}