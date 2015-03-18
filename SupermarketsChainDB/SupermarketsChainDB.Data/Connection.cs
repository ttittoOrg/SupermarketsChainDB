namespace SupermarketsChainDB.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Connection
    {
        private const string oracleConnectionString = "User Id=SuperMarketChain;Password=111333a;Data Source=localhost:1521/xe";
        private const string sqlConnectionString = "";
        private const string mySqlConnectionString = "";
        private const string mongoDbConnectionStringLocalhost = "mongodb://127.0.0.1 SalesByProductReports";
        private const string mongoDbConnectionStringCloud = "mongodb://teamlepus:softuni777@ds061767.mongolab.com:61767/salesbyproductreports salesbyproductreports";

        public static string GetOracleConnectionString()
        {
            return oracleConnectionString;
        }

        public static string GetSqlConnectionString()
        {
            return sqlConnectionString;
        }

        public static string GetMySqlConnectionString()
        {
            return mySqlConnectionString;
        }

        public static string GetMongoConnectionString()
        {
            return mongoDbConnectionStringLocalhost;
        }

        public static string GetMongoConnectionStringCloud()
        {
            return mongoDbConnectionStringCloud;
        }
    }
}