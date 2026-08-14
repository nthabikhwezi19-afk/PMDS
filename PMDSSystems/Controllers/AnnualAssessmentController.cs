using Microsoft.AspNetCore.Mvc;
using PMDSSystems.Data;
using PMDSSystems.Models;
using System.Linq;

namespace PMDSSystems.Controllers
{
    public class AnnualAssessmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnualAssessmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================================
        // CREATE - GET
        // ================================
        [HttpGet]
        public IActionResult Create()
        {
            // Create a new Annual Assessment model
            var model = new AnnualAssessment();

            // Get the logged-in user's email
            var email = User.Identity?.Name;

            if (!string.IsNullOrEmpty(email))
            {
                // Find employee using their email
                var employee = _context.Employees
                    .FirstOrDefault(e => e.Email == email);

                if (employee != null)
                {
                    // Automatically populate Persal Number
                    model.PersalNumber = employee.PersalNumber;
                }
            }

            return View(model);
        }


        // ================================
        // CREATE - POST
        // ================================
        [HttpPost]
        [ValidateAntiForgeryToken]
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


        // ================================
        // MODERATION
        // ================================
        public IActionResult Moderation()
        {
            return View();
        }
    }
}