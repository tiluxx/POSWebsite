using POSWebsite.Models;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf;
using System;
using System.Data;
using Syncfusion.Drawing;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FastMember;

namespace POSWebsite.Utils
{
    public class PDFGenerator
    {
        public ActionResult CreatePDF(Order order, List<OrderDetail> orderDetails)
        {
            using (PdfDocument document = new PdfDocument())
            {
                document.PageSettings.Orientation = PdfPageOrientation.Landscape;
                document.PageSettings.Margins.All = 50;
                PdfPage page = document.Pages.Add();
                PdfGraphics graphics = page.Graphics;

                RectangleF bounds = new RectangleF(176, 0, 390, 130);
                PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
                bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
                graphics.DrawRectangle(solidBrush, bounds);
                PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
                PdfTextElement element = new PdfTextElement("Order Report", subHeadingFont)
                {
                    Brush = PdfBrushes.White
                };

                PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));
                string currentDate = "Export Date: " + DateTime.Now.ToString("MMMM dd yyyy, hh:mm: tt", CultureInfo.InvariantCulture);

                SizeF textSize = subHeadingFont.MeasureString(currentDate);
                PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
                graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);
                PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.Helvetica, 10f);
                element = new PdfTextElement($"Order ID: {order.Id}", timesRoman)
                {
                    Brush = new PdfSolidBrush(new PdfColor(126, 155, 203))
                };
                result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
                element = new PdfTextElement($"Order Date: {order.CreationDate.ToString("MMMM dd yyyy, hh:mm: tt", CultureInfo.InvariantCulture)}", timesRoman)
                {
                    Brush = new PdfSolidBrush(new PdfColor(126, 155, 203))
                };
                result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
                element = new PdfTextElement($"Customer ID: {order.CustomerId}", timesRoman)
                {
                    Brush = new PdfSolidBrush(new PdfColor(126, 155, 203))
                };
                result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));

                PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
                PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
                PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
                graphics.DrawLine(linePen, startPoint, endPoint);

                DataTable productDetails = new DataTable();
                using (var reader = ObjectReader.Create(orderDetails))
                {
                    productDetails.Load(reader);
                }
                PdfGrid grid = new PdfGrid
                {
                    DataSource = productDetails
                };
                PdfGridCellStyle cellStyle = new PdfGridCellStyle();
                cellStyle.Borders.All = PdfPens.White;
                PdfGridRow header = grid.Headers[0];
                PdfGridCellStyle headerStyle = new PdfGridCellStyle();
                headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
                headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
                headerStyle.TextBrush = PdfBrushes.White;
                headerStyle.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);

                for (int i = 0; i < header.Cells.Count; i++)
                {
                    if (i == 0 || i == 1)
                        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                    else
                        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                }

                header.ApplyStyle(headerStyle);
                cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
                cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 10f);
                cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));
                PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat
                {
                    Layout = PdfLayoutType.Paginate
                };
                PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

                MemoryStream ms = new MemoryStream();
                document.Save(ms);
                ms.Position = 0;

                FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");
                fileStreamResult.FileDownloadName = $"Order_Report_{order.Id}_{DateTime.Now.ToString("ddMMMMyyyyHHmmssfffff")}.pdf";
                document.Close(true);
                return fileStreamResult;
            }
        }
    }
}