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
    public class ClientsController : Controller
    {
        private WatchDB db = new WatchDB();

        // GET: Clients
        public ActionResult Index()
        {
            return View(db.Clients.ToList());
        }

        [HttpPost]
        public ActionResult Index(string fname, string lname, string mail, int? phone)
        {
            var Clients = db.Clients.ToList().Where(p => (p.ClientFirstName.StartsWith(fname) && p.ClientLastName.StartsWith(lname) && p.Email.StartsWith(mail)));
            if (phone != null)
            {
                var b = Clients.ToList().Where(p => p.PhoneNumber.Equals(phone));
                return View(b.ToList());
            }
            return View(Clients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientID,ClientFirstName,ClientLastName,Email,PhoneNumber,Password")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientID,ClientFirstName,ClientLastName,Email,PhoneNumber,Password")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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

        public ActionResult Account()
        {
            if (TempData["name"] != null)
            {
                String CustomerName = TempData["name"].ToString();
                TempData.Keep();

                var deals = (from bo in db.Clients
                             join lo in db.Deals
                             on bo.ClientID equals lo.ClientID
                             where bo.ClientFirstName.StartsWith(CustomerName)
                             select lo);


                var Asset = (from bo in db.Watches
                             join lo in deals
                             on bo.WatchID equals lo.WatchID
                             where bo.WatchID == lo.WatchID
                             select new { assetName = bo.WatchName, type = bo.WatchType });


                ICollection<Asset> list = new Collection<Asset>();

                foreach (var v in Asset)
                {
                    list.Add(new Asset(Asset.Count(), v.assetName, v.type));

                }

                ViewBag.data = list;
                ICollection<Stat> pList = new Collection<Stat>();

                var Asset2 = (from bo in db.Watches
                              join lo in deals
                              on bo.WatchID equals lo.WatchID
                              where bo.WatchID == lo.WatchID
                              group bo by bo.WatchType into j
                              select j);

                foreach (var v in Asset2)
                {
                    pList.Add(new Stat(v.Key, v.Count()));
                }

                int max = 0;
                foreach (var c in pList)
                {
                    if (c.Values > max)
                    {
                        max = c.Values;
                        ViewBag.type = c.Key;
                    }
                }
                TempData.Keep();
                return View(db.Deals.ToList());
            }

            return RedirectToAction("Index");
        }
    }

    public class Asset
    {
        int num;
        public string assetName;
        public string assetType;

        public Asset() { }
        public Asset(int num, string assetName, string assetType)
        {
            this.num = num;
            this.assetName = assetName;
            this.assetType = assetType;
        }
    }
}

