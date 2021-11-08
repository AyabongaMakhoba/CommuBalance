using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PayFast.AspNet;
using Commu_balance.Models;
using System.Configuration;
using PayFast;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using System.Threading.Tasks;
using Syncfusion.Pdf.Grid;

namespace Commu_balance.Controllers
{
    [Authorize]
    public class DonationsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public DonationsController()
        {
            this.payFastSettings = new PayFastSettings();
            this.payFastSettings.MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            this.payFastSettings.MerchantKey = ConfigurationManager.AppSettings["MerchantKey"];
            this.payFastSettings.PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            this.payFastSettings.ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"];
            this.payFastSettings.ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"];
            this.payFastSettings.ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            this.payFastSettings.CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
            this.payFastSettings.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
        }

        public ActionResult PDF()
        {
            return View();
        }

        // GET: Donations
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            var donations = db.Donations.Include(d => d.Profile).Where(d => d.Profile_ID == id);
            return View(donations.ToList());
        }
        public ActionResult DonationProgress()
        {
            var id = User.Identity.GetUserId();
            var donations = db.Donations.Include(d => d.Profile).Where(d => d.Donation_Date.Month == DateTime.Now.Month);
            var howmanyDonations = db.Donations.Include(d => d.Profile).Where(d => d.Donation_Date.Month == DateTime.Now.Month).Count();
            var howmanyRequests = db.Requests.Include(r => r.Profile).Include(r => r.RequestStatus).Where(d => d.Request_Date.Month == DateTime.Now.Month).Count();
            ViewBag.NumDonations = howmanyDonations;
            ViewBag.NumRequests = howmanyRequests;

            return View(donations.ToList());
        }

        [ChildActionOnly]
        public ActionResult PartialIndex()
        {
            
            var donations = db.Donations.Include(d => d.Profile).Where(d => d.Donation_Date.Month == DateTime.Now.Month);
            return PartialView(donations.ToList());
        }

        // GET: Donations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return HttpNotFound();
            }
            //Create a new PDF document
            PdfDocument document = new PdfDocument();

            //Add a page to the document
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            ////Set the standard font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

            //Load the image as stream.
            FileStream imageStream = new FileStream(Server.MapPath(@"~/LOGO/dafa.png"), FileMode.Open, FileAccess.Read);
            RectangleF bounds = new RectangleF(45, 0, 400, 60);
            PdfImage image = PdfImage.FromStream(imageStream);
            //Draws the image to the PDF page
            page.Graphics.DrawImage(image, bounds);


            PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(0, 0, 0));
            bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            //Draws a rectangle to place the heading in that region.
            graphics.DrawRectangle(solidBrush, bounds);
            //Creates a font for adding the heading in the page
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
            //Creates a text element to add the invoice number
            PdfTextElement element = new PdfTextElement("DONATION RECEIPT ", subHeadingFont);
            element.Brush = PdfBrushes.White;

            //Draws the heading on the page
            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));
            string currentDate = "DATE " + DateTime.Now.ToString("MM/dd/yyyy");
            //Measures the width of the text to place it in the correct location
            SizeF textSize = subHeadingFont.MeasureString(currentDate);
            PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
            //Draws the date by using DrawString method
            graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);
            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);

            //Thank you Text
            element = new PdfTextElement("Thank You For Your Gift" + "\n" +
                "The amount you have given will make a difference to those who are less fortunate" + "\n" + "\n" +
                "This receipt is an attestation that we gratefully received your generous contribution to our" + "\n" +
                "humble organisation.", timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(0, 0, 0));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            //Creates text elements to add the address and draw it to the page.
            element = new PdfTextElement("DONATED BY" + "\n" + donation.Profile.Profile_Name + " " + donation.Profile.Profile_Surname + "\n" + "\n" +
                "DONATION AMOUNT" + "\n" + "R" + donation.Donation_Amt + "\n" + "\n" +
                "DONATION DATE" + "\n" + donation.Donation_Date.ToShortDateString() + "\n" + "\n" +
                "ID NUMBER" + "\n" + donation.Profile.User_IDNo +"\n" + "\n" +
                "ADDRESS" + "\n" + donation.Profile.Profile_Address + "\n" + "\n" +
                "PHONE NUMBER" + "\n" + donation.Profile.Profile_Cellnumber, timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(0, 0, 0));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            //Draws a line at the bottom of the address
            graphics.DrawLine(linePen, startPoint, endPoint);

            //Saving the PDF to the MemoryStream
            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            //Set the position as '0'.
            stream.Position = 0;

            //Download the PDF document in the browser
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");

            fileStreamResult.FileDownloadName = "DONATIONRECEIPT.pdf";

            return fileStreamResult;
        }

        // GET: Donations/Create
        public ActionResult Create()
        {
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo");


            return View();
        }

        // POST: Donations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Donation_ID,Donation_Amt,Donation_Date,Profile_ID")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                var uid = User.Identity.GetUserId();
                donation.Profile_ID = uid;
                donation.Donation_Date = DateTime.Today;
                db.Donations.Add(donation);
                db.SaveChanges();
                return RedirectToAction("OnceOff", new { id = donation.Donation_ID });
            }

            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", donation.Profile_ID);
            return View(donation);
        }

        // GET: Donations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", donation.Profile_ID);
            return View(donation);
        }

        // POST: Donations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Donation_ID,Donation_Amt,Donation_Date,Profile_ID")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Profile_ID = new SelectList(db.Profiles, "Profile_ID", "User_IDNo", donation.Profile_ID);
            return View(donation);
        }

        // GET: Donations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return HttpNotFound();
            }
            return View(donation);
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Donation donation = db.Donations.Find(id);
            db.Donations.Remove(donation);
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

        #region Fields

        private readonly PayFastSettings payFastSettings;

        #endregion Fields

        public ActionResult Recurring()
        {
            var recurringRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            // Merchant Details
            recurringRequest.merchant_id = this.payFastSettings.MerchantId;
            recurringRequest.merchant_key = this.payFastSettings.MerchantKey;
            recurringRequest.return_url = this.payFastSettings.ReturnUrl;
            recurringRequest.cancel_url = this.payFastSettings.CancelUrl;
            recurringRequest.notify_url = this.payFastSettings.NotifyUrl;
            // Buyer Details
            recurringRequest.email_address = "nkosi@finalstride.com";
            // Transaction Details
            recurringRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            recurringRequest.amount = 20;
            recurringRequest.item_name = "Recurring Option";
            recurringRequest.item_description = "Some details about the recurring option";
            // Transaction Options
            recurringRequest.email_confirmation = true;
            recurringRequest.confirmation_address = "drnendwandwe@gmail.com";
            // Recurring Billing Details
            recurringRequest.subscription_type = SubscriptionType.Subscription;
            recurringRequest.billing_date = DateTime.Now;
            recurringRequest.recurring_amount = 20;
            recurringRequest.frequency = BillingFrequency.Monthly;
            recurringRequest.cycles = 0;
            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{recurringRequest.ToString()}";
            return Redirect(redirectUrl);
        }


        public ActionResult OnceOff(int id)
        {

            //var uid = User.Identity.GetUserId();
            //var appointments = db.Appointments.Include(a => a.Client).Where(x => x.ClientId == uid).Where(a => a.paymentstatus == false).Where(a => a.status == false);
            Donation Donation = db.Donations.Find(id);



            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);
            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;
            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            double amount = (double)Donation.Donation_Amt;
            //var products = db.Items.Select(x => x.Item_Name).ToList();
            // Transaction Details
            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = amount;
            onceOffRequest.item_name = "Your appointment Number is " + id;
            onceOffRequest.item_description = "You are now paying your rental fee";
            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

            //  order.paymentstatus = true;
            db.Entry(Donation).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect(redirectUrl);
        }


        public ActionResult AdHoc()
        {
            var adHocRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            adHocRequest.merchant_id = this.payFastSettings.MerchantId;
            adHocRequest.merchant_key = this.payFastSettings.MerchantKey;
            adHocRequest.return_url = this.payFastSettings.ReturnUrl;
            adHocRequest.cancel_url = this.payFastSettings.CancelUrl;
            adHocRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            adHocRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            adHocRequest.m_payment_id = "";
            adHocRequest.amount = 70;
            adHocRequest.item_name = "Adhoc Agreement";
            adHocRequest.item_description = "Some details about the adhoc agreement";

            // Transaction Options
            adHocRequest.email_confirmation = true;
            adHocRequest.confirmation_address = "sbtu01@payfast.co.za";

            // Recurring Billing Details
            adHocRequest.subscription_type = SubscriptionType.AdHoc;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{adHocRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        public ActionResult Return()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Notify([ModelBinder(typeof(PayFastNotifyModelBinder))] PayFastNotify payFastNotifyViewModel)
        {
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

            System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

            // Currently seems that the data validation only works for successful payments
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}
