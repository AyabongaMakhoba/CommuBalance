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
    public class RequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Requests
        public ActionResult Index()
        {
            var requests = db.Requests.Include(r => r.Profile).Include(r => r.RequestStatus);
            return View(requests.ToList());
        }
        [ChildActionOnly]
        public ActionResult IndexPartial()
        {
            var requests = db.Requests.Include(r => r.Profile).Include(r => r.RequestStatus);
            return PartialView(requests.ToList());
        }

        //public ActionResult Map(int? id)
        //{
        //    var address = (from i in db.Requests
        //                   where i.Request_ID == id
        //                   select i.Profile.Profile_Address).FirstOrDefault();
        //    ViewBag.Address = address;
        //    return View();
        //}
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult ViewRequests()
        {
            var id = User.Identity.GetUserId();
            var requests = db.Requests.Include(r => r.Profile).Where(r => r.Profile.Profile_ID == id);
            return View(requests.ToList());
        }
        public ActionResult DriversView()
        {
            var id = User.Identity.GetUserId();
            var requests = db.Schedules.Include(r => r.Request).Where(r => r.Driver.Driver_ID == id ).Where (r=> r.Request.RequestStatus.Request_Satus== "En route for delivery" || r.Request.RequestStatus.RequestStatus_ID == 3);
            return View(requests.ToList());
        }

        // GET: Requests/Details/5

        public ActionResult RequestDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
			var uid = User.Identity.GetUserId();
            Request request = new Request();
            request.Profile_ID = uid;
            request.Request_Date = DateTime.Now;
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo");
            ViewBag.RequestStatus_ID = new SelectList(db.RequestStatus, "RequestStatus_ID", "Request_Satus");
            return View(request);
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Request_ID,Request_Date,Profile_ID,RequestStatus_ID")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.name = request.GetName();
                request.RequestStatus_ID = 1;
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Success");
            }

            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", request.Profile_ID);
            ViewBag.RequestStatus_ID = new SelectList(db.RequestStatus, "RequestStatus_ID", "Request_Satus", request.RequestStatus_ID);
            return View(request);
        }

        public ActionResult ConfirmRequest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", request.Profile_ID);
            ViewBag.RequestStatus_ID = new SelectList(db.RequestStatus,"RequestStatus_ID", "Request_Satus", request.RequestStatus_ID);
            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmRequest([Bind(Include = "Request_ID,Request_Date,Profile_ID,RequestStatus_ID")] Request request)
        {
            if (ModelState.IsValid)
            {

                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", request.Profile_ID);
            ViewBag.RequestStatus_ID = new SelectList(db.RequestStatus.Where(a => a.Request_Satus == "En route for delivery"), "RequestStatus_ID", "Request_Satus", request.RequestStatus_ID);
            return View(request);
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", request.Profile_ID);
            ViewBag.RequestStatus_ID = new SelectList(db.RequestStatus.Where(a => a.Request_Satus == "En route for delivery"), "RequestStatus_ID", "Request_Satus", request.RequestStatus_ID);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Request_ID,Request_Date,Profile_ID,RequestStatus_ID")] Request request)
        {
            if (ModelState.IsValid)
            {
              
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", request.Profile_ID);
            ViewBag.RequestStatus_ID = new SelectList(db.RequestStatus.Where(a => a.Request_Satus == "En route for delivery"), "RequestStatus_ID", "Request_Satus", request.RequestStatus_ID);
            return View(request);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
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
