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
    public class SouvenirsController : Controller
    {
        private PrezzieContext db = new PrezzieContext();


        public ActionResult CreateSouvenir()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CreateSouvenir(SouvenirView model)
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

                return RedirectToAction("Index");

            }

            return View();

        }




        // GET: Souvenirs
        public ActionResult Index()
        {
            var souvenirs = db.souvenirs.Include(s => s.souvenirInfo);
            return View(souvenirs.ToList());
        }

        // GET: Souvenirs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Souvenir souvenir = db.souvenirs.Find(id);
            if (souvenir == null)
            {
                return HttpNotFound();
            }
            return View(souvenir);
        }

       

        // GET: Souvenirs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Souvenir souvenir = db.souvenirs.Find(id);
            if (souvenir == null)
            {
                return HttpNotFound();
            }
            ViewBag.souvenirId = new SelectList(db.souvenirInfos, "souvenirId", "description", souvenir.souvenirId);
            return View(souvenir);
        }

        // POST: Souvenirs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "souvenirId,souvenirName,countrySouv")] Souvenir souvenir)
        {
            if (ModelState.IsValid)
            {
                db.Entry(souvenir).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.souvenirId = new SelectList(db.souvenirInfos, "souvenirId", "description", souvenir.souvenirId);
            return View(souvenir);
        }

        // GET: Souvenirs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Souvenir souvenir = db.souvenirs.Find(id);
            if (souvenir == null)
            {
                return HttpNotFound();
            }
            return View(souvenir);
        }

        // POST: Souvenirs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Souvenir souvenir = db.souvenirs.Find(id);
            db.souvenirs.Remove(souvenir);
            db.SaveChanges();
            SouvenirInfo souvenirInfo = db.souvenirInfos.Find(id);
            db.souvenirInfos.Remove(souvenirInfo);
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
