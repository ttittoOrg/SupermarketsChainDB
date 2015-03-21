using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePDFSalesReports
{
    public class PdfReport
    {

        public DateTime Date { get; set; }
        public string Product { get; set; }

        public int Quantity { get; set; }

        public string Location { get; set; }

        public decimal Sum { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalSum { get; set; }

        // this is for testing purposes only TO BE DELETED!
        public override string ToString()
        {
            return string.Format("\nProduct:{0} \nQuantity:{1}\nUnit Price: {2}\nLocation:{3} \nSum:{4} \nTotal: {5} \nDate: {6}",
                this.Product,
                this.Quantity,
                  this.UnitPrice,
                this.Location,
                this.Sum,
                this.TotalSum,
                this.Date);
        }
    }
}
