using Microsoft.AspNet.Identity;
using NewsEngine2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsEngine2._0.Controllers
{
    [Authorize(Roles = "User,Editor,Administrator")]
    public class CommentController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Comment
        public ActionResult Index(int newsId)
        {
            ViewBag.Comments = db.Comments.Where(x => x.NewsId == newsId);
            return View();
        }

        public ActionResult Edit(int commentId)
        {
            Comment comment = db.Comments.Find(commentId);
            return View(comment);
        }

        [HttpPost]
        public ActionResult Edit(Comment editedComment)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    Comment commentToEdit = db.Comments.Find(editedComment.NewsId);
                    if (commentToEdit.UserId == User.Identity.GetUserId())
                    {
                        if (TryUpdateModel(commentToEdit))
                        {
                            commentToEdit.Content = editedComment.Content;

                            db.SaveChanges();
                        }
                        return RedirectToAction("Show","News",editedComment.NewsId);
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                        return RedirectToAction("Show", "News", editedComment.NewsId);
                    }

                }
                else
                {
                    return View(editedComment);
                }
            }
            catch (Exception e)
            {
                return View(editedComment);
            }
        }

        public ActionResult New(int newsId)
        {

            Comment comment = new Comment();

            comment.UserId = User.Identity.GetUserId();
            comment.NewsId = newsId;

            return View();
        }

        [HttpPost]
        public ActionResult New(Comment newComment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(newComment);
                    db.SaveChanges();
                    TempData["message"] = "Comentariul a fost adaugat";
                    return RedirectToAction("Show","News",newComment.NewsId);
                }
                else
                {
                    return View(newComment);
                }
            }
            catch (Exception e)
            {
                return View(newComment);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int commentId)
        {
            Comment comment = db.Comments.Find(commentId);
            if (comment.UserId == User.Identity.GetUserId() ||
                User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un comentariu care nu va apartine!";
                return RedirectToAction("Show","News", comment.NewsId);
            }
        }
    }
}