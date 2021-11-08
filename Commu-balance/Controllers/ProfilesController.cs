using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Commu_balance.Models;
using Microsoft.AspNet.Identity;

namespace Commu_balance.Controllers
{
    public class ProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profiles
        public ActionResult Index()
        {
            return View(db.Profiles.ToList());
        }
        // GET: Profiles/Details/5
        public ActionResult Details()
        {
            var uid = User.Identity.GetUserId();
            var id = uid;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // GET: Profiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Profile_ID,User_IDNo,Profile_Name,Profile_Surname,Profile_Cellnumber,Profile_Address,Profile_Email")] Profile profile)
        {
            if (ModelState.IsValid)
            {

                db.Profiles.Add(profile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profile);
        }

   

        // GET: Profiles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Profile_ID,User_IDNo,Profile_Name,Profile_Surname,Profile_Cellnumber,Profile_Address,Profile_Email")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                var uid = User.Identity.GetUserId();
                var getCat = (from c in db.User_Categories
                              where c.Profile_ID == uid
                              select c.Profile_ID).FirstOrDefault();
                if(getCat == null)
                    return RedirectToAction("Create", "User_Category", new { id = uid });
                else
                    return RedirectToAction("Edit", "User_Category");
           
            }
            return View(profile);
        }

        // GET: Profiles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Profile profile = db.Profiles.Find(id);
            db.Profiles.Remove(profile);
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
