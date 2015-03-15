namespace SupermarketsChainDb.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Oracle.DataAccess.Client;
    using SupermarketsChainDB.Data;
    using SupermarketsChainDB.Models;

    public static class OracleToSqlDb
    {
        private static string ConnectionString = Connection.GetOracleConnectionString();
        private static OracleConnection con;

        public static void MigrateToSql()
        {
            MigrateMeasures();
            MigrateVendors();
            MigrateProducts();
        }

        private static void Connect()
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

        private static void MigrateMeasures()
        {
            SupermarketSystemData data = new SupermarketSystemData();
            con = new OracleConnection { ConnectionString = ConnectionString };
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT M_ID, MEASURE_NAME FROM MEASURE_UNITS";
            
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    data.Measures.Add(new Measure 
                    {
                        MeasureName = (string)reader["MEASURE_NAME"]
                    });
                    
                }
                data.SaveChanges();
                reader.Dispose();
            }
            Close();
        }

        private static void MigrateVendors()
        {
            SupermarketSystemData data = new SupermarketSystemData();
            con = new OracleConnection { ConnectionString = ConnectionString };
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT V_ID,VENDOR_NAME FROM VENDORS";

            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    data.Vendors.Add(new Vendor 
                    {
                        VendorName = (string)reader["VENDOR_NAME"]
                    });
                    
                }
                data.SaveChanges();
                reader.Dispose();
            }
            Close();
        }


        private static void MigrateProducts()
        {
            SupermarketSystemData data = new SupermarketSystemData();
            con = new OracleConnection { ConnectionString = ConnectionString };
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT P_ID, " +
                                    "VENDOR_ID," + 
                                    "PRODUCT_NAME, " +
                                    "MEASURE_ID, " +
                                    "PRICE, " +
                                    "PRODUCT_TYPE " +  
                              "FROM PRODUCTS";

            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // for debugging
                    var vendorId = reader["VENDOR_ID"];
                    string productName = (string)reader["PRODUCT_NAME"];
                    int measureId = int.Parse((string)reader["MEASURE_ID"]);
                    decimal price = decimal.Parse((string)reader["PRICE"]);

                    data.Products.Add(new Product 
                    {
                        VendorId = (int)vendorId,
                        ProductName = productName,
                        MeasureId = measureId,
                        Price = price
                        //ProductType = (string)reader["PRODUCT_TYPE"]
                    });
                    
                }
                data.SaveChanges();
                reader.Dispose();
            }
            Close();
        }

        private static void Close()
        {
            con.Close();
            con.Dispose();
        } 
    }
}