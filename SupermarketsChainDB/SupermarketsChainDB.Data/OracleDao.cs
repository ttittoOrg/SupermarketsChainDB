namespace SupermarketsChainDB.Data.OracleDb
{
    using System;
    //using System.Data.OracleClient;
    using System.Collections.Generic;

    using Oracle.DataAccess.Client;
    using System.Linq;

    using SupermarketsChainDB.Models;

    //using Oracle.DataAccess.Client;

    public class OracleDao
    {
        private const string ConnectionString = 
            "User Id=SuperMarketChain;Password=111333a;Data Source=localhost:1521/xe";

        private static OracleConnection con;

        public static void Connect()
        {
            var con = new OracleConnection();
            if (OracleConnection.IsAvailable)
            {
                con.ConnectionString = "context connection=true";
            }
            else
            {
                con = new OracleConnection { ConnectionString = ConnectionString };
                con.Open();
                Console.WriteLine("Connected to Oracle" + con.ServerVersion);
            }
        }

        public static List<string> GetProducts()
        {
            con = new OracleConnection { ConnectionString = ConnectionString };
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
                    string productName = (string)reader["PRODUCT_NAME"];
                    products.Add(productName);
                    //Console.WriteLine(myField);
                }

                reader.Dispose();
            }

            return products;
        }

        public static void Close()
        {
            con.Close();
            con.Dispose();
        } 

        //ConnectAndQuery();
    }
}