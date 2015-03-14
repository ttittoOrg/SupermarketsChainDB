namespace SupermarketsChainDb.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Connection
    {
        private const string oracleConnectionString = "";
        private const string sqlConnectionString = "";
        private const string mySqlConnectionString = "";
        private const string mongoConnectionString = "";

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
            return mongoConnectionString;
        }
    }
}