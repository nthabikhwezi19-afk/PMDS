using Microsoft.AspNetCore.Mvc;
using PMDSSystems.Data;
using PMDSSystems.Models;

namespace PMDSSystems.Controllers
{
    public class AnnualAssessmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnualAssessmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AnnualAssessment model)
        {
            if (ModelState.IsValid)
            {
                _context.AnnualAssessments.Add(model);
                _context.SaveChanges();

                TempData["Success"] = "Saved successfully!";
                return RedirectToAction("Create");
            }

            return View(model);
        }
        public IActionResult Moderation()
        {
            return View();
        }
    }

}