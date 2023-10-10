using Microsoft.AspNetCore.Mvc;
using myWebApp.Models;
using myWebApp.Models.Data;

namespace myWebApp.ViewComponents
{
    public class MajorViewComponent:ViewComponent
    {
        SchoolContext db;
        List<Major> majors;
        public MajorViewComponent(SchoolContext _context)
        {
            db= _context;
            majors= db.Majors.ToList();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RenderMajor", majors);
        }
    }
}
