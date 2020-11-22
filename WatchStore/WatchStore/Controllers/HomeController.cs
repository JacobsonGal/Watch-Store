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
    public class HomeController : Controller
    {
        IDictionary<string, string> ManagerMap = new Dictionary<string, string>();
        IDictionary<string, string> ClientsMap = new Dictionary<string, string>();
        private WatchDB db = new WatchDB();
        static int smartCounter = 0;
        static int analogCounter = 0;
        static int sportCounter = 0;
        static int digitalCounter = 0;

        public ActionResult Index()
        {
            return View(db.Watches.ToList());
        }

        [HttpPost]
        public ActionResult Index(string type, string brand, string gender, int? price)
        {
            var watches = db.Watches.ToList().Where(w => (w.Brand.StartsWith(brand) && w.WatchType.StartsWith(type) && w.Gender.StartsWith(gender)));
            if (price != null)
            {
                var b = watches.ToList().Where(w => w.price < price);
                return View(b.ToList());
            }

            if (type.StartsWith("Smart Watch"))
                smartCounter++;
            else if (type.StartsWith("Analog Watch"))
                analogCounter++;
            else if (type.StartsWith("Sport Watch"))
                sportCounter++;
            else if (type.StartsWith("Digital Watch"))
                digitalCounter++;

            if (smartCounter > analogCounter && smartCounter > sportCounter && smartCounter > digitalCounter)
                TempData["sugType"] = "Smart Watch";
            else if (analogCounter > smartCounter && analogCounter > sportCounter && analogCounter > digitalCounter)
                TempData["sugType"] = "Analog Watch";
            else if (sportCounter > smartCounter && sportCounter > analogCounter && sportCounter > digitalCounter)
                TempData["sugType"] = "Sport Watch";
            else
                TempData["sugType"] = "Digital Watch";

            return View(watches.ToList());
        }

        [HttpPost]
        public ActionResult Login(string usrname, string usrpassword)
        {
            if (usrname != null)
            {
                TempData["name"] = usrname.ToString();
               // TempData["img"] = "/Content/Resources/Team/" + name.ToString() + ".jpeg";
                TempData["img"] = "https://media-exp1.licdn.com/dms/image/C4E03AQFztoBPChLfwQ/profile-displayphoto-shrink_200_200/0?e=1609977600&v=beta&t=v30MRlwSjrdXtrrxwEPTf-qr6HCrPTfH7SX__TUprJo";
            }
 
            foreach (Manager b in db.Managers)
            {
                ManagerMap.Add(b.ManagerName, b.ManagerPassword);
            }
            foreach (Client c in db.Clients)
            {
                ClientsMap.Add(c.ClientFirstName, c.Email);
            }

            if (ManagerMap.ContainsKey(usrname))
            {
                if (ManagerMap[usrname].Equals(usrpassword))
                {
                    TempData["Role"] = "Admin";
                }
            }
            else if (ClientsMap.ContainsKey(usrname))
            {
                if (ClientsMap[usrname].Equals(usrpassword))
                {
                    TempData["Role"] = "Customer";
                    TempData["totalamount"] = 0;
                }
            }
            else
                TempData["Role"] = "Guest";
            TempData["numOfProducts"]=0;
            TempData.Keep();

            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            TempData["name"] = null;
            TempData["Role"] = null;
            return RedirectToAction("Index");
        }

    
    }
}