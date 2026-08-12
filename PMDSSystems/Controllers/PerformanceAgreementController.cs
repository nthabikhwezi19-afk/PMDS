using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using System.Linq;

namespace PMDSSystems.Controllers
{
    public class PerformanceAgreementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerformanceAgreementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Create
        public IActionResult Create()
        {
            // Initialize with 4 KRAs
            var model = new PerformanceAgreement
            {
                KRAs = new List<KRA>
                {
                    new KRA(),
                    new KRA(),
                    new KRA(),
                    new KRA()
                }
            };
            return View(model);
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PerformanceAgreement model)
        {
            // Remove validation for fields that might not be in the form
            ModelState.Remove("JobPurpose");
            ModelState.Remove("JobKnowledge");
            ModelState.Remove("Responsibility");
            ModelState.Remove("QualityOfWork");
            ModelState.Remove("TechnicalSkills");
            ModelState.Remove("Reliability");
            ModelState.Remove("Communication");
            ModelState.Remove("TeamWork");
            ModelState.Remove("Leadership");

            if (ModelState.IsValid)
            {
                try
                {
                    // Remove any KRAs that are empty
                    if (model.KRAs != null)
                    {
                        model.KRAs = model.KRAs.Where(k =>
                            !string.IsNullOrEmpty(k.Name) ||
                            !string.IsNullOrEmpty(k.Weight) ||
                            !string.IsNullOrEmpty(k.Activities) ||
                            !string.IsNullOrEmpty(k.Standards) ||
                            !string.IsNullOrEmpty(k.BathoPele) ||
                            !string.IsNullOrEmpty(k.GAFs)
                        ).ToList();
                    }

                    // Save to database
                    _context.PerformanceAgreements.Add(model);
                    _context.SaveChanges();

                    // Set success message
                    TempData["Success"] = "Performance Agreement saved successfully!";
                    return RedirectToAction("Agreement");
                }
                catch (Exception ex)
                {
                    // Log the error
                    ModelState.AddModelError("", "Error saving data: " + ex.Message);

                    // If KRA list is null, reinitialize it for the view
                    if (model.KRAs == null)
                    {
                        model.KRAs = new List<KRA>
                        {
                            new KRA(),
                            new KRA(),
                            new KRA(),
                            new KRA()
                        };
                    }
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            if (model.KRAs == null)
            {
                model.KRAs = new List<KRA>
                {
                    new KRA(),
                    new KRA(),
                    new KRA(),
                    new KRA()
                };
            }
            return View(model);
        }

        public IActionResult Agreement()
        {
            return View();
        }

        public IActionResult Index()
        {
            var agreements = _context.PerformanceAgreements
                .Include(p => p.KRAs)
                .ToList();
            return View(agreements);
        }
    }
}