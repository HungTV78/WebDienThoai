using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeFirst.Models;

namespace CodeFirst.ApiControllers
{
    public class ProductsController : ApiController
    {
        private MyDBContext db = new MyDBContext();
        public List<Product> Get()
        {
            List<Product> products = db.Products.ToList();
            return products;
        }       
    }
}
