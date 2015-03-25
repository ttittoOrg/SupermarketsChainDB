namespace SupermarketsChainDB.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MySql.Data.MySqlClient;

    public static class MsSqlToMySql
    {
        private static readonly string ConnectionString = Connection.GetMySqlConnectionString();

        private static MySqlConnection connection;

        public static void MigrateToMySql()
        {
            CreateDatabase();
            MigrateMeasures();
            MigrateVendors();
            MigrateProducts();
            MigrateExpenses();
            MigrateSales();
        }

        // just for testing purposes
        private static void Connect()
        {
            connection = new MySqlConnection { ConnectionString = ConnectionString };
            connection.Open();
            Console.WriteLine("Connected to MySql" + connection.ServerVersion);
            connection.Close();
        }

        private static void CreateDatabase()
        {
            const string InitialConnectionString = "Server=localhost;Port=3306;Uid=root;Pwd=;";

            using (connection = new MySqlConnection { ConnectionString = InitialConnectionString })
            {
                try
                {
                    connection.Open();
                    MySqlCommand createDatabase = connection.CreateCommand();
                    createDatabase.CommandText = "CREATE DATABASE IF NOT EXISTS SuperMarketChain;";
                    createDatabase.ExecuteNonQuery();
                    connection.Close();

                    Console.WriteLine("Database created successfully (if not already created)");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("The database couldn't be created!");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void MigrateMeasures()
        {
            var data = new SupermarketSystemData();
            var measures = data.Measures.All().ToList();

            using (connection = new MySqlConnection { ConnectionString = ConnectionString })
            {
                try
                {
                    connection.Open();
                    MySqlCommand createTable = connection.CreateCommand();
                    createTable.CommandText =
                        "CREATE TABLE IF NOT EXISTS `Measures` ( " +
                            "Id INT NOT NULL AUTO_INCREMENT, " +
                            "Measure_Name nvarchar(50) NOT NULL, " +
                            "CONSTRAINT pk_MeasuresId PRIMARY KEY (Id) " +
                        ");";
                    createTable.ExecuteNonQuery();

                    MySqlCommand insertCmd = connection.CreateCommand();

                    foreach (var measure in measures)
                    {
                        // Using input data check
                        //insertCmd.CommandText =
                        //    "INSERT INTO Measures(Id, Measure_Name) " +
                        //    "SELECT * FROM(SELECT" +
                        //                        "(" +
                        //                        "'@Id', " +
                        //                        "'@measureName'" +
                        //                        ") " + 
                        //                   "AS tmp ";
                        //insertCmd.CommandText += "WHERE NOT EXISTS(" +
                        //        "SELECT Id, Measure_Name FROM Measures " +
                        //        "WHERE Id = @Id" +
                        //        " AND " +
                        //        "Measure_Name = '@measureName'" +
                        //        ") LIMIT 1;";
                        //insertCmd.Parameters.AddWithValue("@Id", measure.Id);
                        //insertCmd.Parameters.AddWithValue("@measureName", measure.MeasureName);
                        //insertCmd.CommandTimeout = 15;
                        //insertCmd.CommandType = CommandType.Text;
                        //insertCmd.ExecuteNonQuery();


                        insertCmd.CommandText =
                        "INSERT INTO Measures(Id, Measure_Name) " +
                        "VALUES (" + measure.Id + ", " +
                                 "'" + measure.MeasureName + "'" +
                                 ")";
                        insertCmd.ExecuteNonQuery();
                    }

                    Console.WriteLine("Successfully migrated Measures table from MS SQL to MySQL");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Something went wrong during the Measures table migration!");
                    Console.WriteLine(ex.Message);
                }
            }

            connection.Close();
        }

        private static void MigrateVendors()
        {
            var data = new SupermarketSystemData();
            var vendors = data.Vendors.All().ToList();

            using (connection = new MySqlConnection { ConnectionString = ConnectionString })
            {
                try
                {
                    connection.Open();
                    MySqlCommand createTable = connection.CreateCommand();
                    createTable.CommandText =
                        "CREATE TABLE IF NOT EXISTS `Vendors` ( " +
                            "Id INT NOT NULL AUTO_INCREMENT, " +
                            "Vendor_Name nvarchar(50) NOT NULL, " +
                            "CONSTRAINT pk_VendorsId PRIMARY KEY (Id) " +
                        ");";
                    createTable.ExecuteNonQuery();

                    MySqlCommand insertCmd = connection.CreateCommand();

                    foreach (var vendor in vendors)
                    {
                        insertCmd.CommandText =
                        "INSERT INTO Vendors(Id, Vendor_Name) " +
                        "VALUES (" + vendor.Id + ", " + 
                                 "'" + vendor.VendorName + "'" +
                                 ")";
                        insertCmd.ExecuteNonQuery();
                    }

                    Console.WriteLine("Successfully migrated Vendors table from MS SQL to MySQL");

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Something went wrong during the Vendors table migration!");
                    Console.WriteLine(ex.Message);
                }
            }

            connection.Close();
        }

        private static void MigrateProducts()
        {
            var data = new SupermarketSystemData();
            var products = data.Products.All().ToList();

            using (connection = new MySqlConnection { ConnectionString = ConnectionString })
            {
                try
                {
                    connection.Open();
                    MySqlCommand createTable = connection.CreateCommand();
                    createTable.CommandText =
                        "CREATE TABLE IF NOT EXISTS `Products` ( " +
                            "Id INT NOT NULL AUTO_INCREMENT, " +
                            "Vendor_Id INT NOT NULL, " +
                            "Product_Name nvarchar(200) NOT NULL, " +
                            "Measure_Id INT NOT NULL, " +
                            "Price DECIMAL(19,4) NOT NULL, " +
                            "ProductType INT NOT NULL, " +
                            "CONSTRAINT pk_ProductsId PRIMARY KEY (Id), " +
                            "CONSTRAINT fk_VendorsProducts FOREIGN KEY (Vendor_Id) REFERENCES Vendors(Id), " +
                            "CONSTRAINT fk_MeasuresProducts FOREIGN KEY (Measure_Id) REFERENCES Measures(Id) " +
                        ");";
                    createTable.ExecuteNonQuery();

                    MySqlCommand insertCmd = connection.CreateCommand();

                    foreach (var product in products)
                    {
                        insertCmd.CommandText =
                        "INSERT INTO Products(Id, Vendor_Id, Product_Name, Measure_Id, Price, ProductType) " +
                        "VALUES ( " + 
                                 "'" + product.Id + "', " +
                                 "'" + product.VendorId + "', " +
                                 "'" + product.ProductName + "', " +
                                 "'" + product.MeasureId + "', " +
                                 "'" + product.Price + "', " +
                                 "'" + product.ProductType + "'" +
                                 ")";
                        insertCmd.ExecuteNonQuery();
                    }

                    Console.WriteLine("Successfully migrated Products table from MS SQL to MySQL");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Something went wrong during the Products table migration!");
                    Console.WriteLine(ex.Message);
                }
            }

            connection.Close();
        }

        private static void MigrateExpenses()
        {
            var data = new SupermarketSystemData();
            var expenses = data.Expenses.All().ToList();

            using (connection = new MySqlConnection { ConnectionString = ConnectionString })
            {
                try
                {
                    connection.Open();
                    MySqlCommand createTable = connection.CreateCommand();
                    createTable.CommandText =
                        "CREATE TABLE IF NOT EXISTS `Expenses` (" +
                            "Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY, " +
                            "VendorId INT NOT NULL, " +
                            "`Date` DATETIME NOT NULL, " +
                            "Total DECIMAL(18,2) NOT NULL" +
                        ");";
                    createTable.ExecuteNonQuery();

                    createTable.CommandText = "ALTER TABLE Expenses " +
                        "ADD CONSTRAINT fk_VendorsExpenses FOREIGN KEY (VendorId) REFERENCES Vendors(Id)";
                    createTable.ExecuteNonQuery();

                    MySqlCommand insertCmd = connection.CreateCommand();

                    foreach (var expense in expenses)
                    {
                        string formatForMySql = expense.Date.ToString("yyyy-MM-dd HH:mm:ss");

                        insertCmd.CommandText =
                        "INSERT INTO Expenses(Id, VendorId, Date, Total) " +
                        "VALUES (" + expense.Id + ", " +
                                     expense.VendorId + ", " +
                                     "'" + formatForMySql + "', " +
                                     expense.Total +
                                 ")";
                        insertCmd.ExecuteNonQuery();
                    }

                    Console.WriteLine("Successfully migrated Expenses table from MS SQL to MySQL");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Something went wrong during the Expenses table migration!");
                    Console.WriteLine(ex.Message);
                }
            }

            connection.Close();
        }

        private static void MigrateSales()
        {
            var data = new SupermarketSystemData();
            var sales = data.Sales.All().ToList();

            using (connection = new MySqlConnection { ConnectionString = ConnectionString })
            {
                try
                {
                    connection.Open();
                    MySqlCommand createTable = connection.CreateCommand();
                    createTable.CommandText =
                        "CREATE TABLE IF NOT EXISTS `Sales` (" +
                            "Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY, " +
                            "StoreId INT NOT NULL, " +
                            "ProductId INT NOT NULL, " +
                            "Quantity INT NOT NULL, " +
                            "SinglePrice DECIMAL(18,2) NOT NULL, " +
                            "Sum DECIMAL(18,2) NOT NULL, " +
                            "`Date` DATETIME NOT NULL" +
                        ");";
                    createTable.ExecuteNonQuery();

                    createTable.CommandText = "ALTER TABLE Sales " +
                        "ADD CONSTRAINT fk_ProductsSales FOREIGN KEY (ProductId) REFERENCES Products(Id)";
                    createTable.ExecuteNonQuery();

                    MySqlCommand insertCmd = connection.CreateCommand();

                    foreach (var sale in sales)
                    {
                        string dateFormatForMySql = sale.Date.ToString("yyyy-MM-dd HH:mm:ss");

                        insertCmd.CommandText =
                        "INSERT INTO Sales(Id, StoreId, ProductId, Quantity, SinglePrice, Sum, `Date`) " +
                        "VALUES (" + sale.Id + ", " +
                                     sale.StoreId + ", " +
                                     sale.ProductId + ", " +
                                     sale.Quantity + ", " +
                                     sale.SinglePrice + ", " +
                                     sale.Sum + ", " +
                                     "'" + dateFormatForMySql + "'" +
                                ")";
                        insertCmd.ExecuteNonQuery();
                    }

                    Console.WriteLine("Successfully migrated Sales table from MS SQL to MySQL");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Something went wrong during the Sales table migration!");
                    Console.WriteLine(ex.Message);
                }
            }

            connection.Close();
        }
    }
}