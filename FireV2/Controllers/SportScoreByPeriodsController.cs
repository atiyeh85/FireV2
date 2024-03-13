using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FireV2.Models;
using System.Transactions;

namespace FireV2.Controllers
{
    public class SportScoreByPeriodsController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: SportScoreByPeriods
        public ActionResult Index()
        {
            var sportScoreByPeriods = db.SportScoreByPeriods.Include(s => s.Period).Include(s => s.SportsScoreTitle);
            return View(sportScoreByPeriods.ToList());
        }

        // GET: SportScoreByPeriods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SportScoreByPeriod sportScoreByPeriod = db.SportScoreByPeriods.Find(id);
            if (sportScoreByPeriod == null)
            {
                return HttpNotFound();
            }
            return View(sportScoreByPeriod);
        }

        // GET: SportScoreByPeriods/Create
        public ActionResult Create(int id, int atid)
        {
            var Listofp = db.Periods.Where(p => p.PeriodId == id).FirstOrDefault();
            ViewBag.Fromdate = Listofp.FromDate;
            ViewBag.Todate = Listofp.Todate;
            var ath = db.Athletes.Where(a => a.AthleteId == atid).FirstOrDefault();
            var pid = db.Periods.Where(a => a.PeriodId == id).FirstOrDefault();
            var selected = db.SportScoreByPeriods.Where(p => p.AthleteId == atid && p.PeriodId == id).Select(p => p.SScoreId).ToList();
            ViewBag.AthleteId = atid;
            ViewBag.FullName = ath.FullName;
            ViewBag.SeasonTitle = pid.SeasonTitle;
            ViewBag.PeriodId = id;
            ViewBag.Selected = selected;
            var model = db.SportsScoreTitles.ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int PeriodId, int AthleteId, string items)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var original = db.SportScoreByPeriods.Where(p => p.AthleteId == AthleteId
                    && p.PeriodId == PeriodId).ToList();
                    while (original.Count > 0)
                    {
                        db.SportScoreByPeriods.Remove(original.First());
                        original.Remove(original.First());
                        db.SaveChanges();
                    }
                    int SportScoreValue = 0;
                    foreach (var item in items.Split(';'))
                    {
                        if (string.IsNullOrWhiteSpace(item))
                            continue;
                        int id = int.Parse(item);

                        db.SportScoreByPeriods.Add(new SportScoreByPeriod() 
                        { AthleteId = AthleteId, PeriodId = PeriodId, SScoreId = id });
                        SportScoreValue += int.Parse(System.Configuration.ConfigurationManager.AppSettings["SportScoreValue"]);
                    }

                    var history = db.PhysicalExamHistories.Where(s => s.AthleteId == AthleteId 
                    && s.PeriodId == PeriodId).FirstOrDefault();
                    if (null != history)
                    {
                        double old = history.SportsScore.HasValue ? history.SportsScore.Value : 0;
                        history.TotalScore += SportScoreValue - old;
                        history.SportsScore = SportScoreValue;
                        db.Entry(history).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    scope.Complete();
                    return RedirectToAction("DataEntry", "Athletes", new { id = PeriodId, atid = AthleteId });

                }
                catch (Exception ex)
                {
                }
            }
            //ViewBag.ItemsByPeriod = db.PhReadinessItemByPeriods.Where(p => p.PeriodId == physicalExamHistory.PeriodId).ToList();
            return View();
        }


        // POST: SportScoreByPeriods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "SScoreBypriodId,SScoreId,PeriodId")] SportScoreByPeriod sportScoreByPeriod)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SportScoreByPeriods.Add(sportScoreByPeriod);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "Year", sportScoreByPeriod.PeriodId);
        //    ViewBag.SScoreId = new SelectList(db.SportsScoreTitles, "SScoreId", "SsScoreTitle", sportScoreByPeriod.SScoreId);
        //    return View(sportScoreByPeriod);
        //}

        // GET: SportScoreByPeriods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SportScoreByPeriod sportScoreByPeriod = db.SportScoreByPeriods.Find(id);
            if (sportScoreByPeriod == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "Year", sportScoreByPeriod.PeriodId);
            ViewBag.SScoreId = new SelectList(db.SportsScoreTitles, "SScoreId", "SsScoreTitle", sportScoreByPeriod.SScoreId);
            return View(sportScoreByPeriod);
        }

        // POST: SportScoreByPeriods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SScoreBypriodId,SScoreId,PeriodId")] SportScoreByPeriod sportScoreByPeriod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sportScoreByPeriod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "Year", sportScoreByPeriod.PeriodId);
            ViewBag.SScoreId = new SelectList(db.SportsScoreTitles, "SScoreId", "SsScoreTitle", sportScoreByPeriod.SScoreId);
            return View(sportScoreByPeriod);
        }

        // GET: SportScoreByPeriods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SportScoreByPeriod sportScoreByPeriod = db.SportScoreByPeriods.Find(id);
            if (sportScoreByPeriod == null)
            {
                return HttpNotFound();
            }
            return View(sportScoreByPeriod);
        }

        // POST: SportScoreByPeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SportScoreByPeriod sportScoreByPeriod = db.SportScoreByPeriods.Find(id);
            db.SportScoreByPeriods.Remove(sportScoreByPeriod);
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
