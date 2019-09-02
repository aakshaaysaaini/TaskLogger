using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskLoggerApplication.Models;

namespace TaskLoggerApplication.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        public ActionResult Index()
        {
            ProjectTasksViewModel projectTasks = new ProjectTasksViewModel();
            projectTasks.Projects = db.Projects.ToList();
            projectTasks.Users = db.Users.Where(i => !(i.UserName.Equals(User.Identity.Name))).ToList();
            ViewBag.logName = User.Identity.Name;

            return View(projectTasks);
        }

        [NonAction]
        public double calcDuration(DateTime start, DateTime end)
        {
            double days = (end - start).TotalDays;

            return days;
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            List<String> userrName = db.Users.Where(i => i.Project.ID != 0).Select(i => i.UserName).ToList();
            //List<SelectListItem> selectListItems = new List<SelectListItem>();

            //foreach(var uName in userrName)
            //{
            //    selectListItems.Add(new SelectListItem
            //    {
            //        Text = uName,
            //        Value = uName
            //    });
            //}
            //ViewBag.ListItems = selectListItems;
            ViewBag.ListItems = userrName;

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectName,StartDate,EndDate,Duration")] Project project, List<String> Username)
        {
            project.Duration = calcDuration(project.StartDate, project.EndDate);
            //if (ModelState.IsValid)
            //{

            foreach (var un in Username)
                {
                project.ApplicationUsers.Add(db.Users.Where(i => i.UserName == un).First());
                }
            db.Projects.Add(project);
            db.SaveChanges();

            //db.Projects.Add(project);
            //db.SaveChanges();
            return RedirectToAction("Index");
            //}

            //return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectName,StartDate,EndDate,Duration")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
