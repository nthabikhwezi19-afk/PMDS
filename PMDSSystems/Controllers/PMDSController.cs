using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using PMDSSystems.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // ============================================================
        // PART A - CREATE / EDIT
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // ========================================================
            // GET LOGGED-IN USER
            // ========================================================

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            // ========================================================
            // FIND EMPLOYEE LINKED TO LOGGED-IN USER
            // ========================================================

            var employee = await _context.Employees
                .Include(e => e.Supervisor)
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                return NotFound(
                    "Your employee profile is not linked to your login account.");
            }

            // ========================================================
            // DEBUG INFORMATION
            // ========================================================

            Console.WriteLine("========================================");
            Console.WriteLine("PART A - EMPLOYEE LOADED");
            Console.WriteLine($"Employee ID: {employee.Id}");
            Console.WriteLine($"Persal Number: {employee.PersalNumber}");
            Console.WriteLine($"First Name: {employee.FirstName}");
            Console.WriteLine($"Last Name: {employee.LastName}");
            Console.WriteLine($"Initials: {employee.Initials}");
            Console.WriteLine($"Department: {employee.Department}");
            Console.WriteLine($"Position: {employee.Position}");
            Console.WriteLine($"Post Designation: {employee.PostDesignation}");
            Console.WriteLine($"Post Level: {employee.PostLevel}");
            Console.WriteLine($"Appointment in DCS: {employee.AppointmentInDcsDate}");
            Console.WriteLine($"Appointment in Current Rank: {employee.AppointmentDateInCurrentRank}");
            Console.WriteLine($"OSD Description: {employee.OSDDescription}");
            Console.WriteLine($"Previous Station: {employee.PreviousStation}");
            Console.WriteLine($"Transfer Date: {employee.TransferDate}");
            Console.WriteLine($"Previous Performance: {employee.PreviousCyclePerformance}");
            Console.WriteLine($"Supervisor ID: {employee.SupervisorId}");

            if (employee.Supervisor != null)
            {
                Console.WriteLine(
                    $"Supervisor: {employee.Supervisor.FirstName} {employee.Supervisor.LastName}");

                Console.WriteLine(
                    $"Supervisor Initials: {employee.Supervisor.Initials}");

                Console.WriteLine(
                    $"Supervisor Post Level: {employee.Supervisor.PostLevel}");
            }
            else
            {
                Console.WriteLine("Supervisor: NOT ASSIGNED");
            }

            Console.WriteLine("========================================");

            // ========================================================
            // CHECK IF PART A ALREADY EXISTS
            // ========================================================

            var existing = await _context.PMDSForms
                .FirstOrDefaultAsync(
                    x => x.EmployeeId == employee.Id);

            PMDSForm model;

            // ========================================================
            // USE EXISTING PART A IF FOUND
            // ========================================================

            if (existing != null)
            {
                model = existing;
            }
            else
            {
                model = new PMDSForm();
            }

            // ========================================================
            // ALWAYS LOAD EMPLOYEE INFORMATION
            //
            // Employee table is the source of truth for these fields.
            // This runs even when Part A already exists.
            // ========================================================

            model.EmployeeId = employee.Id;

            model.PersalNumber =
                employee.PersalNumber;

            model.SurnameInitials =
                $"{employee.LastName} {employee.Initials}";

            model.Directorate =
                employee.Department ?? string.Empty;

            model.PostDesignation =
                !string.IsNullOrWhiteSpace(employee.PostDesignation)
                    ? employee.PostDesignation
                    : employee.Position ?? string.Empty;

            model.CurrentRank =
                employee.PostLevel ?? string.Empty;

            model.PostLevel =
                employee.PostLevel;

            model.RelatedOSDDescription =
                employee.OSDDescription ?? string.Empty;

            model.OSDDescription =
                employee.OSDDescription;

            model.AppointmentDate =
                employee.AppointmentInDcsDate?.Date;

            model.AppointmentDateInCurrentRank =
                employee.AppointmentDateInCurrentRank?.Date;

            model.PreviousStation =
                employee.PreviousStation;

            model.TransferDate =
                employee.TransferDate?.Date;

            model.PreviousCyclePerformance =
                employee.PreviousCyclePerformance;

            // ========================================================
            // LOAD SUPERVISOR
            // ========================================================

            if (employee.Supervisor != null)
            {
                var supervisor = employee.Supervisor;

                model.SupervisorName =
                    $"{supervisor.FirstName} {supervisor.LastName}";

                model.SupervisorSurnameInitials =
                    $"{supervisor.LastName} {supervisor.Initials}";

                model.SupervisorRankPostLevel =
                    supervisor.PostLevel ?? string.Empty;
            }
            else
            {
                model.SupervisorName = string.Empty;
                model.SupervisorSurnameInitials = string.Empty;
                model.SupervisorRankPostLevel = string.Empty;
            }

            // ========================================================
            // DEBUG - FINAL MODEL SENT TO VIEW
            // ========================================================

            Console.WriteLine("========================================");
            Console.WriteLine("PART A - MODEL SENT TO VIEW");
            Console.WriteLine($"Persal: {model.PersalNumber}");
            Console.WriteLine($"Surname Initials: {model.SurnameInitials}");
            Console.WriteLine($"Directorate: {model.Directorate}");
            Console.WriteLine($"Post Designation: {model.PostDesignation}");
            Console.WriteLine($"Current Rank: {model.CurrentRank}");
            Console.WriteLine($"Supervisor: {model.SupervisorSurnameInitials}");
            Console.WriteLine($"Supervisor Rank: {model.SupervisorRankPostLevel}");
            Console.WriteLine($"OSD: {model.RelatedOSDDescription}");
            Console.WriteLine($"Appointment Date: {model.AppointmentDate}");
            Console.WriteLine(
                $"Appointment/Promotion Date: {model.AppointmentDateInCurrentRank}");
            Console.WriteLine($"Previous Station: {model.PreviousStation}");
            Console.WriteLine($"Transfer Date: {model.TransferDate}");
            Console.WriteLine(
                $"Previous Performance: {model.PreviousCyclePerformance}");
            Console.WriteLine($"Start Year: {model.StartYear}");
            Console.WriteLine($"End Year: {model.EndYear}");
            Console.WriteLine("========================================");

            return View(model);
        }


        // ============================================================
        // PART A - SAVE
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PMDSForm model)
        {
            // ========================================================
            // GET LOGGED-IN USER
            // ========================================================

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            // ========================================================
            // FIND EMPLOYEE
            // ========================================================

            var employee = await _context.Employees
                .Include(e => e.Supervisor)
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                return NotFound(
                    "Your employee profile is not linked to your login account.");
            }

            // ========================================================
            // SECURITY
            //
            // NEVER TRUST EMPLOYEE ID OR PERSAL NUMBER FROM FORM.
            // ========================================================

            model.EmployeeId =
                employee.Id;

            model.PersalNumber =
                employee.PersalNumber;

            // ========================================================
            // REMOVE VALIDATION FOR SYSTEM-ASSIGNED FIELDS
            // ========================================================

            ModelState.Remove(
                nameof(PMDSForm.EmployeeId));

            ModelState.Remove(
                nameof(PMDSForm.PersalNumber));

            // ========================================================
            // REQUIRE START YEAR
            // ========================================================

            if (!model.StartYear.HasValue)
            {
                ModelState.AddModelError(
                    nameof(PMDSForm.StartYear),
                    "Please select the Start Year.");
            }

            // ========================================================
            // AUTOMATICALLY SET END YEAR
            // ========================================================

            if (model.StartYear.HasValue)
            {
                model.EndYear =
                    model.StartYear.Value + 1;

                ModelState.Remove(
                    nameof(PMDSForm.EndYear));
            }

            // ========================================================
            // ALWAYS REBUILD EMPLOYEE INFORMATION
            //
            // These values come from Employees, not the browser.
            // ========================================================

            model.SurnameInitials =
                $"{employee.LastName} {employee.Initials}";

            model.Directorate =
                employee.Department ?? string.Empty;

            model.PostDesignation =
                !string.IsNullOrWhiteSpace(employee.PostDesignation)
                    ? employee.PostDesignation
                    : employee.Position ?? string.Empty;

            model.CurrentRank =
                employee.PostLevel ?? string.Empty;

            model.PostLevel =
                employee.PostLevel;

            model.RelatedOSDDescription =
                employee.OSDDescription ?? string.Empty;

            model.OSDDescription =
                employee.OSDDescription;

            model.AppointmentDate =
                employee.AppointmentInDcsDate?.Date;

            model.AppointmentDateInCurrentRank =
                employee.AppointmentDateInCurrentRank?.Date;

            model.PreviousStation =
                employee.PreviousStation;

            model.TransferDate =
                employee.TransferDate?.Date;

            model.PreviousCyclePerformance =
                employee.PreviousCyclePerformance;

            // ========================================================
            // SUPERVISOR
            // ========================================================

            if (employee.Supervisor != null)
            {
                model.SupervisorName =
                    $"{employee.Supervisor.FirstName} {employee.Supervisor.LastName}";

                model.SupervisorSurnameInitials =
                    $"{employee.Supervisor.LastName} {employee.Supervisor.Initials}";

                model.SupervisorRankPostLevel =
                    employee.Supervisor.PostLevel ?? string.Empty;
            }
            else
            {
                model.SupervisorName = string.Empty;
                model.SupervisorSurnameInitials = string.Empty;
                model.SupervisorRankPostLevel = string.Empty;
            }

            // ========================================================
            // VALIDATE
            // ========================================================

            if (!ModelState.IsValid)
            {
                // Make sure the model still contains the employee
                // information if validation fails.

                model.EmployeeId =
                    employee.Id;

                model.PersalNumber =
                    employee.PersalNumber;

                return View(model);
            }

            // ========================================================
            // FIND EXISTING PART A
            // ========================================================

            var existing = await _context.PMDSForms
                .FirstOrDefaultAsync(
                    x => x.EmployeeId == employee.Id);

            // ========================================================
            // CREATE NEW PART A
            // ========================================================

            if (existing == null)
            {
                model.Employee = null;

                _context.PMDSForms.Add(model);

                Console.WriteLine("========================================");
                Console.WriteLine("NEW PART A CREATED");
                Console.WriteLine($"Employee ID: {employee.Id}");
                Console.WriteLine($"Persal Number: {employee.PersalNumber}");
                Console.WriteLine($"Surname Initials: {model.SurnameInitials}");
                Console.WriteLine($"Directorate: {model.Directorate}");
                Console.WriteLine($"Post Designation: {model.PostDesignation}");
                Console.WriteLine($"Current Rank: {model.CurrentRank}");
                Console.WriteLine($"Supervisor: {model.SupervisorSurnameInitials}");
                Console.WriteLine($"Supervisor Rank: {model.SupervisorRankPostLevel}");
                Console.WriteLine($"OSD: {model.RelatedOSDDescription}");
                Console.WriteLine($"Previous Station: {model.PreviousStation}");
                Console.WriteLine($"Transfer Date: {model.TransferDate}");
                Console.WriteLine(
                    $"Previous Performance: {model.PreviousCyclePerformance}");
                Console.WriteLine($"Start Year: {model.StartYear}");
                Console.WriteLine($"End Year: {model.EndYear}");
                Console.WriteLine("========================================");
            }
            else
            {
                // ====================================================
                // UPDATE EXISTING PART A
                // ====================================================

                existing.EmployeeId =
                    employee.Id;

                existing.PersalNumber =
                    employee.PersalNumber;

                existing.SurnameInitials =
                    model.SurnameInitials;

                existing.Directorate =
                    model.Directorate;

                existing.PostDesignation =
                    model.PostDesignation;

                existing.SupervisorName =
                    model.SupervisorName;

                existing.SupervisorSurnameInitials =
                    model.SupervisorSurnameInitials;

                existing.SupervisorRankPostLevel =
                    model.SupervisorRankPostLevel;

                existing.AppointmentDate =
                    model.AppointmentDate;

                existing.AppointmentDateInCurrentRank =
                    model.AppointmentDateInCurrentRank;

                existing.CurrentRank =
                    model.CurrentRank;

                existing.RelatedOSDDescription =
                    model.RelatedOSDDescription;

                existing.OSDDescription =
                    model.OSDDescription;

                existing.PostLevel =
                    model.PostLevel;

                existing.PreviousStation =
                    model.PreviousStation;

                existing.TransferDate =
                    model.TransferDate;

                existing.PreviousCyclePerformance =
                    model.PreviousCyclePerformance;

                existing.StartYear =
                    model.StartYear;

                existing.EndYear =
                    model.EndYear;

                Console.WriteLine("========================================");
                Console.WriteLine("EXISTING PART A UPDATED");
                Console.WriteLine($"Part A ID: {existing.Id}");
                Console.WriteLine($"Employee ID: {existing.EmployeeId}");
                Console.WriteLine($"Persal Number: {existing.PersalNumber}");
                Console.WriteLine($"Surname Initials: {existing.SurnameInitials}");
                Console.WriteLine($"Directorate: {existing.Directorate}");
                Console.WriteLine($"Post Designation: {existing.PostDesignation}");
                Console.WriteLine($"Current Rank: {existing.CurrentRank}");
                Console.WriteLine($"Supervisor: {existing.SupervisorSurnameInitials}");
                Console.WriteLine($"Supervisor Rank: {existing.SupervisorRankPostLevel}");
                Console.WriteLine($"OSD: {existing.RelatedOSDDescription}");
                Console.WriteLine($"Previous Station: {existing.PreviousStation}");
                Console.WriteLine($"Transfer Date: {existing.TransferDate}");
                Console.WriteLine(
                    $"Previous Performance: {existing.PreviousCyclePerformance}");
                Console.WriteLine($"Start Year: {existing.StartYear}");
                Console.WriteLine($"End Year: {existing.EndYear}");
                Console.WriteLine("========================================");
            }

            // ========================================================
            // SAVE
            // ========================================================

            await _context.SaveChangesAsync();

            // ========================================================
            // SUCCESS
            // ========================================================

            TempData["SuccessMessage"] =
                "Part A saved successfully.";

            // ========================================================
            // GO TO PERFORMANCE AGREEMENT
            // ========================================================

            return RedirectToAction(
                "Create",
                "PerformanceAgreement");
        }


        // ============================================================
        // PERSONAL DEVELOPMENT PLAN (PDP)
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> PersonalDevelopmentPlan()
        {
            var userId = _userManager.GetUserId(User);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                return NotFound(
                    "No employee record is linked to this account.");
            }

            Employee? supervisor = null;

            if (employee.SupervisorId.HasValue)
            {
                supervisor = await _context.Employees
                    .FirstOrDefaultAsync(
                        e => e.Id == employee.SupervisorId.Value);
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


        // ============================================================
        // SAVE PDP
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SavePDP(PDPModel model)
        {
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

            // ========================================================
            // EXISTING MID-TERM REVIEW
            // ========================================================

            if (id.HasValue)
            {
                review = await _context.MidTermReviews
                    .Include(r => r.KraEvaluations)
                    .FirstOrDefaultAsync(
                        r => r.Id == id.Value);

                if (review == null)
                {
                    return NotFound();
                }

                return View(review);
            }

            // ========================================================
            // NEW MID-TERM REVIEW
            // ========================================================

            review = new MidTermReview
            {
                EmployeeId =
                    employeeId ?? "Unassigned",

                ReviewPeriod =
                    "1 April 2025 - 31 October 2025",

                KraEvaluations =
                    new List<MidTermKraEvaluation>()
            };

            // ========================================================
            // LOAD LATEST PERFORMANCE AGREEMENT
            // ========================================================

            var performanceAgreement =
                await _context.PerformanceAgreements
                    .Include(p => p.KRAs)
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefaultAsync();

            // ========================================================
            // LOAD KRAs
            // ========================================================

            if (performanceAgreement != null &&
                performanceAgreement.KRAs != null &&
                performanceAgreement.KRAs.Any())
            {
                int kraNumber = 1;

                foreach (
                    var kra in performanceAgreement.KRAs
                        .OrderBy(k => k.Id))
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

                            KraDescription =
                                kra.Name ?? string.Empty,

                            Weight = weight,

                            AchievementStandard =
                                string.Empty,

                            SupervisorComments =
                                string.Empty,

                            OwnRating = 0,

                            SupervisorRating = 0,

                            AgreedRating = 0
                        });

                    kraNumber++;
                }
            }

            // ========================================================
            // FALLBACK KRAs
            // ========================================================

            if (!review.KraEvaluations.Any())
            {
                for (int i = 1; i <= 4; i++)
                {
                    review.KraEvaluations.Add(
                        new MidTermKraEvaluation
                        {
                            KraNumber = i,

                            KraDescription =
                                $"KRA {i}",

                            Weight = 0
                        });
                }
            }

            return View(review);
        }


        // ============================================================
        // SAVE MID-TERM REVIEW
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMidTermReview(
            MidTermReview model)
        {
            decimal totalWeight =
                model.KraEvaluations.Sum(
                    k => k.Weight);

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


        // ============================================================
        // CYCLES
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> Cycles()
        {
            var userId =
                _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(
                    e => e.UserId == userId);

            if (employee == null)
            {
                return NotFound(
                    "Your employee profile has not been linked " +
                    "to your user account.");
            }

            var model = new CyclesViewModel
            {
                Employee = employee
            };

            // ========================================================
            // FIND EMPLOYEES ASSIGNED TO CURRENT EMPLOYEE
            // ========================================================

            model.MyEmployees =
                await _context.Employees
                    .Where(
                        e => e.SupervisorId == employee.Id)
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .ToListAsync();

            return View(model);
        }


        // ============================================================
        // PERSONAL ASSISTANCE PLAN (PAP)
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> PersonalAssistancePlan(
            int? id)
        {
            PersonalAssistancePlan pap;

            // ========================================================
            // EXISTING PAP
            // ========================================================

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

            // ========================================================
            // NEW PAP
            // ========================================================

            pap = new PersonalAssistancePlan();

            // ========================================================
            // GET LOGGED-IN USER
            // ========================================================

            var userId =
                _userManager.GetUserId(User);

            if (!string.IsNullOrEmpty(userId))
            {
                var employee =
                    await _context.Employees
                        .FirstOrDefaultAsync(
                            e => e.UserId == userId);

                if (employee != null)
                {
                    pap.PersalNo =
                        employee.PersalNumber;

                    pap.EmployeeName =
                        $"{employee.FirstName} " +
                        $"{employee.LastName}";

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


        // ============================================================
        // SAVE PERSONAL ASSISTANCE PLAN
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            SavePersonalAssistancePlan(
                PersonalAssistancePlan model)
        {
            // ========================================================
            // REMOVE VALIDATION ERRORS FROM READ-ONLY FIELDS
            // ========================================================

            ModelState.Remove(
                nameof(model.EmployeeName));

            ModelState.Remove(
                nameof(model.Post));

            ModelState.Remove(
                nameof(model.PersalNo));

            ModelState.Remove(
                nameof(model.SupervisorName));

            ModelState.Remove(
                nameof(model.SupervisorSignature));

            ModelState.Remove(
                nameof(model.SupervisorInitialsAndSurname));

            // ========================================================
            // SAVE PAP
            // ========================================================

            if (model.Id == 0)
            {
                _context.PersonalAssistancePlans
                    .Add(model);
            }
            else
            {
                _context.PersonalAssistancePlans
                    .Update(model);
            }

            // ========================================================
            // PREVENT NULL REMEDIAL STEPS
            // ========================================================

            if (model.RemedialSteps == null)
            {
                model.RemedialSteps =
                    string.Empty;
            }

            // ========================================================
            // SAVE DATABASE
            // ========================================================

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Personal Assistance Plan saved successfully!";

            // ========================================================
            // GO TO ANNUAL ASSESSMENT
            // ========================================================

            return Redirect(
                "/AnnualAssessment/create");
        }
    }
}