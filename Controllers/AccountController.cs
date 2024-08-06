using QuanLyCayXanh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCayXanh.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        QuanLyCayXanhEntities db = new QuanLyCayXanhEntities();
        // GET: Account/Login
        public ActionResult Login()
        { //   // Nếu đã có session "TaiKhoan", chuyển hướng người dùng đến trang chính
            if (Session["TaiKhoan"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {// đọc giá trị 
            var Hoten = f["Account"];
            var MatKhau = f["Password"];
            var dbUser = db.Users.SingleOrDefault(u => u.Account == Hoten && u.Password == MatKhau);// tìm trong csdl 
                
            if (dbUser!=null)
            {
                Session["TaiKhoan"] = dbUser;
                // Lưu thông tin người dùng vào session để giữ trạng thái đăng nhập
                // chuyển trang
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Trả về view đăng nhập với thông báo lỗi
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session["TaiKhoan"] = null;
       
            return RedirectToAction("Index", "Home"); 
        }

    }
}