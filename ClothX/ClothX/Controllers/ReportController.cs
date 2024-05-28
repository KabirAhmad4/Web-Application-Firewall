using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ClothX.DbModels;
using ClothX.CustomAttributes;
using ClothX.Services;
using ClothX.Utility;

namespace ClothX.Controllers
{
    public class ReportController : Controller
    {
        // Fields for PDF generation
        private int totalCols = 0;
        private Document document = new Document();
        private Font font;
        private Font boldFont = new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.BOLD);
        private Font textFont = new Font(Font.FontFamily.TIMES_ROMAN, 12);
        private PdfPTable table;
        private PdfPCell cell;
        private MemoryStream stream = new MemoryStream();

        // User and Role Managers for Identity
        UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;


        public ReportController(IServiceProvider service)
        {
            _userManager = service.GetService<UserManager<IdentityUser>>();
            _roleManager = service.GetService<RoleManager<IdentityRole>>();
        }

        // Helper method to create a header cell for a PDF table
        private PdfPCell GetHeaderCell(string txt)
        {
            cell = new PdfPCell(new Phrase(txt));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(128, 128, 128);
            return cell;
        }

        // Helper method to create a regular cell for a PDF table
        private PdfPCell GetTableCell(string txt)
        {
            cell = new PdfPCell(new Phrase(txt));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            return cell;
        }

        // Helper method to generate the title page of the PDF
        private Document TitlePage(ref Document document, string title, string header)
        {
            // Set page size, margins, author, date, title, header
            document.SetPageSize(PageSize.A4);
            document.SetMargins(30, 30, 30, 30);
            document.AddAuthor("Software Square Society Development team");
            document.AddCreationDate();
            document.AddTitle(title);
            document.AddHeader("Title", header);

            // Adding title page elements
            document.NewPage();
            Paragraph Title = new Paragraph(header, boldFont);
            Title.SpacingBefore = 50f;
            Title.SpacingAfter = 50f;
            Title.Font.Size = 28;
            Title.Alignment = Element.ALIGN_CENTER;
            document.Add(Title);



            return document;
        }

        // Helper method to generate a table of orders in the PDF
        private Document OrdersTable(Document document, string titleText, List<ClientOrder> orders)
        {
            try
            {
                // Create a new page for the table
                document.NewPage();

                // Title for the table
                Paragraph title = new Paragraph(titleText, boldFont);
                title.SpacingBefore = 20f;
                title.SpacingAfter = 20f;
                title.Font.Size = 20;
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Set up the table
                totalCols = 5;
                table = new PdfPTable(totalCols);
                table.WidthPercentage = 100;

                // Add header cells to the table
                table.AddCell(GetHeaderCell("User"));
                table.AddCell(GetHeaderCell("Order"));
                table.AddCell(GetHeaderCell("Order Type"));
                table.AddCell(GetHeaderCell("Price"));
                table.AddCell(GetHeaderCell("Status"));

                // Add data cells to the table
                foreach (var o in orders)
                {
                    table.AddCell(GetTableCell(o.Client.FirstName + " " + o.Client.LastName));
                    table.AddCell(GetTableCell(o.Title));
                    table.AddCell(GetTableCell(o.OrderTypeNavigation.Name));
                    table.AddCell(GetTableCell(o.Price.ToString()));

                    // Determine the order status and add corresponding cell
                    if (o.IsActive == false)
                    {
                        table.AddCell(GetTableCell("In-Active"));
                    }
                    else if (o.IsDelivered == true)
                    {
                        table.AddCell(GetTableCell("Delivered"));
                    }
                    else if (o.IsConfirmed == true)
                    {
                        table.AddCell(GetTableCell("Confirmed"));
                    }
                    else if (o.IsPaid == true)
                    {
                        table.AddCell(GetTableCell("Paid"));
                    }
                    else
                    {
                        table.AddCell(GetTableCell("Not Paid"));
                    }
                }

                // Add the table to the document
                document.Add(table);

            }
            catch (Exception ex)
            {
                // Handle any exceptions and set TempData for error message
                TempData["Message"] = "Error While Saving Changes";
                TempData["Class"] = "alert-danger";
            }

            return document;
        }

        // Action method to generate and return a PDF report for client orders
        [ClothXPermissionAuthorize("ClientOrders")]
        public IActionResult Order(bool Month = true)
        {
            try
            {
                // Initialize PDF writer and open the document
                PdfWriter.GetInstance(document, stream);
                document.Open();

                // Retrieve orders from the database based on the selected criteria
                List<ClientOrder> orders = OrderUtility.Instance.GetOrders(Month);

                // Generate the PDF content using helper methods
                document = OrdersTable(document, Month == true ? "Monthly Orders Report" : "Yearly Orders Report", orders);

                // Close the document and convert it to a byte array for file download
                document.Close();
                byte[] abytes = stream.ToArray();
                return File(abytes, "application/pdf");
            }
            catch (Exception ex)
            {
                // Log any exceptions and redirect to a server error page
                ErrorLogger.Instance.ErrorLoggingFunction(ex.Message, this.ToString());
                return RedirectToAction("ServerError", "Error");
            }
        }


    }
}
