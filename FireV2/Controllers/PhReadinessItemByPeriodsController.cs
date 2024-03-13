using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FireV2.Models;
using Microsoft.Ajax.Utilities;

namespace FireV2.Controllers
{
    public class PhReadinessItemByPeriodsController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: PhReadinessItemByPeriods
        public ActionResult Index()
        {
            var phReadinessItemByPeriods = db.PhReadinessItemByPeriods.Include(p => p.PhysicalReadinesitem);
            return View(phReadinessItemByPeriods.ToList());
        }

        // GET: PhReadinessItemByPeriods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhReadinessItemByPeriod phReadinessItemByPeriod = db.PhReadinessItemByPeriods.Find(id);
            if (phReadinessItemByPeriod == null)
            {
                return HttpNotFound();
            }
            return View(phReadinessItemByPeriod);
        }

        // GET: PhReadinessItemByPeriods/Create
        public ActionResult Create()
        {
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle");
            ViewBag.items = new SelectList(db.PhysicalReadinesitems, "PhReadinessItemId", "PhReadinessItemTitle");
            return View();
        }

        // POST: PhReadinessItemByPeriods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int Periodid, PhReadinessItemByPeriod phReadinessItemByPeriod, int[] items=null)
        {
            if (ModelState.IsValid)
            {
                if (items!=null)
                {
                    foreach (var item in items)
                    {
                        phReadinessItemByPeriod.PeriodId = Periodid;
                        phReadinessItemByPeriod.PhReadinessItemId = Periodid;
                        db.PhReadinessItemByPeriods.Add(phReadinessItemByPeriod);
                        db.SaveChanges();
                    }
                   
                }
                
                return RedirectToAction("Index");
            }

            ViewBag.PhReadinessItemId = new SelectList(db.PhysicalReadinesitems, "PhReadinessItemId", "PhReadinessItemTitle", phReadinessItemByPeriod.PhReadinessItemId);
            return View(phReadinessItemByPeriod);
        }
        [AllowAnonymous]
        public ActionResult GetItems(int PeriodId)
        {
            ViewBag.items= db.PhysicalReadinesitems.ToList();
            ViewBag.selected = db.PhReadinessItemByPeriods.Where(p => p.PeriodId == PeriodId).Select(s=>s.PhReadinessItemId).ToList();
            return PartialView();
        }
        // GET: PhReadinessItemByPeriods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhReadinessItemByPeriod phReadinessItemByPeriod = db.PhReadinessItemByPeriods.Find(id);
            if (phReadinessItemByPeriod == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhReadinessItemId = new SelectList(db.PhysicalReadinesitems, "PhReadinessItemId", "PhReadinessItemTitle", phReadinessItemByPeriod.PhReadinessItemId);
            return View(phReadinessItemByPeriod);
        }

        // POST: PhReadinessItemByPeriods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhReadinessItemId,PeriodId")] PhReadinessItemByPeriod phReadinessItemByPeriod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phReadinessItemByPeriod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhReadinessItemId = new SelectList(db.PhysicalReadinesitems, "PhReadinessItemId", "PhReadinessItemTitle", phReadinessItemByPeriod.PhReadinessItemId);
            return View(phReadinessItemByPeriod);
        }

        // GET: PhReadinessItemByPeriods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhReadinessItemByPeriod phReadinessItemByPeriod = db.PhReadinessItemByPeriods.Find(id);
            if (phReadinessItemByPeriod == null)
            {
                return HttpNotFound();
            }
            return View(phReadinessItemByPeriod);
        }

        // POST: PhReadinessItemByPeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhReadinessItemByPeriod phReadinessItemByPeriod = db.PhReadinessItemByPeriods.Find(id);
            db.PhReadinessItemByPeriods.Remove(phReadinessItemByPeriod);
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
