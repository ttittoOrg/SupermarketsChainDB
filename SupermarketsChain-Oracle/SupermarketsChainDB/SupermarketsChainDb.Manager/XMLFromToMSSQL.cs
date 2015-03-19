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

        private Vendor GetVendorByName(string vendorName)
        {
            Vendor vendor = this.data.Vendors.Search(v => v.VendorName == vendorName).FirstOrDefault();
            return vendor;
        }
    }
}
