using ProgramTest2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgramTest2.Controllers
{
    public class ProductController : Controller
    {
        // GET: lista proizvoda
        public ActionResult Index()
        {
            myDBEntities myDB = new myDBEntities();
            List<Product> products = myDB.Product.Where(x=> x.IsActive == true).ToList();
            return View(products);
        }
        
        public ActionResult Details(int Id)
        {
            myDBEntities myDB = new myDBEntities();
            Product product = myDB.Product.Single(x => x.Id == Id);
            return View(product);
        }
        // dodavanje proizvoda
        [HttpGet]
        public ActionResult Create()
        {
            myDBEntities myDB = new myDBEntities();
            List<Category> categories = myDB.Category.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                myDBEntities myDB = new myDBEntities();
                List<Category> categories = myDB.Category.ToList();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");

                Product newProduct = new Product();
                newProduct.Name = product.Name.Trim();
                newProduct.Price = product.Price;
                newProduct.CategoryId = product.CategoryId;
                newProduct.IsActive = true;
                newProduct.Modified = DateTime.Now;
                newProduct.Created = DateTime.Now;

                myDB.Product.Add(newProduct);
                myDB.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage); 
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }


            return RedirectToAction("Index", "Product");
        }

        //uređivanje proizvoda
        public ActionResult Edit(int Id)
        {
            myDBEntities myDB = new myDBEntities();
            Product product = myDB.Product.Single(x => x.Id == Id);
            List<Category> categories = myDB.Category.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product editProduct)
        {
            myDBEntities myDB = new myDBEntities();
            Product product = myDB.Product.Single(x => x.Id == editProduct.Id);
            List<Category> categories = myDB.Category.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            try {
                product.Name = editProduct.Name.Trim();
                product.Price = editProduct.Price;
                product.Modified = DateTime.Now;
                product.Created = product.Created;
                product.IsActive = true;
                product.CategoryId = editProduct.CategoryId;
                myDB.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            return RedirectToAction("Index");
        }
        //brisanje proizvoda
        public ActionResult Delete(int Id)
        {
            myDBEntities myDB = new myDBEntities();
            Product product = myDB.Product.Single(x => x.Id == Id && x.IsActive == true);
            if (product != null) {
                product.IsActive = false;
                try
                {
                    myDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return RedirectToAction("Index", "Product");
        }
    }
}