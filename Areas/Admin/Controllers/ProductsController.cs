using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirst.Models;
using CodeFirst.Filters;
using System.IO;

namespace CodeFirst.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class ProductsController : Controller
    {
        MyDBContext db = new MyDBContext();

        // GET: Admin/Products
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

        public ActionResult Create()
        {
            //ViewBag.Categories = db.Categories.ToList();
            //ViewBag.Brands = db.Brands.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product p, HttpPostedFileBase imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (MyDBContext db = new MyDBContext())
                    {
                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            if (imageFile.ContentLength > 2000000)
                            {
                                ModelState.AddModelError("", "Kích thước file không được lớn hơn 2MB.");
                                return View();
                            }

                            var allowExs = new[] { ".jpg", ".png" };
                            var fileEx = Path.GetExtension(imageFile.FileName).ToLower();

                            if (!allowExs.Contains(fileEx))
                            {
                                ModelState.AddModelError("", "Phần mở rộng file không hỗ trợ.");
                                return View();
                            }

                            p.Image = "";
                            db.Products.Add(p);
                            db.SaveChanges();

                            var originalFileName = Path.GetFileName(imageFile.FileName);
                            var directoryPath = Server.MapPath("~/Image");

                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            var path = Path.Combine(directoryPath, originalFileName);
                            imageFile.SaveAs(path);

                            p.Image = originalFileName;
                            db.SaveChanges();

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            p.Image = "";
                            db.Products.Add(p);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (DirectoryNotFoundException)
            {
                ModelState.AddModelError("", "Không tìm thấy thư mục để lưu trữ hình ảnh.");
            }
            catch (IOException)
            {
                ModelState.AddModelError("", "Lỗi khi lưu hình ảnh vào thư mục.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi khi lưu file: {ex.Message}");
            }

            return View();
        }


        public ActionResult Edit(int id)
        {
            Product product = db.Products.Where(row => row.Id == id).FirstOrDefault();
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product pro)
        {
            Product product = db.Products.Where(row => row.Id == pro.Id).FirstOrDefault();

            // Update
            product.Name = pro.Name;
            product.Price = pro.Price;
            product.Description = pro.Description;
            product.Image = pro.Image;
            product.Brand_Id = pro.Brand_Id;
            product.CategoryId = pro.CategoryId;

            db.SaveChanges(); // lưu vào csdl
            return RedirectToAction("Index"); // Dòng này ngay lập tức chuyển hướng người dùng
        }

        public ActionResult Delete(int id)
        {
            Product product = db.Products.Where(row => row.Id == id).FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(int id, Product p)
        {
            Product product = db.Products.Where(row => row.Id == id).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
