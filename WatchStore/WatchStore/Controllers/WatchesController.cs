using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatchStore.Models;
using WatchStore.Controllers;

namespace WatchStore.Controllers
{
    public class WatchesController : Controller
    {
        private WatchDB db = new WatchDB();
        static bool activeDelete = false;
        static IDictionary<int,bool> activeDel = new Dictionary<int,bool>();

        // GET: Watches
        public ActionResult Index()
        {
            return View(db.Watches.ToList());
        }


        [HttpPost]
        public ActionResult Index(string name, string type, int? price)
        {
            var watches = db.Watches.ToList().Where(w => (w.WatchName.StartsWith(name) && w.WatchType.StartsWith(type)));
            if (price != null)
            {
                var b = watches.ToList().Where(w => w.price.Equals(price));
                return View(b.ToList());
            }
            return View(watches.ToList());
        }

        // GET: Watches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Watch watch = db.Watches.Find(id);
            if (watch == null)
            {
                return HttpNotFound();
            }
            return View(watch);
        }

        // GET: Watches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Watches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WatchID,WatchName,Brand,WatchType,Gender,Resistant,price")] Watch watch)
        {
            if (ModelState.IsValid)
            {
                db.Watches.Add(watch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(watch);
        }

        // GET: Watches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Watch watch = db.Watches.Find(id);
            if (watch == null)
            {
                return HttpNotFound();
            }
            return View(watch);
        }

        // POST: Watches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WatchID,WatchName,Brand,WatchType,Gender,Resistant,price")] Watch watch)
        {
            if (activeDelete)
            {
                activeDelete = false;
                if(activeDel.ContainsKey(watch.WatchID))
                {
                    activeDel.Remove(watch.WatchID);
                    return RedirectToAction("Error");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(watch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(watch);
        }

        // GET: Watches/Delete/5
        public ActionResult Delete(int? id)
        {
  
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Watch watch = db.Watches.Find(id);
            if (watch == null)
            {
                return HttpNotFound();
            }
            activeDelete = true;
            if (!activeDel.ContainsKey((int) id))
            {
                activeDel.Add((int)id, true);
            }
            return View(watch);
        }

        // POST: Watches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Watch watch = db.Watches.Find(id);
            db.Watches.Remove(watch);
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

        [HttpGet]
        public ActionResult Group()
        {
            var group = (from bo in db.Watches
                         group bo by bo.WatchType into j
                         select new Group<string, Watch> { Key = j.Key, Values = j });
            return View(group.ToList());
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

    }
}
