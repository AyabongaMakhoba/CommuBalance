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
    public class SchedulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Schedules
        public ActionResult Index()
        {
            var schedules = db.Schedules.Include(s => s.Driver).Include(s => s.Request);
            return View(schedules.ToList());
        }
        public ActionResult Map(int? id)
        {
            var schedules = db.Schedules.Include(s => s.Driver).Include(s => s.Request).Where(s=>s.Request.Request_ID == id);

            return View(schedules.ToList());
        }

        //      public ActionResult ViewRequests()
        //{
        //          var id = User.Identity.GetUserId();
        //          var requests = db.Requests.Include(r => r.Profile).Where(r => r.Profile.Profile_ID == id && r.RequestStatus_ID == 2);
        //          return View(requests.ToList());
        //}
        // GET: Schedules/Details/5
        public ActionResult ScheduleDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        public ActionResult Details(int? id)
        {
            int? findId = (from i in db.Schedules
                          where i.Request.Request_ID == id
                          select i.Schedule_ID).FirstOrDefault();
                          
            if (findId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(findId);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            Schedule schedule = new Schedule();
            schedule.Schedule_Date = DateTime.Now;
            ViewBag.Driver_ID = new SelectList(db.Drivers, "Driver_ID", "Driver_Name");
            ViewBag.Request_ID = new SelectList(db.Requests.Where(a => a.RequestStatus_ID == 2), "Request_ID", "Profile_ID");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Schedule_ID,Schedule_Date,Request_ID,Driver_ID")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                Request getCat = (from c in db.Requests
                              where c.Request_ID == schedule.Request_ID
                              select c).FirstOrDefault();
                getCat.RequestStatus_ID = 3;
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("AddNew", "QRCodes", new { id = schedule.Schedule_ID });
            }

            ViewBag.Driver_ID = new SelectList(db.Drivers, "Driver_ID", "Driver_Name", schedule.Driver_ID);
            ViewBag.Request_ID = new SelectList(db.Requests.Where(a => a.RequestStatus_ID == 2 ), "Request_ID", "Profile_ID", schedule.Request_ID);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.Driver_ID = new SelectList(db.Drivers, "Driver_ID", "Driver_Name", schedule.Driver_ID);
            ViewBag.Request_ID = new SelectList(db.Requests, "Request_ID", "Profile_ID", schedule.Request_ID);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Schedule_ID,Schedule_Date,Request_ID,Driver_ID")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Driver_ID = new SelectList(db.Drivers, "Driver_ID", "Driver_Name", schedule.Driver_ID);
            ViewBag.Request_ID = new SelectList(db.Requests, "Request_ID", "Profile_ID", schedule.Request_ID);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
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
