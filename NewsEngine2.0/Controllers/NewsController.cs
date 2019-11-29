
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
            ViewBag.News = db.News.Include("User").Include("Category");
            return View();
        }

        public ActionResult Show(int id)
        {
            News news = db.News.Find(id);
            return View(news);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(News news)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.News.Add(news);
                    db.SaveChanges();
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

        public ActionResult Edit(int id)
        {
            News news = db.News.Find(id);
            return View(news);
        }

        [HttpPut]
        public ActionResult Edit(News editedNews)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    News newsToEdit = db.News.Find(editedNews.NewsId);
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
                    return View(editedNews);
                }
            }
            catch(Exception e)
            {
                return View(editedNews);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
            db.SaveChanges();
            return RedirectToAction("Index");
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
    }
}