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
    public class InfoDocumentTitleTypesController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: InfoDocumentTitleTypes
        public ActionResult Index()
        {
            var infoDocumentTitleTypes = db.InfoDocumentTitleTypes.Include(i => i.InfoDocumentTitle);
            return View(infoDocumentTitleTypes.ToList());
        }

        // GET: InfoDocumentTitleTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoDocumentTitleType infoDocumentTitleType = db.InfoDocumentTitleTypes.Find(id);
            if (infoDocumentTitleType == null)
            {
                return HttpNotFound();
            }
            return View(infoDocumentTitleType);
        }

        [AllowAnonymous]

        // GET: CoopreationPersons/Create
        public ActionResult Create(int id)
        {
            ViewBag.InfoDocTitleId = new SelectList(db.InfoDocumentTitles, "InfoDocTitleId", "InfoDocTitle");
            var Athlete = db.Athletes.Find(id);
            var model = new DocTypeAthelet() { AthleteId = id, Athlete = Athlete };
            return View(model);
        }
        // POST: InfoDocumentTitleTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InfoDocumentTitleType infoDocumentTitleType)
        {
            if (ModelState.IsValid)
            {
                db.InfoDocumentTitleTypes.Add(infoDocumentTitleType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InfoDocTitleId = new SelectList(db.InfoDocumentTitles, "InfoDocTitleId", "InfoDocTitle", infoDocumentTitleType.InfoDocTitleId);
            return View(infoDocumentTitleType);
        }

        // GET: InfoDocumentTitleTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoDocumentTitleType infoDocumentTitleType = db.InfoDocumentTitleTypes.Find(id);
            if (infoDocumentTitleType == null)
            {
                return HttpNotFound();
            }
            ViewBag.InfoDocTitleId = new SelectList(db.InfoDocumentTitles, "InfoDocTitleId", "InfoDocTitle", infoDocumentTitleType.InfoDocTitleId);
            return View(infoDocumentTitleType);
        }

        // POST: InfoDocumentTitleTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InfoDocTitleTypeId,InfoDocTitleType,InfoDocTitleId")] InfoDocumentTitleType infoDocumentTitleType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(infoDocumentTitleType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InfoDocTitleId = new SelectList(db.InfoDocumentTitles, "InfoDocTitleId", "InfoDocTitle", infoDocumentTitleType.InfoDocTitleId);
            return View(infoDocumentTitleType);
        }

        // GET: InfoDocumentTitleTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoDocumentTitleType infoDocumentTitleType = db.InfoDocumentTitleTypes.Find(id);
            if (infoDocumentTitleType == null)
            {
                return HttpNotFound();
            }
            return View(infoDocumentTitleType);
        }

        // POST: InfoDocumentTitleTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InfoDocumentTitleType infoDocumentTitleType = db.InfoDocumentTitleTypes.Find(id);
            db.InfoDocumentTitleTypes.Remove(infoDocumentTitleType);
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
