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
        private string zipFilePath;
        private Dictionary<string, List<string>> reportsPaths;
        private List<string> connectionStrings;
        private List<Report> reports;
        private const string UnpackDirectoryName = "Extracted Reports";
        private SupermarketSystemData data = new SupermarketSystemData();

        public SalesReportsMigrator(string zipFilePath)
        {
            this.ZipFilePath = zipFilePath;
            this.reportsPaths = new Dictionary<string, List<string>>();
            this.connectionStrings = new List<string>();
            this.reports = new List<Report>();
        }

        public string ZipFilePath
        {
            get
            {
                return this.zipFilePath;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Path can't be null");
                }

                this.zipFilePath = value;
            }
        }

        public void ExtractReports()
        {
            using (ZipFile zip1 = ZipFile.Read(this.ZipFilePath))
            {
                foreach (var item in zip1)
                {
                    item.Extract(UnpackDirectoryName, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }

        public void GetAllReports()
        {
            this.TraverseFolders(UnpackDirectoryName);
            this.GetConnectionStrings();

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
                        
                        Report newReport = new Report();
                        //Sale newSale = new Sale();

                        string[] conArgs = connectionString.Split('\\');

                        if (!string.IsNullOrEmpty(conArgs[1]))
                        {
                            DateTime date = this.ParseDate(conArgs[1]);
                            // Date of sale
                            newReport.Date = date;
                            //newSale.Date = date;
                        }

                        DataRow headerRow = dataTable.Rows[0];
                        string storeName = headerRow.ItemArray[0].ToString();
                        // Name of the store
                        /*Store currentStore = data.Stores.Search(s => s.Name == storeName).FirstOrDefault();
                        if (currentStore == null)
                        {
                            var newStore = new Store { Name = storeName };
                            data.Stores.Add(newStore);
                            data.SaveChanges();
                            // TODO: To optimized !!!
                            currentStore = data.Stores.Search(s => s.Name == storeName).FirstOrDefault();
                        }*/

                        newReport.Name = storeName;
                        //newSale.StoreID = currentStore.ID;

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
                                    newReport.ProductID.Add(product.ID);
                                    //newSale.ProductID = product.ID;
                                }
                                else
                                {
                                    throw new ArgumentNullException("Product", "Not existing product!");
                                }
                                    
                            }

                            string quantityValue = dataRow.ItemArray[1].ToString();
                            if (!string.IsNullOrEmpty(quantityValue))
                            {
                                if (char.IsDigit(quantityValue[0]))
                                {
                                    newReport.Quantity.Add(int.Parse(quantityValue));
                                    //newSale.Quantity = int.Parse(quantityValue);
                                }
                            }

                            string unitPriceValue = dataRow.ItemArray[2].ToString();
                            if (!string.IsNullOrEmpty(unitPriceValue))
                            {
                                if (char.IsDigit(unitPriceValue[0]))
                                {
                                    newReport.UnitPrice.Add(decimal.Parse(unitPriceValue));
                                    //newSale.SinglePrice = decimal.Parse(unitPriceValue);
                                }
                            }

                            string sumValue = dataRow.ItemArray[3].ToString();
                            if (!string.IsNullOrEmpty(sumValue))
                            {
                                if (char.IsDigit(sumValue[0]))
                                {
                                    newReport.Sum.Add(decimal.Parse(sumValue));
                                    //newSale.Sum = decimal.Parse(sumValue);
                                }
                            }
                        }

                        this.reports.Add(newReport);
                        //data.Sales.Add(newSale);
                    }

                    excelConnection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public void FillTable()
        {
            //var context = new SupermarketSystemDbContext();
            SupermarketSystemData data = new SupermarketSystemData();
            
            foreach (var report in this.reports)
            {
                var supermarket = data.Stores.Search(s => s.Name == report.Name).FirstOrDefault();

                if (supermarket == null)
                {
                    var newSupermarket = new Store()
                    {
                        Name = report.Name
                    };

                    data.Stores.Add(newSupermarket);
                    supermarket = newSupermarket;
                    data.SaveChanges();
                }

                var products = report.ProductID.Count;

                for (int i = 0; i < products; i++)
                {
                    var newSale = new Sale()
                    {
                        StoreID = supermarket.ID,
                        ProductID = report.ProductID[i],
                        Quantity = report.Quantity[i],
                        SinglePrice = report.UnitPrice[i],
                        Sum = report.Sum[i],
                        Date = report.Date
                    };

                    data.Sales.Add(newSale);
                }
            }

            data.SaveChanges();
        }

        public void DeleteReports()
        {
            try
            {
                Directory.Delete(UnpackDirectoryName, true);
            }
            catch (Exception)
            {
            }
        }

        private void TraverseFolders(string sDir)
        {
            try
            {
                if (sDir != UnpackDirectoryName)
                {
                    DirectoryInfo folderInfo = new DirectoryInfo(sDir);
                    string folderName = folderInfo.Name;

                    if (!this.reportsPaths.ContainsKey(folderName))
                    {
                        this.reportsPaths[folderName] = new List<string>();
                    }

                    foreach (var file in Directory.GetFiles(sDir, "*.xls"))
                    {
                        FileInfo info = new FileInfo(file);
                        this.reportsPaths[folderName].Add(info.Name);
                    }
                }

                foreach (var item in Directory.GetDirectories(sDir))
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

        public DateTime ParseDate(string date)
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