using NewsEngine2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;


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
                 ViewBag.ProposedNews = db.ProposedNews.Include("User").Include("Category").OrderBy(x => x.CreatedDate); ;
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.message = TempData["message"].ToString();
                }

                
                return View();
            }

            else
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var myUser = manager.FindById(User.Identity.GetUserId());
                var userPropNews = db.ProposedNews.Include("User").Include("Category").Where(x => x.UserId == myUser.Id );
                ViewBag.ProposedNews = userPropNews;
                return View();
            }
        }

        public ActionResult Show(int id)
        {
            ProposedNews news = db.ProposedNews.Find(id);
            return View(news);
        }
        public ActionResult New()
        {
            ProposedNews propNews = new ProposedNews();
            propNews.Categories = GetAllCategories();
            return View(propNews);
        }

        [HttpPost]
        public ActionResult New(ProposedNews news)
        {
            news.Categories = GetAllCategories();
            if (User.IsInRole("User"))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.ProposedNews.Add(news);
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


        public ActionResult Edit(int id)
        {
            ProposedNews propNews = db.ProposedNews.Find(id);
            propNews.Categories = GetAllCategories();
            return View(propNews);
        }

        [HttpPut]
        public ActionResult Edit(int id, ProposedNews requestNews)
        {
            requestNews.Categories = GetAllCategories();
            if (User.IsInRole("User"))
            {

                try
                {
                    if (ModelState.IsValid)
                    {
                        ProposedNews news = db.ProposedNews.Find(id);
                        if (TryUpdateModel(news))
                        {
                            news.Title = requestNews.Title;
                            news.Content = requestNews.Content;
                            news.CreatedDate = requestNews.CreatedDate;
                            news.CategoryId = requestNews.CategoryId;
                            db.SaveChanges();
                            TempData["message"] = "Articolul a fost modificat!";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(requestNews);
                    }

                }
                catch (Exception e)
                {
                    return View(requestNews);
                }
            }
            else
            {
                TempData["message"] = "Articolul poate fi editat doar de propriul user";
                return RedirectToAction("Index");
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            ProposedNews propNews = db.ProposedNews.Find(id);
            db.ProposedNews.Remove(propNews);
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