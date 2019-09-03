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
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tasks
        public ActionResult Index(string uName, string pName)
        {
            ProjectTasksViewModel projectUsers = new ProjectTasksViewModel();
            if (uName != null && pName == null)
            {
                projectUsers.Users = db.Users.Where(i => i.UserName == uName).ToList();
                projectUsers.Projects = db.Projects.ToList();

            }
            else if(pName != null && uName == null)
            {
                projectUsers.Projects = db.Projects.Where(i => i.ProjectName == pName).ToList();
                projectUsers.Users = db.Users.ToList();
            }
            else
            {
                projectUsers.Projects = db.Projects.ToList();
                projectUsers.Users = db.Users.ToList();
                projectUsers.Tasks = db.Tasks.ToList();
            }

            return View(projectUsers);

        }

        [HttpPost]
        public ActionResult Index(int sn, string userName, string projectName)
        {
            return RedirectToAction("Index", new { uName = userName, pName = projectName});
        }

        public String Ajax_getDescription(int ID)
        {
            Tasks tasks = new Tasks
            {
                ID = ID,
                TaskDescription = db.Tasks.FirstOrDefault(x => x.ID == ID).TaskDescription
            };
            return tasks.TaskDescription;
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tasks tasks, string userName)
        {
            tasks.User = db.Users.First(i => i.UserName == userName);
            if (ModelState.IsValid)
            {
                db.Tasks.Add(tasks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TaskName,TaskDescription,Status")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tasks);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tasks tasks = db.Tasks.Find(id);
            db.Tasks.Remove(tasks);
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
