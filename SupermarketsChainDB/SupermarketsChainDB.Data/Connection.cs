using System;

namespace SupermarketsChainDB.Data
{
    public static class Connection
    {
        private const string OracleConnectionString =
            "User Id=SuperMarketChain;Password=111333a;Data Source=localhost:1521/xe";

        private const string SqlConnectionString = "";

        private const string MySqlConnectionString =
            "Server=localhost;Port=3306;Database=SuperMarketChain;Uid=root;Pwd=111333a;";

        private const string MongoDbConnectionStringLocalhost = "mongodb://127.0.0.1 SalesByProductReports";
        private const string MongoDbConnectionStringCloud = 
            "mongodb://teamlepus:softuni777@ds061767.mongolab.com:61767/salesbyproductreports salesbyproductreports";

        private const string SqLiteConnectionString = "Data Source=MarketChain.sqlite;Version=3;";

        public static string GetSqLiteConnectionString() 
        {
            return SqLiteConnectionString;
        }

        public static string GetOracleConnectionString()
        {
            return OracleConnectionString;
        }

        public static string GetSqlConnectionString()
        {
            return SqlConnectionString;
        }

        public static string GetMySqlConnectionString()
        {
            return MySqlConnectionString;
        }

        public static string GetMongoConnectionString()
        {
            return MongoDbConnectionStringLocalhost;
        }

        public static string GetMongoConnectionStringCloud()
        {
            return MongoDbConnectionStringCloud;
        }
    }
}