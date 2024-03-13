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

    public class PhRedinessHistoryItemsController : Controller
    {
        private StoreDb db = new StoreDb();
        public ActionResult dfdf()
        {

            try
            {
                var list = db.Athletes.ToList();
                var year = db.Periods.Select(r => r.Year);
                RengAthleteInPeriod RaI = new RengAthleteInPeriod();
                foreach (var l in list)
                {
                    var y = 1397;
                    var DOfB = Convert.ToInt32(l.Birthdate.Substring(0, 4));
                    var Age = y - DOfB;
                    var AgeRengeId = db.AgeRenges.Where(a => Age >= a.MinRenge && Age <= a.MaxReng).Select(m => m.AgeRengeId).FirstOrDefault();
                    RaI.AgeRengeId = AgeRengeId;
                    RaI.YearOfP = y;
                    RaI.AthleteId = l.AthleteId;
                    RaI.Age = Age;
                    RaI.InsertDate = Utility.PertionDate.Today();
                    db.RengAthleteInPeriods.Add(RaI);
                    db.SaveChanges();

                }
            }
            catch (Exception)
            {

                throw;
            }

            return View();


        }
        // GET: PhRedinessHistoryItems
        public ActionResult Index()
        {
            var phRedinessHistoryItems = db.PhRedinessHistoryItems.Include(p => p.PhysicalExamHistory).Include(p => p.PhysicalReadinesitem);
            return View(phRedinessHistoryItems.ToList());
        }

        // GET: PhRedinessHistoryItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhRedinessHistoryItem phRedinessHistoryItem = db.PhRedinessHistoryItems.Find(id);
            if (phRedinessHistoryItem == null)
            {
                return HttpNotFound();
            }
            return View(phRedinessHistoryItem);
        }

        // GET: PhRedinessHistoryItems/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: PhRedinessHistoryItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int PhysicalExamHistoryId, List<PhRedinessHistoryItem> items)
        {
            if (ModelState.IsValid)
            {
                //db.PhRedinessHistoryItems.Add(phRedinessHistoryItem);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return PartialView();
        }

        // GET: PhRedinessHistoryItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhRedinessHistoryItem phRedinessHistoryItem = db.PhRedinessHistoryItems.Find(id);
            if (phRedinessHistoryItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhysicalExamHistoryId = new SelectList(db.PhysicalExamHistories, "PhysicalExamHistoryId", "TeamWorkTime", phRedinessHistoryItem.PhysicalExamHistoryId);
            ViewBag.PhReadinessItemId = new SelectList(db.PhysicalReadinesitems, "PhReadinessItemId", "PhReadinessItemTitle", phRedinessHistoryItem.PhReadinessItemId);
            return View(phRedinessHistoryItem);
        }

        // POST: PhRedinessHistoryItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhRHistoryItemId,PhysicalExamHistoryId,PhReadinessItemId,Time,Score")] PhRedinessHistoryItem phRedinessHistoryItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phRedinessHistoryItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhysicalExamHistoryId = new SelectList(db.PhysicalExamHistories, "PhysicalExamHistoryId", "TeamWorkTime", phRedinessHistoryItem.PhysicalExamHistoryId);
            ViewBag.PhReadinessItemId = new SelectList(db.PhysicalReadinesitems, "PhReadinessItemId", "PhReadinessItemTitle", phRedinessHistoryItem.PhReadinessItemId);
            return View(phRedinessHistoryItem);
        }

        // GET: PhRedinessHistoryItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhRedinessHistoryItem phRedinessHistoryItem = db.PhRedinessHistoryItems.Find(id);
            if (phRedinessHistoryItem == null)
            {
                return HttpNotFound();
            }
            return View(phRedinessHistoryItem);
        }

        // POST: PhRedinessHistoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhRedinessHistoryItem phRedinessHistoryItem = db.PhRedinessHistoryItems.Find(id);
            db.PhRedinessHistoryItems.Remove(phRedinessHistoryItem);
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
