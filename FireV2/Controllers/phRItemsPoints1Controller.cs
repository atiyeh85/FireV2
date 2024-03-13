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
    public class phRItemsPoints1Controller : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: phRItemsPoints1
        public ActionResult Index()
        {
            var phRItemsPoints = db.phRItemsPoints.Include(p => p.AgeRenge).Include(p => p.PhysicalReadinesitem);
            return View(phRItemsPoints.ToList());
        }

        // GET: phRItemsPoints1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phRItemsPoint phRItemsPoint = db.phRItemsPoints.Find(id);
            if (phRItemsPoint == null)
            {
                return HttpNotFound();
            }
            return View(phRItemsPoint);
        }

        // GET: phRItemsPoints1/Create
        public ActionResult Create()
        {
            ViewBag.AgeRengeId = new SelectList(db.AgeRenges, "AgeRengeId", "AgeRengeId");
            ViewBag.PhReadinessItemId = new SelectList(db.PhysicalReadinesitems, "PhReadinessItemId", "PhReadinessItemTitle");
            return View();
        }

        // POST: phRItemsPoints1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "phRItemsPointId,Time,Point,AgeRengeId,PhReadinessItemId")] phRItemsPoint phRItemsPoint)
        {
            if (ModelState.IsValid)
            {
                db.phRItemsPoints.Add(phRItemsPoint);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgeRengeId = new SelectList(db.AgeRenges, "AgeRengeId", "AgeRengeId", phRItemsPoint.AgeRengeId);
            ViewBag.PhReadinessItemId = new SelectList(db.PhysicalReadinesitems, "PhReadinessItemId", "PhReadinessItemTitle", phRItemsPoint.PhReadinessItemId);
            return View(phRItemsPoint);
        }

        // GET: phRItemsPoints1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phRItemsPoint phRItemsPoint = db.phRItemsPoints.Find(id);
            if (phRItemsPoint == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgeRengeId = new SelectList(db.AgeRenges, "AgeRengeId", "AgeRengeId", phRItemsPoint.AgeRengeId);
            ViewBag.PhReadinessItemId = new SelectList(db.PhysicalReadinesitems, "PhReadinessItemId", "PhReadinessItemTitle", phRItemsPoint.PhReadinessItemId);
            return View(phRItemsPoint);
        }

        // POST: phRItemsPoints1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "phRItemsPointId,Time,Point,AgeRengeId,PhReadinessItemId")] phRItemsPoint phRItemsPoint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phRItemsPoint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("edite");
            }
            ViewBag.AgeRengeId = new SelectList(db.AgeRenges, "AgeRengeId", "AgeRengeId", phRItemsPoint.AgeRengeId);
            ViewBag.PhReadinessItemId = new SelectList(db.PhysicalReadinesitems, "PhReadinessItemId", "PhReadinessItemTitle", phRItemsPoint.PhReadinessItemId);
            return View(phRItemsPoint);
        }

        // GET: phRItemsPoints1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phRItemsPoint phRItemsPoint = db.phRItemsPoints.Find(id);
            if (phRItemsPoint == null)
            {
                return HttpNotFound();
            }
            return View(phRItemsPoint);
        }

        // POST: phRItemsPoints1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            phRItemsPoint phRItemsPoint = db.phRItemsPoints.Find(id);
            db.phRItemsPoints.Remove(phRItemsPoint);
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
