namespace SupermarketsChainDb.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.Linq;
    using Ionic.Zip;
    using SupermarketsChainDB.Data;
    using SupermarketsChainDB.Models;

    public class SalesReportsMigrator
    {
        // http://dotnetzip.codeplex.com/wikipage?title=CS-Examples
        private const string UnpackDirectoryName = "Extracted Reports";
        private string zipFilePath;
        private Dictionary<string, List<string>> reportsPaths;
        private List<string> connectionStrings;     
        private SupermarketSystemData data = new SupermarketSystemData();

        public SalesReportsMigrator(string zipFilePath)
        {
            this.ZipFilePath = zipFilePath;
            this.reportsPaths = new Dictionary<string, List<string>>();
            this.connectionStrings = new List<string>();
        }

        public void MigrateSalesReport()
        {
            this.ExtractReports();
            this.PrepareExelData();
            this.FillReportsInDatabase();
            this.DeleteReports();
        }

        private void PrepareExelData()
        {
            this.TraverseFolders(UnpackDirectoryName);
            this.GetConnectionStrings();
        }

        private void FillReportsInDatabase()
        {
            foreach (var connectionString in this.connectionStrings)
            {
                OleDbConnection excelConnection = new OleDbConnection(connectionString);
                excelConnection.Open();
                try
                {
                    using (excelConnection)
                    {
                        OleDbDataAdapter dataAdapter = new OleDbDataAdapter("select * from [Sales$]", excelConnection);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        Sale newSale = new Sale();

                        string[] conArgs = connectionString.Split('\\');

                        if (!string.IsNullOrEmpty(conArgs[1]))
                        {
                            DateTime date = this.ParseDate(conArgs[1]);
                            // Date of sale
                            newSale.Date = date;
                        }

                        DataRow headerRow = dataTable.Rows[0];
                        // Name of the store
                        string storeName = headerRow.ItemArray[0].ToString();
                        Store currentStore = data.Stores.Search(s => s.Name == storeName).FirstOrDefault();

                        if (currentStore == null)
                        {
                            var newStore = new Store { Name = storeName };
                            data.Stores.Add(newStore);
                            data.SaveChanges();
                            // TODO: To optimized !!!
                            currentStore = data.Stores.Search(s => s.Name == storeName).FirstOrDefault();
                        }

                        newSale.StoreId = currentStore.Id;

                        int reportsLength = dataTable.Rows.Count - 1;
                        for (int row = 2; row < reportsLength; row++)
                        {
                            DataRow dataRow = dataTable.Rows[row];

                            // Product name
                            string productName = dataRow.ItemArray[0].ToString();
                            if (!string.IsNullOrEmpty(productName))
                            {
                                var product = data.Products.Search(p => p.ProductName == productName).FirstOrDefault();
                                if (product != null)
                                {
                                    newSale.ProductId = product.Id;
                                }
                                else
                                {
                                    throw new ArgumentNullException("Product", "Not existing product!");
                                }
                            }

                            // Add sale product quantity
                            string quantityValue = dataRow.ItemArray[1].ToString();
                            if (!string.IsNullOrEmpty(quantityValue) && char.IsDigit(quantityValue[0]))
                            {
                                newSale.Quantity = int.Parse(quantityValue);
                            }

                            // Add sale product single price
                            string unitPriceValue = dataRow.ItemArray[2].ToString();
                            if (!string.IsNullOrEmpty(unitPriceValue) && char.IsDigit(unitPriceValue[0]))
                            {
                                newSale.SinglePrice = decimal.Parse(unitPriceValue);
                            }

                            // Add sale sum
                            string sumValue = dataRow.ItemArray[3].ToString();
                            if (!string.IsNullOrEmpty(sumValue) && char.IsDigit(sumValue[0]))
                            {
                                newSale.Sum = decimal.Parse(sumValue);
                            }

                            data.Sales.Add(newSale);
                            data.SaveChanges();
                        }
                    }

                    excelConnection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Read excel exception! {0}", ex);
                }
            }               
        }

        private string ZipFilePath
        {
            get
            {
                return this.zipFilePath;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("ZipFilePath", "Path to zip file shouldn't be null!");
                }

                this.zipFilePath = value;
            }
        }

        private void ExtractReports()
        {
            using (ZipFile zipFile = ZipFile.Read(this.ZipFilePath))
            {
                foreach (var item in zipFile)
                {
                    item.Extract(UnpackDirectoryName, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }

        private void DeleteReports()
        {
            try
            {
                Directory.Delete(UnpackDirectoryName, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete Reports Exeption! {0}", ex);
            }
        }

        private void TraverseFolders(string directory)
        {
            try
            {
                if (directory != UnpackDirectoryName)
                {
                    DirectoryInfo folderInfo = new DirectoryInfo(directory);
                    string folderName = folderInfo.Name;

                    if (!this.reportsPaths.ContainsKey(folderName))
                    {
                        this.reportsPaths[folderName] = new List<string>();
                    }

                    foreach (var file in Directory.GetFiles(directory, "*.xls"))
                    {
                        FileInfo info = new FileInfo(file);
                        this.reportsPaths[folderName].Add(info.Name);
                    }
                }

                foreach (var item in Directory.GetDirectories(directory))
                {
                    TraverseFolders(item);
                }
            }
            catch (Exception excpt)
            {
                Console.WriteLine(excpt);
            }
        }

        private void GetConnectionStrings()
        {
            
            foreach (var keyValuePair in this.reportsPaths)
            {
                foreach (var value in keyValuePair.Value)
                {

                    string excelPath = UnpackDirectoryName + @"\" + keyValuePair.Key + @"\" + value;

                    var connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelPath + ";Extended Properties=Excel 8.0;";
                    this.connectionStrings.Add(connectionString);

                }
            }
        }

        private DateTime ParseDate(string date)
        {
            string[] args = date.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            int year = int.Parse(args[2]);
            int day = int.Parse(args[0]);
            int month = 0;

            switch (args[1].ToLower())
            {
                case "jan": month = 1; break;
                case "feb": month = 2; break;
                case "mar": month = 3; break;
                case "apr": month = 4; break;
                case "may": month = 5; break;
                case "jun": month = 6; break;
                case "jul": month = 7; break;
                case "aug": month = 8; break;
                case "sep": month = 9; break;
                case "oct": month = 10; break;
                case "nov": month = 11; break;
                case "dec": month = 12; break;
                default:
                    break;
            }

            DateTime parsedDate = new DateTime(year, month, day);

            return parsedDate;
        }
    }
}