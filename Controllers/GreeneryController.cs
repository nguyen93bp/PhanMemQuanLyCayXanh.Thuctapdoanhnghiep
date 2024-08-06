using iTextSharp.text.pdf;
using iTextSharp.text;
using QuanLyCayXanh.Models;
using System;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace QuanLyCayXanh.Controllers
{
    public class GreeneryController : Controller
    {
        // GET: Greenery
        QuanLyCayXanhEntities db = new QuanLyCayXanhEntities();

        public ActionResult Index()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(db.Greeneries.ToList());
        }




        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            Greenery greenery = new Greenery();

            // Kiểm tra trường Name
            if (string.IsNullOrEmpty(f["Name"]))
            {
                ModelState.AddModelError("Name", "Tên không được để trống.");
            }
            else
            {
                greenery.Name = f["Name"];
            }

            // Kiểm tra trường Createdate
            DateTime createdDate;
            string dateString = f["Createdate"];
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out createdDate))
            {
                greenery.Createdate = createdDate;
            }
            else
            {
                ModelState.AddModelError("Createdate", "Ngày tạo không hợp lệ. Vui lòng nhập đúng định dạng yyyy-MM-dd.");
            }

            // Kiểm tra trường Age
            int age;
            if (string.IsNullOrEmpty(f["Age"]) || !Int32.TryParse(f["Age"], out age))
            {
                ModelState.AddModelError("Age", "Tuổi không hợp lệ.");
            }
            else
            {
                greenery.Age = age;
            }

            // Kiểm tra trường Type
            if (string.IsNullOrEmpty(f["Type"]))
            {
                ModelState.AddModelError("Type", "Loại không được để trống.");
            }
            else
            {
                greenery.Type = f["Type"];
            }

            // Kiểm tra trường Health
            if (string.IsNullOrEmpty(f["Health"]))
            {
                ModelState.AddModelError("Health", "Tình trạng sức khỏe không được để trống.");
            }
            else
            {
                greenery.Health = f["Health"];
            }

            // Kiểm tra trường Location
            if (string.IsNullOrEmpty(f["Location"]))
            {
                ModelState.AddModelError("Location", "Vị trí không được để trống.");
            }
            else
            {
                greenery.Location = f["Location"];
            }

            // Kiểm tra trường Quantity
            int quantity;
            if (string.IsNullOrEmpty(f["Quantity"]) || !Int32.TryParse(f["Quantity"], out quantity))
            {
                ModelState.AddModelError("Quantity", "Số lượng không hợp lệ.");
            }
            else
            {
                greenery.Quantity = quantity;
            }

            // Kiểm tra trường FileUpload
            if (fFileUpload == null || fFileUpload.ContentLength == 0)
            {
                ModelState.AddModelError("Image", "Hình ảnh không được để trống.");
            }
            else
            {
                var sFileName = Path.GetFileName(fFileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                if (!System.IO.File.Exists(path))
                {
                    fFileUpload.SaveAs(path);
                    greenery.Image = sFileName;
                }
                else
                {
                    ModelState.AddModelError("Image", "Hình ảnh đã tồn tại.");
                }
            }

            if (ModelState.IsValid)
            {
                db.Greeneries.Add(greenery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(greenery);
        }



        public ActionResult Edit(int id)
        {
            var greenery = db.Greeneries.SingleOrDefault(n => n.GreeneryID == id);
            if (greenery == null)
            {
                return HttpNotFound();
            }
            return View(greenery);
        }

        [HttpPost]
        public ActionResult Edit(int id, Greenery model, HttpPostedFileBase fFileUpload)
        {
            var greenery = db.Greeneries.SingleOrDefault(n => n.GreeneryID == id);
            if (greenery == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra và cập nhật trường Name
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", "Tên không được để trống.");
            }
            else
            {
                greenery.Name = model.Name;
            }

            // Kiểm tra và cập nhật trường Createdate
            if (model.Createdate == default(DateTime) || model.Createdate > DateTime.Now)
            {
                ModelState.AddModelError("Createdate", "Ngày trồng không hợp lệ. Vui lòng nhập đúng định dạng.");
            }
            else
            {
                greenery.Createdate = model.Createdate;
            }

            // Kiểm tra và cập nhật trường Age
            if (model.Age <= 0)
            {
                ModelState.AddModelError("Age", "Tuổi không hợp lệ. Vui lòng nhập số nguyên lớn hơn 0.");
            }
            else
            {
                greenery.Age = model.Age;
            }

            // Kiểm tra và cập nhật trường Type
            if (string.IsNullOrEmpty(model.Type))
            {
                ModelState.AddModelError("Type", "Loại không được để trống.");
            }
            else
            {
                greenery.Type = model.Type;
            }

            // Kiểm tra và cập nhật trường Health
            if (string.IsNullOrEmpty(model.Health))
            {
                ModelState.AddModelError("Health", "Tình trạng sức khỏe không được để trống.");
            }
            else
            {
                greenery.Health = model.Health;
            }

            // Kiểm tra và cập nhật trường Location
            if (string.IsNullOrEmpty(model.Location))
            {
                ModelState.AddModelError("Location", "Vị trí không được để trống.");
            }
            else
            {
                greenery.Location = model.Location;
            }

            // Kiểm tra và cập nhật trường Quantity
            if (model.Quantity <= 0)
            {
                ModelState.AddModelError("Quantity", "Số lượng không hợp lệ. Vui lòng nhập số nguyên lớn hơn 0.");
            }
            else
            {
                greenery.Quantity = model.Quantity;
            }

            // Xử lý upload file
            if (fFileUpload != null && fFileUpload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fFileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                fFileUpload.SaveAs(path);
                greenery.Image = fileName;
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    // Xử lý hoặc ghi lại ngoại lệ nếu cần thiết
                    foreach (var error in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in error.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    return View(greenery); // Trả về view chỉnh sửa với các lỗi xác thực
                }
            }

            return View(greenery); // Trả về view chỉnh sửa với các lỗi xác thực
        }





        public ActionResult Details(int id)
        {
            var greeneryID = db.Greeneries.SingleOrDefault(n => n.GreeneryID == id);
            if (greeneryID == null)
            {
                return HttpNotFound();
            }
            return View(greeneryID);
        }

        public ActionResult Delete(int id)
        {
            var greeneryID = db.Greeneries.SingleOrDefault(n => n.GreeneryID == id);
            if (greeneryID == null)
            {
                return HttpNotFound();
            }
            return View(greeneryID);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var greeneryID = db.Greeneries.SingleOrDefault(n => n.GreeneryID == id);
            if (greeneryID == null)
            {
                return HttpNotFound();
            }
            db.Greeneries.Remove(greeneryID);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Greenery/ExportToPdf
        public ActionResult ExportToPdf()
        {
            var greeneries = db.Greeneries.ToList();

            // Calculate statistics
            int totalCount = greeneries.Count();
            int totalTypesCount = greeneries.Select(g => g.Type).Distinct().Count();
            int totalQuantity = greeneries.Sum(g => g.Quantity ?? 0); // Calculate total quantity of trees

            // Pass data to view
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalTypesCount = totalTypesCount;
            ViewBag.TotalQuantity = totalQuantity; // Pass total quantity to the view

            return View(greeneries);
        }

        [HttpPost]
        public ActionResult GeneratePdf()
        {
            var greeneries = db.Greeneries.ToList();

            // Tính toán thống kê
            int totalTrees = greeneries.Count();
            int totalTypes = greeneries.Select(g => g.Type).Distinct().Count();
            int totalQuantity = greeneries.Sum(g => g.Quantity ?? 0);

            using (var workStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(document, workStream).CloseStream = false;

                document.Open();

                var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);

                var title = "Báo Cáo Cây Xanh";
                var reportDate = vietnamTime.ToString("dd/MM/yyyy");

                string fontPath = Server.MapPath("~/Fonts/times.ttf");
                BaseFont baseFont;
                try
                {
                    baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Font not found: {ex.Message}");
                    baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                }

                var font = new Font(baseFont, 12, Font.NORMAL);

                document.Add(new Paragraph(title, font));
                document.Add(new Paragraph("Ngày xuất báo cáo: " + reportDate, font));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("Tổng số cây: " + totalTrees, font));
                document.Add(new Paragraph("Tổng số loại: " + totalTypes, font));
                document.Add(new Paragraph("Tổng số lượng cây trồng: " + totalQuantity, font));
                document.Add(new Paragraph("\n"));

                var table = new PdfPTable(8); // Cập nhật số cột thành 8
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2f, 2f, 1.5f, 2f, 2f, 2f, 2f, 1.5f });

                // Thêm tiêu đề cột
                table.AddCell(new PdfPCell(new Phrase("Tên cây", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Loại cây", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Tuổi", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Sức khỏe", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Vị trí", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Hình ảnh", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Ngày trồng", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Số lượng", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                

                foreach (var greenery in greeneries)
                {
                    table.AddCell(new PdfPCell(new Phrase(greenery.Name, font)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(greenery.Type, font)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(greenery.Age.ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(greenery.Health, font)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(greenery.Location, font)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    if (!string.IsNullOrEmpty(greenery.Image))
                    {
                        try
                        {
                            var imgPath = Server.MapPath("~/Images/" + greenery.Image);
                            var img = iTextSharp.text.Image.GetInstance(imgPath);
                            img.ScaleToFit(100f, 100f); // Điều chỉnh kích thước hình ảnh
                            var imgCell = new PdfPCell(img, true)
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                VerticalAlignment = Element.ALIGN_MIDDLE,
                                Padding = 5
                            };
                            table.AddCell(imgCell);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Image not found: {ex.Message}");
                            table.AddCell(new PdfPCell(new Phrase("Không có ảnh", font)));
                        }
                    }
                    else
                    {
                        table.AddCell(new PdfPCell(new Phrase("Không có ảnh", font)));
                    }
                    table.AddCell(new PdfPCell(new Phrase(greenery.Createdate.ToString("dd/MM/yyyy"), font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(greenery.Quantity.ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                    
                }

                document.Add(table);
                document.Close();

                byte[] byteInfo = workStream.ToArray();
                workStream.Position = 0;

                return File(byteInfo, "application/pdf", "BaoCaoCayXanh.pdf");
            }
        }




    }
}
