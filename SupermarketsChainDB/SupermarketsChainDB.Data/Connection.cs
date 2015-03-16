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
        private const string mongoDatabaseHost = "mongodb://127.0.0.1";
        private const string mongoDatabaseName = "SalesByProductReports";

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

        public static string[] GetMongoConnectionString()
        {
            string[] connectionSting = new string[] {mongoDatabaseHost, mongoDatabaseName };
            return connectionSting;
        }
    }
}