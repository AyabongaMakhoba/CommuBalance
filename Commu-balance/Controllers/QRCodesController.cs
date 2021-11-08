using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZXing;
using Commu_balance.Models;

namespace Commu_balance.Controllers
{
    public class QRCodesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private string GenerateQRCode(string qrcodeText)
        {
            string folderPath = "~/Images/";
            string imagePath = "~/Images/QrCode.jpg";
            // create new Directory if not exist
            if (!Directory.Exists(Server.MapPath(folderPath)))
            {
                Directory.CreateDirectory(Server.MapPath(folderPath));
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = Server.MapPath(imagePath);
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return imagePath;
        }

        public ActionResult Read()
        {
            return View(ReadQRCode());
        }

        private QRCode ReadQRCode()
        {
            QRCode barcodeModel = new QRCode();
            string barcodeText = "";
            string imagePath = "~/Images/QrCode.jpg";
            string barcodePath = Server.MapPath(imagePath);
            var barcodeReader = new BarcodeReader();
            //Decode the image to text
            var result = barcodeReader.Decode(new Bitmap(barcodePath));
            if (result != null)
            {
                barcodeText = result.Text;
            }
            return new QRCode() { QRCodeText = barcodeText, QRCodeImagePath = imagePath };
        }


        public ActionResult AddNew(int id)
        {
            QRCode objQR = new QRCode();
            objQR.Schedule_ID = id;
            objQR.QRCodeText = "https://www.Commu-Balance.azurewebsites.net/Create/RequestStatus/" + objQR.Schedule_ID;
            objQR.QRCodeImagePath = GenerateQRCode(objQR.QRCodeText);
            db.QRCodes.Add(objQR);
            db.SaveChanges();
            return RedirectToAction("Details2",new { id = objQR.QRId});
        }

        // GET: QRCodes
        public ActionResult Index()
        {
            var qRCodes = db.QRCodes.Include(q => q.Schedule);
            return View(qRCodes.ToList());
        }

        // GET: QRCodes/Details/5
        public ActionResult Details2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QRCode qRCode = db.QRCodes.Find(id);
            if (qRCode == null)
            {
                return HttpNotFound();
            }
            return View(qRCode);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QRCode qRCode = db.QRCodes.Find(id);
            if (qRCode == null)
            {
                return HttpNotFound();
            }
            return View(qRCode);
        }

        // GET: QRCodes/Create
        public ActionResult Create()
        {
            ViewBag.Schedule_ID = new SelectList(db.Schedules, "Schedule_ID", "Driver_ID");
            return View();
        }

        // POST: QRCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QRId,QRCodeText,QRCodeImagePath,Schedule_ID")] QRCode qRCode)
        {
            if (ModelState.IsValid)
            {
                db.QRCodes.Add(qRCode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Schedule_ID = new SelectList(db.Schedules, "Schedule_ID", "Driver_ID", qRCode.Schedule_ID);
            return View(qRCode);
        }

        // GET: QRCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QRCode qRCode = db.QRCodes.Find(id);
            if (qRCode == null)
            {
                return HttpNotFound();
            }
            ViewBag.Schedule_ID = new SelectList(db.Schedules, "Schedule_ID", "Driver_ID", qRCode.Schedule_ID);
            return View(qRCode);
        }

        // POST: QRCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QRId,QRCodeText,QRCodeImagePath,Schedule_ID")] QRCode qRCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qRCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Schedule_ID = new SelectList(db.Schedules, "Schedule_ID", "Driver_ID", qRCode.Schedule_ID);
            return View(qRCode);
        }

        // GET: QRCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QRCode qRCode = db.QRCodes.Find(id);
            if (qRCode == null)
            {
                return HttpNotFound();
            }
            return View(qRCode);
        }

        // POST: QRCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QRCode qRCode = db.QRCodes.Find(id);
            db.QRCodes.Remove(qRCode);
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
