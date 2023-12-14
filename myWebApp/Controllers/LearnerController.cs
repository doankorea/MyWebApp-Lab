using myWebApp.Models.Data;
using myWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Drawing.Printing;

namespace Lab1.Controllers
{
    public class LearnerController : Controller
    {
        private SchoolContext db;
        public LearnerController(SchoolContext context)
        {
            db = context;
        }
        //khai báo biên toàn cuc pageSize
        private int pageSize = 3;
        public IActionResult Index(int? mid) { 
            var learners = (IQueryable<Learner>)db.Learners
             .Include(m => m.Major);
            if (mid != null)
            {
                learners = (IQueryable<Learner>)db.Learners
                     .Where(l => l.MajorID == mid)
                     .Include(m => m.Major);
            }//tính } so trang
                int pageNum =(int)Math.Ceiling(learners.Count() / (float)pageSize);
                //tr sô trang vê view dê hiên thi nav-trang
                ViewBag.pageNum = pageNum;
                //lay dü liêu trang dau
                var result = learners.Take(pageSize).ToList();
                return View(result);
            }
        public IActionResult LearnerFilter(int? mid, string? keyword, int? pageIndex) {
            //lay toàn bô learners trong dbset chuyen vê IQuerrable<Learner> de query
            var learners = (IQueryable<Learner>) db.Learners;
            //lay chi so trang, nêu chi so trang null thi gán ngam dinh bang 1
            int page = (int)(pageIndex == null || pageIndex <= 0 ? 1 : pageIndex);
            //nêu có mid thi loc learner theo mid (chuyên ngành)
            if (mid != null)
            {
                //loc
                learners = learners.Where(l => l.MajorID == mid);
                //gui mid vê view dê ghi lai trên nav-phân trang
                ViewBag.mid = mid;
            }
            if (keyword != null)
            {
                //tim kiêm
                learners = learners.Where(l => l.FirstMidName.ToLower()
                            .Contains(keyword.ToLower()));
                //gui keyword vê view dê ghi trên nav-phân trang
                ViewBag.keyword = keyword;
            }
            //tính so trang
            int pageNum = (int)Math.Ceiling(learners.Count() / (float)pageSize);
            //gui so trang vê view dê hiên thi nav-trang
            ViewBag.pageNum = pageNum;
            //chon dü liêu trong trang hiên tai
            var result = learners.Skip(pageSize * (page - 1))
                 .Take(pageSize).Include(m => m.Major);
            return PartialView("LearnerTable", result);
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
            ViewBag.MajorID = majors;
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
        //thêm 2 action edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }
            var learner = db.Learners.Find(id);
            if (learner == null)
            {
                return NotFound();
            }
            ViewBag.MajorId = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,
        [Bind("LearnerID, FirstMidName, LastName, MajorID, EnrollmentDate")] Learner learner)
        {
            if (id != learner.LearnerID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(learner);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerID))
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
            ViewBag.MajorId = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }
        private bool LearnerExists(int id)
        {
            return (db.Learners?.Any(e => e.LearnerID == id)).GetValueOrDefault();
        }
        //thêm 2 action edit 
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }
            var learner = db.Learners.Include(l => l.Major)
            .Include(e => e.Enrollments)
            .FirstOrDefault(m => m.LearnerID == id);
            if (learner == null)
            {
                return NotFound();
            }
            if (learner.Enrollments.Count() > 0)
            {
                return Content("This learner has some enrollments, can't delete!");
            }
            return View(learner);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (db.Learners == null)
            {
                return Problem("Entity set 'Learners' is null.");
            }
            var learner = db.Learners.Find(id);
            if (learner != null)
            {
                db.Learners.Remove(learner);
            }
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}