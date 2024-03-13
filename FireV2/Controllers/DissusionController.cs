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
    public class DissusionController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: /Dissusion/
        public ActionResult Index()
        {
            var dissusions = db.Dissusions.Include(d => d.Athlete).Include(d => d.Period);
            return View(dissusions.ToList());
        }
        public ActionResult vitiation(int disid,int athid,int id,Dissusion Dissusion)
        {
            var dissusions = db.Dissusions.Where(d=>d.DissPerId== disid).Where(d=>d.Isactive==true).FirstOrDefault();
            if (dissusions!=null)
            {
                dissusions.Isactive = false;
                db.Entry(dissusions).State = EntityState.Modified;

            }
            var model = new Dissusion()
            {
                AthleteId = athid,
                 InsertUser=User.Identity.Name,
                  InsetDate=Utility.PertionDate.Today(),
                   InsetTIme=DateTime.Now.ToShortTimeString(),
                    PeriodId=id,
                     Isactive=true,
                     Note=Dissusion.Note,

            };
            return View();
        }
        // GET: /Dissusion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dissusion dissusion = db.Dissusions.Find(id);
            if (dissusion == null)
            {
                return HttpNotFound();
            }
            return View(dissusion);
        }
        [AllowAnonymous]
        public ActionResult GetDisTitle(int id,int? pid)
        {
            ViewBag.List = db.Dissusions.Where(d => d.AthleteId == id && d.PeriodId==pid).Select(p => p.DissuasionsTitle.DissType).Distinct().ToList();
            var model = db.Dissusions.Where(d => d.AthleteId == id && d.PeriodId == pid).Include(p => p.DissuasionsTitle).ToList();
            ViewBag.li = db.DissTypes.ToList();
            return PartialView(model);
        }
        [AllowAnonymous]
        public ActionResult GetFullDisTitle(int id, int? pid)
        {
            ViewBag.List = db.Dissusions.Where(d => d.AthleteId == id ).Select(p => p.DissuasionsTitle.DissType).Distinct().ToList();
            var model = db.Dissusions.Where(d => d.AthleteId == id).Include(p => p.DissuasionsTitle).ToList();
            ViewBag.pr = db.Dissusions.Where(d => d.AthleteId == id).Select(p=>p.PeriodId).Distinct().ToList();
            ViewBag.li = db.DissTypes.ToList();
            ViewBag.period = db.Periods.ToList();
            return PartialView(model);
        }
        [HttpPost]
        [AllowAnonymous]

        public ActionResult DeleteDicTitle(int id)
        {
            try
            {
                var model = db.Dissusions.Find(id);
                if (null != model)
                {
                    db.Dissusions.Remove(model);
                    db.SaveChanges();
                    return Content("true");
                }
            }
            catch (Exception ex)
            {

            }
            return Content("false");
        }
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
        public ActionResult Create(Period period)
        {
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
        //public ActionResult AddDisTitle(int? id)
        //{
        //    ViewBag.DissTypeid = new SelectList(db.DissTypes, "DissTypeid", "DissTypeTitle");
        //    ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle");
        //    TempData["atid"] = id;
        //    //TempData["id"] = id;
        //    return PartialView();
        //}
        public ActionResult AddDisTitle( int? id)
        {
            ViewBag.DissTypeid = new SelectList(db.DissTypes, "DissTypeid", "DissTypeTitle");
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle");
            TempData["atid"] = id;
            ViewBag.FullName = db.Athletes.Where(a => a.AthleteId == id).Select(a=>a.Fnam +" "+a.Lname).FirstOrDefault();
            //TempData["id"] = id;
            return View();
        }
        [HttpPost]
        public ActionResult AddDisTitle(int AthleteId, int PeriodId, Dissusion[] details)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in details)
                    {
                        Dissusion model = new Dissusion();
                        model.AthleteId = AthleteId;
                        model.PeriodId = PeriodId;
                        model.DissTitleId = item.DissTitleId;
                        model.Isactive = true;
                        model.InsertUser = User.Identity.Name;
                        model.InsetDate = Utility.PertionDate.Today();
                        model.InsetTIme = DateTime.Now.ToShortTimeString();
                        var Isch = db.Dissusions.Where(d => d.AthleteId == AthleteId && d.PeriodId == PeriodId && d.DissTitleId == item.DissTitleId);
                        db.Dissusions.Add(model);
                        db.SaveChanges();
                    }
                    return Content("true");
                }
                return Content("false");
            }
            catch (Exception)
            {

                throw;
            }

           
        }
        public ActionResult InfoDisTitleTypes(int id)
        {
            return PartialView(db.DissuasionsTitles.Where(c => c.DissTypeid == id).ToList());
        }
        // GET: /Dissusion/Create


        // POST: /Dissusion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DissPerId,PeriodId,AthleteId,Note,Diss")] Dissusion dissusion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dissusion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AthleteId = new SelectList(db.Athletes, "AthleteId", "Fnam", dissusion.AthleteId);
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle", dissusion.PeriodId);
            return View(dissusion);
        }

        // GET: /Dissusion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dissusion dissusion = db.Dissusions.Find(id);
            if (dissusion == null)
            {
                return HttpNotFound();
            }
            return View(dissusion);
        }

        // POST: /Dissusion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dissusion dissusion = db.Dissusions.Find(id);
            db.Dissusions.Remove(dissusion);
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
