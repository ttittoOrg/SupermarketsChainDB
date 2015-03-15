namespace SupermarketsChainDb.Manager
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Oracle.DataAccess.Client;

    public class OracleToMSSQL
    {
        private string connectionString = Connection.GetOracleConnectionString();

        private static OracleConnection con;

        public void Connect()
        {
            var con = new OracleConnection();
            if (OracleConnection.IsAvailable)
            {
                con.ConnectionString = "context connection=true";
            }
            else
            {
                con = new OracleConnection { ConnectionString = connectionString };
                con.Open();
                Console.WriteLine("Connected to Oracle" + con.ServerVersion);
            }
        }

        public List<string> GetProducts()
        {
            con = new OracleConnection { ConnectionString = connectionString };
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT P_ID, " +
                                    "VENDOR_ID," +
                                    "PRODUCT_NAME, " +
                                    "MEASURE_ID, " +
                                    "PRICE, " +
                                    "QUANTITY, " +
                                    "PRODUCT_TYPE " +
                              "FROM PRODUCTS";

            var products = new List<string>();
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string productName = (string)reader["PRODUCT_TYPE"];
                    products.Add(productName);
                    //Console.WriteLine(myField);
                }
            }

            return products;
        }

        public void Close()
        {
            con.Close();
            con.Dispose();
        }
    }
}