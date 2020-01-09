
using Microsoft.AspNet.Identity;
using NewsEngine2._0.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace NewsEngine2._0.Controllers
{
    public class NewsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private int _perPage = 5;

        // GET: News

        public ActionResult Index()
        {
            var news = db.News.Include("User").Include("Category").OrderByDescending(x => x.CreateDate);
            if (TempData.ContainsKey("message"))

            {
                ViewBag.message = TempData["message"].ToString();
            }

            var totalItems = news.Count();

            var currectPage = Convert.ToInt32(Request.Params.Get("page"));

            var offset = 0;

            if (!currectPage.Equals(0))
            {
                offset = (currectPage - 1) * this._perPage;
            }

            var paginatedArticles = news.Skip(offset).Take(this._perPage);
            ViewBag.perPage = this._perPage;
            ViewBag.total = totalItems;

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.News = paginatedArticles;

            return View();
        }

        [Authorize(Roles = "Editor")]
        public ActionResult EditorIndex()
        {
            if (User.IsInRole("Editor"))
            {
                var news = db.News.Where(x => x.UserId == User.Identity.GetUserId());

                var totalItems = news.Count();

                var currectPage = Convert.ToInt32(Request.Params.Get("page"));

                var offset = 0;

                if (!currectPage.Equals(0))
                {
                    offset = (currectPage - 1) * this._perPage;
                }

                var paginatedArticles = news.Skip(offset).Take(this._perPage);
                ViewBag.perPage = this._perPage;
                ViewBag.total = totalItems;

                ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
                ViewBag.News = paginatedArticles;

                return View();
            }
            else
            {
                TempData["message"] = "Nu sunteti autorizat in acesta zona!";
                return RedirectToAction("Index");
            }
        }


        public ActionResult Show(int id)
        {
            News news = db.News.Find(id);
            news.Comments = GetAllComments(news.NewsId);
            var media = db.Media.Where(x => x.NewsId == id).ToArray();
            news.Medias = media;
            return View(news);
        }

        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult New()
        {
            News news = new News();

            news.CreateDate = DateTime.Now;
            news.UserId = User.Identity.GetUserId();
            news.Categories = GetAllCategories();

            return View(news);


        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult New(News news)
        {

            news.CreateDate = DateTime.Now ;
            news.UserId = User.Identity.GetUserId();
            news.Categories = GetAllCategories();
            try
            {

                if (ModelState.IsValid)
                {

                    db.News.Add(news);
                    db.SaveChanges();
                    TempData["message"] = "Articolul a fost adaugat";
                    return RedirectToAction("PhotoUpload");
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

        [Authorize(Roles ="Administrator,Editor")]
        public ActionResult PhotoUpload()
        {
            
            
            return View();
        }
        [HttpPost]
        public ActionResult PhotoUpload(HttpPostedFileBase file)
        {
            Media photo = new Media();
            News news = db.News.OrderByDescending(x => x.CreateDate).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/")
                                                          + file.FileName);
                    photo.Path = file.FileName;
                }
                photo.NewsId = news.NewsId;
                photo.MediaTypeId = 1;
                db.Media.Add(photo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(); 
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

        [NonAction]
        public IEnumerable<SelectListItem> GetAllComments(int newsId)
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();

            // Extragem toate categoriile din baza de date
            var comments = db.Comments.Where(x => x.NewsId == newsId);

            // iteram prin categorii
            foreach (var comment in comments)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = comment.CommentId.ToString(),
                    Text = comment.Content.ToString()
                });
            }

            // returnam lista de categorii
            return selectList;
        }


    }
}