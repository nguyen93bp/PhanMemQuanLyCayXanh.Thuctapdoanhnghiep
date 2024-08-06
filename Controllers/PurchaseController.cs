using iTextSharp.text.pdf;
using iTextSharp.text;
using QuanLyCayXanh.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCayXanh.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: Purchase
        QuanLyCayXanhEntities db = new QuanLyCayXanhEntities();

        public ActionResult Index()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(db.Buys.ToList());
        }

        

        public ActionResult Create()
        {
            ViewBag.GreeneryID = new SelectList(db.Greeneries.ToList().OrderBy(n => n.Name), "GreeneryID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            if (string.IsNullOrWhiteSpace(f["GreeneryID"]))
            {
                ModelState.AddModelError("GreeneryID", "Không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(f["Provider"]))
            {
                ModelState.AddModelError("Provider", "Không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(f["Quantity"]))
            {
                ModelState.AddModelError("Quantity", "Không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(f["Price"]))
            {
                ModelState.AddModelError("Price", "Không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(f["Date"]))
            {
                ModelState.AddModelError("Date", "Không được để trống.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.GreeneryID = new SelectList(db.Greeneries.ToList().OrderBy(n => n.Name), "GreeneryID", "Name");
                return View();
            }

            Buy buy = new Buy();
            buy.GreeneryID = int.Parse(f["GreeneryID"]);
            buy.Provider = f["Provider"];
            buy.Quantity = int.Parse(f["Quantity"]);
            buy.Price = int.Parse(f["Price"]);

            DateTime dateValue;
            if (DateTime.TryParseExact(f["Date"], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dateValue))
            {
                buy.Date = dateValue;
            }
            else
            {
                ModelState.AddModelError("Date", "Định dạng ngày không hợp lệ (định dạng phải là yyyy-MM-dd).");
                ViewBag.GreeneryID = new SelectList(db.Greeneries.ToList().OrderBy(n => n.Name), "GreeneryID", "Name");
                return View();
            }

            if (fFileUpload != null)
            {
                var sFileName = Path.GetFileName(fFileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                if (!System.IO.File.Exists(path))
                {
                    fFileUpload.SaveAs(path);
                }
                buy.Image = sFileName;
            }

            db.Buys.Add(buy);
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        public ActionResult Edit(int id)
        {
            var buy = db.Buys.SingleOrDefault(n => n.BuyID == id);
            if (buy == null)
            {
                return HttpNotFound();
            }

            ViewBag.GreeneryID = new SelectList(db.Greeneries.ToList().OrderBy(n => n.Name), "GreeneryID", "Name", buy.GreeneryID);
            return View(buy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Buy model, HttpPostedFileBase fFileUpload)
        {
            // Kiểm tra lỗi xác thực của các trường không được để trống
            if (model.GreeneryID == 0)
            {
                ModelState.AddModelError("GreeneryID", "Tên cây xanh không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(model.Provider))
            {
                ModelState.AddModelError("Provider", "Nhà cung cấp không được để trống.");
            }
            if (model.Quantity <= 0)
            {
                ModelState.AddModelError("Quantity", "Số lượng phải lớn hơn 0.");
            }
            if (model.Price <= 0)
            {
                ModelState.AddModelError("Price", "Giá tiền phải lớn hơn 0.");
            }
            if (model.Date == DateTime.MinValue)
            {
                ModelState.AddModelError("Date", "Ngày mua không được để trống.");
            }

            // Nếu có lỗi, trở về view với thông báo lỗi
            if (!ModelState.IsValid)
            {
                ViewBag.GreeneryID = new SelectList(db.Greeneries.ToList().OrderBy(n => n.Name), "GreeneryID", "Name", model.GreeneryID);
                return View(model);
            }

            var buy = db.Buys.SingleOrDefault(n => n.BuyID == model.BuyID);
            if (buy == null)
            {
                return HttpNotFound();
            }

            // Cập nhật thông tin từ form
            buy.GreeneryID = model.GreeneryID;
            buy.Provider = model.Provider;
            buy.Quantity = model.Quantity;
            buy.Date = model.Date;
            buy.Price = model.Price;

            // Kiểm tra và xử lý file upload
            if (fFileUpload != null && fFileUpload.ContentLength > 0)
            {
                var sFileName = Path.GetFileName(fFileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                if (!System.IO.File.Exists(path))
                {
                    fFileUpload.SaveAs(path);
                }
                buy.Image = sFileName;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }








        public ActionResult Details(int id)
        {
            var buy = db.Buys.SingleOrDefault(n => n.BuyID == id);
            if (buy == null)
            {
                return HttpNotFound();
            }
            return View(buy);
        }

        public ActionResult Delete(int id)
        {
            var buy = db.Buys.SingleOrDefault(n => n.BuyID == id);
            if (buy == null)
            {
                return HttpNotFound();
            }
            return View(buy);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var buy = db.Buys.SingleOrDefault(n => n.BuyID == id);
            if (buy == null)
            {
                return HttpNotFound();
            }
            db.Buys.Remove(buy);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public ActionResult ExportToPdf()
        {
            var purchases = db.Buys.ToList();

            // Calculate statistics
            int totalPurchases = purchases.Count();
            int totalProviders = purchases.Select(p => p.Provider).Distinct().Count();
            int totalTypes = purchases.Select(p => p.GreeneryID).Distinct().Count(); // Assume GreeneryID represents the type of greenery
            int totalQuantity = purchases.Sum(p => p.Quantity);
            int totalPrice = purchases.Sum(p => p.Price);

            // Pass data to view
            ViewBag.TotalCount = totalPurchases;
            ViewBag.TotalProviders = totalProviders;
            ViewBag.TotalTypes = totalTypes;
            ViewBag.TotalQuantity = totalQuantity;
            ViewBag.TotalPrice = totalPrice;

            return View(purchases);
        }


        // POST: Purchase/GeneratePdf
        [HttpPost]
        public ActionResult GeneratePdf()
        {
            var purchases = db.Buys.ToList();

            int totalTypes = purchases.Select(p => p.GreeneryID).Distinct().Count(); // Assume GreeneryID represents the type of greenery
            int totalQuantity = purchases.Sum(p => p.Quantity);
            decimal totalPrice = purchases.Sum(b => b.Price * b.Quantity);

            // MemoryStream to hold the generated PDF
            MemoryStream workStream = new MemoryStream();

            // Create a new PDF document
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);

            // Initialize PDF writer and bind it to the MemoryStream
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Open the document
            document.Open();

            // Set the time zone to Vietnam
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);

            // Add title
            string title = "Báo Cáo Cây Xanh";
            string reportDate = vietnamTime.ToString("dd/MM/yyyy");

            // Output the report date
            Console.WriteLine(reportDate);

            // Đường dẫn tương đối đến font
            string fontPath = Server.MapPath("~/Fonts/times.ttf");

            // Tạo BaseFont từ font đã cài đặt
            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            // Tạo một Font sử dụng BaseFont đã tạo
            Font font = new Font(baseFont, 12, Font.NORMAL);

            // Add title using the specified font
            document.Add(new Paragraph(title, font));

            // Add report date
            document.Add(new Paragraph("Ngày xuất báo cáo: " + reportDate, font));

            // Add statistics
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Tổng loại cây: " + totalTypes, font));
            document.Add(new Paragraph("Tổng số lượng: " + totalQuantity, font));
            document.Add(new Paragraph("Tổng giá trị phân bón: " + totalPrice.ToString("N0") + " ₫", font)); // Thay đổi định dạng giá

            document.Add(new Paragraph("\n"));

            // Add table
            PdfPTable table = new PdfPTable(6); // 7 columns as per your requirement

            // Set width percentage of the table
            table.WidthPercentage = 100;

            // Add headers
            table.AddCell(new PdfPCell(new Phrase("Tên Cây Xanh", font)));
            table.AddCell(new PdfPCell(new Phrase("Số lượng", font)));
            table.AddCell(new PdfPCell(new Phrase("Nhà cung cấp", font)));
            table.AddCell(new PdfPCell(new Phrase("Ngày mua", font)));
            table.AddCell(new PdfPCell(new Phrase("Giá", font)));
            table.AddCell(new PdfPCell(new Phrase("Ảnh", font)));

            // Add data rows
            foreach (var buy in purchases)
            {
                table.AddCell(new PdfPCell(new Phrase(buy.Greenery.Name, font)));
                table.AddCell(new PdfPCell(new Phrase(buy.Quantity.ToString(), font)));
                table.AddCell(new PdfPCell(new Phrase(buy.Provider, font)));
                
                table.AddCell(new PdfPCell(new Phrase(buy.Date.ToString("dd-MM-yyyy"), font)));
                table.AddCell(new PdfPCell(new Phrase(buy.Price.ToString("N0") + " ₫", font))); // Thay đổi định dạng giá

                // Xử lý hình ảnh nếu có
                if (!string.IsNullOrEmpty(buy.Image))
                {
                    string imagePath = Server.MapPath("~/Images/" + buy.Image);

                    // Kiểm tra xem tệp hình ảnh có tồn tại không
                    if (System.IO.File.Exists(imagePath))
                    {
                        // Lấy đường dẫn tới file ảnh và tạo đối tượng hình ảnh
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);

                        // Tạo ô bảng chứa hình ảnh
                        PdfPCell imgCell = new PdfPCell(img, true);
                        imgCell.HorizontalAlignment = Element.ALIGN_CENTER;

                        // Thêm ô chứa hình ảnh vào bảng
                        table.AddCell(imgCell);
                    }
                    else
                    {
                        // Nếu không có tệp hình ảnh, thêm một ô chứa dòng chữ "Ảnh không tìm thấy"
                        table.AddCell(new PdfPCell(new Phrase("Ảnh không tìm thấy", font)));
                    }
                }
                else
                {
                    // Nếu không có hình ảnh, thêm một ô chứa dòng chữ "Không có ảnh"
                    table.AddCell(new PdfPCell(new Phrase("Không có ảnh", font)));
                }

            }

            // Add the table to the document
            document.Add(table);

            // Close the document
            document.Close();

            // Convert the MemoryStream to byte array for File result
            byte[] byteInfo = workStream.ToArray();

            // Write the byte array to the MemoryStream
            workStream.Write(byteInfo, 0, byteInfo.Length);

            // Set the position to start of the stream
            workStream.Position = 0;

            // Return the File result for download
            return File(workStream, "application/pdf", "BaoCaoMuaCayXanh.pdf");
        }

    }
}
