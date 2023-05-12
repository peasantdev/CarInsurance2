using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance2.Models;

namespace CarInsurance2.Controllers
{


    public class InsureeController : Controller
    {

        private InsuranceEntities db = new InsuranceEntities();

        

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }
        // GET: Quote
        public ActionResult Quote(int? id)
        {

            Insuree insuree = db.Insurees.Find(id);
            Quote quote = new Quote();
            

            var age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            decimal totalQuote = 50;
            // Age Constraints
            if (age <= 18)
            {
                totalQuote += 100;
                quote.quoteAge = 100;
            }

            if (age >= 19 && age <= 25)
            {
                totalQuote += 50;
                quote.quoteAge = 50;
            }
            if (age > 25)
            {
                totalQuote += 25;
                quote.quoteAge = 25;
            }

            // Car Constraints
            if (quote.CarYear < 2000 || quote.CarYear > 2015)
            {
                totalQuote += 25;
                quote.quoteCar = 25;
            }
            if (quote.CarMake == "porsche")
            {
                totalQuote += 25;
                quote.quoteCar += 25;
            }
            if (quote.CarModel == "911 carrera")
            {
                totalQuote += 25;
                quote.quoteCar += 25;

            }
            // Driving History Constraints
            if (quote.SpeedingTickets > 0)
            {
                totalQuote += quote.SpeedingTickets * 10;
                quote.quoteTickets = quote.SpeedingTickets * 10;
            }
            if (quote.DUI)
            {
                totalQuote *= Convert.ToDecimal(1.25);
                quote.quoteDUI = totalQuote * Convert.ToDecimal(.25);
            }

            // Coverage Requested
            if (quote.CoverageType)
            {
                totalQuote *= Convert.ToDecimal(1.50);
            }
            ViewBag.Quote = Math.Round(totalQuote, 2);
            return View(quote);

        }
        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        //Admin Panel

        public ActionResult Admin()
        {
            return View(db.Insurees.ToList());
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

