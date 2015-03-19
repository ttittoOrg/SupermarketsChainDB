namespace GeneratePDFSalesReports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using SupermarketsChainDB.Data;

    public class PdfReportHandler
    {
        private SupermarketSystemData data;

        private string reportsFilePath;

        public PdfReportHandler(SupermarketSystemData data, string path)
        {
            this.data = data;
            this.reportsFilePath = path;

            // this.reportsFilePath = reportsFilePath;
        }

        private enum TextAlign
        {
            Left = 0,

            Center = 1,

            Right = 2,
        };

        public void CreateReport(DateTime startDate, DateTime endDate)
        {
            string directoryCreating = this.reportsFilePath + " Between " + startDate.ToString("dd-MM-yyyy") + " " + endDate.ToString("dd-MM-yyyy") + ".pdf";


            if (File.Exists(directoryCreating))
            {
                File.Delete(directoryCreating);
            }

            var pdfReport = new Document();
            var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(
                pdfReport,
                new System.IO.FileStream(directoryCreating, System.IO.FileMode.Create));

            pdfReport.Open();
            using (pdfReport)
            {
                var reportTable = new PdfPTable(5);

                this.AddCellToTable(reportTable, "Aggregated Sales Report", TextAlign.Center, 5);
                var reportsOrderedByDate = this.GenerateReports(startDate, endDate);
                var currentDate = reportsOrderedByDate.First().Date;
                decimal currentSum = 0;
                decimal totalSum = 0;
                var headerColor = new BaseColor(217, 217, 217);
                this.AddHeader(reportTable, currentDate, headerColor);

                // aligment  0 = left, 1 = center, 2 = right
                foreach (var report in reportsOrderedByDate)
                {
                    if (currentDate != report.Date)
                    {

                        totalSum += report.Sum;
                        this.AddFooter(reportTable, "Total sum for " + currentDate.ToString("dd-MM-yyy"), currentSum);
                        currentSum = 0;
                        currentSum += report.Sum;
                        currentDate = report.Date;
                        this.AddHeader(reportTable, currentDate, headerColor);
                        this.AddCellToTable(reportTable, report.Product, 0);
                        this.AddCellToTable(reportTable, report.Quantity.ToString(), 0);
                        this.AddCellToTable(reportTable, report.UnitPrice.ToString(), 0);
                        this.AddCellToTable(reportTable, report.Location, 0);
                        this.AddCellToTable(reportTable, report.Sum.ToString(), 0);
                    }
                    else
                    {

                        this.AddCellToTable(reportTable, report.Product, 0);
                        this.AddCellToTable(reportTable, report.Quantity.ToString(), 0);
                        this.AddCellToTable(reportTable, report.UnitPrice.ToString(), 0);
                        this.AddCellToTable(reportTable, report.Location, 0);
                        this.AddCellToTable(reportTable, report.Sum.ToString(), 0);
                        totalSum += (decimal)report.Sum;
                        currentSum += (decimal)report.Sum;
                    }
                }

                this.AddFooter(reportTable, "Total sum for " + currentDate.ToString("dd-MM-yyy"), currentSum);
                Console.WriteLine(currentSum);
                this.AddFooter(reportTable, "Grand total:", totalSum);

                pdfReport.Add(reportTable);
                Console.WriteLine(reportsOrderedByDate.Count);

            }
        }

        private void AddFooter(PdfPTable reportTable, string cellInfo, decimal sum)
        {
            this.AddCellToTable(reportTable, cellInfo, TextAlign.Right, 4);
            this.AddCellToTable(reportTable, sum.ToString(), TextAlign.Right);
        }

        private void AddHeader(PdfPTable reportTable, DateTime currentDate, BaseColor headerColor)
        {
            this.AddCellToTable(
                table: reportTable,
                cellText: "Date: " + currentDate.ToString(),
                textAlingment: TextAlign.Left,
                colspan: 5,
                cellColor: headerColor);

            this.AddCellToTable(
                table: reportTable,
                cellText: "Product",
                textAlingment: TextAlign.Left,
                cellColor: headerColor);
            this.AddCellToTable(
                table: reportTable,
                cellText: "Quantity",
                textAlingment: TextAlign.Left,
                cellColor: headerColor);
            this.AddCellToTable(
                table: reportTable,
                cellText: "Unit Price",
                textAlingment: TextAlign.Left,
                cellColor: headerColor);
            this.AddCellToTable(
                table: reportTable,
                cellText: "Location ",
                textAlingment: TextAlign.Left,
                cellColor: headerColor);
            this.AddCellToTable(
                table: reportTable,
                cellText: "Sum",
                textAlingment: TextAlign.Left,
                cellColor: headerColor);
        }

        private void AddCellToTable(
            PdfPTable table,
            string cellText,
            TextAlign textAlingment,
            int colspan = 1,
            BaseColor cellColor = null)
        {
            var cell = new PdfPCell(new Phrase(cellText));
            cell.Colspan = colspan;
            cell.HorizontalAlignment = (int)textAlingment;
            if (cellColor != null)
            {
                cell.BackgroundColor = cellColor;
            }

            table.AddCell(cell);
        }

        // private void AddParagraph(Document doc, int alignment, iTextSharp.text.Font font, iTextSharp.text.IElement content)
        // {
        // Paragraph paragraph = new Paragraph();
        // paragraph.SetLeading(0f, 1.2f);
        // paragraph.Alignment = alignment;
        // paragraph.Font = font;
        // paragraph.Add(content);
        // doc.Add(paragraph);
        // }
        private List<PdfReport> GenerateReports(DateTime startDate, DateTime endDate)
        {
            var sales =
                this.data.Sales.All()
                    .Where(x => x.Date >= startDate && x.Date <= endDate)
                    .Include(s => s.Product)
                    .Include(s => s.Store)
                    .Select(
                        s =>
                        new
                            {
                                Product = s.Product.ProductName,
                                Location = s.Product.Vendor.VendorName,
                                Quantity = s.Quantity,
                                Sum = s.Sum,
                                UnitPrice = s.SinglePrice,
                                Date = s.Date
                            })
                    .OrderBy(x => x.Date)
                    .ThenBy(x => x.Sum)
                    .ToList();

            var salesReports = new List<PdfReport>();

            foreach (var sale in sales)
            {
                PdfReport currentReport;
                currentReport = new PdfReport()
                                    {
                                        Location = sale.Location,
                                        Product = sale.Product,
                                        Quantity = sale.Quantity,
                                        Sum = sale.Sum,
                                        UnitPrice = sale.UnitPrice,
                                        Date = sale.Date
                                    };

                salesReports.Add(currentReport);
            }

            return salesReports;
        }
    }
}