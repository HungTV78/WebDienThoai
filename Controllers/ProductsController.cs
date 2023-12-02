using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirst.Models;

namespace CodeFirst.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        MyDBContext db = new MyDBContext();

        // GET: Home
        public ActionResult Index(string search = "", string SortClumn = "Id", string IconClass = "fa-sort-asc", int page = 1)
        {
            List<Product> products = db.Products.Where(row => row.Name.Contains(search)).ToList();
            ViewBag.Search = search;
            //Sort
            ViewBag.SortClumn = SortClumn;
            ViewBag.IconClass = IconClass;
            if (SortClumn == "Price")
            {
                if (IconClass == "fa-sort-asc")
                {
                    products = products.OrderBy(row => row.Price).ToList();
                }
                else
                {
                    products = products.OrderByDescending(row => row.Price).ToList();
                }
            }

            //Paging
            int NoOfRecordPerPage = 8;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(products.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            products = products.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            return View(products);
        }
        public ActionResult SearchBrand(int id)
        {
            List<Product> pro = db.Products.Where(row => row.Brand_Id == id).ToList();

            return View(pro);
        }

        public ActionResult Detail(int id)
        {
            // Truy xuất thông tin sản phẩm dựa trên ID được cung cấp
            Product product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound(); // Hoặc xử lý trường hợp không tìm thấy sản phẩm
            }

            return View(product);
        }
    }
}