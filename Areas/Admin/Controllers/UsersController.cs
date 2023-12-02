using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirst.Identity;
using CodeFirst.Filters;
using System.Net;
using System.Data.Entity;


namespace CodeFirst.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class UsersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Admin/Users
        public ActionResult Index()
        {
            List<AppUser> users = db.Users.ToList();
            return View(users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AppUser user)
        {
            if (ModelState.IsValid)
            {
                // Xử lý logic để thêm user vào cơ sở dữ liệu
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AppUser user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(AppUser updatedUser)
        {
            if (ModelState.IsValid)
            {
                AppUser user = db.Users.Find(updatedUser.Id);

                if (user == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật thông tin của người dùng
                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;
                user.PhoneNumber = updatedUser.PhoneNumber;
                user.City = updatedUser.City;
                user.Birthday = updatedUser.Birthday;

                db.SaveChanges(); // Lưu vào cơ sở dữ liệu
                return RedirectToAction("Index"); // Chuyển hướng người dùng sau khi cập nhật
            }

            return View(updatedUser);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AppUser user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AppUser user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
