using QuanLyCayXanh.Models;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyCayXanh.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles
        QuanLyCayXanhEntities db = new QuanLyCayXanhEntities();

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Roledetails.OrderBy(n => n.Name), "IDRoles", "Name");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(User user, FormCollection f)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Gán Role từ dropdownlist vào user.Role
                    user.Role = int.Parse(f["RoleID"]);

                    // Thêm user vào db và lưu thay đổi
                    db.Users.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating user: " + ex.Message);
            }

            // Nếu có lỗi, cần phải load lại dropdownlist RoleID với các giá trị đã chọn
            ViewBag.RoleID = new SelectList(db.Roledetails.OrderBy(n => n.Name), "IDRoles", "Name", user.Role);
            return View(user);
        }


        public ActionResult Edit(int id)
        {
            ViewBag.RoleID = new SelectList(db.Roledetails.ToList().OrderBy(n => n.Name), "IDRoles", "Name");
            var roles = db.Users.SingleOrDefault(n => n.IDuser == id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection f)
        {
            var Role = db.Users.SingleOrDefault(n => n.IDuser == id);
            if (Role == null)
            {
                return HttpNotFound();
            }
            Role.Role = int.Parse(f["RoleID"]);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /*  public ActionResult Details(int id)
          {
              var roles = db.User.SingleOrDefault(n => n.IDuser == id);
              if (roles == null)
              {
                  return HttpNotFound();
              }
              return View(roles);
          }*/
        public ActionResult Details(int id)
        {
            var user = db.Users.SingleOrDefault(n => n.IDuser == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult Delete(int id)
        {
            var roles = db.Users.SingleOrDefault(n => n.IDuser == id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var roles = db.Users.SingleOrDefault(n => n.IDuser == id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            db.Users.Remove(roles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }




    }
}
