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
    public class BMIsController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: /BMIs/
        public ActionResult Index(int? id)
        {
            var bmis = db.BMIS.Include(b => b.Athlete).Where(b=>b.AthleteId==id).OrderByDescending(b=>b.ABmiid);

            return View(bmis.ToList());
        }

        // GET: /BMIs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            BMI bmi = db.BMIS.Find(id);
            if (bmi == null)
            {
                return HttpNotFound();
            }
            return View(bmi);
        }

        // GET: /BMIs/Create
        public ActionResult Create(int? id)
        {
            ViewBag.AthleteId = id;
            var Athlete = db.Athletes.Where(a => a.AthleteId == id).FirstOrDefault();
            ViewBag.FullName = Athlete.FullName;
            {
                if (Athlete == null)
                {
                    return HttpNotFound();

                }
                ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle");

                return View();
            }
        }
        public double CalculateBMI(double Size, double Weight)
        {
            double _BmiDec = Weight / (Size * Size )* 100;
           //var _bmi = Math.(_BmiDec, 2);
//              bmi = Math.pow(weight / height, 2)
//return Math.round(bmi, 2)
           return _BmiDec*100;
        }
        // POST: /BMIs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( BMI bmi)
        {
            var Athlete=db.Athletes.Where(a=>a.AthleteId==bmi.AthleteId).FirstOrDefault();
            if (Athlete!=null)
	{
		 var Size=bmi.Size;
              var Weight=bmi.Weight;
                var CalculateBmi=CalculateBMI(Size,Weight);
                bmi.BMIRenj =Convert.ToString( Math.Round(CalculateBmi,1));
                if (CalculateBmi<=18.5)
                {
                    
                    ViewBag.Message = "ورزشکار عزیز ، با توجه به وزن و قد شما در رنج لاغر قرار گرفته اید!";
                    bmi.ABmiText = ViewBag.Message;
                    TempData["Message"] = "ورزشکار عزیز ، با توجه به وزن و قد شما در رنج لاغر قرار گرفته اید!";
                }
                else if (CalculateBmi>18.5 &&CalculateBmi<=24.9)
                {
                    ViewBag.Message = "ورزشکار عزیز، با توجه به قد و وزن شما در رنج طبیعی قرار گرفته اید! ";
                    bmi.ABmiText = ViewBag.Message;
                    TempData["Message"] = "ورزشکار عزیز، با توجه به قد و وزن شما در رنج طبیعی قرار گرفته اید! ";
                }
                else if (CalculateBmi >= 25 && CalculateBmi <= 29.9)
                {
                    ViewBag.Message = " ورزشکار عزیز، با توجه به قد و وزن شما در رنج اضافه وزن قرار گرفته اید! ";
                    bmi.ABmiText = ViewBag.Message;
                    TempData["Message"] = " ورزشکار عزیز، با توجه به قد و وزن شما در رنج اضافه وزن قرار گرفته اید! ";
                }
                else if (CalculateBmi >= 30 && CalculateBmi <= 34.5)
                {
                    ViewBag.Message = "ورزشکار عزیز، با توجه به قد و وزن شما در رنج چاقی درجه 1 قرار گرفته اید! ";
                    bmi.ABmiText = ViewBag.Message;
                    TempData["Message"] = "ورزشکار عزیز، با توجه به قد و وزن شما در رنج چاقی درجه 1 قرار گرفته اید! ";
                }
                else if (CalculateBmi >= 35 && CalculateBmi <= 39.9)
                {
                    ViewBag.Message = "ورزشکار عزیز، با توجه به قد و وزن شما در رنج چاقی درجه 2 قرار گرفته اید! ";
                    bmi.ABmiText = ViewBag.Message;
                    TempData["Message"] = "ورزشکار عزیز، با توجه به قد و وزن شما در رنج چاقی درجه 2 قرار گرفته اید! ";

                }
                else if (CalculateBmi >= 40 )
                {
                    ViewBag.Message = "ورزشکار عزیز، با توجه به قد و وزن شما در رنج چاقی درجه 3 قرار گرفته اید! ";
                    TempData["Message"] = "ورزشکار عزیز، با توجه به قد و وزن شما در رنج چاقی درجه 3 قرار گرفته اید! ";
                    bmi.ABmiText = ViewBag.Message;
                }
	}
            if (ModelState.IsValid)
            {
                db.BMIS.Add(bmi);
                db.SaveChanges();
                return RedirectToAction("create", "BMIs", new { id = bmi.AthleteId });
            }
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle");
            ViewBag.AthleteId = new SelectList(db.Athletes, "AthleteId", "Fnam", bmi.AthleteId);
            return View(bmi);
        }

        // GET: /BMIs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BMI bmi = db.BMIS.Find(id);
            if (bmi == null)
            {
                return HttpNotFound();
            }
            ViewBag.AthleteId = new SelectList(db.Athletes, "AthleteId", "Fnam", bmi.AthleteId);
            return View(bmi);
        }

        // POST: /BMIs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ABmiid,ABmiText,Date,Size,Weight,BMIRenj,AthleteId")] BMI bmi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bmi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AthleteId = new SelectList(db.Athletes, "AthleteId", "Fnam", bmi.AthleteId);
            return View(bmi);
        }

        // GET: /BMIs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BMI bmi = db.BMIS.Find(id);
            if (bmi == null)
            {
                return HttpNotFound();
            }
            return View(bmi);
        }

        // POST: /BMIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BMI bmi = db.BMIS.Find(id);
            var athlete = bmi.AthleteId;
            db.BMIS.Remove(bmi);
            db.SaveChanges();
            return RedirectToAction("create", "BMIs", new { id = athlete });

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
