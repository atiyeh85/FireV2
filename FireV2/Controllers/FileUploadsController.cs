using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FireV2.Models;
using System.IO;
using MvcFileUploader.Models;
using MvcFileUploader;

namespace FireV2.Controllers
{
    public class FileUploadsController : Controller
    {
        private StoreDb db = new StoreDb();
        string UploadPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/FileUpload/"));

        // GET: FileUploads
        public ActionResult Index(int id)
        {
            var fileUploads = db.FileUploads.Include(f => f.Medical).Where(f=>f.Medicalid==id);
            return View(fileUploads.ToList());
        }
        public ActionResult UploadFile(int entityId) // optionally receive values specified with Html helper
        {
            var today = Utility.PertionDate.Today();
            int Medicalid = Convert.ToInt32(entityId);
            var Athlete = db.Medicals.Where(a => a.Medicalid == Medicalid).FirstOrDefault();
            FileUpload FU = new Models.FileUpload();
            var at = db.Athletes.Where(a => a.AthleteId == Athlete.AthleteId).FirstOrDefault();
            //var BussinessLocat = db.Uploads.Where(e => e.BussinessLocatId == entityId).FirstOrDefault();
            // here we can send in some extra info to be included with the delete url 
            var statuses = new List<ViewDataUploadFileResult>();
            var FileName = "img";
            for (var i = 0; i < Request.Files.Count; i++)
            {
                string path = Path.Combine(UploadPath, today.Replace("/", "-"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path,  Convert.ToString(at.AthleteId));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //path = Path.Combine(path, FileName+i);
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                var st = FileSaver.StoreFile(x =>
                {

                    x.File = Request.Files[i];
                    x.FileName = Request.Files[i].FileName;// default is filename suffixed with filetimestamp
                    path = Path.Combine(path, x.FileName);
                    FU.AddressFile = "~/Content/FileUpload/" + today.Replace("/", "-") + "/" + at.AthleteId+ "/" + x.FileName;
                    FU.DateUpload = Utility.PertionDate.Today();
                    FU.Medicalid = entityId;
                    FU.UserUpload =User.Identity.Name;
                    x.File = Request.Files[i];
                    //note how we are adding an additional value to be posted with delete request
                    //and giving it the same value posted with upload
                    x.DeleteUrl = Url.Action("DeleteFile", new { entityId = entityId });
                    x.StorageDirectory = Server.MapPath("~/Content/FileUpload");
                    x.UrlPrefix = "/Content/FileUpload";// this is used to generate the relative url of the file
                    x.File.SaveAs(path);

                    //overriding defaults
                    x.FileName = Request.Files[i].FileName;// default is filename suffixed with filetimestamp
                    x.ThrowExceptions = true;//default is false, if false exception message is set in error property
                });


                db.FileUploads.Add(FU);
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
        // GET: FileUploads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileUpload fileUpload = db.FileUploads.Find(id);
            if (fileUpload == null)
            {
                return HttpNotFound();
            }
            return View(fileUpload);
        }

        // GET: FileUploads/Create
        public ActionResult Create()
        {
            ViewBag.Medicalid = new SelectList(db.Medicals, "Medicalid", "DateUpload");
            return View();
        }

        // POST: FileUploads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FileUploadid,Medicalid,AddressFile,DateUpload,UserUpload")] FileUpload fileUpload)
        {
            if (ModelState.IsValid)
            {
                db.FileUploads.Add(fileUpload);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Medicalid = new SelectList(db.Medicals, "Medicalid", "DateUpload", fileUpload.Medicalid);
            return View(fileUpload);
        }

        // GET: FileUploads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileUpload fileUpload = db.FileUploads.Find(id);
            if (fileUpload == null)
            {
                return HttpNotFound();
            }
            ViewBag.Medicalid = new SelectList(db.Medicals, "Medicalid", "DateUpload", fileUpload.Medicalid);
            return View(fileUpload);
        }

        // POST: FileUploads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FileUploadid,Medicalid,AddressFile,DateUpload,UserUpload")] FileUpload fileUpload)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fileUpload).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Medicalid = new SelectList(db.Medicals, "Medicalid", "DateUpload", fileUpload.Medicalid);
            return View(fileUpload);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult DeleteFile(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileUpload upload = db.FileUploads.Find(id);
            var Medicalid = upload.Medicalid;
            if (upload == null)
            {
                return HttpNotFound();
            }
            db.FileUploads.Remove(upload);
            db.SaveChanges();
            return Content("true");

        }
        // GET: FileUploads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileUpload fileUpload = db.FileUploads.Find(id);
            if (fileUpload == null)
            {
                return HttpNotFound();
            }
            return View(fileUpload);
        }

        // POST: FileUploads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FileUpload fileUpload = db.FileUploads.Find(id);
            db.FileUploads.Remove(fileUpload);
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
