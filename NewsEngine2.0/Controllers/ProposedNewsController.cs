using NewsEngine2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NewsEngine2._0.Controllers
{
    public class ProposedNewsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProposedNews
        public ActionResult Index()
        {
            if (User.IsInRole("Editor") || User.IsInRole("Admin"))
            {
                var propNews = db.News.Include("Category");
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.message = TempData["message"].ToString();
                }

                ViewBag.ProposedNews = propNews;
                return View();
            }

            else
            {
                var userPropNews = db.News.Include("Category").Where(x => x.UserId == User.Identity.GetUserId());
                ViewBag.ProposedNews = userPropNews;
                return View();
            }
        }

        public ActionResult New()
        {
            News propNews = new News();
            propNews.IsActive = true;
            propNews.Categories = GetAllCategories();
            return View(propNews);
        }

        [HttpPost]
        public ActionResult New(News news)
        {
            news.Categories = GetAllCategories();
            if (User.IsInRole("User"))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.News.Add(news);
                        db.SaveChanges();
                        TempData["message"] = "Stirea a fost propusa cu succes";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(news);
                    }
                }
                catch (Exception e)
                {
                    return View(news);
                }

            }
            else
            {
                TempData["message"] = "Stirea poate fi propusa doar de user";
                return RedirectToAction("Index");

            }
        }


      
       

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            News propNews = db.News.Find(id);
            db.News.Remove(propNews);
            TempData["message"] = "Propunerea a fost stearsa!";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();
            var categories = from cat in db.Categories
                             select cat;

            foreach (var category in categories)
            {

                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name.ToString()
                });
            }
            return selectList;
        }

    }
}
