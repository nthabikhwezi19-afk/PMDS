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
            var model = new AnnualAssessment();

            // Get logged-in user's email
            var email = User.Identity?.Name;

            if (!string.IsNullOrEmpty(email))
            {
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
            // =========================================
            // SIGNATURES ARE OPTIONAL
            // =========================================

            // Remove signature validation from ModelState
            // so the assessment can be submitted without
            // either signature.

            ModelState.Remove(nameof(model.EmployeeSignature));
            ModelState.Remove(nameof(model.SupervisorSignature));


            // =========================================
            // SAVE ASSESSMENT
            // =========================================

            if (ModelState.IsValid)
            {
                _context.AnnualAssessments.Add(model);

                _context.SaveChanges();

                TempData["Success"] =
                    "Annual Assessment saved successfully!";


                // =========================================
                // GO TO NEXT PAGE
                // =========================================

                return RedirectToAction("Moderation");
            }


            // If another required field is missing,
            // remain on the Create page.
            return View(model);
        }


        // ================================
        // MODERATION
        // ================================
        [HttpGet]
        public IActionResult Moderation()
        {
            return View();
        }
    }
}