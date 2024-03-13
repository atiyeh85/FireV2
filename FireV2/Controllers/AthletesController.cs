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
using System.Collections;
using PagedList;
using System.IO;
using System.Web.UI.WebControls;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using MvcFileUploader.Models;
using MvcFileUploader;

namespace FireV2.Controllers
{
    [Authorize]
    public class AthletesController : Controller
    {
        private StoreDb db = new StoreDb();
        string UploadPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/ImageUpload/"));
        //public ActionResult Archive(int? id,  int? page, string Fname = "", string Lname = "")
        //{

        //}
        public ActionResult UploadFile(int? entityId) // optionally receive values specified with Html helper
        {
            var Athlete = db.Athletes.Where(a =>a.Flag==true && a.AthleteId == entityId).FirstOrDefault();
            //var BussinessLocat = db.Uploads.Where(e => e.BussinessLocatId == entityId).FirstOrDefault();
            // here we can send in some extra info to be included with the delete url 
            var statuses = new List<ViewDataUploadFileResult>();
            for (var i = 0; i < Request.Files.Count; i++)
            {
                string path = Path.Combine(UploadPath, Athlete.Birthdate.Replace("/", "-"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, Athlete.PersonalCode + "-" + Athlete.FullName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var st = FileSaver.StoreFile(x =>
                {

                    x.File = Request.Files[i];
                    x.FileName = Request.Files[i].FileName;// default is filename suffixed with filetimestamp
                    path = Path.Combine(path, x.FileName);
                    Athlete.Picture = "~/Content/ImageUpload/" + Athlete.Birthdate.Replace("/", "-") + "/" + Athlete.PersonalCode + "-" + Athlete.FullName + "/" + x.FileName;

                    x.File = Request.Files[i];
                    //note how we are adding an additional value to be posted with delete request
                    //and giving it the same value posted with upload
                    x.DeleteUrl = Url.Action("DeleteFile", new { entityId = entityId });
                    x.StorageDirectory = Server.MapPath("~/MyFile");
                    x.UrlPrefix = "/Content/ImageUpload/";// this is used to generate the relative url of the file
                    x.File.SaveAs(path);

                    //overriding defaults
                    x.FileName = Request.Files[i].FileName;// default is filename suffixed with filetimestamp
                    x.ThrowExceptions = true;//default is false, if false exception message is set in error property
                });


                db.Athletes.Add(Athlete);
                db.SaveChanges();
                statuses.Add(st);
            }

            //statuses contains all the uploaded files details (if error occurs then check error property is not null or empty)
            //todo: add additional code to generate thumbnail for videos, associate files with entities etc

            //adding thumbnail url for jquery file upload javascript plugin
            statuses.ForEach(x => x.thumbnailUrl = x.url + "?width=80&height=80"); // uses ImageResizer httpmodule to resize images from this url

            //setting custom download url instead of direct url to file which is default
            statuses.ForEach(x => x.url = Url.Action("DownloadFile", new { fileUrl = x.url, mimetype = x.type }));


            //server side error generation, generate some random error if entity id is 13
            if (entityId == 13)
            {
                var rnd = new Random();
                statuses.ForEach(x =>
                {
                    //setting the error property removes the deleteUrl, thumbnailUrl and url property values
                    x.error = rnd.Next(0, 2) > 0 ? "We do not have any entity with unlucky Id : '13'" : String.Format("Your file size is {0} bytes which is un-acceptable", x.size);
                    //delete file by using FullPath property
                    if (System.IO.File.Exists(x.FullPath)) System.IO.File.Delete(x.FullPath);
                });
            }

            var viewresult = Json(new { files = statuses });
            //for IE8 which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
                viewresult.ContentType = "text/plain";

            return viewresult;
        }
     public ActionResult FullFlag()
        {
            var hh = db.Athletes.Where(d=>d.Flag==null).ToList();
          
            foreach (var item in hh)
            {
                item.Flag = true;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();

            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult Archive(string sortOrder, string currentFilter, string currentFilter2, string Fname,string Lname, int? page)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("firstpage", "Athletes");

            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (Fname != null && Lname!=null)
            {
                page = 1;
            }
            else
            {
                Fname = currentFilter;
                Lname = currentFilter2;
            }

            ViewBag.CurrentFilter = Fname;
            ViewBag.CurrentFilter2 = Lname;

            var students = from s in db.Athletes.Where(a=>a.Flag==true)
                           select s;
            if (!String.IsNullOrEmpty(Fname))
            {
                students = students.Where(s => s.Fnam.Contains(Fname)
                                     );
            }
            if (!String.IsNullOrEmpty(Lname))
            {
                students = students.Where(s => s.Lname.Contains(Lname)
                                      );
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Fnam);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.AthleteId);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.AthleteId);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.Fnam);
                    break;

            }
            int pageSize =50;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }
        [AllowAnonymous]
        public ActionResult ReportA(List<string> ids)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            ArrayList myAL = new ArrayList();
            List<ListAtheletevm> lst = new List<ListAtheletevm>();
            GridView gv = new GridView();
           
            var ListAthelete = db.Athletes.Where(a=>a.Flag==true).Include(p => p.PostSazmani).Include(p => p.Degree).Include(p => p.ShiftWork).Select(p => new ListAtheletevm()
            {
              
                AthleteId = p.AthleteId,
                FullName = p.Fnam + " " + p.Lname,
                DegreeName = p.Degree.DegreeName,
                PSazmaniTitle = p.PostSazmani.PSazmaniTitle,
                Expertise = p.Expertise,
                ShWorkTitle = p.ShiftWork.ShWorkTitle,
                StationTitle = p.ShiftWork.Station.StationTitle,
                PersonalCode = p.PersonalCode,
                GoorohKhoni = p.GoorohKhoni,
                Picture = p.Picture,
                ss=p.Birthdate
              

            }).ToList();
           
            if (ids!=null)
            {
                foreach (var id in ids)
                {
                    var intid = Convert.ToInt32(id);
                  var  ll = ListAthelete.Where(l => l.AthleteId == intid).FirstOrDefault();
                    var mm = ListAthelete.Where(l => l.AthleteId == intid).FirstOrDefault();
                    lst.Add(ll);
                }
            }

            TempData["List"] = lst;

            return View(myAL);
        }

        public ActionResult MathA() {
            var ll = db.PhysicalExamHistories.Include(p => p.Athlete).Include(p => p.Period).ToList()
    ;
            
            foreach (PhysicalExamHistory item in ll)
            {
              
                var Birthdate = item.Athlete.Birthdate;
                var yearB =Convert.ToInt32( Birthdate.Substring(0, 4));
                var yearN = item.Period.Year ;
                var Age= yearN - yearB;
                //var ageR = db.AgeRenges.Where(d => Age >= d.MinRenge && Age <= d.MaxReng).Select(d=>d.AgeRengeId).FirstOrDefault();
                item.AgeReng = Age;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View();
        }
      
        [AllowAnonymous]

       
     

        string GetAgerenj(string date,int Date2)
        {
            try
            {
                System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
                DateTime dt = Convert.ToDateTime(date);
                var Yeardate = p.GetYear(dt);
                var NowYearToMiladi =Date2;
                var age = NowYearToMiladi - Yeardate;
                return age.ToString();
            }
            catch (Exception ex)
            {
            }
            return "1";

        }
        //[AllowAnonymous]

        //public ActionResult MathAge()
        //{
        //    var athletes = db.PhysicalExamHistories.Include(d=>d.Athlete).ToList();
        //    foreach (PhysicalExamHistory item in athletes)
        //    {
        //        var p = item.Period.Year;

        //        var birth = item.Athlete.Birthdate;
        //        var age = GetAgerenj(birth,p);
        //        item.AgeReng = age;
        //        db.Entry(item).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    return View(athletes);
        //}
     

        [AllowAnonymous]

        public ActionResult ChangeImage()
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            return View();
        }
        [AllowAnonymous]

        public ActionResult GetAutoCompleteFnam(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            { term = ""; }
            var list = db.Athletes.Where(a=>a.Flag==true).Select(a => a.Fnam).Distinct().ToList();
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
            var list = db.Athletes.Where(a=>a.Flag==true).Select(a => a.Lname).Distinct().ToList();
            string[] AutoCompleteOptions = new string[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                AutoCompleteOptions[i] = list[i];
            }

            return this.Json(AutoCompleteOptions.Where(t => t.StartsWith(term, StringComparison.InvariantCultureIgnoreCase)),
                            JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult EditArchive()
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            var dd = db.Athletes.Where(A=>A.Flag==true&&    A.IsArchive!=false).ToList();
            foreach (var item in dd)
            {
                item.IsArchive = false;
                db.Entry(item).State = EntityState.Modified;

            }
            db.SaveChanges();
            return View();
        }
        [AllowAnonymous]

        public ActionResult GetAutoComplete(string term)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (string.IsNullOrWhiteSpace(term))
            { term = ""; }
            var list = db.Athletes.Where(a=>a.Flag==true).Select(a => a.Fnam +" "+ a.Lname).ToList();
            string[] AutoCompleteOptions = new string[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                AutoCompleteOptions[i] = list[i];
            }
          
            return this.Json(AutoCompleteOptions.Where(t => t.StartsWith(term, StringComparison.InvariantCultureIgnoreCase)),
                            JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]

        // GET: Athletes
        public ActionResult fIndex(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            //var athletes = db.Athletes.Where(a => a.AthleteId == id).ToList();
            var athletes = db.Athletes.Where(a => a.AthleteId == id).Include(a => a.Degree).Include(a => a.Semat).ToList();
            var Age = athletes.Select(p => p.Birthdate).FirstOrDefault();
            ViewBag.age = GetAge(Age);
            return View(athletes);
        }
        [AllowAnonymous]

        public ActionResult AllList(int? id,int? PostSazmaniId, int? page, string Fname="", string Lname="")
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("firstpage", "Athletes");

            }
            if (!Utility.UAC.Validate(User, "CanViewAthelte"))
                return View("Sorry");
            if (page.HasValue && page < 1)
                return null;

            var Athlete = db.Athletes.Where(p => p.Flag == true )
                .AsQueryable();
            var kkk = Athlete.ToList();
            if (!string.IsNullOrEmpty(Fname))
            {
                ViewBag.Fname = Fname;
                Athlete = Athlete.Where(p => p.Fnam.Contains(Fname));
            }
            if (!string.IsNullOrEmpty(Lname))
            {
                ViewBag.Lname = Lname;
                Athlete = Athlete.Where(p => p.Lname.Contains(Lname));
            }
            var fff = Athlete.ToList();
            if (PostSazmaniId.HasValue && PostSazmaniId.Value != 0)
            {
                ViewBag.PostSazmaniId = PostSazmaniId;
                Athlete = Athlete.Where(p => p.PostSazmaniId == PostSazmaniId);
            }
            Athlete = Athlete.OrderByDescending(p => p.AthleteId);
            const int pageSize = 250;
            var listPaged = Athlete.ToPagedList(page ?? 1, pageSize);
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                listPaged = null;

            if (listPaged == null)
                return HttpNotFound();
            ViewBag.PostSazmaniId = new SelectList(db.PostSazmanis, "PostSazmaniId", "PSazmaniTitle", PostSazmaniId);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AllList", listPaged)
                : View(listPaged);
        }
        
        [HttpPost]
        [AllowAnonymous]

        public ActionResult DeleteNote(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            try
            {
                var model = db.DocTypeAthelets.Find(id);
                if (null != model)
                {
                    model.Note = "";
                    db.Entry(model).State = EntityState.Modified;

                    //db.CoopreationPersons.Remove(model.Note);
                    db.SaveChanges();
                    return Content("true");


                }
            }
            catch (Exception ex)
            {

            }
            return Content("false");

        }
        [AllowAnonymous]

        string GetAge(string date)
        {

            try
            {
                System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
                DateTime dt = p.ToDateTime(int.Parse(date.Substring(0, 4)),
                    int.Parse(date.Substring(5, 2)),
                    int.Parse(date.Substring(8, 2)), 0, 0, 0, 0);

                int age = (int)DateTime.Now.Subtract(dt).TotalDays / 365;
                string Age = Convert.ToString(age);
                return Age;
            }
            catch (Exception ex)
            {
            }
            return "1";

        }
      
        [AllowAnonymous]

        public ActionResult AddDocTitle(int id)
        {
            ViewBag.InfoDocTitleId = new SelectList(db.InfoDocumentTitles, "InfoDocTitleId", "InfoDocTitle");
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddDocTitle(int AthleteId, int InfoDocTitleId, DocTypeAthelet[] details)
        {
            try
            {
                //var oldItems = db.DocTypeAthelets.Where(c => c.AthleteId == AthleteId && c.InfoDocumentTitleType.InfoDocTitleId == InfoDocTitleId).ToList();

                //if (oldItems.Any())
                //{
                //    foreach (var item in oldItems)
                //        db.DocTypeAthelets.Remove(item);
                //    db.SaveChanges();
                //}

                foreach (var item in details)
                {
                    DocTypeAthelet model = new DocTypeAthelet()
                    {
                        AthleteId = AthleteId,
                        InfoDocTitleTypeId = item.InfoDocTitleTypeId,
                        Note = item.Note
                    };
                    db.DocTypeAthelets.Add(model);
                }

                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                    return Content("true");
                }
            }
            catch (Exception ex)
            {
            }


            return Content("false");
        }
        [AllowAnonymous]

        public ActionResult GetDocTitle(int id)
        {
            var model = db.DocTypeAthelets.Where(d => d.AthleteId == id && d.InfoDocumentTitleType.InfoDocTitleTypeId == d.InfoDocTitleTypeId).ToList();
            ViewBag.li = db.InfoDocumentTitles.ToList();
            return PartialView(model);
        }
        [AllowAnonymous]

        // GET: Athletes/Details/5
        public ActionResult Details(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (!Utility.UAC.Validate(User, "CanAthletesDetails"))
                return View("Sorry");
            ViewBag.InfoDocTitleId = new SelectList(db.InfoDocumentTitles, "InfoDocTitleId", "InfoDocTitle");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Athlete athlete = db.Athletes.Where(p => p.AthleteId == id).FirstOrDefault();
            if (athlete == null)
            {
                return HttpNotFound();
            }
            return View(athlete);
        }
        [AllowAnonymous]

        public ActionResult GetCity(int OstanId)
        {
            ViewBag.CityId = new SelectList(db.Cities.Where(c => c.OstanId == OstanId).OrderBy(c => c.CitiyName).ToList(), "CityId", "CitiyName");
            return PartialView();
        }
        [AllowAnonymous]

        public ActionResult GetShift(int StationId)
        {
            ViewBag.ShiftWorkId = new SelectList(db.ShiftWorks.Where(c => c.StationId == StationId).OrderBy(c => c.ShWorkTitle).ToList(), "ShiftWorkId", "ShWorkTitle");
            return PartialView();
        }
        [AllowAnonymous]

        // GET: Athletes/Create
        public ActionResult Create()
        {
            DateTime date = DateTime.Now;
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (!Utility.UAC.Validate(User, "CanCreateAthlete"))
                return View("Sorry");
            
            ViewBag.PeriodId = new SelectList(db.Periods, "PeriodId", "SeasonTitle");
            ViewBag.PhyEduItemId = new SelectList(db.PhysicalEducationItems, "PhyEduItemId", "PhyEducationTitle");

            ViewBag.OstanId = new SelectList(db.Ostans, "OstanId", "OstanName");
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CitiyName");
            ViewBag.DegreeId = new SelectList(db.Degrees, "DegreeId", "DegreeName");
            ViewBag.EmployeeStatusId = new SelectList(db.EmployeeStatus, "EmployeeStatusId", "EmStatusTitle");
            ViewBag.EmTypeId = new SelectList(db.EmployeTypes, "EmTypeId", "HowEpmTitle");
            ViewBag.MaritalStatusId = new SelectList(db.MaritalStatus, "MaritalStatusId", "MaritalStatusTitle");
            ViewBag.PostSazmaniId = new SelectList(db.PostSazmanis, "PostSazmaniId", "PSazmaniTitle");
            ViewBag.SematId = new SelectList(db.Semats, "SematId", "SematName");
            ViewBag.ShareId = new SelectList(db.Shares, "ShareId", "IsShare");
            ViewBag.StationId = new SelectList(db.Stations, "StationId", "StationTitle");
            ViewBag.ShiftWorkId = new SelectList(db.ShiftWorks, "ShiftWorkId", "ShWorkTitle");
            return View();
        }
        [AllowAnonymous]
        public ActionResult TitleDoc(int id)
        {
            var Item = db.InfoDocumentTitleTypes.Where(c => c.InfoDocTitleId == id).ToList();
            return PartialView(Item);
        }

        // POST: Athletes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public ActionResult Create(Athlete athlete)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (!Utility.UAC.Validate(User, "InsertInfoFireFighter"))
                return View("Sorry");
            HttpPostedFileBase file = Request.Files.Count > 0 ? Request.Files[0] : null;

            if (file != null && !string.IsNullOrWhiteSpace(file.FileName) && !(file.FileName.ToLower().EndsWith(".jpg") || file.FileName.ToLower().EndsWith(".png") || file.FileName.ToLower().EndsWith(".bmp") || file.FileName.ToLower().EndsWith(".JPG")))
            {
                ModelState.AddModelError("Image", "فایل انتخاب شده برای تصویر است");
                return View("ErrorImage");
            }

            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(UploadPath, athlete.Birthdate.Replace("/", "-"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = Path.Combine(path, athlete.PersonalCode + "-" + athlete.FullName);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, file.FileName);
                    athlete.Picture = "~/Content/ImageUpload/" + athlete.Birthdate.Replace("/", "-") + "/" + athlete.PersonalCode + "-" + athlete.FullName + "/" + file.FileName;

                    file.SaveAs(path);
                }
                athlete.IsArchive = false;
                athlete.Flag = true;
                if (athlete.ShiftWorkId==null)
                {
                    athlete.ShiftWorkId =27;
                }
                athlete.RegisterDate = Utility.PertionDate.Today();
                athlete.UserInsert = User.Identity.Name;
                    db.Athletes.Add(athlete);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = athlete.AthleteId });
            }
            ViewBag.OstanId = new SelectList(db.Ostans, "OstanId", "OstanName");
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CitiyName");
                ViewBag.DegreeId = new SelectList(db.Degrees, "DegreeId", "DegreeName");
                ViewBag.EmployeeStatusId = new SelectList(db.EmployeeStatus, "EmployeeStatusId", "EmStatusTitle");
                ViewBag.EmTypeId = new SelectList(db.EmployeTypes, "EmTypeId", "HowEpmTitle");
                ViewBag.MaritalStatusId = new SelectList(db.MaritalStatus, "MaritalStatusId", "MaritalStatusTitle");
                ViewBag.PostSazmaniId = new SelectList(db.PostSazmanis, "PostSazmaniId", "PSazmaniTitle");
                ViewBag.ShareId = new SelectList(db.Shares, "ShareId", "IsShare");
                ViewBag.StationId = new SelectList(db.Stations, "StationId", "StationTitle");
                ViewBag.ShiftWorkId = new SelectList(db.ShiftWorks, "ShiftWorkId", "ShWorkTitle");
                return View(athlete);
            }
        [AllowAnonymous]

        // GET: Athletes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Athlete athlete = db.Athletes.Find(id);
            if (athlete == null)
            {
                return HttpNotFound();
            }
            ViewBag.OstanId = new SelectList(db.Ostans, "OstanId", "OstanName",athlete.City.OstanId);
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CitiyName",athlete.CityId);
            ViewBag.StationId = new SelectList(db.Stations, "StationId", "StationTitle", athlete.ShiftWork.StationId);

            ViewBag.DegreeId = new SelectList(db.Degrees, "DegreeId", "DegreeName", athlete.DegreeId);
            ViewBag.EmployeeStatusId = new SelectList(db.EmployeeStatus, "EmployeeStatusId", "EmStatusTitle", athlete.EmployeeStatusId);
            ViewBag.EmTypeId = new SelectList(db.EmployeTypes, "EmTypeId", "HowEpmTitle", athlete.EmTypeId);
            ViewBag.MaritalStatusId = new SelectList(db.MaritalStatus, "MaritalStatusId", "MaritalStatusTitle", athlete.MaritalStatusId);
            ViewBag.PostSazmaniId = new SelectList(db.PostSazmanis, "PostSazmaniId", "PSazmaniTitle", athlete.PostSazmaniId);
            ViewBag.ShareId = new SelectList(db.Shares, "ShareId", "IsShare", athlete.ShareId);
            ViewBag.ShiftWorkId = new SelectList(db.ShiftWorks, "ShiftWorkId", "ShWorkTitle", athlete.ShiftWorkId);
            return View(athlete);
        }

        // POST: Athletes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public ActionResult Edit(Athlete athlete)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (!Utility.UAC.Validate(User, "CanEditAthlete"))
                return View("Sorry");
            HttpPostedFileBase file = Request.Files.Count > 0 ? Request.Files[0] : null;

            if (file != null && !string.IsNullOrWhiteSpace(file.FileName) && !(file.FileName.ToLower().EndsWith(".jpg") || file.FileName.ToLower().EndsWith(".png") || file.FileName.ToLower().EndsWith(".bmp") || file.FileName.ToLower().EndsWith(".JPG")))
            {
                ModelState.AddModelError("Image", "فایل انتخاب شده برای تصویر است");
                return View("ErrorImage");
            }
            var Athid = athlete.AthleteId;
           

            var _Ath = db.Athletes.Where(a=>a.AthleteId== athlete.AthleteId).FirstOrDefault();
            var ISactive = _Ath.IsArchive;
            Athlete athll = new Athlete();
            var Ath = db.Athletes.Where(a=>a.AthleteId== athlete.AthleteId).FirstOrDefault();
            
            athll.Fnam = _Ath.Fnam;
            athll.Lname = _Ath.Lname;
            athll.Shenasnameh = _Ath.Shenasnameh;
            athll.Expertise = _Ath.Expertise;
            athll.FatherName = _Ath.FatherName;
            athll.ShiftWorkId = _Ath.ShiftWorkId;
            athll.Birthdate = _Ath.Birthdate;
            athll.PersonalCode = _Ath.PersonalCode;
            athll.Telephone = _Ath.Telephone;
            athll.CityId = _Ath.CityId;
            athll.SematId = _Ath.SematId;
            athll.Melicode = _Ath.Melicode;
            athll.EmployeeStatusId = _Ath.EmployeeStatusId;
            athll.DegreeId = _Ath.DegreeId;
            athll.PostSazmaniId = _Ath.PostSazmaniId;
            athll.ShareId = _Ath.ShareId;
            athll.IsArchive = _Ath.IsArchive;
            athll.NoteAthlete = _Ath.NoteAthlete;
            athll.GoorohKhoni = _Ath.GoorohKhoni;
            athll.AddressH = _Ath.AddressH;
            athll.AddressW = _Ath.AddressW;
            athll.Sadereh = _Ath.Sadereh;
            athll.bimeh = _Ath.bimeh;
            athll.MaritalStatusId = _Ath.MaritalStatusId;
            athll.Reshteh = _Ath.Reshteh;
            athll.Mobile = _Ath.Mobile;
            athll.PointEnter = _Ath.PointEnter;
            athll.EnterDate = _Ath.EnterDate;
            athll.UserInsert = _Ath.UserInsert;
            athll.RegisterDate = _Ath.RegisterDate;
            athll.Flag = false;
            athll.Athid = Athid;
            db.Athletes.Add(athll);
            db.SaveChanges();

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        string path = Path.Combine(UploadPath, athlete.Birthdate.Replace("/", "-"));
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        path = Path.Combine(path, athlete.PersonalCode + "-" + athlete.FullName);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        path = Path.Combine(path, file.FileName);
                        Ath.Picture = "~/Content/ImageUpload/" + athlete.Birthdate.Replace("/", "-") + "/" + athlete.PersonalCode + "-" + athlete.FullName + "/" + file.FileName;
                        file.SaveAs(path);
                    }
                    Ath.LastEditDate = Utility.PertionDate.Today();
                    Ath.LastEditeUser = User.Identity.Name;
                    Ath.Fnam = athlete.Fnam;
                    Ath.Lname = athlete.Lname;
                    Ath.Shenasnameh = athlete.Shenasnameh;
                    Ath.Expertise = athlete.Expertise;
                    Ath.FatherName = athlete.FatherName;
                    Ath.ShiftWorkId = athlete.ShiftWorkId;
                    Ath.Birthdate = athlete.Birthdate;
                    Ath.PersonalCode = athlete.PersonalCode;
                    Ath.CityId = athlete.CityId;
                    Ath.SematId = athlete.SematId;
                    Ath.EmployeeStatusId = athlete.EmployeeStatusId;
                    Ath.DegreeId = athlete.DegreeId;
                    Ath.PostSazmaniId = athlete.PostSazmaniId;
                    Ath.ShareId = athlete.ShareId;
                    Ath.IsArchive = ISactive;
                    Ath.EmTypeId = athlete.EmTypeId;
                    Ath.NoteAthlete = athlete.NoteAthlete;
                    Ath.GoorohKhoni = athlete.GoorohKhoni;
                    Ath.Sadereh = athlete.Sadereh;
                    Ath.bimeh = athlete.bimeh;
                    Ath.MaritalStatusId = athlete.MaritalStatusId;
                    Ath.Reshteh = athlete.Reshteh;
                    Ath.Mobile = athlete.Mobile;
                    Ath.PointEnter = athlete.PointEnter;
                    Ath.DateEnter = athlete.DateEnter;
                    Ath.EnterDate = athlete.EnterDate;
                    Ath.UserInsert = User.Identity.Name;
                    Ath.RegisterDate = Utility.PertionDate.Today();
                    Ath.Melicode = athlete.Melicode;
                    Ath.Flag = true;
                    db.Entry(Ath).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = athlete.AthleteId });
                }
                catch (Exception)
                {

                    throw;
                }
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CitiyName", Ath.CityId);
            ViewBag.StationId = new SelectList(db.Stations, "StationId", "StationTitle", Ath.ShiftWork.StationId);
            ViewBag.OstanId = new SelectList(db.Ostans, "OstanId", "OstanName", Ath.City.OstanId);
            ViewBag.DegreeId = new SelectList(db.Degrees, "DegreeId", "DegreeName", Ath.DegreeId);
            ViewBag.EmployeeStatusId = new SelectList(db.EmployeeStatus, "EmployeeStatusId", "EmStatusTitle", Ath.EmployeeStatusId);
            ViewBag.EmTypeId = new SelectList(db.EmployeTypes, "EmTypeId", "HowEpmTitle", Ath.EmTypeId);
            ViewBag.MaritalStatusId = new SelectList(db.MaritalStatus, "MaritalStatusId", "MaritalStatusTitle", Ath.MaritalStatusId);
            ViewBag.PostSazmaniId = new SelectList(db.PostSazmanis, "PostSazmaniId", "PSazmaniTitle", Ath.PostSazmaniId);
            ViewBag.SematId = new SelectList(db.Semats, "SematId", "SematName", Ath.SematId);
            ViewBag.ShareId = new SelectList(db.Shares, "ShareId", "IsShare", Ath.ShareId);
            ViewBag.ShiftWorkId = new SelectList(db.ShiftWorks, "ShiftWorkId", "ShWorkTitle", Ath.ShiftWorkId);
            return View(Ath);
        }

        // GET: Athletes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Athlete athlete = db.Athletes.Find(id);
            if (athlete == null)
            {
                return HttpNotFound();
            }
            return View(athlete);
        }

        // POST: Athletes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Athlete athlete = db.Athletes.Find(id);
                db.Athletes.Remove(athlete);
                db.SaveChanges();
                return RedirectToAction("AllList");
            }
            catch (Exception)
            {
                return RedirectToAction("AllList");
            }
           
        }

        public ActionResult DataEntry(int? id, int? atid)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (!Utility.UAC.Validate(User, "InsertExam"))
                return View("Sorry");
            ViewBag.PeriodId = new SelectList(db.Periods.ToList(), "PeriodId", "SeasonTitle", id);
            return View();
        }
        [AllowAnonymous]

        public ActionResult Report(string from = "", string to = "")
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (!Utility.UAC.Validate(User, "CanReport"))
                return View("Sorry");
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                var model = db.PhysicalExamHistories.OrderBy(e => e.Athlete.Birthdate).Select(e => new AthleteReportVM() { TotalScore = e.TotalScore, AthleteId = e.AthleteId, Name = e.Athlete.Fnam + " " + e.Athlete.Lname,  Picture=e.Athlete.Picture  }).ToList();

                ViewBag.message = "لطفا برای  جستجو دوره را انتخاب نمائید ";
                ViewBag.ListAthlete = db.Athletes.ToList();

                return View(model);
            }
            int from_year = int.Parse(from.Substring(0, 4));
            int from_season = int.Parse(from.Substring(4, 1));

            int to_year = int.Parse(to.Substring(0, 4));
            int to_season = int.Parse(to.Substring(4, 1));

            var pids = db.Periods.Where(p => p.Year >= from_year && p.Season >= from_season && p.Year <= to_year && p.Season <= to_season).Select(p => p.PeriodId).ToList();
            ViewBag.ListAthlete = db.Athletes.ToList();
            var models = db.PhysicalExamHistories.Where(e => pids.Contains(e.PeriodId)).OrderBy(e => e.Athlete.Birthdate).Select(e => new AthleteReportVM() { TotalScore = e.TotalScore, AthleteId = e.AthleteId, Name = e.Athlete.Fnam + " " + e.Athlete.Lname }).ToList();
            return View(models);
        }
        [AllowAnonymous]
        public ActionResult SportsScore(int? id)
        {
            ViewBag.PeriodId = new SelectList(db.Periods.ToList(), "PeriodId", "SeasonTitle", id);
            return View();
        }
        [AllowAnonymous]
        public ActionResult HistoryByPeriod(int id, int atid)
        {
            var Diss = db.Dissusions.Where(p => p.AthleteId == atid && p.PeriodId == id).ToList();
          
            var model = new AthleteHistoryVM();
            model.IndividualOperations = db.IndividualHistorys.Any(i => i.AthleteId == atid && i.PeriodId == id);
            model.PhysicalReadiness = db.PhRedinessHistoryItems.Any(p => p.PhysicalExamHistory.AthleteId == atid && p.PhysicalExamHistory.PeriodId == id);
                model.SportsScore = db.SportScoreByPeriods.Any(p => p.AthleteId == atid && p.PeriodId == id);
            model.DissUsionPeriod = db.Dissusions.Any(p => p.AthleteId == atid && p.PeriodId == id);
            model.PeriodId = id;
            model.AthleteId = atid;
            model.Picture = db.Athletes.Find(atid).Picture;
            foreach (var item in Diss)
            {
                if (item.DissuasionsTitle.DissTypeid == 1)
                {
                    model.PhDis = true;
                }
                else if (item.DissuasionsTitle.DissTypeid == 2)
                {
                    model.IndDis = true;

                }
                else if (item.DissuasionsTitle.DissTypeid == 3)
                {
                    model.TeamDis = true;

                }
                else
                {
                    model.FullDis = true;

                }
            }
            return PartialView(model);
        }
        [AllowAnonymous]
        public ActionResult SearchAthlete(string query)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (string.IsNullOrWhiteSpace(query))
                if (Request.IsAjaxRequest())
                    return Json(JsonResponseFactory.ErrorResponse("پیدا نشد"), JsonRequestBehavior.DenyGet);
                else
                    return new EmptyResult();

            int id = 0;
            bool result = true;
            try
            {
                id = int.Parse(query);
            }
            catch
            {

            }

            var model = db.Athletes.Where(a => a.AthleteId == id || (a.Fnam + " " + a.Lname).Contains(query) && a.Flag == true && a.IsArchive == false ).FirstOrDefault();
            if (Request.IsAjaxRequest())
            {
                if (null != model)
                    return Json(JsonResponseFactory.SuccessResponse(new { AthleteId = model.AthleteId, AthleteName =" آزمون آمادگی جسمانی و عملیاتی آتشنشان   "+"  "+ model.FullName }), JsonRequestBehavior.DenyGet);
                else
                    return Json(JsonResponseFactory.ErrorResponse("پیدا نشد"), JsonRequestBehavior.DenyGet);
            }
            else
            {
                if (null != model)

                    return Content(" آزمون آمادگی جسمانی و عملیاتی آتشنشان   " + "  " + model.FullName);
                else
                    return new EmptyResult();
            }
            //return Json(new { result }, JsonRequestBehavior.AllowGet);
            //ViewBag.message = "موردی یافت نشد";
            //return View();
        }
        [AllowAnonymous]

        public ActionResult GetTitle(int id)
        {

            var model = db.DocTypeAthelets.Where(c => c.AthleteId == id);
            return PartialView(model);
        }
        [HttpPost]
        [AllowAnonymous]

        public ActionResult DeleteDocTitle(int id)
        {
            try
            {
                var model = db.DocTypeAthelets.Find(id);
                if (null != model)
                {
                    db.DocTypeAthelets.Remove(model);
                    db.SaveChanges();
                    return Content("true");
                }
            }
            catch (Exception ex)
            {

            }
            return Content("false");
        }
        [AllowAnonymous]

        public ActionResult AddCooperation(int id)
        {
            var model = new DocTypeAthelet() { AthleteId = id };
            ViewBag.InfoDocTitleId = new SelectList(db.InfoDocumentTitles, "InfoDocTitleId", "InfoDocTitle");
            return PartialView(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddInFoDoc(DocTypeAthelet model, int[] details)
        {
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");

            }
            if (db.DocTypeAthelets.Count(c => c.AthleteId == model.AthleteId && c.InfoDocumentTitleType.InfoDocTitleTypeId == model.InfoDocumentTitleType.InfoDocTitleTypeId) > 0)
            {
                var DocTypeAtId = db.DocTypeAthelets.Where(c => c.AthleteId == model.AthleteId && c.InfoDocumentTitleType.InfoDocTitleTypeId == model.InfoDocumentTitleType.InfoDocTitleTypeId).FirstOrDefault();
                if (details == null)
                {
                    db.Entry(DocTypeAtId).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content("true");
                }
                foreach (var item in details)
                {
                    var Item = db.InfoDocumentTitleTypes.Find(item);
                    //DocTypeAtId.InfoDocumentTitleType.Add();

                }
                if (ModelState.IsValid)
                {
                    db.Entry(DocTypeAtId).State = EntityState.Modified;
                    db.SaveChanges();

                    return Content("true");
                }

                return Content("false");

            }
            foreach (var item in details)
            {
                var mo = db.InfoDocumentTitleTypes.Where(i => i.InfoDocTitleTypeId == item).FirstOrDefault();

                //model.InfoDocumentTitleTypes.Add(mo);
            }

            if (ModelState.IsValid)
            {
                db.DocTypeAthelets.Add(model);
                db.SaveChanges();
                return Content("true");
            }

            return Content("false");
        }
        [AllowAnonymous]

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
