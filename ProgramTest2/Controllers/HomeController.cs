using ProgramTest2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgramTest2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult MockCategories()
        {
            myDBEntities myDB = new myDBEntities();
            List<Category> categories = myDB.Category.ToList();
            List<String> listOfCategories = new List<string>{ "Food", "Beverages", "Other" };
            Category newCategory = new Category();
            if (categories == null) {
                try
                {
                    foreach (string category in listOfCategories)
                    {
                        newCategory.Name = category;
                        myDB.Category.Add(newCategory);
                    }
                    myDB.SaveChanges();
                }
                catch (Exception ex) {
                    throw ex;
                }
            }
            return RedirectToAction("Index", "Product");
        }  
    }
}