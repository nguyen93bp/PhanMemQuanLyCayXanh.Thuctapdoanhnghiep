using iTextSharp.text.pdf;
using iTextSharp.text;
using QuanLyCayXanh.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace QuanLyCayXanh.Controllers
{
    public class FertilizerController : Controller
    {
        // GET: Fertilizer
        QuanLyCayXanhEntities db = new QuanLyCayXanhEntities();

        public ActionResult Index()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(db.Fertilizers.ToList());
        }

        /* public ActionResult Create()
         {
             return View();
         }

         [HttpPost]
         public ActionResult Create(FormCollection f, HttpPostedFileBase fFileUpload)
         {
             Fertilizer fer = new Fertilizer();

             fer.Name = f["Name"];
             fer.Type = f["Type"];
             fer.Quantity = int.Parse(f["Quantity"]);
             fer.Location = f["Location"];
             fer.Nametree = f["Nametree"];
             fer.Water = f["Water"];
             fer.Fertilization = f["Fertilization"];
             fer.Applyingfertilizer = f["Applyingfertilizer"];
             *//*if (fFileUpload != null)
             {
                 var sFileName = Path.GetFileName(fFileUpload.FileName);
                 var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                 if (!System.IO.File.Exists(path))
                 {
                     fFileUpload.SaveAs(path);

                 }
                 fer.Image = sFileName;
             }*//*
             db.Fertilizer.Add(fer);
             db.SaveChanges();
             return RedirectToAction("Index");
         }*/
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            Fertilizer fer = new Fertilizer();

            // Kiểm tra trường Name
            if (string.IsNullOrEmpty(f["Name"]))
            {
                ModelState.AddModelError("Name", "Tên không được để trống.");
            }
            else
            {
                fer.Name = f["Name"];
            }

            // Kiểm tra trường Type
            if (string.IsNullOrEmpty(f["Type"]))
            {
                ModelState.AddModelError("Type", "Loại không được để trống.");
            }
            else
            {
                fer.Type = f["Type"];
            }

            // Kiểm tra trường Quantity
            int quantity;
            if (string.IsNullOrEmpty(f["Quantity"]) || !int.TryParse(f["Quantity"], out quantity))
            {
                ModelState.AddModelError("Quantity", "Số lượng không hợp lệ. Vui lòng nhập số nguyên.");
            }
            else
            {
                fer.Quantity = quantity;
            }

            // Kiểm tra trường Location
            if (string.IsNullOrEmpty(f["Location"]))
            {
                ModelState.AddModelError("Location", "Vị trí không được để trống.");
            }
            else
            {
                fer.Location = f["Location"];
            }

            // Kiểm tra trường Nametree
            if (string.IsNullOrEmpty(f["Nametree"]))
            {
                ModelState.AddModelError("Nametree", "Tên cây không được để trống.");
            }
            else
            {
                fer.Nametree = f["Nametree"];
            }

            // Kiểm tra trường Water
            if (string.IsNullOrEmpty(f["Water"]))
            {
                ModelState.AddModelError("Water", "Nước không được để trống.");
            }
            else
            {
                fer.Water = f["Water"];
            }

            // Kiểm tra trường Fertilization
            if (string.IsNullOrEmpty(f["Fertilization"]))
            {
                ModelState.AddModelError("Fertilization", "Phân bón không được để trống.");
            }
            else
            {
                fer.Fertilization = f["Fertilization"];
            }

            // Kiểm tra trường Applyingfertilizer
            if (string.IsNullOrEmpty(f["Applyingfertilizer"]))
            {
                ModelState.AddModelError("Applyingfertilizer", "Cách bón phân không được để trống.");
            }
            else
            {
                fer.Applyingfertilizer = f["Applyingfertilizer"];
            }

            // Kiểm tra và lưu ảnh
            /*if (fFileUpload != null && fFileUpload.ContentLength > 0)
            {
                var sFileName = Path.GetFileName(fFileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                if (!System.IO.File.Exists(path))
                {
                    fFileUpload.SaveAs(path);
                    fer.Image = sFileName;
                }
            }*/

            // Nếu ModelState hợp lệ, lưu thông tin vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                db.Fertilizers.Add(fer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Trả về view cùng với các thông báo lỗi
            return View(fer);
        }


        public ActionResult Edit(int id)
        {
            var fertilizer = db.Fertilizers.SingleOrDefault(n => n.FertilizerID == id);
            if (fertilizer == null)
            {
                return HttpNotFound();
            }
            return View(fertilizer);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var fertilizer = db.Fertilizers.SingleOrDefault(n => n.FertilizerID == id);
            if (fertilizer == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra trường Name
            if (string.IsNullOrEmpty(f["Name"]))
            {
                ModelState.AddModelError("Name", "Tên không được để trống.");
            }
            else
            {
                fertilizer.Name = f["Name"];
            }

            // Kiểm tra trường Type
            if (string.IsNullOrEmpty(f["Type"]))
            {
                ModelState.AddModelError("Type", "Loại không được để trống.");
            }
            else
            {
                fertilizer.Type = f["Type"];
            }

            // Kiểm tra trường Quantity
            int quantity;
            if (string.IsNullOrEmpty(f["Quantity"]) || !int.TryParse(f["Quantity"], out quantity))
            {
                ModelState.AddModelError("Quantity", "Số lượng không hợp lệ. Vui lòng nhập số nguyên.");
            }
            else
            {
                fertilizer.Quantity = quantity;
            }

            // Kiểm tra trường Location
            if (string.IsNullOrEmpty(f["Location"]))
            {
                ModelState.AddModelError("Location", "Vị trí không được để trống.");
            }
            else
            {
                fertilizer.Location = f["Location"];
            }

            // Kiểm tra trường Nametree
            if (string.IsNullOrEmpty(f["Nametree"]))
            {
                ModelState.AddModelError("Nametree", "Tên cây không được để trống.");
            }
            else
            {
                fertilizer.Nametree = f["Nametree"];
            }

            // Kiểm tra trường Water
            if (string.IsNullOrEmpty(f["Water"]))
            {
                ModelState.AddModelError("Water", "Nước không được để trống.");
            }
            else
            {
                fertilizer.Water = f["Water"];
            }

            // Kiểm tra trường Fertilization
            if (string.IsNullOrEmpty(f["Fertilization"]))
            {
                ModelState.AddModelError("Fertilization", "Phân bón không được để trống.");
            }
            else
            {
                fertilizer.Fertilization = f["Fertilization"];
            }

            // Kiểm tra trường Applyingfertilizer
            if (string.IsNullOrEmpty(f["Applyingfertilizer"]))
            {
                ModelState.AddModelError("Applyingfertilizer", "Cách bón phân không được để trống.");
            }
            else
            {
                fertilizer.Applyingfertilizer = f["Applyingfertilizer"];
            }

           

            // Nếu ModelState hợp lệ, lưu thông tin vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Trả về view cùng với các thông báo lỗi
            return View(fertilizer);
        }


        public ActionResult Details(int id)
        {
            var fertilizerID = db.Fertilizers.SingleOrDefault(n => n.FertilizerID == id);
            if (fertilizerID == null)
            {
                return HttpNotFound();
            }
            return View(fertilizerID);
        }

        public ActionResult Delete(int id)
        {
            var fertilizerID = db.Fertilizers.SingleOrDefault(n => n.FertilizerID == id);
            if (fertilizerID == null)
            {
                return HttpNotFound();
            }
            return View(fertilizerID);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var fertilizerID = db.Fertilizers.SingleOrDefault(n => n.FertilizerID == id);
            if (fertilizerID == null)
            {
                return HttpNotFound();
            }
            db.Fertilizers.Remove(fertilizerID);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //adaasdadadasd
        public ActionResult ExportToPdf()
        {
            var fertilizers = db.Fertilizers.ToList();

            // Calculate statistics
            int totalCount = fertilizers.Count();
            int totalTypesCount = fertilizers.Select(f => f.Type).Distinct().Count();
            int totalQuantity = fertilizers.Sum(f => (f.Quantity));

            // Pass data to view
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalTypesCount = totalTypesCount;
            ViewBag.TotalQuantity = totalQuantity;

            return View(fertilizers);
        }

        [HttpPost]
        public ActionResult GeneratePdf()
        {
            var fertilizers = db.Fertilizers.ToList();

            // Calculate statistics
            int totalCount = fertilizers.Count();
            int totalTypesCount = fertilizers.Select(f => f.Type).Distinct().Count();
            int totalQuantity = fertilizers.Sum(f => f.Quantity);

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

            // Create a Font using the created BaseFont
            Font font = new Font(baseFont, 12, Font.NORMAL);

            // Add title using the specified font
            document.Add(new Paragraph(title, font));

            // Add report date
            document.Add(new Paragraph("Ngày xuất báo cáo: " + reportDate, font));

            // Add statistics
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Tổng số lượng phân bón: " + totalQuantity, font));
            document.Add(new Paragraph("Tổng số loại phân bón: " + totalTypesCount, font));
            document.Add(new Paragraph("Tổng số phân bón: " + totalCount, font));
            document.Add(new Paragraph("\n"));

            // Add table
            PdfPTable table = new PdfPTable(8); // 9 columns as per your requirement

            // Set width percentage of the table
            table.WidthPercentage = 100;

            // Add headers
/*            table.AddCell(new PdfPCell(new Phrase("ID", font)));*/            
            table.AddCell(new PdfPCell(new Phrase("Tên phân bón", font)));
            table.AddCell(new PdfPCell(new Phrase("Loại phân bón", font)));
            table.AddCell(new PdfPCell(new Phrase("Số Lượng", font)));
            table.AddCell(new PdfPCell(new Phrase("Vị Trí", font)));
            table.AddCell(new PdfPCell(new Phrase("Tên Cây", font)));
            table.AddCell(new PdfPCell(new Phrase("Số lần tưới nước", font)));
            table.AddCell(new PdfPCell(new Phrase("Số phần bón phân định kì", font)));
            table.AddCell(new PdfPCell(new Phrase("Số phần bỏ phân", font)));
            // table.AddCell(new PdfPCell(new Phrase("Hình Ảnh", font))); // Add image column header

            // Add data rows
            foreach (var fertilizer in fertilizers)
            {
/*                table.AddCell(new PdfPCell(new Phrase(fertilizer.FertilizerID.ToString(), font)));*/   
                table.AddCell(new PdfPCell(new Phrase(fertilizer.Name, font)));
                table.AddCell(new PdfPCell(new Phrase(fertilizer.Type, font)));
                table.AddCell(new PdfPCell(new Phrase(fertilizer.Quantity.ToString(), font)));
                table.AddCell(new PdfPCell(new Phrase(fertilizer.Location, font)));
                table.AddCell(new PdfPCell(new Phrase(fertilizer.Nametree, font)));
                table.AddCell(new PdfPCell(new Phrase(fertilizer.Water, font)));
                table.AddCell(new PdfPCell(new Phrase(fertilizer.Fertilization, font)));
                table.AddCell(new PdfPCell(new Phrase(fertilizer.Applyingfertilizer, font)));

                // Add image cell
                /* if (!string.IsNullOrEmpty(fertilizer.Image))
                 {
                     string imagePath = Server.MapPath("~/Images/" + fertilizer.Image); // Path to image
                     iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                     img.ScaleAbsolute(50f, 50f); // Scale image as per your requirement
                     PdfPCell imageCell = new PdfPCell(img);
                     table.AddCell(imageCell);
                 }
                 else
                 {
                     table.AddCell(""); // Add empty cell if no image
                 }*/
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
            return File(workStream, "application/pdf", "BaoCaoPhanBon.pdf");
        }

    }
}
