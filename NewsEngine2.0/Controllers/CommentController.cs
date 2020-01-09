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


        public ActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);
            return View(comment);
        }

        [HttpPost]
        public ActionResult Edit(Comment editedComment)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    Comment commentToEdit = db.Comments.Find(editedComment.CommentId);
                    if (commentToEdit.UserId == User.Identity.GetUserId())
                    {
                        if (TryUpdateModel(commentToEdit))
                        {
                            commentToEdit.Description = editedComment.Description;

                            db.SaveChanges();
                        }
                        return Redirect("http://localhost:53164/News/Show/" + commentToEdit.NewsId.ToString());
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

        public ActionResult New(int? id)
        {
            Comment comment = new Comment();
            comment.NewsId = id.GetValueOrDefault();
            return PartialView("Comment", comment);
        }

        [HttpPost]
        public ActionResult New(Comment newComment)
        {
            newComment.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(newComment);
                    db.SaveChanges();
                    TempData["message"] = "Comentariul a fost adaugat";
                    return Redirect("http://localhost:53164/News/Show/" + newComment.NewsId.ToString());
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
        public ActionResult Delete(int ? id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment.UserId == User.Identity.GetUserId() ||
                User.IsInRole("Administrator") || User.IsInRole("Editor"))
            {
                var newsId = comment.NewsId.ToString();
                db.Comments.Remove(comment);
                db.SaveChanges();
                return Redirect("http://localhost:53164/News/Show/" + newsId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un comentariu care nu va apartine!";
                return RedirectToAction("Show", "News", comment.NewsId);
            }
        }
    }
}