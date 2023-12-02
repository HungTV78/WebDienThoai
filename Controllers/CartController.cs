using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirst.Models;
using CodeFirst.Filters;

namespace CodeFirst.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        [MyAuthenFilter]
        public ActionResult Index()
        {
            MyDBContext db = new MyDBContext();
            List<Cart> carts = db.Carts.ToList();
            return View(carts);
        }

        public ActionResult Add(int id = 0)
        {
            if (id > 0)
            {
                MyDBContext db = new MyDBContext();
                Cart cartItem = db.Carts.Where(c => c.Id == id).FirstOrDefault();
                if (cartItem != null)
                {
                    cartItem.Quantity += 1;
                }
                else
                {
                    Cart cart = new Cart();
                    cart.Id = id;
                    cart.Quantity = 1;
                    db.Carts.Add(cart);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult UpdateQuantity(int quan = 0, int proid = 0)
        {
            MyDBContext db = new MyDBContext();
            if (quan > 0)
            {
                Cart cartItem = db.Carts.Where(c => c.Id == proid).FirstOrDefault();
                if (cartItem != null)
                {
                    cartItem.Quantity = quan;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int proid = 0)
        {
            MyDBContext db = new MyDBContext();
            Cart cartItem = db.Carts.Where(c => c.Id == proid).FirstOrDefault();

            if (cartItem != null)
            {
                db.Carts.Remove(cartItem);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}