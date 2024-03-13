using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FireV2.Models;

namespace FireV2.Controllers
{
    public class ArchivesController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: Archives
        public ActionResult Index()
        {
            var archives = db.Archives.Include(a => a.Athlete);
            return View(archives.ToList());
        }

        // GET: Archives/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archive archive = db.Archives.Find(id);
            if (archive == null)
            {
                return HttpNotFound();
            }
            return View(archive);
        }

        // GET: Archives/Create
        public ActionResult Create(int? id)
        {
            ViewBag.id = id;
            return View();
        }
        public ActionResult Circulate(int? id)
        {
            ViewBag.id = id;
            return View();
        }
        // POST: Archives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Archive archive)
        {
            var Arch = new Archive();
            var Athelete = db.Athletes.Where(p=>p.AthleteId==archive.AthleteId).FirstOrDefault();
            Arch.Date = Utility.PertionDate.Today();
            Arch.Time = DateTime.Now.ToShortTimeString();
            Arch.ArchiveNote = archive.ArchiveNote;
            Arch.UsetArchive =User.Identity.Name;
            Arch.AthleteId = archive.AthleteId;
            Arch.IsArchive =true;
            Athelete.IsArchive = true;
            if (ModelState.IsValid)
            {
                db.Entry(Athelete).State = EntityState.Modified;
                db.Archives.Add(Arch);
                db.SaveChanges();
                return Content("true");
            }

            return View(archive);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Circulate(Archive archive)
        {
            var Arch = new Archive();
            var Athelete = db.Athletes.Where(p => p.AthleteId == archive.AthleteId).FirstOrDefault();
            Arch.Date = Utility.PertionDate.Today();
            Arch.Time = DateTime.Now.ToShortTimeString();
            Arch.ArchiveNote = archive.ArchiveNote;
            Arch.UsetArchive = User.Identity.Name;
            Arch.AthleteId = archive.AthleteId;
            Arch.IsArchive = false;
            Athelete.IsArchive = false;
            if (ModelState.IsValid)
            {
                db.Entry(Athelete).State = EntityState.Modified;
                db.Archives.Add(Arch);
                db.SaveChanges();
                return Content("true");
            }

            return View(archive);
        }
        // GET: Archives/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archive archive = db.Archives.Find(id);
            if (archive == null)
            {
                return HttpNotFound();
            }
            ViewBag.AthleteId = new SelectList(db.Athletes, "AthleteId", "Fnam", archive.AthleteId);
            return View(archive);
        }

        // POST: Archives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IsArchiveid,ArchiveNote,Date,IsArchive,AthleteId")] Archive archive)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archive).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AthleteId = new SelectList(db.Athletes, "AthleteId", "Fnam", archive.AthleteId);
            return View(archive);
        }

        // GET: Archives/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archive archive = db.Archives.Find(id);
            if (archive == null)
            {
                return HttpNotFound();
            }
            return View(archive);
        }

        // POST: Archives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Archive archive = db.Archives.Find(id);
            db.Archives.Remove(archive);
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
