using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PMDSSystems.Controllers
{
    public class AnnualAssessmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnualAssessmentController(ApplicationDbContext context)
        {
            _context = context;
        }


        // ============================================================
        // CREATE - GET
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AnnualAssessment();

            // ========================================================
            // GET LOGGED-IN EMPLOYEE
            // ========================================================

            var email = User.Identity?.Name;

            if (!string.IsNullOrWhiteSpace(email))
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == email);

                if (employee != null)
                {
                    model.PersalNumber = employee.PersalNumber;
                }
            }


            // ========================================================
            // LOAD LATEST PERFORMANCE AGREEMENT
            //
            // THIS IS THE SAME APPROACH AS YOUR WORKING MID-TERM CODE
            // ========================================================

            var performanceAgreement =
                await _context.PerformanceAgreements
                    .Include(p => p.KRAs)
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefaultAsync();


            // ========================================================
            // CHECK WHETHER PERFORMANCE AGREEMENT WAS FOUND
            // ========================================================

            if (performanceAgreement == null)
            {
                ViewBag.KraMessage =
                    "No Performance Agreement was found.";

                return View(model);
            }


            // ========================================================
            // CHECK WHETHER KRAs WERE FOUND
            // ========================================================

            if (performanceAgreement.KRAs == null ||
                !performanceAgreement.KRAs.Any())
            {
                ViewBag.KraMessage =
                    "Performance Agreement was found, but no KRAs were found.";

                return View(model);
            }


            // ========================================================
            // LOAD KRAs
            // ========================================================

            var kras = performanceAgreement.KRAs
                .OrderBy(k => k.Id)
                .Take(4)
                .ToList();


            // ========================================================
            // KRA 1
            // ========================================================

            if (kras.Count >= 1)
            {
                model.KRA1Name = kras[0].Name;
                model.KRA1Weight = ParseWeight(kras[0].Weight);
            }


            // ========================================================
            // KRA 2
            // ========================================================

            if (kras.Count >= 2)
            {
                model.KRA2Name = kras[1].Name;
                model.KRA2Weight = ParseWeight(kras[1].Weight);
            }


            // ========================================================
            // KRA 3
            // ========================================================

            if (kras.Count >= 3)
            {
                model.KRA3Name = kras[2].Name;
                model.KRA3Weight = ParseWeight(kras[2].Weight);
            }


            // ========================================================
            // KRA 4
            // ========================================================

            if (kras.Count >= 4)
            {
                model.KRA4Name = kras[3].Name;
                model.KRA4Weight = ParseWeight(kras[3].Weight);
            }


            // ========================================================
            // DISPLAY SUCCESS MESSAGE FOR DEBUGGING
            // ========================================================

            ViewBag.KraMessage =
                $"{kras.Count} KRA(s) loaded from Performance Agreement #{performanceAgreement.Id}.";


            return View(model);
        }


        // ============================================================
        // PARSE PERFORMANCE AGREEMENT WEIGHT
        // ============================================================

        private static int ParseWeight(string? weight)
        {
            if (string.IsNullOrWhiteSpace(weight))
            {
                return 0;
            }

            weight = weight
                .Replace("%", "")
                .Trim();

            if (decimal.TryParse(weight, out decimal result))
            {
                return (int)result;
            }

            return 0;
        }


        // ============================================================
        // CREATE - POST
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            AnnualAssessment model)
        {
            // ========================================================
            // SIGNATURES
            // ========================================================

            ModelState.Remove(nameof(model.EmployeeSignature));
            ModelState.Remove(nameof(model.SupervisorSignature));


            // ========================================================
            // GET LOGGED-IN EMPLOYEE
            // ========================================================

            var email = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }


            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == email);


            if (employee == null)
            {
                return NotFound(
                    "Employee profile could not be found.");
            }


            // ========================================================
            // AUTO-POPULATE PERSAL
            // ========================================================

            model.PersalNumber = employee.PersalNumber;


            // ========================================================
            // LOAD PERFORMANCE AGREEMENT
            //
            // SAME AS MID-TERM
            // ========================================================

            var performanceAgreement =
                await _context.PerformanceAgreements
                    .Include(p => p.KRAs)
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefaultAsync();


            // ========================================================
            // LOAD KRAs AGAIN FROM DATABASE
            // ========================================================

            if (performanceAgreement != null &&
                performanceAgreement.KRAs != null &&
                performanceAgreement.KRAs.Any())
            {
                var kras = performanceAgreement.KRAs
                    .OrderBy(k => k.Id)
                    .Take(4)
                    .ToList();


                // ====================================================
                // KRA 1
                // ====================================================

                model.KRA1Name =
                    kras.Count >= 1
                        ? kras[0].Name
                        : null;

                model.KRA1Weight =
                    kras.Count >= 1
                        ? ParseWeight(kras[0].Weight)
                        : 0;


                // ====================================================
                // KRA 2
                // ====================================================

                model.KRA2Name =
                    kras.Count >= 2
                        ? kras[1].Name
                        : null;

                model.KRA2Weight =
                    kras.Count >= 2
                        ? ParseWeight(kras[1].Weight)
                        : 0;


                // ====================================================
                // KRA 3
                // ====================================================

                model.KRA3Name =
                    kras.Count >= 3
                        ? kras[2].Name
                        : null;

                model.KRA3Weight =
                    kras.Count >= 3
                        ? ParseWeight(kras[2].Weight)
                        : 0;


                // ====================================================
                // KRA 4
                // ====================================================

                model.KRA4Name =
                    kras.Count >= 4
                        ? kras[3].Name
                        : null;

                model.KRA4Weight =
                    kras.Count >= 4
                        ? ParseWeight(kras[3].Weight)
                        : 0;
            }


            // ========================================================
            // SAVE
            // ========================================================

            if (ModelState.IsValid)
            {
                _context.AnnualAssessments.Add(model);

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Annual Assessment saved successfully!";

                return RedirectToAction(nameof(Moderation));
            }


            return View(model);
        }


        // ============================================================
        // MODERATION
        // ============================================================

        [HttpGet]
        public IActionResult Moderation()
        {
            return View();
        }
    }
}