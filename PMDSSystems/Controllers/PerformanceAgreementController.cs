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

        // ============================================================
        // GET: PerformanceAgreement/Create
        // ============================================================
        [HttpGet]
        public IActionResult Create()
        {
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


        // ============================================================
        // POST: PerformanceAgreement/Create
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerformanceAgreement model)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("PERFORMANCE AGREEMENT CREATE POST HIT");
            Console.WriteLine("Job Tasks: [" + model.JobRelatedTasks + "]");
            Console.WriteLine("Salary Level: [" + model.SalaryLevel + "]");
            Console.WriteLine("Number of KRAs: " + model.NumberOfKRAs);
            Console.WriteLine("KRA COUNT: " + (model.KRAs?.Count ?? 0));

            if (model.KRAs != null)
            {
                foreach (var kra in model.KRAs)
                {
                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("KRA Name: [" + kra.Name + "]");
                    Console.WriteLine("KRA Weight: [" + kra.Weight + "]");
                    Console.WriteLine("Activities: [" + kra.Activities + "]");
                    Console.WriteLine("Standards: [" + kra.Standards + "]");
                    Console.WriteLine("Batho Pele: [" + kra.BathoPele + "]");
                    Console.WriteLine("GAFs: [" + kra.GAFs + "]");
                }
            }

            Console.WriteLine("========================================");

            // Remove validation for fields not being used
            ModelState.Remove("JobPurpose");
            ModelState.Remove("JobKnowledge");
            ModelState.Remove("Responsibility");
            ModelState.Remove("QualityOfWork");
            ModelState.Remove("TechnicalSkills");
            ModelState.Remove("Reliability");
            ModelState.Remove("Communication");
            ModelState.Remove("TeamWork");
            ModelState.Remove("Leadership");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("MODEL STATE IS INVALID");

                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine(
                            "FIELD: " + state.Key +
                            " ERROR: " + error.ErrorMessage
                        );
                    }
                }

                return View(model);
            }

            try
            {
                // Make sure KRA list exists
                if (model.KRAs == null)
                {
                    model.KRAs = new List<KRA>();
                }

                // Remove empty KRA rows
                model.KRAs = model.KRAs
                    .Where(k =>
                        !string.IsNullOrWhiteSpace(k.Name) ||
                        !string.IsNullOrWhiteSpace(k.Weight) ||
                        !string.IsNullOrWhiteSpace(k.Activities) ||
                        !string.IsNullOrWhiteSpace(k.Standards) ||
                        !string.IsNullOrWhiteSpace(k.BathoPele) ||
                        !string.IsNullOrWhiteSpace(k.GAFs)
                    )
                    .ToList();

                // Set actual number of KRAs
                model.NumberOfKRAs = model.KRAs.Count;

                // --------------------------------------------------
                // SAVE PERFORMANCE AGREEMENT FIRST
                // --------------------------------------------------

                _context.PerformanceAgreements.Add(model);

                await _context.SaveChangesAsync();

                Console.WriteLine("========================================");
                Console.WriteLine("PERFORMANCE AGREEMENT SAVED");
                Console.WriteLine("AGREEMENT ID: " + model.Id);
                Console.WriteLine("========================================");

                // --------------------------------------------------
                // SAVE KRAs
                // --------------------------------------------------

                foreach (var kra in model.KRAs)
                {
                    kra.Id = 0;

                    kra.PerformanceAgreementId = model.Id;

                    _context.KRAs.Add(kra);
                }

                await _context.SaveChangesAsync();

                Console.WriteLine("========================================");
                Console.WriteLine("KRAs SAVED SUCCESSFULLY");
                Console.WriteLine("========================================");

                return Content(
                    "SUCCESS! Performance Agreement saved successfully. " +
                    "Agreement ID: " + model.Id +
                    " | Number of KRAs: " + model.KRAs.Count
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("DATABASE SAVE ERROR");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("========================================");

                return Content(
                    "DATABASE ERROR: " +
                    (ex.InnerException?.Message ?? ex.Message)
                );
            }
        }


        // ============================================================
        // GET: PerformanceAgreement/Agreement
        // ============================================================
        [HttpGet]
        public IActionResult Agreement()
        {
            return View();
        }


        // ============================================================
        // GET: PerformanceAgreement/Index
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var agreements = await _context.PerformanceAgreements
                .Include(p => p.KRAs)
                .AsNoTracking()
                .ToListAsync();

            return View(agreements);
        }
    }
}