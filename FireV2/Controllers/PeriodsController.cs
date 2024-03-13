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
    public class PeriodsController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: Periods
        public ActionResult Index()
        {
            var periods = db.Periods.Include(p => p.PackPackInd).Include(p => p.PackPointTeam);
            return View(periods.ToList());
        }

        // GET: Periods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Period period = db.Periods.Find(id);
            if (period == null)
            {
                return HttpNotFound();
            }
            return View(period);
        }

        // GET: Periods/Create
        public ActionResult Create()
        {
            ViewBag.PackIndid = new SelectList(db.PackPackInds, "PackIndid", "PackIndTItle");
            ViewBag.PackTeamid = new SelectList(db.PackPointTeams, "PackTeamid", "PackTeamTItle");
            return View();
        }

        // POST: Periods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Period period,string date2,string date1)
        {

            period.Year =Convert.ToInt32( ConvertNumbersToEnglish(period.FromDate.Substring(0, 4)));
            period.FromDate = ConvertNumbersToEnglish(period.FromDate);
            period.Todate = ConvertNumbersToEnglish(period.Todate);
            period.UserInsert = User.Identity.Name;
            period.DateInsert = Utility.PertionDate.Today();
            if (ModelState.IsValid)
            {
                db.Periods.Add(period);
                db.SaveChanges();
                return RedirectToAction("create");
            }

            ViewBag.PackIndid = new SelectList(db.PackPackInds, "PackIndid", "PackIndTItle", period.PackIndid);
            ViewBag.PackTeamid = new SelectList(db.PackPointTeams, "PackTeamid", "PackTeamTItle", period.PackTeamid);
            return View(period);
        }
        public static string ConvertNumbersToEnglish(string str)
        {
            return str.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
        }
        // GET: Periods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Period period = db.Periods.Find(id);
            if (period == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackIndid = new SelectList(db.PackPackInds, "PackIndid", "PackIndTItle", period.PackIndid);
            ViewBag.PackTeamid = new SelectList(db.PackPointTeams, "PackTeamid", "PackTeamTItle", period.PackTeamid);
            return View(period);
        }

        // POST: Periods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Period period)
        {
            var per = db.Periods.Where(a => a.PeriodId == period.PeriodId).FirstOrDefault();
            per.Year = Convert.ToInt32(ConvertNumbersToEnglish(period.FromDate.Substring(0, 4)));
            per.FromDate = ConvertNumbersToEnglish(period.FromDate);
            per.Todate = ConvertNumbersToEnglish(period.Todate);
            per.UserInsert = User.Identity.Name;
            per.DateInsert = Utility.PertionDate.Today();
            
            if (ModelState.IsValid)
            {
                db.Entry(per).State = EntityState.Modified;
                db.SaveChanges();
                return Content("true");
            }
            ViewBag.PackIndid = new SelectList(db.PackPackInds, "PackIndid", "PackIndTItle", period.PackIndid);
            ViewBag.PackTeamid = new SelectList(db.PackPointTeams, "PackTeamid", "PackTeamTItle", period.PackTeamid);
            return View(per);
        }

        // GET: Periods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Period period = db.Periods.Find(id);
            if (period == null)
            {
                return HttpNotFound();
            }
            return View(period);
        }

        // POST: Periods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Period period = db.Periods.Find(id);
            db.Periods.Remove(period);
            db.SaveChanges();
           return Content("true");

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
