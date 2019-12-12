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
    [Authorize(Roles ="Administrator")]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: User
        public ActionResult Index()
        {
            ViewBag.Users = db.Users;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            return View();
        }

        public ActionResult Show(int id)
        {
            ApplicationUser user = db.Users.Find(id);
            return View(user);

        }

        public ActionResult Edit(int id)
        {
            ApplicationUser user = db.Users.Find(id);
            return View(user);
        }

        [HttpPut]
        public ActionResult Edit(int id, ApplicationUser requestUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = db.Users.Find(id);
                    if (TryUpdateModel(user))
                    {
                        user.UserName = requestUser.UserName;
                        user.PhoneNumber = requestUser.PhoneNumber;
                        user.Email = requestUser.Email;
                        db.SaveChanges();
                        TempData["message"] = "Userul a fost modificat!";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requestUser);
                }
                
            }
            catch( Exception e)
            {
                return View(requestUser);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            ApplicationUser user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            TempData["message"] = "Userul a fost sters cu succes";
            return RedirectToAction("Index");
        }
        

    }
}