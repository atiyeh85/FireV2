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
    [Authorize]
    public class IndividualHistoriesController : Controller
    {
        private StoreDb db = new StoreDb();
        [AllowAnonymous]
        // GET: IndividualHistories
        public ActionResult Index()
        {
            var individualHistorys = db.IndividualHistorys.Include(i => i.Athlete).Include(i => i.Period);
            return View(individualHistorys.ToList());
        }
        [AllowAnonymous]

        // GET: IndividualHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IndividualHistory individualHistory = db.IndividualHistorys.Find(id);
            if (individualHistory == null)
            {
                return HttpNotFound();
            }
            return View(individualHistory);
        }

        // GET: IndividualHistories/Create
        [AllowAnonymous]

        public ActionResult Create(int id, int atid)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            var Listofp = db.Periods.Where(p => p.PeriodId == id).FirstOrDefault();

            ViewBag.Fromdate = Listofp.FromDate;
            ViewBag.Todate = Listofp.Todate;
            ViewBag.Id = id;
            var model = db.IndividualHistorys.Include(p => p.IndivisualOperationHistoryItems).Where(p => p.AthleteId == atid && p.PeriodId == id).FirstOrDefault();
            ViewBag.ItemsByPeriod = db.IndOperationItemsByPeriods.Where(p => p.PeriodId == id).ToList();
            if (null == model)
            {
                var At = db.Athletes.Where(a => a.AthleteId == atid).FirstOrDefault();

                model = new IndividualHistory
                {
                    Athlete = At,
                    Period = db.Periods.Where(p => p.PeriodId == id).FirstOrDefault(),
                    PeriodId = id,
                    AthleteId = atid,
                    IndOperScore = 0,
                    IndOperTotalFaults = "0",
                    IndOperTotalFinalOper = "0",
                    
                    IndOperTotalTime = "0",
                };
            }
            return View(model);
        }



        // POST: IndividualHistorys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public ActionResult Create(IndividualHistory individualHistory, List<IndivisualOperationHistoryItem> items)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            using (var scope = new TransactionScope())
            {
                try
                {
                    var original = db.IndividualHistorys.Where(p => p.AthleteId == individualHistory.AthleteId && p.PeriodId == individualHistory.PeriodId).FirstOrDefault();
                    var Athlete = db.Athletes.Where(a => a.AthleteId == individualHistory.AthleteId).Select(a => a.Birthdate).FirstOrDefault();
                    var year = db.Periods.Where(p => p.PeriodId == individualHistory.PeriodId).Select(p => p.Year).FirstOrDefault();

                    var SearchRAp = db.RengAthleteInPeriods.Where(r => r.AthleteId == individualHistory.AthleteId && r.YearOfP == year).FirstOrDefault();
                    if (SearchRAp == null)
                    {
                        var agerengP = new RengAthleteInPeriod();
                        agerengP.AgeRengeId =Utility.PertionDate.GetAgeRangeId(Athlete, year);
                        agerengP.InsertDate = Utility.PertionDate.Today();
                        agerengP.UserDate = User.Identity.Name;
                        agerengP.YearOfP = year;
                        agerengP.AthleteId = individualHistory.AthleteId;
                        switch (Convert.ToString(agerengP.AgeRengeId))
                        {
                            case "1":
                                agerengP.AgeReng = "رنج سنی اول ";
                                break;
                            case "2":
                                agerengP.AgeReng = "رنج سنی دوم ";
                                break;
                            case "3":
                                agerengP.AgeReng = "رنج سنی سوم ";
                                break;
                            case "5":
                                agerengP.AgeReng = "رنج سنی چهارم ";
                                break;
                            default:
                                break;
                        }
                        db.RengAthleteInPeriods.Add(agerengP);
                        db.SaveChanges();
                    }
                    if (null == original)
                    {
                        if (ModelState.IsValid)
                        {
                            db.IndividualHistorys.Add(individualHistory);
                            foreach (var item in items)
                                individualHistory.IndivisualOperationHistoryItems.Add(item);
                            db.SaveChanges();

                            var history = db.PhysicalExamHistories.Where(s => s.AthleteId == individualHistory.AthleteId && s.PeriodId == individualHistory.PeriodId).FirstOrDefault();
                            if (null != history)
                            {
                                double old = history.IndTotalScore.HasValue ? history.IndTotalScore.Value : 0;
                                history.TotalScore += individualHistory.IndOperScore - old;
                                history.IndTotalScore = individualHistory.IndOperScore;
                                db.Entry(history).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            scope.Complete();

                            return RedirectToAction("DataEntry", "Athletes", new { id = individualHistory.PeriodId, atid = individualHistory.AthleteId });
                        }

                    }
                    else
                    {
                        original.IndOperScore = individualHistory.IndOperScore;
                        original.IndOperTotalFaults = individualHistory.IndOperTotalFaults;
                        original.IndOperTotalFinalOper = individualHistory.IndOperTotalFinalOper;
                        original.IndOperTotalTime = individualHistory.IndOperTotalTime;

                        while (original.IndivisualOperationHistoryItems.Count > 0)
                            db.IndivisualOperationHistoryItems.Remove(original.IndivisualOperationHistoryItems.FirstOrDefault());

                        foreach (var item in items)
                            original.IndivisualOperationHistoryItems.Add(item);

                        db.Entry(original).State = EntityState.Modified;
                        db.SaveChanges();

                        var history = db.PhysicalExamHistories.Where(s => s.AthleteId == individualHistory.AthleteId && s.PeriodId == individualHistory.PeriodId).FirstOrDefault();
                        if (null != history)
                        {
                            double old = history.IndTotalScore.HasValue ? history.IndTotalScore.Value : 0;
                            history.TotalScore += individualHistory.IndOperScore - old;
                            history.IndTotalScore = individualHistory.IndOperScore;
                            db.Entry(history).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        scope.Complete();

                        //return RedirectToAction("DataEntry", "Athletes", new { id = original.Period, atid = original.AthleteId });
                        return RedirectToAction("DataEntry", "Athletes", new { id = original.PeriodId, atid=original.AthleteId });
                    }

                }
                catch (Exception ex)
                {

                }
            }

            ViewBag.ItemsByPeriod = db.IndOperationItemsByPeriods.Where(p => p.PeriodId == individualHistory.PeriodId).ToList();
            return View(individualHistory);
        }
        [AllowAnonymous]

        public ActionResult GetScoreByTime(int id,string time)
        {
            var PackIndId = db.Periods.Where(p => p.PeriodId == id).FirstOrDefault();
            var model = db.IndOperationPoints.Where(p => p.PackIndId == PackIndId.PackIndid && string.Compare(p.Time, time) <= 0).OrderByDescending(p => p.Time).FirstOrDefault();
            if (null != model)
                return Content(model.Points.ToString());

            return Content("0");
        }

        // GET: IndividualHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IndividualHistory individualHistory = db.IndividualHistorys.Find(id);
            if (individualHistory == null)
            {
                return HttpNotFound();
            }
            return View(individualHistory);
        }

        // POST: IndividualHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IndividualHistory individualHistory = db.IndividualHistorys.Find(id);
            db.IndividualHistorys.Remove(individualHistory);
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
