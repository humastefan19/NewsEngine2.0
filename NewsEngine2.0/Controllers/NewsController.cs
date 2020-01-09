using Microsoft.AspNet.Identity;
using NewsEngine2._0.Dto.MediaDto;
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

        public ActionResult Index(string searchString)
        {
            //var news = db.News.Include("User").Include("Category").OrderByDescending(x => x.CreateDate);
            var news = from n in db.News
                       join u in db.Users on n.UserId equals u.Id
                       join c in db.Categories on n.CategoryId equals c.CategoryId
                       select n;

            if (!String.IsNullOrEmpty(searchString))
            {

                news = news.Where(s => s.Content.Contains(searchString) || s.Title.Contains(searchString)).OrderByDescending(x => x.CreateDate);
            }
        
            else
            {
                news = news.OrderByDescending(x => x.CreateDate);
            }

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            List<MediaDto> medias = new List<MediaDto>();

            foreach (News item in news)
            {
                medias.Add(new MediaDto
                {
                    News = item,
                    Medias = db.Media.Where(x => x.NewsId == item.NewsId).ToList()
                });
            }

            var totalItems = medias.Count();

            var currectPage = Convert.ToInt32(Request.Params.Get("page"));

            var offset = 0;


            if (!currectPage.Equals(0))
            {
                offset = (currectPage - 1) * this._perPage;
            }

            var paginatedArticles = medias.Skip(offset).Take(this._perPage);
            ViewBag.perPage = this._perPage;
            ViewBag.total = totalItems;

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.News = paginatedArticles;

            ViewBag.Categories = db.Categories;

            return View();
        }
        public ActionResult IndexProposed()
        {
            var news = db.News.Include("User").Include("Category").OrderByDescending(x => x.CreateDate);
          
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            List<MediaDto> medias = new List<MediaDto>();

            foreach (News item in news)
            {
                medias.Add(new MediaDto
                {
                    News = item,
                    Medias = db.Media.Where(x => x.NewsId == item.NewsId).ToList()
                });
            }

            var totalItems = medias.Count();

            var currectPage = Convert.ToInt32(Request.Params.Get("page"));

            var offset = 0;


            if (!currectPage.Equals(0))
            {
                offset = (currectPage - 1) * this._perPage;
            }

            var paginatedArticles = medias.Skip(offset).Take(this._perPage);
            ViewBag.perPage = this._perPage;
            ViewBag.total = totalItems;

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.News = paginatedArticles;

            ViewBag.Categories = db.Categories;

            return View();
        }
        public ActionResult IndexByCategory(int id)
        {
            var news = db.News.Include("User").Include("Category").Where(x => x.CategoryId == id).OrderByDescending(x => x.CreateDate);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            List<MediaDto> medias = new List<MediaDto>();

            foreach (News item in news)
            {
                medias.Add(new MediaDto
                {
                    News = item,
                    Medias = db.Media.Where(x => x.NewsId == item.NewsId).ToList()
                });
            }

            var totalItems = medias.Count();

            var currectPage = Convert.ToInt32(Request.Params.Get("page"));

            var offset = 0;


            if (!currectPage.Equals(0))
            {
                offset = (currectPage - 1) * this._perPage;
            }

            var paginatedArticles = medias.Skip(offset).Take(this._perPage);
            ViewBag.perPage = this._perPage;
            ViewBag.total = totalItems;

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.News = paginatedArticles;
            ViewBag.Categories = db.Categories;
            ViewBag.Id = id;
            return View();
        }

        public ActionResult AddToNews(int ?id)
        {
            News addNews = db.News.Find(id);
            addNews.IsProposed = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        

        public ActionResult IndexSorted(int id)
        {

            List<News> news = new List<News>();
            if (id == 1)
            {
                news = db.News.OrderBy(x => x.Title).ToList();
            }
            else if (id == 2)
            {
                news = db.News.OrderByDescending(x => x.CreateDate).ToList();
            }
            else if (id == 3)
            {
                news = db.News.OrderBy(x => x.CreateDate).ToList();
            }

            List<MediaDto> medias = new List<MediaDto>();

            foreach (News item in news)
            {
                medias.Add(new MediaDto
                {
                    News = item,
                    Medias = db.Media.Where(x => x.NewsId == item.NewsId).ToList()
                });
            }

            var totalItems = medias.Count();

            var currectPage = Convert.ToInt32(Request.Params.Get("page"));

            var offset = 0;


            if (!currectPage.Equals(0))
            {
                offset = (currectPage - 1) * this._perPage;
            }

            var paginatedArticles = medias.Skip(offset).Take(this._perPage);
            ViewBag.perPage = this._perPage;
            ViewBag.total = totalItems;

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.News = paginatedArticles;
            ViewBag.Categories = db.Categories;
            ViewBag.Id = id;

            return View();
        }

        [Authorize(Roles = "Editor")]
        public ActionResult EditorIndex()
        {
            if (User.IsInRole("Editor"))
            {
                var userId = User.Identity.GetUserId();
                var news = db.News.Include("User").Include("Category").Where(x => x.UserId == userId);
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.message = TempData["message"].ToString();
                }
                List<MediaDto> medias = new List<MediaDto>();

                foreach (News item in news)
                {
                    medias.Add(new MediaDto
                    {
                        News = item,
                        Medias = db.Media.Where(x => x.NewsId == item.NewsId).ToList()
                    });
                }

                var totalItems = medias.Count();

                var currectPage = Convert.ToInt32(Request.Params.Get("page"));

                var offset = 0;


                if (!currectPage.Equals(0))
                {
                    offset = (currectPage - 1) * this._perPage;
                }

                var paginatedArticles = medias.Skip(offset).Take(this._perPage);
                ViewBag.perPage = this._perPage;
                ViewBag.total = totalItems;

                ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
                ViewBag.News = paginatedArticles;
                ViewBag.Categories = db.Categories;
                return View();
            }
            else
            {
                TempData["message"] = "Nu sunteti autorizat in acesta zona!";
                return RedirectToAction("Index");
            }
        }


        public ActionResult Show(int? id)
        {
            News news = db.News.Find(id);
            news.Comments = GetAllComments(news.NewsId);
            var media = db.Media.Where(x => x.NewsId == id).ToArray();
            news.Medias = media;
            ViewBag.Comments = GetAllComments(news.NewsId);
            return View(news);
        }

        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult New()
        {
            News news = new News();

            news.CreateDate = DateTime.Today;
            news.UserId = User.Identity.GetUserId();
            news.Categories = GetAllCategories();

            return View(news);


        }
        public ActionResult NewProposed()
        {
            News propNews = new News();
            propNews.CreateDate = DateTime.Now;
            propNews.UserId = User.Identity.GetUserId();
            propNews.IsProposed = true;
            propNews.Categories = GetAllCategories();
            return View(propNews);
        }
        [HttpPost]
        public ActionResult NewProposed(News news)
        {
            Media img = new Media();
            news.CreateDate = DateTime.Now;
            news.UserId = User.Identity.GetUserId();
            news.IsProposed = true;
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

        [HttpPost]
        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult New(News news)
        {
            Media img = new Media();
            news.CreateDate = DateTime.Now;
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

        [Authorize(Roles = "Administrator,Editor")]
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

        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult Edit(int id)
        {
            News news = db.News.Find(id);
            ViewBag.news = news;
            news.Categories = GetAllCategories();
            if (news.UserId == User.Identity.GetUserId() || User.IsInRole("Adminisitrator"))
            {
                return View(news);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul de a edita articolul";
                return RedirectToAction("Index");
            }

        }

        [Authorize(Roles = "Administrator,Editor")]
        [HttpPut]
        public ActionResult Edit(News editedNews)
        {
            editedNews.Categories = GetAllCategories();
            try
            {
                if (ModelState.IsValid)
                {
                    News newsToEdit = db.News.Find(editedNews.NewsId);
                    if (newsToEdit.UserId == User.Identity.GetUserId() ||
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
            catch (Exception e)
            {
                return View(editedNews);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator,Editor")]
        public ActionResult Delete(int id)
        {
            News news = db.News.Find(id);
            if (news.UserId == User.Identity.GetUserId() ||
                User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                db.News.Remove(news);
                db.SaveChanges();
                return RedirectToAction("IndexProposed");
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
        public IEnumerable<Comment> GetAllComments(int newsId)
        {
            return db.Comments.Where(x => x.NewsId == newsId).ToList();
        }




    }
}