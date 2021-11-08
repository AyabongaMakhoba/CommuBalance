using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Commu_balance.Models;
using Microsoft.AspNet.Identity;

namespace Commu_balance.Controllers
{
    public class User_CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User_Category
        public ActionResult Index()
        {
            var user_Categories = db.User_Categories.Include(u => u.Profile);
            return View(user_Categories.ToList());
        }

        // GET: User_Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Category user_Category = db.User_Categories.Find(id);
            if (user_Category == null)
            {
                return HttpNotFound();
            }
            return View(user_Category);
        }

        // GET: User_Category/Create
        public ActionResult Create(string id)
        {
            User_Category Category = new User_Category();
            Category.Profile_ID = id;

			ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo");
            return View(Category);
        }

        // POST: User_Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserCat_ID,Category_Name,Profile_ID")] User_Category user_Category)
        {
            if (ModelState.IsValid)
            {

                db.User_Categories.Add(user_Category);
                db.SaveChanges();
                return RedirectToAction("Create","Documents", new {id = user_Category.Profile_ID});
            }

            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", user_Category.Profile_ID);
            return View(user_Category);
        }

        // GET: User_Category/Edit/5
        public ActionResult Edit()
        {
            var uid = User.Identity.GetUserId();
            int? id = (from c in db.User_Categories
                          where c.Profile_ID == uid
                          select c.UserCat_ID).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Category user_Category = db.User_Categories.Find(id);
            if (user_Category == null)
            {
                return HttpNotFound();
            }
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", user_Category.Profile_ID);
            return View(user_Category);
        }

        // POST: User_Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserCat_ID,Category_Name,Profile_ID")] User_Category user_Category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", user_Category.Profile_ID);
            return View(user_Category);
        }

        // GET: User_Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Category user_Category = db.User_Categories.Find(id);
            if (user_Category == null)
            {
                return HttpNotFound();
            }
            return View(user_Category);
        }

        // POST: User_Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User_Category user_Category = db.User_Categories.Find(id);
            db.User_Categories.Remove(user_Category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
