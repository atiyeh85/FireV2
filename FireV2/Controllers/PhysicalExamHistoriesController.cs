using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FireV2.Models;
using FireV2.ViewModels;
using System.Transactions;
using PagedList;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System.Collections;
using System.Web.UI.WebControls;
using Stimulsoft.Base;
using static System.Net.Mime.MediaTypeNames;

namespace FireV2.Controllers
{

    [Authorize]
    public class PhysicalExamHistoriesController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: PhysicalExamHistories/Details/5
        [AllowAnonymous]
        public ActionResult Report(List<int> ids, List<int> idss)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            GridView gv = new GridView();
            var PhHistories = db.PhysicalExamHistories.Include(p => p.Athlete).Include(p => p.Period).Include(p => p.Athlete.PostSazmani).ToList();
            var phlist = PhHistories.Select(s => new PhysicalHistoryReportVM()
            {
                AthleteId = s.AthleteId,
                PhysicalExamHistoryId = s.PhysicalExamHistoryId,
                DissPerId = 0,
                Birthdate = s.Athlete.Birthdate,
                Age = Convert.ToString(s.AgeReng),
                StatusScore = s.StatusScore,
                Picture = s.Athlete.Picture,
                AthleteName = s.Athlete.Fnam + " " + s.Athlete.Lname,
                FatherName = s.Athlete.FatherName,
                PeriodTitle = s.Period.SeasonTitle,
                PSazmaniTitle = s.Athlete.PostSazmani.PSazmaniTitle,
                SportsScore = s.SportsScore,
                TotalPhysicalScore = s.TotalPhysicalScore,
                IndTotalScore = s.IndTotalScore,
                TeamWorkFaltTime = s.TeamWorkFaltTime,
                TotalTimeWorkTime = s.TotalTimeWorkTime,
                TeamWorkScore = s.TeamWorkScore,
                IndOperTotalFinalOper = s.IndOperTotalFinalOper,
                TotalScore = s.TotalScore,

                Titles = s.Athlete.Dissusions.Where(d => d.PeriodId == s.PeriodId).Select(d => d.DissuasionsTitle.DissTitles).ToList(),
                Note = string.Join("", s.Athlete.Dissusions.Where(d => d.PeriodId == s.PeriodId).Select(d => d.DissuasionsTitle.DissTitles).ToList())
            });

            ArrayList myAL = new ArrayList();
            if (ids != null)
            {
                foreach (int id in ids)
                {
                    if (id != 0)
                    {
                        var list = phlist.Where(v => v.PhysicalExamHistoryId == id).FirstOrDefault();
                        myAL.Add(list);

                    }

                }
            }

            if (idss != null)
            {
                foreach (var item in idss)
                {
                    if (item != 0)
                    {
                        var list = phlist.Where(v => v.DissPerId == item).FirstOrDefault();
                        myAL.Add(list);
                    }
                }
            }

            TempData["List"] = myAL;
            Session["List"] = myAL;
            return View();
        }


        [AllowAnonymous]
        public ActionResult Report_perAth(int id)
        {

            var list = db.PhysicalExamHistories.Where(a => a.AthleteId == id).ToList();
            ViewBag.period = list.Select(s => s.Period.SeasonTitle).ToList();
            return View();
        }
        [AllowAnonymous]
        public ActionResult GetAutoCompleteFnam(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            { term = ""; }
            var list = db.Athletes.Where(a => a.Flag == true && a.IsArchive == false).Select(a => a.Fnam).Distinct().ToList();
            string[] AutoCompleteOptions = new string[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                AutoCompleteOptions[i] = list[i];
            }

            return this.Json(AutoCompleteOptions.Where(t => t.StartsWith(term, StringComparison.InvariantCultureIgnoreCase)),
                            JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]

        public ActionResult GetAutoCompleteLname(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            { term = ""; }
            var list = db.Athletes.Where(a => a.Flag == true && a.IsArchive == false).Select(a => a.Lname).Distinct().ToList();
            string[] AutoCompleteOptions = new string[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                AutoCompleteOptions[i] = list[i];
            }

            return this.Json(AutoCompleteOptions.Where(t => t.StartsWith(term, StringComparison.InvariantCultureIgnoreCase)),
                            JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetStudents(string sidx, string sort, int page, int rows, bool _search, string searchField,
         string searchOper, string searchString)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var physicalExamHistories = db.PhysicalExamHistories.Include(p => p.Athlete).Include(p => p.Period).Include(p => p.Period.Dissusions).OrderBy(p => p.Athlete.Birthdate).Where(p => p.Athlete.IsArchive == false)
                .AsQueryable();
            var PhList = db.PhysicalExamHistories.Select(
                p => new PhysicalHistoryReportVM()
                {
                    PhysicalExamHistoryId = p.PhysicalExamHistoryId,
                    AthleteName = p.Athlete.Fnam + " " + p.Athlete.Lname,
                    IndTotalScore = p.IndTotalScore,

                    TotalScore = p.TotalScore,
                });

            if (_search)
            {
                switch (searchField)
                {
                    case "Fnam":
                        PhList = PhList.Where(p => p.AthleteName.Contains(searchString));
                        break;
                    case "Lname":
                        PhList = PhList.Where(t => t.AthleteName.Contains(searchString));
                        break;

                }
            }
            int totalRecords = PhList.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                PhList = PhList.OrderByDescending(p => p.AthleteName);
                PhList = PhList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                PhList = PhList.OrderBy(t => t.AthleteName);
                PhList = PhList.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = PhList
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListOfAthlet()
        {
            List<PhysicalHistoryReportVM> phvm = new List<PhysicalHistoryReportVM>();
            var physicalExamHistories = db.PhysicalExamHistories.Include(p => p.Athlete).Include(p => p.Period).Include(p => p.Period.Dissusions).Where(p => p.Athlete.IsArchive == false).ToList()
              ;
            foreach (var item in physicalExamHistories)

            {

                PhysicalHistoryReportVM objcvm = new PhysicalHistoryReportVM(); // ViewModel

                objcvm.AthleteName = item.Athlete.FullName;

                objcvm.TotalPhysicalScore = item.TotalPhysicalScore;
                objcvm.PSazmaniTitle = item.Athlete.PostSazmani.PSazmaniTitle;



                phvm.Add(objcvm);
            }
            return View(phvm);
        }
        [AllowAnonymous]

        public ActionResult Index(int? PeriodId, int? PostSazmaniId, int? StationId, int? ShiftWorkId,
            int? page, string Fname = "", string Lname = "", int[] items = null, int[] Rengs = null)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            ArrayList list = new ArrayList();
            if (page.HasValue && page < 1)
                return null;
            var physicalExamHistories = db.PhysicalExamHistories.Include(p => p.Athlete).Include(p => p.Period).
                Include(p => p.Period.Dissusions).Where(p => p.Athlete.IsArchive == false).AsQueryable();

            List<PhysicalHistoryReportVM> phvm = new List<PhysicalHistoryReportVM>();
            if (PeriodId.HasValue && PeriodId.Value != 0)
            {
                ViewBag.PeriodId = PeriodId;
                physicalExamHistories = physicalExamHistories.Where(p => p.PeriodId == PeriodId);
            }
            var Yearofp = db.Periods.Where(p => p.PeriodId == PeriodId).Select(p => p.Year).FirstOrDefault();

            if (Rengs != null)
            {

                physicalExamHistories = physicalExamHistories.Where(d => Rengs.Contains(d.Reng));
            }
            if (items != null)
            {
                physicalExamHistories = physicalExamHistories.Where(p => items.Contains(p.Athlete.PostSazmaniId));
            }

            if (!string.IsNullOrEmpty(Fname))
            {
                ViewBag.Fname = Fname;
                physicalExamHistories = physicalExamHistories.Where(p => p.Athlete.Fnam.Contains(Fname));
            }

            if (!string.IsNullOrEmpty(Lname))
            {
                ViewBag.Lname = Lname;
                physicalExamHistories = physicalExamHistories.Where(p => p.Athlete.Lname.Contains(Lname));
            }
            if (StationId.HasValue && StationId.Value != 0)
            {
                ViewBag.StationId = StationId;
                physicalExamHistories = physicalExamHistories.Where(p => p.Athlete.ShiftWork.StationId == StationId);
            }
            if (ShiftWorkId.HasValue && ShiftWorkId.Value != 0)
            {
                ViewBag.ShiftWorkId = ShiftWorkId;
                physicalExamHistories = physicalExamHistories.Where(p => p.Athlete.ShiftWorkId == ShiftWorkId);
            }

            if (PostSazmaniId.HasValue && PostSazmaniId.Value != 0)
            {
                ViewBag.PostSazmaniId = PostSazmaniId;
                physicalExamHistories = physicalExamHistories.Where(p => p.Athlete.PostSazmaniId == PostSazmaniId);
            }
            foreach (var item in physicalExamHistories)
            {
                PhysicalHistoryReportVM objcvm = new PhysicalHistoryReportVM(); // ViewModel
                objcvm.AthleteName = item.Athlete.FullName;
                objcvm.TotalPhysicalScore = item.TotalPhysicalScore;
                objcvm.PSazmaniTitle = item.Athlete.PostSazmani.PSazmaniTitle;
                objcvm.PhysicalExamHistoryId = item.PhysicalExamHistoryId;
                objcvm.AthleteId = item.AthleteId;
                objcvm.DissPerId = 0;
                objcvm.Reng = item.Reng;
                objcvm.Birthdate = item.Athlete.Birthdate;
                objcvm.Picture = item.Athlete.Picture;
                objcvm.AthleteName = item.Athlete.Fnam + " " + item.Athlete.Lname;
                objcvm.IndOperTotalFinalOper = item.IndOperTotalFinalOper;
                objcvm.IndTotalScore = item.IndTotalScore;
                objcvm.PeriodId = item.PeriodId;
                objcvm.Age = Convert.ToString(item.AgeReng);
                objcvm.PeriodTitle = item.Period.SeasonTitle;
                objcvm.SportsScore = item.SportsScore;
                objcvm.TeamWorkFaltTime = item.TeamWorkFaltTime;
                objcvm.TeamWorkScore = item.TeamWorkScore;
                objcvm.TeamWorkTime = item.TeamWorkTime;
                objcvm.TotalPhysicalScore = item.TotalPhysicalScore;
                objcvm.TotalScore = item.TotalScore;
                objcvm.TotalTimeWorkTime = item.TotalTimeWorkTime;
                objcvm.PSazmaniTitle = item.Athlete.PostSazmani.PSazmaniTitle;
                objcvm.Titles = item.Athlete.Dissusions.Where(s => s.PeriodId == item.PeriodId).Select(s => s.DissuasionsTitle.DissTitles).ToList();
                phvm.Add(objcvm);
            }
            phvm = phvm.OrderByDescending(p => p.PeriodId).ThenByDescending(p => p.TotalScore).ToList();
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle", PeriodId);
            if (items != null)
            {
                ViewBag.items = items.ToList();
            }

            if (Rengs != null)
            {
                ViewBag.Rengs = Rengs.ToList();
            }
            ViewBag.PostSazmaniId = new SelectList(db.PostSazmanis, "PostSazmaniId", "PSazmaniTitle", PostSazmaniId);
            ViewBag.StationId = new SelectList(db.Stations, "StationId", "StationTitle", StationId);
            ViewBag.ShiftWorkId = new SelectList(db.ShiftWorks.Where(s => s.StationId == StationId), "ShiftWorkId", "ShWorkTitle", ShiftWorkId);

            ViewBag.PostSazmani = db.PostSazmanis.ToList();
            ViewBag.Reng = db.AgeRenges.ToList();
            ViewBag.ListPhysic = db.PhRedinessHistoryItems.ToList();
            return View(phvm);
        }
        [AllowAnonymous]

        public ActionResult Details(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhysicalExamHistory physicalExamHistory = db.PhysicalExamHistories.Find(id);
            if (physicalExamHistory == null)
            {
                return HttpNotFound();
            }
            return View(physicalExamHistory);
        }
        [AllowAnonymous]

        public ActionResult AgeRengeAthlete(int? id)
        {

            return View();
        }
        [HttpPost]
        [AllowAnonymous]

        public ActionResult AgeRengeAthlete()
        {

            var at = db.Athletes.ToList();
            RengAthleteInPeriod ara = new RengAthleteInPeriod();
            foreach (var item in at)
            {

                ara.AthleteId = item.AthleteId;
                var Listyear = db.Periods.Select(s => s.Year).Distinct().ToList();
                foreach (var y in Listyear)
                {
                    var AR = Convert.ToString(Utility.PertionDate.GetAgeRangeId(item.Birthdate, y));
                    var SearchRAp = db.RengAthleteInPeriods.Where(r => r.AthleteId == item.AthleteId && r.YearOfP == y).FirstOrDefault();
                    ara.InsertDate = Utility.PertionDate.Today();
                    ara.UserDate = User.Identity.Name;
                    ara.YearOfP = y;

                    if (SearchRAp == null)
                    {
                        if (AR == "1")
                        {
                            ara.AgeReng = "رنج سنی اول ";
                            ara.AgeRengeId = 1;


                        }
                        else if (AR == "2")
                        {
                            ara.AthleteId = item.AthleteId;
                            ara.AgeReng = "رنج سنی دوم ";
                        }
                        else if (AR == "3")
                        {
                            ara.AgeReng = "رنج سنی سوم  ";
                            ara.AgeRengeId = 3;
                        }
                        else if (AR == "5")
                        {
                            ara.AgeReng = "رنج سنی چهارم  ";
                            ara.AgeRengeId = 5;
                        }
                        db.RengAthleteInPeriods.Add(ara);
                        db.SaveChanges();
                    }
                }
            }
            return View();

        }
        [AllowAnonymous]

        // GET: PhysicalExamHistories/Create
        public ActionResult Create(int id, int atid)
        {

            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            ViewBag.PhDis = false;
            var Diss = db.Dissusions.Where(p => p.AthleteId == atid && p.PeriodId == id).ToList();
            foreach (var item in Diss)
            {
                if (item.DissuasionsTitle.DissTypeid == 3)
                {
                    ViewBag.PhDis = true;
                }

            }
            var Listofp = db.Periods.Where(p => p.PeriodId == id).FirstOrDefault();
            var date = Convert.ToDateTime(Listofp.FromDate);
            ViewBag.Id = id;
            ViewBag.Fromdate = Listofp.FromDate;
            ViewBag.Todate = Listofp.Todate;

            if (!Utility.UAC.Validate(User, "CanPhysicalExamHistoriesCreate"))
                return View("Sorry");
            var model = db.PhysicalExamHistories.Include(p => p.PhRedinessHistoryItems).Include(p => p.Athlete).Where(p => p.AthleteId == atid && p.PeriodId == id).FirstOrDefault();
            ViewBag.ItemsByPeriod = db.PhReadinessItemByPeriods.Where(p => p.PeriodId == id).ToList();
            var hh = System.Configuration.ConfigurationManager.AppSettings["SportScoreValue"];
            int sport = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SportScoreValue"]) * db.SportScoreByPeriods.Count(s => s.AthleteId == atid && s.PeriodId == id);
            var IndividualHistory = db.IndividualHistorys.Where(i => i.AthleteId == atid && i.PeriodId == id).FirstOrDefault();
            var Athlete = db.Athletes.Where(a => a.Flag == true && a.AthleteId == atid).FirstOrDefault();

            if (IndividualHistory == null)
            {
                if (null == model)
                {
                    model = new PhysicalExamHistory
                    {
                        Athlete = Athlete,
                        Period = db.Periods.Where(p => p.PeriodId == id).FirstOrDefault(),
                        PeriodId = id,
                        AthleteId = atid,
                        IndTotalScore = null != IndividualHistory ? IndividualHistory.IndOperScore : 0,
                        IndOperTotalFinalOper = "0",
                        SportsScore = sport,
                        TeamWorkScore = 0,
                        TotalTimeWorkTime = "0.00",
                        TeamWorkFaltTime = "0.00",
                        TotalPhysicalScore = 0,
                        TotalScore = 0
                    };
                }
            }
            else
            {
                var strInOTF = Convert.ToString(IndividualHistory.IndOperTotalFinalOper);
                var _IndTotalScore = (IndividualHistory.IndOperScore);
                if (null == model)
                {

                    model = new PhysicalExamHistory
                    {
                        Athlete = Athlete,
                        Period = db.Periods.Where(p => p.PeriodId == id).FirstOrDefault(),
                        PeriodId = id,
                        AthleteId = atid,
                        IndTotalScore = null != IndividualHistory ? IndividualHistory.IndOperScore : 0,
                        IndOperTotalFinalOper = null != IndividualHistory ? strInOTF : "0",
                        SportsScore = sport,
                        TeamWorkScore = 0,
                        TotalTimeWorkTime = "0",
                        TeamWorkFaltTime = "0.00",
                        TotalPhysicalScore = 0,
                        TotalScore = 0
                    };
                    model.IndOperTotalFinalOper = strInOTF;
                }
                model.IndOperTotalFinalOper = strInOTF;
                model.DateInsert = Utility.PertionDate.Today();
                model.UserInsert = User.Identity.Name;
                model.IndTotalScore = _IndTotalScore;
                model.TeamWorkScore = null != IndividualHistory ? model.TeamWorkScore : 0;
                model.TotalTimeWorkTime = null != IndividualHistory ? model.TotalTimeWorkTime : "0";
                model.TeamWorkFaltTime = null != IndividualHistory ? model.TeamWorkFaltTime : "0.00";
                model.TeamWorkTime = null != IndividualHistory ? model.TeamWorkTime : "0";
                model.TotalScore = model.TotalScore;
                ViewBag.FaltTeam = IndividualHistory.IndOperTotalFaults;
                ViewBag.teamScore = IndividualHistory.IndOperTotalTime;
            }

            //}


            return View(model);
        }

        // POST: PhysicalExamHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public ActionResult Create(PhysicalExamHistory physicalExamHistory, List<PhRedinessHistoryItem> items)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (!Utility.UAC.Validate(User, "CanPhysicalExamHistoriesCreate"))
                return View("Sorry");
            var EditT = new EditTable();
            using (var scope = new TransactionScope())
            {
                try
                {


                    var athlete = db.Athletes.Where(a => a.Flag == true && a.AthleteId == physicalExamHistory.AthleteId).FirstOrDefault();
                    var original = db.PhysicalExamHistories.Where(p => p.AthleteId == physicalExamHistory.AthleteId && p.PeriodId == physicalExamHistory.PeriodId).FirstOrDefault();
                    var Athlete = db.Athletes.Where(a => a.Flag == true && a.AthleteId == physicalExamHistory.AthleteId).Select(a => a.Birthdate).FirstOrDefault();
                    var year = db.Periods.Where(p => p.PeriodId == physicalExamHistory.PeriodId).Select(p => p.Year).FirstOrDefault();
                    var age = Utility.PertionDate.GetAge(Athlete, year);
                    var SearchRAp = db.RengAthleteInPeriods.Where(r => r.AthleteId == physicalExamHistory.AthleteId && r.YearOfP == year).FirstOrDefault();
                    if (SearchRAp == null)
                    {
                        var agerengP = new RengAthleteInPeriod();
                        agerengP.AgeRengeId = Utility.PertionDate.GetAgeRangeId(Athlete, year);
                        agerengP.YearOfP = year;
                        agerengP.AthleteId = physicalExamHistory.AthleteId;
                        agerengP.InsertDate = Utility.PertionDate.Today();
                        agerengP.UserDate = User.Identity.Name;
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
                            physicalExamHistory.AgeReng = age;
                            physicalExamHistory.Reng = Utility.PertionDate.GetAgeRangeId(age);
                            physicalExamHistory.DateInsert = Utility.PertionDate.Today();
                            physicalExamHistory.UserInsert = User.Identity.Name;
                            physicalExamHistory.TimeInsert = DateTime.Now.ToShortTimeString();
                            db.PhysicalExamHistories.Add(physicalExamHistory);
                            foreach (var item in items)
                                physicalExamHistory.PhRedinessHistoryItems.Add(item);
                            db.SaveChanges();
                            scope.Complete();
                            return RedirectToAction("DataEntry", "Athletes", new { id = physicalExamHistory.PeriodId, atid = physicalExamHistory.AthleteId });
                        }
                    }
                    else
                    {
                        original.AgeReng = age;
                        original.Reng = Utility.PertionDate.GetAgeRangeId(age);
                        original.TeamWorkFaltTime = physicalExamHistory.TeamWorkFaltTime;
                        original.TotalTimeWorkTime = physicalExamHistory.TotalTimeWorkTime;
                        original.TeamWorkScore = physicalExamHistory.TeamWorkScore;

                        original.IndOperTotalFinalOper = physicalExamHistory.IndOperTotalFinalOper;
                        original.PhysicalExamHistoryNote = physicalExamHistory.PhysicalExamHistoryNote;
                        original.SportsScore = physicalExamHistory.SportsScore;
                        original.TotalPhysicalScore = physicalExamHistory.TotalPhysicalScore;
                        original.TotalScore = physicalExamHistory.TotalScore;

                        while (original.PhRedinessHistoryItems.Count > 0)
                            db.PhRedinessHistoryItems.Remove(original.PhRedinessHistoryItems.FirstOrDefault());

                        foreach (var item in items)
                            original.PhRedinessHistoryItems.Add(item);

                        db.Entry(original).State = EntityState.Modified;
                        db.SaveChanges();
                        scope.Complete();
                        return RedirectToAction("DataEntry", "Athletes", new { id = original.PeriodId, atid = original.AthleteId });

                    }
                }
                catch (Exception ex)
                {
                }
            }
            ViewBag.ItemsByPeriod = db.PhReadinessItemByPeriods.Where(p => p.PeriodId == physicalExamHistory.PeriodId).ToList();
            return View(physicalExamHistory);
        }
        [AllowAnonymous]

        public ActionResult GetScoreByTime(int id, string time, int atid, int prid)

        {
            var athlete = db.Athletes.Find(atid);
            var Year = db.Periods.Where(p => p.PeriodId == prid).Select(p => p.Year).FirstOrDefault();
            int agerangeid = Utility.PertionDate.GetAgeRangeId(athlete.Birthdate, Year);
            var model = db.phRItemsPoints.Where(p => p.PhReadinessItemId == id && p.AgeRengeId == agerangeid && string.Compare(p.Time, time) <= 0).OrderByDescending(p => p.Time).FirstOrDefault();

            if (null != model)
                return Content(model.Point.ToString());

            return Content("0");
        }

        [AllowAnonymous]

        public ActionResult GetTeamScoreByTime(int id, string time)
        {
            var PackTeamid = db.Periods.Where(p => p.PeriodId == id).FirstOrDefault();
            var model = db.TeamWorkPoints.Where(p => p.PackTeamid == PackTeamid.PackTeamid && string.Compare(p.Time, time) <= 0).OrderByDescending(p => p.Time).FirstOrDefault();
            if (null != model)
                return Content(model.Points.ToString());

            return Content("0");
        }
        [AllowAnonymous]

        public ActionResult firstpage()
        {

            return View();
        }
        // GET: PhysicalExamHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhysicalExamHistory physicalExamHistory = db.PhysicalExamHistories.Find(id);
            if (physicalExamHistory == null)
            {
                return HttpNotFound();
            }
            return View(physicalExamHistory);
        }

        // POST: PhysicalExamHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhysicalExamHistory physicalExamHistory = db.PhysicalExamHistories.Find(id);
            db.PhysicalExamHistories.Remove(physicalExamHistory);
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
