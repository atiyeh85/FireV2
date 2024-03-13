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
    public class AthTypeMedicalsController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: AthTypeMedicals
        public ActionResult Index()
        {
            return View(db.AthTypeMedicals.ToList());
        }

        // GET: AthTypeMedicals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AthTypeMedical athTypeMedical = db.AthTypeMedicals.Find(id);
            if (athTypeMedical == null)
            {
                return HttpNotFound();
            }
            return View(athTypeMedical);
        }

        // GET: AthTypeMedicals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AthTypeMedicals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AthTypeMid,Medicalid,MTitleTypeid")] AthTypeMedical athTypeMedical)
        {
            if (ModelState.IsValid)
            {
                db.AthTypeMedicals.Add(athTypeMedical);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(athTypeMedical);
        }

        // GET: AthTypeMedicals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AthTypeMedical athTypeMedical = db.AthTypeMedicals.Find(id);
            if (athTypeMedical == null)
            {
                return HttpNotFound();
            }
            return View(athTypeMedical);
        }

        // POST: AthTypeMedicals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AthTypeMid,Medicalid,MTitleTypeid")] AthTypeMedical athTypeMedical)
        {
            if (ModelState.IsValid)
            {
                db.Entry(athTypeMedical).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(athTypeMedical);
        }

        // GET: AthTypeMedicals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AthTypeMedical athTypeMedical = db.AthTypeMedicals.Find(id);
            if (athTypeMedical == null)
            {
                return HttpNotFound();
            }
            return View(athTypeMedical);
        }

        // POST: AthTypeMedicals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AthTypeMedical athTypeMedical = db.AthTypeMedicals.Find(id);
            db.AthTypeMedicals.Remove(athTypeMedical);
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
