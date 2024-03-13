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
    public class RengAthleteInPeriodsController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: RengAthleteInPeriods
        public ActionResult Index()
        {
            return View(db.RengAthleteInPeriods.ToList());
        }

        // GET: RengAthleteInPeriods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RengAthleteInPeriod rengAthleteInPeriod = db.RengAthleteInPeriods.Find(id);
            if (rengAthleteInPeriod == null)
            {
                return HttpNotFound();
            }
            return View(rengAthleteInPeriod);
        }

        // GET: RengAthleteInPeriods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RengAthleteInPeriods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( RengAthleteInPeriod rengAthleteInPeriod)
        {
            var list = db.Athletes.ToList();
            var year = db.Periods.Select(r => r.Year);
            foreach (var l in list)
            {

                var y = 1396;
                
                    var DOfB = Convert.ToInt32(l.Birthdate.Substring(0, 4));
                    var Age = y - DOfB;
                    var AgeRengeId = db.AgeRenges.Where(a => Age >= a.MinRenge && Age <= a.MaxReng).Select(m => m.AgeRengeId).FirstOrDefault();
                    rengAthleteInPeriod.AgeRengeId = AgeRengeId;
                    rengAthleteInPeriod.YearOfP = y;
                    rengAthleteInPeriod.AthleteId = l.AthleteId;
                    rengAthleteInPeriod.InsertDate = Utility.PertionDate.Today();
                    db.RengAthleteInPeriods.Add(rengAthleteInPeriod);
                db.SaveChanges();

            }


            return View();
            
        }

        // GET: RengAthleteInPeriods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RengAthleteInPeriod rengAthleteInPeriod = db.RengAthleteInPeriods.Find(id);
            if (rengAthleteInPeriod == null)
            {
                return HttpNotFound();
            }
            return View(rengAthleteInPeriod);
        }

        // POST: RengAthleteInPeriods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RangePeriodId,AgeReng,UserDate,InsertDate,AthleteId,YearOfP,AgeRengeId")] RengAthleteInPeriod rengAthleteInPeriod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rengAthleteInPeriod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rengAthleteInPeriod);
        }

        // GET: RengAthleteInPeriods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RengAthleteInPeriod rengAthleteInPeriod = db.RengAthleteInPeriods.Find(id);
            if (rengAthleteInPeriod == null)
            {
                return HttpNotFound();
            }
            return View(rengAthleteInPeriod);
        }

        // POST: RengAthleteInPeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RengAthleteInPeriod rengAthleteInPeriod = db.RengAthleteInPeriods.Find(id);
            db.RengAthleteInPeriods.Remove(rengAthleteInPeriod);
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
