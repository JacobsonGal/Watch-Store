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
    public class DealsController : Controller
    {
        private WatchDB db = new WatchDB();
        static IDictionary<int,List<Watch>> cart = new Dictionary<int,List<Watch>>();

        // GET: Deals
        public ActionResult Index()
        {
            var deals = db.Deals.Include(d => d.Client).Include(d => d.Watch);
            return View(deals.ToList());
        }

        [HttpPost]
        public ActionResult Index(string name, string type, int? deal)
        {
            var Deals = db.Deals.ToList().Where(p => (p.Client.ClientFirstName.StartsWith(name) && p.Watch.WatchType.StartsWith(type)));
            if (deal != null)
            {
                var b = Deals.ToList().Where(p => p.DealID.Equals(deal));
                return View(b.ToList());
            }
            return View(Deals.ToList());
        }


        // GET: Deals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // GET: Deals/Create
        public ActionResult Create()
        {
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientFirstName");
            ViewBag.WatchID = new SelectList(db.Watches, "WatchID", "WatchName");
            return View();
        }

        // POST: Deals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DealID,ClientID,WatchID")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                db.Deals.Add(deal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientFirstName", deal.ClientID);
            ViewBag.WatchID = new SelectList(db.Watches, "WatchID", "WatchName", deal.WatchID);
            return View(deal);
        }

        // GET: Deals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientFirstName", deal.ClientID);
            ViewBag.WatchID = new SelectList(db.Watches, "WatchID", "WatchName", deal.WatchID);
            return View(deal);
        }

        // POST: Deals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DealID,ClientID,WatchID")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientFirstName", deal.ClientID);
            ViewBag.WatchID = new SelectList(db.Watches, "WatchID", "WatchName", deal.WatchID);
            return View(deal);
        }

        // GET: Deals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // POST: Deals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deal deal = db.Deals.Find(id);
            db.Deals.Remove(deal);
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
        public ActionResult Statistics()
        {
            ICollection<Stat> mylist = new Collection<Stat>();
            var r = (from bo in db.Watches
                     group bo by bo.WatchType into j
                     select j);

            foreach (var v in r)
            {
                mylist.Add(new Stat(v.Key, v.Count()));
            }

            ViewBag.data = mylist;

            ICollection<Stat> mylist2 = new Collection<Stat>();

            var q = (from lo in db.Deals
                     join bo in db.Watches
                     on lo.WatchID equals bo.WatchID
                     where lo.WatchID == bo.WatchID
                     group bo by bo.WatchName into j
                     select j);

            foreach (var v in q)
            {
                mylist2.Add(new Stat(v.Key, v.Count()));
            }

            ViewBag.data2 = mylist2;

            return View();
        }

        [HttpGet]
        public ActionResult Cart()
        {
            TempData["numOfProducts"] = cart.Count();
            if (TempData["name"] != null)
                return View(cart.Values.ToList());

            return View();
        }

        [HttpPost]
        public ActionResult Cart(string checkout)
        {
            foreach(var item in cart)
            {
                Deal deal = new Deal();
                var clients = db.Clients.ToList().Where(p => (p.ClientFirstName.StartsWith(TempData["name"].ToString())));
                deal.Client = clients.ToList().First();
                for (int j = 0; j < item.Value.Count(); j++) { 
                    deal.Watch = db.Watches.Find(item.Value.ElementAt(0).WatchID);
                    if (ModelState.IsValid)
                    {
                        db.Deals.Add(deal);
                        db.SaveChanges();
                    }
                }
            }
            cart.Clear();
            TempData["totalamount"] = 0;
            TempData["numOfProducts"] = 0;
            return RedirectToAction("Cart");
            //return View(deal);
        }

        public ActionResult AddToCart(int watchid)
        {
            Watch temp = db.Watches.Find(watchid);
            if (cart.ContainsKey(watchid))
            {
                foreach (var item in cart)
                {
                    if (item.Key.Equals(watchid))
                    {
                        item.Value.Add(temp);
                    }
                }
            }
            else
            {
                List<Watch> templst = new List<Watch>();
                templst.Add(temp);
                cart.Add(watchid, templst);
            }

            TempData["totalamount"] = (int.Parse(TempData["totalamount"].ToString())+temp.price).ToString();
            TempData["numOfProducts"] = (int.Parse(TempData["numOfProducts"].ToString()) + 1).ToString();
            return new RedirectResult(Url.Action("Index","Home") + "#gallery");
        }

        public ActionResult RemoveFromCart(int watchid)
        {
            Watch temp = db.Watches.Find(watchid);
            foreach (var item in cart)
            {
                if (item.Key.Equals(watchid))
                {
                    if (item.Value.Count() > 1)
                        item.Value.RemoveAt(0);
                    else if (item.Value.Count() == 1)
                        cart.Remove(watchid);
                }
            }
            TempData["totalamount"] = (int.Parse(TempData["totalamount"].ToString()) - temp.price).ToString();
            TempData["numOfProducts"] = (int.Parse(TempData["numOfProducts"].ToString()) - 1 ).ToString();
            return RedirectToAction("Cart", "Deals");
        }

    }



    public class Group<K, T>
    {
        public K Key { get; set; }
        public IEnumerable<T> Values { get; set; }
    }
    public class Stat
    {
        public string Key;
        public int Values;

        public Stat(string key, int values)
        {
            Key = key;
            Values = values;
        }
    }
}

