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
    public class DocTypeAtheletsController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: DocTypeAthelets
        public ActionResult Index()
        {
            var docTypeAthelets = db.DocTypeAthelets.Include(d => d.Athlete).Include(d => d.InfoDocumentTitleType);
            return View(docTypeAthelets.ToList());
        }
        [AllowAnonymous]

        //public ActionResult InfoDocumentTitleTypes(int id,int atid)
        //{
        //    var ids = db.DocTypeAthelets.Where(d => d.AthleteId == atid && d.InfoDocumentTitleType.InfoDocTitleId == id).Select(d => d.InfoDocumentTitleType.InfoDocTitleTypeId).ToList();
        //    return PartialView(db.InfoDocumentTitleTypes.Where(c => c.InfoDocTitleId == id && !ids.Contains(c.InfoDocTitleTypeId)));
        //}
        public ActionResult InfoDocumentTitleTypes(int id)
        {

            return PartialView(db.InfoDocumentTitleTypes.Where(c => c.InfoDocTitleId == id));
        }
        // GET: DocTypeAthelets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocTypeAthelet docTypeAthelet = db.DocTypeAthelets.Find(id);
            if (docTypeAthelet == null)
            {
                return HttpNotFound();
            }
            return View(docTypeAthelet);
        }

        // GET: DocTypeAthelets/Create
        public ActionResult Create(int id)
        {
            ViewBag.AthleteId = new SelectList(db.Athletes, "AthleteId", "Fnam");
            ViewBag.InfoDocTitleId = new SelectList(db.InfoDocumentTitles, "InfoDocTitleId", "InfoDocTitle");
            ViewBag.InfoDocTitleTypeId = new SelectList(db.InfoDocumentTitleTypes, "InfoDocTitleTypeId", "InfoDocTitleType");
            var Athlete = db.Athletes.Find(id);
            var model = new DocTypeAthelet() { AthleteId = id, Athlete = Athlete };


            return View(model);
        }
       

        // POST: DocTypeAthelets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocTypeAthelet docTypeAthelet, int[] details)
        {
            foreach (var item in details)
            {
                DocTypeAthelet model = new DocTypeAthelet()
                {
                     AthleteId=docTypeAthelet.AthleteId,
                      InfoDocTitleTypeId=item,
                      Note=docTypeAthelet.Note
                };
                db.DocTypeAthelets.Add(model);
            }
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CoopreationId = new SelectList(db.InfoDocumentTitles, "CoopreationId", "CoopTitle", InfoDocTitleId);
            //ViewBag.PersonId = new SelectList(db.Athletes, "PersonId", "Fnam", InfoDocTitleId.PersonProfileId);
            return View(docTypeAthelet);
        }

        // GET: DocTypeAthelets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocTypeAthelet docTypeAthelet = db.DocTypeAthelets.Find(id);
            if (docTypeAthelet == null)
            {
                return HttpNotFound();
            }
            ViewBag.AthleteId = new SelectList(db.Athletes, "AthleteId", "Fnam", docTypeAthelet.AthleteId);
            ViewBag.InfoDocTitleTypeId = new SelectList(db.InfoDocumentTitleTypes, "InfoDocTitleTypeId", "InfoDocTitleType", docTypeAthelet.InfoDocTitleTypeId);
            return View(docTypeAthelet);
        }

        // POST: DocTypeAthelets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocTypeAtId,AthleteId,InfoDocTitleTypeId")] DocTypeAthelet docTypeAthelet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(docTypeAthelet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AthleteId = new SelectList(db.Athletes, "AthleteId", "Fnam", docTypeAthelet.AthleteId);
            ViewBag.InfoDocTitleTypeId = new SelectList(db.InfoDocumentTitleTypes, "InfoDocTitleTypeId", "InfoDocTitleType", docTypeAthelet.InfoDocTitleTypeId);
            return View(docTypeAthelet);
        }

        // GET: DocTypeAthelets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocTypeAthelet docTypeAthelet = db.DocTypeAthelets.Find(id);
            if (docTypeAthelet == null)
            {
                return HttpNotFound();
            }
            return View(docTypeAthelet);
        }

        // POST: DocTypeAthelets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocTypeAthelet docTypeAthelet = db.DocTypeAthelets.Find(id);
            db.DocTypeAthelets.Remove(docTypeAthelet);
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
