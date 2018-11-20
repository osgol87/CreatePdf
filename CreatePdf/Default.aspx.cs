using System;
using System.Web.UI;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CreatePdf
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.GeneratePDF(sender, e);
        }
        protected void GeneratePDF(object sender, System.EventArgs e)
        {
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                var rectangle = new Rectangle(PageSize.A4);
                rectangle.Rotation = 1;

                Document document = new Document(rectangle.Rotate(), 10, 10, 10, 20);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                PageEventHelper pageEventHelper = new PageEventHelper();
                writer.PageEvent = pageEventHelper;

                document.Open();

                PdfPTable tbl = new PdfPTable(4);
                tbl.WidthPercentage = 100f;
                tbl.HeaderRows = 2;
                
                PdfPCell header = new PdfPCell(new Phrase("Header", FontFactory.GetFont("Arial", 20, Font.BOLD, BaseColor.BLACK)));
                header.Colspan = 4;
                header.HorizontalAlignment = Element.ALIGN_CENTER;
                
                tbl.AddCell(header);
                tbl.AddCell("Camp 1");

                PdfPCell cell2 = new PdfPCell(new Phrase("Camp 2"));
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.BackgroundColor = BaseColor.BLUE;
                tbl.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase("Camp 3"));
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3.BackgroundColor = BaseColor.GREEN;
                tbl.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("Camp 4"));
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.BackgroundColor = new BaseColor(255, 99, 71);
                tbl.AddCell(cell4);

                PdfPCell bottom = new PdfPCell(new Phrase("Section 1"));
                bottom.VerticalAlignment = Element.ALIGN_MIDDLE;

                tbl.AddCell(bottom);

                PdfPTable nested = new PdfPTable(1);
                nested.AddCell("Row 1");
                nested.AddCell("Row 2");
                nested.AddCell("Row 3");

                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Padding = 0f;
                nesthousing.Colspan = 3;
                tbl.AddCell(nesthousing);

                for (int i = 0; i < 40; i++)
                {
                    tbl.AddCell("Section " + (i + 2));

                    tbl.AddCell("Value 1");
                    tbl.AddCell("Value 2");
                    tbl.AddCell("Value 3");
                }

                PdfPCell line = new PdfPCell(new Phrase("    "));
                line.Colspan = 4;
                line.Border = 0;
                tbl.AddCell(line);

                var uri = new Uri(@"C:\Work\Jules_Verne_autograph.jpg");

                iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(uri);

                tbl.AddCell(image1);
                tbl.AddCell(image1);
                tbl.AddCell(image1);
                tbl.AddCell(image1);

                tbl.AddCell("Julio Verne");
                tbl.AddCell("Julio Verne");
                tbl.AddCell("Julio Verne");
                tbl.AddCell("Julio Verne");

                document.Add(tbl);
                //-------------------------------------------------------------------
                document.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";

                string pdfName = "User";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + pdfName + ".pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();
            }
        }
    }
}