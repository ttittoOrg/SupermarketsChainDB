namespace SupermarketsChainDb.Manager
{
    using SupermarketsChainDB.Data;
    using SupermarketsChainDB.Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;

    public class XMLFromToMSSQL
    {
        private string filePath;
        private ISupermarketSystemData data;
        private XmlDocument xmlDoc;

        public XMLFromToMSSQL(ISupermarketSystemData data, string filePath)
        {
            this.filePath = filePath;
            this.data = data;
        }

        public int SaveExpenses()
        {
            this.xmlDoc = new XmlDocument();
            this.xmlDoc.Load(this.filePath);
            XmlNodeList vendors = xmlDoc.SelectNodes("expenses-by-month/vendor");

            foreach (XmlNode vendor in vendors)
            {
                Vendor vendorEntity = null;
                if (vendor.Attributes["name"] != null)
                {
                    string vendorName = vendor.Attributes["name"].Value;
                    vendorEntity = this.GetVendorByName(vendorName);
                    if (null != vendorEntity)
                    {
                        int vendorId = vendorEntity.Id;
                    }
                    else
                    {
                        throw new ArgumentNullException("Vendor", string.Format("Vendor with name {0} is not found in the database", vendorName));
                    }
                }

                XmlNodeList expenses = vendor.SelectNodes("expenses");
                foreach (XmlNode expense in expenses)
                {
                    string dateStr = expense.Attributes["month"].Value;
                    DateTime date = DateTime.ParseExact(dateStr, "MMM-yyyy", new CultureInfo("en-US"));

                    string totalStr = expense.InnerText;
                    decimal total;
                    if (totalStr != string.Empty)
                    {
                        total = decimal.Parse(totalStr);
                    }
                    else
                    {
                        throw new ArgumentException("The required content for total should not be empty string.");
                    }


                    Expense expenseEntity = new Expense();
                    expenseEntity.VendorId = vendorEntity.Id;
                    expenseEntity.Date = date;
                    expenseEntity.Total = total;

                    this.data.Expenses.Add(expenseEntity);
                }
            }

            return data.SaveChanges();
        }

        public void GenerateSalesByVendorReport(string reportFile, DateTime startDate, DateTime endDate)
        {
            var salesByVendor = from s in this.data.Sales.All()
                                join p in this.data.Products.All() on s.ProductId equals p.Id
                                join v in this.data.Vendors.All() on p.VendorId equals v.Id
                                where s.Date >= startDate && s.Date < endDate
                                group s by new { v.VendorName, s.Date } into gr
                                let SumOfTotals = gr.Sum(s => s.Sum)
                                orderby gr.Key.VendorName
                                select new { gr.Key.VendorName, gr.Key.Date, Total = SumOfTotals };



            XDocument salesReportDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement salesReportXML = new XElement("sales");
            XElement saleEl = null;
            string previousVendor = string.Empty;
            foreach (var pair in salesByVendor)
            {
                XElement summaryEl = null;
                if (previousVendor == pair.VendorName)
                {
                    summaryEl = new XElement("summary",
                       new XAttribute("date", pair.Date.ToString("dd-MMM-yyyy")),
                       new XAttribute("total-sum", pair.Total));
                }
                else
                {
                    saleEl = new XElement("sale", new XAttribute("vendor", pair.VendorName));
                    summaryEl = new XElement("summary",
                       new XAttribute("date", pair.Date.ToString("dd-MMM-yyyy")),
                       new XAttribute("total-sum", pair.Total));
                    salesReportXML.Add(saleEl);
                }

                saleEl.Add(summaryEl);

                previousVendor = pair.VendorName;
            }
            salesReportDoc.Add(salesReportXML);
            salesReportDoc.Save(reportFile);
        }

        private Vendor GetVendorByName(string vendorName)
        {
            Vendor vendor = this.data.Vendors.Search(v => v.VendorName == vendorName).FirstOrDefault();
            return vendor;
        }
    }
}
