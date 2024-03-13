using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FireV2.Models;
using System.IO;
using MvcFileUploader.Models;
using MvcFileUploader;

namespace FireV2.Controllers
{
    public class MedicalsController : Controller
    {
        private StoreDb db = new StoreDb();
        string UploadPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/FileUpload/"));
        // GET: /Medicals/
        public ActionResult Index(int? id, bool? inline, string ui = "bootstrap")
        {
            var medicals = db.Medicals.Where(m =>m.Flag==false && m.AthleteId == id).Include(m => m.Athlete).Include(m => m.Period);
            var model = medicals.Select(s => new FireV2.ViewModels.MedicalVm()
            {
                 AthleteId=s.AthleteId,
                  count=s.FileUploads.Count(),
                   DateInsert=s.DateInsert,
                   UserInsert=s.UserInsert,
                   Note=s.Note,
                    Medicalid=s.Medicalid,
                    PeriodId=s.PeriodId,
                    TypeOfTestId=s.TypeOfTestId,
                    SeasonTitle=s.Period.SeasonTitle,
                    Type=s.TypeOfTest.Type,
                    MItem=s.AthTypeMedicals.Select(a=>a.MedicalTitleType.MedicalItem.MItem).FirstOrDefault(),
                  Titles = s.AthTypeMedicals.Select(a=>a.MedicalTitleType.MTType).ToList()
                    //Titles=

            });
            return View(model.ToList());
        }
      
        // GET: /Medicals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medical medical = db.Medicals.Find(id);
            if (medical == null)
            {
                return HttpNotFound();
            }
            return View(medical);
        }

        // GET: /Medicals/Create
        public ActionResult Create(int? id)
        {
            ViewBag.AthleteId = id;
            var Athlete = db.Athletes.Where(a => a.AthleteId == id).FirstOrDefault();
           ViewBag.FullName = Athlete.FullName;
            ViewBag.MedicalItemid = new SelectList(db.MedicalItems, "MedicalItemid", "MItem");
            ViewBag.TypeOfTestId = new SelectList(db.TypeOfTests, "TypeOfTestId", "Type");
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle");
            return View();
        }
        [AllowAnonymous]
        public ActionResult GetMedicalItemType(int MedicalItemid)
        {
            var date = Utility.PertionDate.Today();

            ViewBag.List = db.MedicalTitleTypes.Where(c => c.MedicalItemid == MedicalItemid).ToList();
            var MedicalTitleType = db.MedicalTitleTypes.Where(h => h.MedicalItemid == MedicalItemid).Select(h => h.MedicalItemid);

            //ViewBag.List = db.Medicals.Include(h => h.Medicaltitle).Where(v =>  v.Medicaltitle.MedicalItemid == MedicalItemid).Select(v => v.HojTitleId).ToList();
            return PartialView();

        }
        // POST: /Medicals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Medical medical,  string items = "")
        {
            
            if (ModelState.IsValid)
            {
                
                Medical Medi = new Medical();
                AthTypeMedical Ath = new AthTypeMedical();
                Medi.Note = medical.Note;
                Medi.UserInsert =User.Identity.Name;
                Medi.DateInsert =Utility.PertionDate.Today();
                Medi.AthleteId = medical.AthleteId;
                Medi.PeriodId = medical.PeriodId;
                Medi.TypeOfTestId = medical.TypeOfTestId;

                db.Medicals.Add(Medi);
                db.SaveChanges();

                var Medicalid = Medi.Medicalid;
                foreach (string item in items.Split(';'))
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                   
                int itemId = int.Parse(item);
                    Ath.Medicalid = Medicalid;
                    Ath.MTitleTypeid = itemId;
                    db.AthTypeMedicals.Add(Ath);
                    db.SaveChanges();
                    
                }
                if (Request.IsAjaxRequest())
                    return Content("true");

                //return RedirectToAction("Create", "Medicals", new { id = medical.AthleteId });
            }
            ViewBag.TypeOfTestId = new SelectList(db.TypeOfTests, "TypeOfTestId", "Type");
            ViewBag.MedicalItemid = new SelectList(db.MedicalItems, "MedicalItemid", "MItem");
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle");

            return View(medical);
        }
        // GET: /Medicals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medical medical = db.Medicals.Find(id);
            if (medical == null)
            {
                return HttpNotFound();
            }
            ViewBag.AthleteId = medical.AthleteId;
            var MedicalItemid = medical.AthTypeMedicals.Select(a => a.MedicalTitleType.MedicalItem.MedicalItemid).FirstOrDefault();
            ViewBag.Listselected = medical.AthTypeMedicals.Where(a => a.MedicalTitleType.MedicalItemid == MedicalItemid).Select(a => a.MedicalTitleType.MTitleTypeid).ToList();
            ViewBag.list = db.MedicalTitleTypes.Where(m => m.MedicalItemid == MedicalItemid).ToList();
            ViewBag.MedicalItemid = new SelectList(db.MedicalItems, "MedicalItemid", "MItem",MedicalItemid);
            ViewBag.TypeOfTestId = new SelectList(db.TypeOfTests, "TypeOfTestId", "Type", medical.TypeOfTestId);
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle", medical.PeriodId);
            return View(medical);
        }
        // POST: /Medicals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Medical medical, string items = "")
        {
           
            Medical _medical = db.Medicals.Find(medical.Medicalid);
            ViewBag.AthleteId = medical.AthleteId;
            var MedicalItemid = medical.AthTypeMedicals.Select(a => a.MedicalTitleType.MedicalItem.MedicalItemid).FirstOrDefault();
            var athtm = new AthTypeMedical();
            //_medical.AthleteId = medical.AthleteId;
            if (ModelState.IsValid)
            {
                if (items != null)
                {
                    while (_medical.AthTypeMedicals.Count > 0)
                        db.AthTypeMedicals.Remove(_medical.AthTypeMedicals.FirstOrDefault());
                    _medical.PeriodId = medical.PeriodId;
                    _medical.UserInsert = medical.UserInsert;
                    _medical.TypeOfTestId = medical.TypeOfTestId;
                    _medical.Note = medical.Note;
                    _medical.LastUserE = User.Identity.Name;
                    _medical.LastDateE =Utility.PertionDate.Today();
                    foreach (string item in items.Split(';'))
                    {
                        if (string.IsNullOrWhiteSpace(item)) continue;
                        athtm.Medicalid = medical.Medicalid;
                        athtm.MTitleTypeid = Convert.ToInt32(item);
                        db.AthTypeMedicals.Add(athtm);
                        db.SaveChanges();

                    }

                }
                db.Entry(_medical).State = EntityState.Modified;
                db.SaveChanges();
                return Content("true");
            }
            ViewBag.MedicalItemid = new SelectList(db.MedicalItems, "MedicalItemid", "MItem", MedicalItemid);
            ViewBag.TypeOfTestId = new SelectList(db.TypeOfTests, "TypeOfTestId", "Type", medical.TypeOfTestId);
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle", medical.PeriodId);
            return View(medical);
        }

        // GET: /Medicals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medical medical = db.Medicals.Find(id);
            if (medical == null)
            {
                return HttpNotFound();
            }
            return View(medical);
        }

        // POST: /Medicals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medical medical = db.Medicals.Find(id);
            medical.Flag = false;
            db.Entry(medical).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Create", "Medicals", new { id = medical.AthleteId });

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
