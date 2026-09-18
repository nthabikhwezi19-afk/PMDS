using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using PMDSSystems.ViewModels;

namespace PMDSSystems.Controllers
{
    [Authorize]
    public class PMDSController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PMDSController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //// =============================
        //// CREATE PMDS FORM
        //// =============================
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PMDSForm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = await _context.PMDSForms
                .FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId);

            if (existing == null)
            {
                _context.PMDSForms.Add(model);
            }
            else
            {
                existing.PersalNumber = model.PersalNumber;
                existing.SurnameInitials = model.SurnameInitials;
                existing.Directorate = model.Directorate;
                existing.PostDesignation = model.PostDesignation;
                existing.SupervisorName = model.SupervisorName;
                existing.AppointmentDate = model.AppointmentDate;
                existing.AppointmentDateInCurrentRank = model.AppointmentDateInCurrentRank;
                existing.CurrentRank = model.CurrentRank;
                existing.RelatedOSDDescription = model.RelatedOSDDescription;
                existing.PreviousStation =  model.PreviousStation;
                existing.TransferDate = model.TransferDate;
                existing.PreviousCyclePerformance = model.PreviousCyclePerformance;


                _context.PMDSForms.Update(existing);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Part A saved successfully.";

            return RedirectToAction("Create", "PerformanceAgreement");
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                return NotFound("Employee profile not found.");
            }

            var model = new PMDSForm
            {
                EmployeeId = employee.Id,

                PersalNumber = employee.PersalNumber,

                SurnameInitials =
                    $"{employee.LastName} {employee.Initials}",

                Directorate =
                    employee.Department ?? "",

                PostDesignation =
                    !string.IsNullOrWhiteSpace(employee.PostDesignation)
                        ? employee.PostDesignation
                        : employee.Position,

                CurrentRank =
                    employee.PostLevel ?? "",

                RelatedOSDDescription =
                    employee.OSDDescription ?? "",

                // Appointment Date in DCS
                AppointmentDate =
                    employee.AppointmentInDcsDate,

                // Appointment / Promotion Date in Current Rank
                AppointmentDateInCurrentRank =
                    employee.AppointmentDateInCurrentRank
            };


            /* =====================================================
               SUPERVISOR
               ===================================================== */

            if (employee.SupervisorId != null)
            {
                var supervisor = await _context.Employees
                    .FirstOrDefaultAsync(
                        e => e.Id == employee.SupervisorId);

                if (supervisor != null)
                {
                    model.SupervisorName =
                        $"{supervisor.FirstName} {supervisor.LastName}";

                    model.SupervisorSurnameInitials =
                        $"{supervisor.LastName} {supervisor.Initials}";

                    model.SupervisorRankPostLevel =
                        supervisor.PostLevel ?? "";
                }
            }


            return View(model);
        }

        // =============================
        // PERSONAL DEVELOPMENT PLAN (PDP)
        // =============================
        [HttpGet]
        public async Task<IActionResult> PersonalDevelopmentPlan()
        {
            var userId = _userManager.GetUserId(User);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                return NotFound("No employee record is linked to this account.");
            }

            Employee? supervisor = null;

            if (employee.SupervisorId.HasValue)
            {
                supervisor = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == employee.SupervisorId.Value);
            }

            var model = new PDPModel
            {
                Id = employee.Id,
                Surname = employee.LastName,
                PersalNo = employee.PersalNumber,
                Directorate = employee.Department,
                IdNumber = employee.IdentificationNumber,
                Branch = employee.BranchOrRegion,
                SalaryLevel = employee.SalaryLevel,
                Gender = employee.Gender,
                Race = employee.Race,
                SupervisorPosition = supervisor?.PostLevel
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SavePDP(PDPModel model)
        {
            // TODO: Save to database if needed
            return RedirectToAction("Success");
        }

        // ============================================================
        // MID-TERM REVIEW
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> MidTermReview(
            int? id,
            string employeeId)
        {
            MidTermReview review;

            // ============================================================
            // EXISTING MID-TERM REVIEW
            // ============================================================
            if (id.HasValue)
            {
                review = await _context.MidTermReviews
                    .Include(r => r.KraEvaluations)
                    .FirstOrDefaultAsync(r => r.Id == id.Value);

                if (review == null)
                {
                    return NotFound();
                }

                return View(review);
            }

            // ============================================================
            // NEW MID-TERM REVIEW
            // ============================================================
            review = new MidTermReview
            {
                EmployeeId = employeeId ?? "Unassigned",

                ReviewPeriod = "1 April 2025 - 31 October 2025",

                KraEvaluations = new List<MidTermKraEvaluation>()
            };

            // ============================================================
            // LOAD THE LATEST PERFORMANCE AGREEMENT
            // ============================================================
            var performanceAgreement = await _context.PerformanceAgreements
                .Include(p => p.KRAs)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            // ============================================================
            // LOAD KRAs FROM PERFORMANCE AGREEMENT
            // ============================================================
            if (performanceAgreement != null &&
                performanceAgreement.KRAs != null &&
                performanceAgreement.KRAs.Any())
            {
                int kraNumber = 1;

                foreach (var kra in performanceAgreement.KRAs.OrderBy(k => k.Id))
                {
                    decimal weight = 0;

                    if (!string.IsNullOrWhiteSpace(kra.Weight))
                    {
                        decimal.TryParse(
                            kra.Weight,
                            out weight);
                    }

                    review.KraEvaluations.Add(
                        new MidTermKraEvaluation
                        {
                            KraNumber = kraNumber,

                            KraDescription = kra.Name ?? string.Empty,

                            Weight = weight,

                            AchievementStandard = string.Empty,

                            SupervisorComments = string.Empty,

                            OwnRating = 0,

                            SupervisorRating = 0,

                            AgreedRating = 0
                        });

                    kraNumber++;
                }
            }

            // ============================================================
            // FALLBACK
            // ============================================================
            if (!review.KraEvaluations.Any())
            {
                for (int i = 1; i <= 4; i++)
                {
                    review.KraEvaluations.Add(
                        new MidTermKraEvaluation
                        {
                            KraNumber = i,

                            KraDescription = $"KRA {i}",

                            Weight = 0
                        });
                }
            }

            return View(review);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMidTermReview(
            MidTermReview model)
        {
            decimal totalWeight =
                model.KraEvaluations.Sum(k => k.Weight);

            if (totalWeight != 100)
            {
                ModelState.AddModelError(
                    string.Empty,
                    $"Total KRA weight must be 100%. " +
                    $"Current: {totalWeight}%");
            }

            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    _context.MidTermReviews.Add(model);
                }
                else
                {
                    _context.MidTermReviews.Update(model);
                }

                await _context.SaveChangesAsync();



                return RedirectToAction(
                    nameof(MidTermReview),
                    new { id = model.Id });
            }

            return View(
                "MidTermReview",
                model);
        }
        [HttpGet]
        public async Task<IActionResult> Cycles()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                return NotFound(
                    "Your employee profile has not been linked to your user account.");
            }

            var model = new CyclesViewModel
            {
                Employee = employee
            };

            // Find employees assigned to the logged-in employee
            model.MyEmployees = await _context.Employees
                .Where(e => e.SupervisorId == employee.Id)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();

            return View(model);
        }





        // =============================
        // PERSONAL ASSISTANCE PLAN (PAP)
        // =============================

        [HttpGet]
        public async Task<IActionResult> PersonalAssistancePlan(int? id)
        {
            PersonalAssistancePlan pap;

            // Existing PAP
            if (id.HasValue)
            {
                pap = await _context.PersonalAssistancePlans
                    .FindAsync(id.Value);

                if (pap == null)
                {
                    return NotFound();
                }

                return View(pap);
            }

            // New PAP
            pap = new PersonalAssistancePlan();

            // Get logged-in user
            var userId = _userManager.GetUserId(User);

            if (!string.IsNullOrEmpty(userId))
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.UserId == userId);

                if (employee != null)
                {
                    pap.PersalNo = employee.PersalNumber;

                    pap.EmployeeName =
                        $"{employee.FirstName} {employee.LastName}";

                    pap.Post =
                        employee.PostDesignation
                        ?? employee.Position
                        ?? string.Empty;

                    pap.SupervisorName =
                        employee.SupervisorSurnameInitials
                        ?? string.Empty;
                }
            }

            return View(pap);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePersonalAssistancePlan(
            PersonalAssistancePlan model)
        {
            // Remove validation errors caused by read-only/system fields
            ModelState.Remove(nameof(model.EmployeeName));
            ModelState.Remove(nameof(model.Post));
            ModelState.Remove(nameof(model.PersalNo));
            ModelState.Remove(nameof(model.SupervisorName));
            ModelState.Remove(nameof(model.SupervisorSignature));
            ModelState.Remove(nameof(model.SupervisorInitialsAndSurname));

            // Save the PAP
            if (model.Id == 0)
            {
                _context.PersonalAssistancePlans.Add(model);
            }
            else
            {
                _context.PersonalAssistancePlans.Update(model);
            }

            if (model.RemedialSteps == null)
            {
                model.RemedialSteps = string.Empty;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Personal Assistance Plan saved successfully!";

            // FORCE REDIRECT TO ANNUAL ASSESSMENT
            return Redirect("/AnnualAssessment/create");
        }
    }
}



