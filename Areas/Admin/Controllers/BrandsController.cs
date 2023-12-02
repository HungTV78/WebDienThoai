using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirst.Models;
using CodeFirst.Filters;

namespace CodeFirst.Areas.Admin.Controllers
{
    public class BrandsController : Controller
    {
        [AdminAuthorization]
        // GET: Admin/Brands
        public ActionResult Index()
        {
            MyDBContext db = new MyDBContext();
            List<Brand> brands = db.Brands.ToList();
            return View(brands);
        }
    }
}