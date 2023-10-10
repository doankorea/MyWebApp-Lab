using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myWebApp.Models;
using myWebApp.Models.Data;
using System;
using System.Security.Cryptography;

namespace myWebApp.Controllers
{
    public class LearnerController : Controller
    {

        private SchoolContext db;
        public LearnerController(SchoolContext context)
        {
            db = context;
        }
        public IActionResult Index(int? mid)
        {
            if(mid == null)
            {
                var learners = db.Learners.Include(m => m.Major).ToList();
                return View(learners);

            }
            else
            {
                var learners= db.Learners.Where(l=> l.MajorID== mid).Include(m=> m.Major).ToList(); 
                return View(learners);

            }
        }
        public IActionResult LearnerByMajorID(int mid)
        { 
        var learners = db.Learners
         .Where(l=>l.MajorID == mid)
         .Include(m=>m.Major).ToList();
         return PartialView("LearnerTable", learners);
            
        }
        public IActionResult Create()
        {
            //dùng 1 trong 2 cách để tạo SelectList gửi về View qua ViewBag để
            //hiển thị danh sách chuyên ngành (Majors)
            var majors = new List<SelectListItem>(); //cách 1
            foreach (var item in db.Majors)
            {
                majors.Add(new SelectListItem
                {
                    Text = item.MajorName,
                    Value = item.MajorID.ToString()
                });
            }
            ViewBag.LearnerID = majors;
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName"); //cách 2
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")]
Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            //lại dùng 1 trong 2 cách tạo SelectList gửi về View để hiển thị danh sách Majors
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }
        //edit
        
        public IActionResult Edit(int id)
        {
           
            
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }
            var learner = db.Learners.Find(id);
            if(learner == null)
            {
                return NotFound();
            }
            ViewBag.MajorId = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);

         
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learners)
        {
            if (id != learners.LearnerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(learners);
                    db.SaveChanges();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learners.LearnerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Tạo SelectList để hiển thị danh sách Majors
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learners.MajorID);
            return View(learners);  
        }
        private bool LearnerExists(int id)
        {
            return (db.Learners?.Any(e => e.LearnerID == id)).GetValueOrDefault();
        }

        // Delete
        
        public IActionResult Delete(int id)
        {

            if (id == null || db.Learners == null)
            {
                return NotFound();
            }

            var learners = db.Learners.Include(l=> l.Major).Include(e=>e.Enrollments).FirstOrDefault(m=>m.LearnerID== id);
            if (learners == null)
            {
                return NotFound();
            }
            if (learners.Enrollments.Count() > 0)
            {
                return Content("This learner has some enrollments, can't delete!");
            }
            
            return View(learners);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            
            if (db.Learners == null)
            {
                return Problem("Entity set 'Learners' is null");
            }
            var learner = db.Learners.Find(id);
            if(learner != null)
            {
                db.Learners.Remove(learner);
            }
           
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }



}
