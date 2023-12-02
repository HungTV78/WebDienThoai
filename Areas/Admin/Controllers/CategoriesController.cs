using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirst.Models;
using CodeFirst.Filters;

namespace CodeFirst.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class CategoriesController : Controller
    {
        // GET: Admin/Categories
        public ActionResult Index()
        {
            MyDBContext db = new MyDBContext();
            List<Category> categories = db.Categories.ToList();
            return View(categories);
        }
    }
}