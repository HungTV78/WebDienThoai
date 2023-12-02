using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirst.Models;

namespace CodeFirst.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        MyDBContext db = new MyDBContext();

        // GET: Admin/Home
        public ActionResult Index(int page = 1)
        {
            int NoOfRecordPerPage = 20; // Đặt số lượng sản phẩm mỗi trang là 20

            int totalProducts = db.Products.Count();
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalProducts) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;

            var paginatedProducts = db.Products.OrderBy(p => p.Id).Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;

            return View(paginatedProducts);
        }
    }
}
