
using Microsoft.AspNet.Identity;
using NewsEngine2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsEngine2._0.Controllers
{
    public class NewsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        

        // GET: News
        
        public ActionResult Index()
        {
            ViewBag.News = db.News.Include("User").Include("Category").OrderBy(x => x.CreateDate);
            if(TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            
            return View();
        }

 
        public ActionResult Show(int id)
        {
            News news = db.News.Find(id);
            return View(news);
        }

        [Authorize (Roles = "Administrator,Editor")]
        public ActionResult New()
        {
            News news = new News();

            news.CreateDate = DateTime.Today;
            news.UserId = User.Identity.GetUserId();
            news.Categories = GetAllCategories();

            return View(news);


        }

        [HttpPost]
        [Authorize (Roles ="Administrator,Editor")]
        public ActionResult New(News news)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.News.Add(news);
                    db.SaveChanges();
                    TempData["message"] = "Articolul a fost adaugat";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(news);
                }
            }
            catch(Exception e)
            {
                return View(news);
            }
        }

        [Authorize (Roles ="Administrator,Editor")]
        public ActionResult Edit(int id)
        {
            News news = db.News.Find(id);
            ViewBag.news = news;
            news.Categories = GetAllCategories();
            if(news.UserId == User.Identity.GetUserId() || User.IsInRole("Adminisitrator"))
            {
                return View(news);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul de a edita articolul";
                return RedirectToAction("Index");
            }
            
        }

        [Authorize (Roles ="Administrator,Editor")]
        [HttpPut]
        public ActionResult Edit(News editedNews)
        {
            editedNews.Categories = GetAllCategories();
            try
            {
                if (ModelState.IsValid)
                {
                    News newsToEdit = db.News.Find(editedNews.NewsId);
                    if(newsToEdit.UserId == User.Identity.GetUserId() ||
                        User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(newsToEdit))
                        {
                            newsToEdit.Title = editedNews.Title;
                            newsToEdit.Content = editedNews.Content;
                            newsToEdit.CategoryId = editedNews.CategoryId;

                            db.SaveChanges();
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    return View(editedNews);
                }
            }
            catch(Exception e)
            {
                return View(editedNews);
            }
        }

        [HttpDelete]
        [Authorize (Roles = "Administrator,Editor")]
        public ActionResult Delete(int id)
        {
            News news = db.News.Find(id);
            if(news.UserId == User.Identity.GetUserId() ||
                User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                db.News.Remove(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un articol care nu va apartine!";
                return RedirectToAction("Index");
            }

        }

        [NonAction]
        public List<SelectListItem> GetSelectNews()
        {
            var selectList = new List<SelectListItem>();
            var news = from ne in db.News select ne;

            foreach (News article in news)
            {
                selectList.Add(new SelectListItem
                {
                    Value = article.NewsId.ToString(),
                    Text = article.Title.ToString()
                });
            }
            return selectList;
        }
        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name.ToString()
                });
            }

            // returnam lista de categorii
            return selectList;
        }
    }
}