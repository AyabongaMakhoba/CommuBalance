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

namespace Commu_balance.Controllers
{
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Documents
        public ActionResult Index()
        {
            var documents = db.Documents.Include(d => d.Profile);
            return View(documents.ToList());
        }

        public ActionResult fullScreen(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult Create(string id)
        {
            Document doc = new Document();
            doc.Profile_ID = id;
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo");
            return View(doc);
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Documents_ID,Documents,Proof_Of_Income,Profile_ID")] Document document, HttpPostedFileBase filelist, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
            
                if (filelist != null && filelist.ContentLength > 0)
                {
                    int filelength = filelist.ContentLength;
                    Byte[] array = new Byte[filelength];
                    filelist.InputStream.Read(array, 0, filelength);
                    document.Documents = array;
                }
                if (upload != null && upload.ContentLength > 0)
                {
                    int filelength = upload.ContentLength;
                    Byte[] array = new Byte[filelength];
                    upload.InputStream.Read(array, 0, filelength);
                    document.Proof_Of_Income = array;
                }
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", document.Profile_ID);
            return View(document);
        }
        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            BinaryReader reader = new BinaryReader(file.InputStream);
            return reader.ReadBytes((int)file.ContentLength);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", document.Profile_ID);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Documents_ID,Documents,Proof_Of_Income,Profile_ID")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", document.Profile_ID);
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
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
