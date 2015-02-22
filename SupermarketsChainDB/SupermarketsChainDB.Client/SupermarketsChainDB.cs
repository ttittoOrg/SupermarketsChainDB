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

    public class SupermarketsChainDB
    {
       
        static void Main(string[] args)
        {
         String ConnStr = "Server={0};Database=SupermarketsChainDB;User Id=yavor;Password=Limon123;";

         String database = "192.168.137.156";

         ConnStr = String.Format(ConnStr, database);

         SqlManagerConnection sqlManager = new SqlManagerConnection(ConnStr);


            DateTime fromDate = DateTime.ParseExact("20.07.2014", "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact("22.07.2014", "dd.MM.yyyy", CultureInfo.InvariantCulture);
            List<Product> p = sqlManager.GetProducts(fromDate, toDate);


            foreach (Product item in p)
            {
                Console.WriteLine(item.Sum);
                Console.WriteLine(item.Quantity);
            }

        }
    }
}
