using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS.api.Models;

namespace CMS.api.Controllers
{
    public class BookingsController : Controller
    {
        private Entities db = new Entities();
        private int PersonID;
        // GET: Bookings
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.Person).Include(b => b.Schedule);
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost]
        public ActionResult Search(String SearchString)
        {
            var result = db.Bookings.Where(x => x.Booking_ID.ToString() == SearchString || x.Customer_Name == SearchString || x.Person.Person_Name == SearchString).ToList();

            if (result != null)
            {
                return View("Index", result);
            }
            else
            {
                Response.Write("<script>alert('No Bookings Found!')</script>");
            }
            return View();
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            PersonID = Convert.ToInt32(Session["PersonID"]);
            ViewBag.Person_ID = new SelectList(db.People.Where(x => x.Person_ID.Equals(PersonID)), "Person_ID", "Person_Name");
            ViewBag.Schedule_ID = new SelectList(db.Schedules, "Schedule_ID", "Schedule_Departure_Date");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Booking_ID,Schedule_ID,Person_ID,Item_Name,Item_Description,Item_Quantity,Customer_Name")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PersonID = Convert.ToInt32(Session["PersonID"]);
            ViewBag.Person_ID = new SelectList(db.People.Where(x => x.Person_ID.Equals(PersonID)), "Person_ID", "Person_Name", booking.Person_ID);
            ViewBag.Schedule_ID = new SelectList(db.Schedules, "Schedule_ID", "Schedule_Departure_Date", booking.Schedule_ID);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.Person_ID = new SelectList(db.People, "Person_ID", "Person_Name", booking.Person_ID);
            ViewBag.Schedule_ID = new SelectList(db.Schedules, "Schedule_ID", "Schedule_Status", booking.Schedule_ID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Booking_ID,Schedule_ID,Person_ID,Item_Name,Item_Description,Item_Quantity,Customer_Name")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Person_ID = new SelectList(db.People, "Person_ID", "Person_Name", booking.Person_ID);
            ViewBag.Schedule_ID = new SelectList(db.Schedules, "Schedule_ID", "Schedule_Status", booking.Schedule_ID);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
