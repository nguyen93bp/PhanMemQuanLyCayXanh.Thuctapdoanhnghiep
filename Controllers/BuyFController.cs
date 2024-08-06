using iTextSharp.text;
using iTextSharp.text.pdf;
using QuanLyCayXanh.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCayXanh.Controllers
{
    public class BuyFController : Controller
    {
        QuanLyCayXanhEntities db = new QuanLyCayXanhEntities   ();

        public ActionResult Index()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(db.BuyFs.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.FertilizerID = new SelectList(db.Fertilizers.ToList().OrderBy(n => n.Name), "FertilizerID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            // kiểm tra ,thông báo lỗi
            if (string.IsNullOrWhiteSpace(f["FertilizerID"]))
            {
                ModelState.AddModelError("FertilizerID", "Không được để trống.");
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
                ViewBag.FertilizerID = new SelectList(db.Fertilizers.ToList().OrderBy(n => n.Name), "FertilizerID", "Name");
                return View();
            }

            BuyF buy = new BuyF(); // gán giá trị
            buy.FertilizerID = int.Parse(f["FertilizerID"]);
            buy.Provider = f["Provider"];
            buy.Quantity = int.Parse(f["Quantity"]);
            buy.Price = int.Parse(f["Price"]);

            /*int DateY;
            if (int.TryParse(f["Date"], out DateY))
            {
                buy.Date = new DateTime(DateY, 1, 1); // Lấy năm và đặt tháng và ngày là mặc định (1/1)
            }
            else
            {
                ModelState.AddModelError("Date", "Invalid date format.");
                ViewBag.FertilizerID = new SelectList(db.Fertilizers.ToList().OrderBy(n => n.Name), "FertilizerID", "Name");
                return View();
            }*/

            DateTime dateValue;
            if (DateTime.TryParseExact(f["Date"], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dateValue))
            {
                buy.Date = dateValue;
            }
            else
            {
                ModelState.AddModelError("Date", "Định dạng ngày không hợp lệ (định dạng phải là yyyy-MM-dd).");
                ViewBag.FertilizerID = new SelectList(db.Fertilizers.ToList().OrderBy(n => n.Name), "FertilizerID", "Name");
                return View();
            }
            /* if (fFileUpload != null)
             {
                 var sFileName = Path.GetFileName(fFileUpload.FileName);
                 var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                 if (!System.IO.File.Exists(path))
                 {
                     fFileUpload.SaveAs(path);
                 }
                 buy.Image = sFileName;
             }*/
            //   Thêm đối tượng buy vào cơ sở dữ liệu và lưu thay đổithêm buy vào csld thay dổi
            db.BuyFs.Add(buy);
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        public ActionResult Edit(int id)
        { // hiển thị from chỉnh sửa
            var buy = db.BuyFs.SingleOrDefault(n => n.BuyFID == id);
            if (buy == null)
            {
                return HttpNotFound();
            }

            ViewBag.FertilizerID = new SelectList(db.Fertilizers.ToList().OrderBy(n => n.Name), "FertilizerID", "Name", buy.FertilizerID);// sắp xếp 
            return View(buy);
        }

        [HttpPost] // xử lí from 
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BuyF buy, HttpPostedFileBase fFileUpload)
        {
            var existingBuy = db.BuyFs.SingleOrDefault(n => n.BuyFID == id);
            if (existingBuy == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid) // kiểm tra dữ liệu
            {
                try
                {
                    // Cập nhật các thuộc tính của existingBuy từ mô hình buy
                    existingBuy.FertilizerID = buy.FertilizerID;
                    existingBuy.Provider = buy.Provider;
                    existingBuy.Quantity = buy.Quantity;
                    existingBuy.Date = buy.Date;
                    existingBuy.Price = buy.Price;

                    // Cập nhật vào cơ sở dữ liệu
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                    ModelState.AddModelError("", "Có lỗi xảy ra khi lưu dữ liệu. Vui lòng thử lại.");
                }
            }

            // Nếu ModelState không hợp lệ, load lại dropdown và hiển thị lại form với thông báo lỗi
            ViewBag.FertilizerID = new SelectList(db.Fertilizers.ToList().OrderBy(n => n.Name), "FertilizerID", "Name", existingBuy.FertilizerID);
            return View(existingBuy);
        }


        // GET: Hiển thị chi tiết đối tượng BuyF

        public ActionResult Details(int id)
        {
            var buyf = db.BuyFs.SingleOrDefault(n => n.BuyFID == id);
            if (buyf == null)
            {
                return HttpNotFound();
            }
            return View(buyf);
        }
        // GET: Hiển thị form xóa đối tượng BuyF
        public ActionResult Delete(int id)
        {
            var buyf = db.BuyFs.SingleOrDefault(n => n.BuyFID == id);
            if (buyf == null)
            {
                return HttpNotFound();
            }
            return View(buyf);
        }
        // POST: Xác nhận xóa đối tượng BuyF
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var buyf = db.BuyFs.SingleOrDefault(n => n.BuyFID == id);
            if (buyf == null)
            {
                return HttpNotFound();
            }
            db.BuyFs.Remove(buyf); // Xóa đối tượng khỏi cơ sở dữ liệu
            db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            return RedirectToAction("Index"); // Chuyển hướng về trang Index
        }

        // GET: BuyF/ExportToPdf
        public ActionResult ExportToPdf()
        {
            var buyfs = db.BuyFs.ToList(); // đọc dữ liệu

            // Tính toán các số liệu thống kê
            int totalCount = buyfs.Count();
            int totalQuantity = buyfs.Sum(b => b.Quantity);
            decimal totalPrice = buyfs.Sum(b => b.Quantity * b.Price); // Tính tổng giá trị phân bón

            // Truyền dữ liệu thống kê đến view thông qua ViewBag
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalQuantity = totalQuantity;
            ViewBag.TotalPrice = totalPrice; // Truyền tổng giá trị đến view

            return View(buyfs);
        }



        // POST: BuyF/GeneratePdf
        [HttpPost]
        public ActionResult GeneratePdf()
        {
            var buyfs = db.BuyFs.ToList(); // đọc dữ liệu

            // tính toán dữ liệu 
            int totalCount = buyfs.Count();
            int totalQuantity = buyfs.Sum(b => b.Quantity);
            decimal totalPrice = buyfs.Sum(b => b.Quantity * b.Price);

            // Tạo MemoryStream để lưu trữ PDF
            MemoryStream workStream = new MemoryStream();
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // mở tài  liệu
            document.Open();

            // thiết lập múi giờ vn
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);

            // thêm tiêu đề 
            string title = "Báo Cáo Cây Xanh";
            string reportDate = vietnamTime.ToString("dd/MM/yyyy");

            // Đường dẫn tương đối đến font
            string fontPath = Server.MapPath("~/Fonts/times.ttf");

            // Tạo BaseFont từ font đã cài đặt
            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            // Create a Font using the created BaseFont
            Font font = new Font(baseFont, 12, Font.NORMAL);

            //  Thêm tiêu đề và ngày báo cáo vào tài liệu

            document.Add(new Paragraph(title, font));
            document.Add(new Paragraph("Ngày xuất báo cáo: " + reportDate, font));

            // Thêm các số liệu thống kê
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Tổng số lần mua: " + totalCount, font));
            document.Add(new Paragraph("Tổng số lượng phân bón: " + totalQuantity, font));
            document.Add(new Paragraph("Tổng giá trị phân bón: " + totalPrice.ToString("N0") + " ₫", font)); // Thay đổi định dạng số để thêm ký hiệu đồng

            document.Add(new Paragraph("\n"));

            // Tạo bảng với 5 cột  // Add table
            PdfPTable table = new PdfPTable(5); // 5 columns as per your requirement

            //// Thiết lập tỷ lệ chiều rộng của bảng
            table.WidthPercentage = 100;

            // Add tiêu đề cột
            table.AddCell(new PdfPCell(new Phrase("Tên Phân Bón", font)));
            table.AddCell(new PdfPCell(new Phrase("Số Lượng", font)));
            table.AddCell(new PdfPCell(new Phrase("Nhà Cung Cấp", font)));
            table.AddCell(new PdfPCell(new Phrase("Ngày mua", font)));
            table.AddCell(new PdfPCell(new Phrase("Giá Tiền", font)));

            // Thêm các hàng dữ liệu  
            foreach (var buyf in buyfs)
            {
                table.AddCell(new PdfPCell(new Phrase(buyf.Fertilizer.Name, font))); // Assuming you have a navigation property named Fertilizer
                table.AddCell(new PdfPCell(new Phrase(buyf.Quantity.ToString(), font)));
                table.AddCell(new PdfPCell(new Phrase(buyf.Provider, font)));
                table.AddCell(new PdfPCell(new Phrase(buyf.Date.ToString("dd-MM-yyyy"), font)));
                table.AddCell(new PdfPCell(new Phrase(buyf.Price.ToString("N0") + " ₫", font))); // Thay đổi định dạng số để thêm ký hiệu đồng
            }

            // Thêm bảng vào tài liệu
            document.Add(table);

            // Đóng tài liệu
            document.Close();

            // Chuyển đổi MemoryStream thành byte array để trả về file PDF
            byte[] byteInfo = workStream.ToArray();

            // Ghi byte array vào MemoryStream
            workStream.Write(byteInfo, 0, byteInfo.Length);

            // Thiết lập vị trí bắt đầu của MemoryStream
            workStream.Position = 0;

            // Trả về file PDF để tải về
            return File(workStream, "application/pdf", "BaoCaoMuaPhanBon.pdf");
        }


    }
}
