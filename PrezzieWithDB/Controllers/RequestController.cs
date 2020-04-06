using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrezzieWithDB.DAL;
using PrezzieWithDB.Models;
using PrezzieWithDB.ViewModels;

namespace PrezzieWithDB.Controllers
{
    public class RequestController : Controller
    {
        private PrezzieContext db = new PrezzieContext();

        // GET: Request
        public ActionResult Index()
        {
            var requests = db.requests.Include(r => r.customer).Include(r => r.souvenir);
            return View(requests.ToList());
        }

        // GET: Request/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var request = GetRequest(id);
            
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Request/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Request/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequestView model)
        {
            if (ModelState.IsValid)
            {
                SouvenirInfo souvenirInfo = new SouvenirInfo();
                souvenirInfo.souvenirId = model.souvenirID;
                souvenirInfo.price = model.price;
                souvenirInfo.descriptionSouv = model.descriptionSouv;

                db.souvenirInfos.Add(souvenirInfo);
                db.SaveChanges();

                Souvenir souvenir = new Souvenir();
                souvenir.souvenirId = souvenirInfo.souvenirId;
                souvenir.souvenirName = model.souvenirName;
                souvenir.countrySouv = model.countrySouv;

                db.souvenirs.Add(souvenir);
                db.SaveChanges();

                Customer customer = db.customers.Find(Session["userName"]);

                Request request = new Request();
                request.souvenir = souvenir;
                request.customer = customer;
                request.amount = model.amount;
                request.reward = model.reward;
                request.status = "new";
                request.souvenirID = model.souvenirID;

                db.requests.Add(request);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Request/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.userName = new SelectList(db.customers, "userName", "countryUser", request.userName);
            ViewBag.souvenirID = new SelectList(db.souvenirs, "souvenirId", "souvenirName", request.souvenirID);
            return View(request);
        }

        // POST: Request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "souvenirID,userName,amount,reward,status")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userName = new SelectList(db.customers, "userName", "countryUser", request.userName);
            ViewBag.souvenirID = new SelectList(db.souvenirs, "souvenirId", "souvenirName", request.souvenirID);
            return View(request);
        }

        // GET: Request/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.requests.Find(id);
            db.requests.Remove(request);
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

        public RequestView GetRequest(int? souvenirID)
        {
            RequestView rv = new RequestView();
            var request = db.requests.Find(souvenirID);

            rv.souvenirID = request.souvenirID;
            rv.userName = request.userName;
            rv.amount = request.amount;
            rv.reward = request.reward;
            rv.status = request.status;

            rv.souvenirName = request.souvenir.souvenirName;
            rv.countrySouv = request.souvenir.countrySouv;

            rv.price = request.souvenir.souvenirInfo.price;
            rv.descriptionSouv = request.souvenir.souvenirInfo.descriptionSouv;

            var customer = db.customers.Find(rv.userName);

            rv.eMail = customer.Profile.eMail;
            rv.countryUser = customer.countryUser;
            rv.firstName = customer.Profile.firstName;
            rv.surname = customer.Profile.surname;
            rv.birthday = customer.Profile.birthday;
            rv.descriptionUser = customer.Profile.descriptionUser;

            return rv;
        }
    }
}
