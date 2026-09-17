using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        // CREATE - GET
        // ============================================================
        // ============================================================
        // CREATE - GET
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("PERFORMANCE AGREEMENT CREATE GET");
            Console.WriteLine("========================================");

            string? currentUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return RedirectToAction("Login", "Account");
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == currentUserId);

            if (employee == null)
            {
                TempData["Error"] = "Employee profile was not found.";
                return RedirectToAction("Index", "Home");
            }

            var model = new PerformanceAgreement
            {
                KRAs = new List<KRA>()
            };

            // Create four empty KRA rows
            for (int i = 0; i < 4; i++)
            {
                model.KRAs.Add(new KRA());
            }

            model.NumberOfKRAs = model.KRAs.Count;

            Console.WriteLine(
                "Employee loaded: " +
                employee.FirstName + " " +
                employee.LastName
            );

            Console.WriteLine(
                "Employee ID: " +
                employee.Id
            );

            Console.WriteLine(
                "Default KRA count: " +
                model.KRAs.Count
            );

            return View(model);
        }

      
// ============================================================
// CREATE - POST
// ============================================================

[HttpPost]

[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(PerformanceAgreement model)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("PERFORMANCE AGREEMENT CREATE POST");
            Console.WriteLine("========================================");

            // ------------------------------------------------------------
            // LOG MAIN AGREEMENT DATA
            // ------------------------------------------------------------

            Console.WriteLine("Job Purpose: " + model.JobPurpose);
            Console.WriteLine("Job Related Tasks: " + model.JobRelatedTasks);
            Console.WriteLine("Salary Level: " + model.SalaryLevel);
            Console.WriteLine("Number Of KRAs: " + model.NumberOfKRAs);

            Console.WriteLine(
                "KRA COUNT RECEIVED: " +
                (model.KRAs == null ? 0 : model.KRAs.Count)
            );

            // ------------------------------------------------------------
            // REMOVE VALIDATION FOR OPTIONAL / UNUSED FIELDS
            // ------------------------------------------------------------

            ModelState.Remove("JobPurpose");
            ModelState.Remove("JobKnowledge");
            ModelState.Remove("Responsibility");
            ModelState.Remove("QualityOfWork");
            ModelState.Remove("TechnicalSkills");
            ModelState.Remove("Reliability");
            ModelState.Remove("Communication");
            ModelState.Remove("TeamWork");
            ModelState.Remove("Leadership");

            // ------------------------------------------------------------
            // MAKE SURE KRA COLLECTION EXISTS
            // ------------------------------------------------------------

            if (model.KRAs == null)
            {
                model.KRAs = new List<KRA>();
            }

            // ------------------------------------------------------------
            // LOG ALL RECEIVED KRAs
            // ------------------------------------------------------------

            int kraNumber = 1;

            foreach (var kra in model.KRAs)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("KRA " + kraNumber);
                Console.WriteLine("Name       : " + kra.Name);
                Console.WriteLine("Weight     : " + kra.Weight);
                Console.WriteLine("Activities : " + kra.Activities);
                Console.WriteLine("Standards  : " + kra.Standards);
                Console.WriteLine("Batho Pele : " + kra.BathoPele);
                Console.WriteLine("GAFs       : " + kra.GAFs);
                Console.WriteLine("----------------------------------------");

                kraNumber++;
            }

            // ------------------------------------------------------------
            // REMOVE COMPLETELY EMPTY KRA ROWS
            // ------------------------------------------------------------

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

            // ------------------------------------------------------------
            // UPDATE NUMBER OF KRAs
            // ------------------------------------------------------------

            model.NumberOfKRAs = model.KRAs.Count;

            Console.WriteLine(
                "FINAL KRA COUNT: " +
                model.KRAs.Count
            );

            // ------------------------------------------------------------
            // VALIDATION
            // ------------------------------------------------------------
            ModelState.Remove("Id");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("MODEL STATE INVALID");
                Console.WriteLine("========================================");

                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine(
                            "FIELD: " +
                            state.Key +
                            " ERROR: " +
                            error.ErrorMessage
                        );
                    }
                }

                return View(model);
            }
            model.Id = 0;
            // ------------------------------------------------------------
            // DATABASE SAVE
            // ------------------------------------------------------------

            try
            {
                Console.WriteLine("========================================");
                Console.WriteLine("STARTING DATABASE SAVE");
                Console.WriteLine("========================================");

                // ========================================================
                // STEP 1
                // SAVE PERFORMANCE AGREEMENT FIRST
                // ========================================================

                // Make sure this is treated as a new agreement.
                model.Id = 0;

                // Do not send the KRA navigation collection with the
                // agreement when doing the first save.
                var krasToSave = model.KRAs;

                model.KRAs = new List<KRA>();

                _context.PerformanceAgreements.Add(model);

                await _context.SaveChangesAsync();

                Console.WriteLine(
                    "PERFORMANCE AGREEMENT SAVED. ID = " +
                    model.Id
                );

                // ========================================================
                // STEP 2
                // SAVE EACH KRA USING THE GENERATED AGREEMENT ID
                // ========================================================

                foreach (var kra in krasToSave)
                {
                    // Make absolutely sure this is a NEW KRA.
                    kra.Id = 0;

                    // This is the important relationship.
                    kra.PerformanceAgreementId = model.Id;

                    // Do not attach the agreement navigation property.
                    kra.PerformanceAgreement = null;

                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("SAVING KRA");
                    Console.WriteLine("Agreement ID : " + kra.PerformanceAgreementId);
                    Console.WriteLine("Name         : " + kra.Name);
                    Console.WriteLine("Weight       : " + kra.Weight);
                    Console.WriteLine("Activities   : " + kra.Activities);
                    Console.WriteLine("Standards    : " + kra.Standards);
                    Console.WriteLine("Batho Pele   : " + kra.BathoPele);
                    Console.WriteLine("GAFs         : " + kra.GAFs);
                    Console.WriteLine("----------------------------------------");

                    _context.KRAs.Add(kra);

                    await _context.SaveChangesAsync();

                    Console.WriteLine(
                        "KRA SAVED SUCCESSFULLY. KRA ID = " +
                        kra.Id
                    );
                }

                // ========================================================
                // STEP 3
                // VERIFY THE AGREEMENT AND KRAs IN DATABASE
                // ========================================================

                var savedAgreement =
                    await _context.PerformanceAgreements
                        .Include(p => p.KRAs)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(
                            p => p.Id == model.Id
                        );

                if (savedAgreement == null)
                {
                    Console.WriteLine(
                        "ERROR: AGREEMENT WAS NOT FOUND AFTER SAVE."
                    );

                    ModelState.AddModelError(
                        "",
                        "The Performance Agreement could not be found after saving."
                    );

                    model.KRAs = krasToSave;

                    return View(model);
                }

                // --------------------------------------------------------
                // DATABASE VERIFICATION
                // --------------------------------------------------------

                Console.WriteLine("========================================");
                Console.WriteLine("DATABASE VERIFICATION");
                Console.WriteLine("========================================");

                Console.WriteLine(
                    "Agreement ID: " +
                    savedAgreement.Id
                );

                Console.WriteLine(
                    "KRAs FOUND IN DATABASE: " +
                    (savedAgreement.KRAs?.Count ?? 0)
                );

                if (savedAgreement.KRAs != null)
                {
                    foreach (var savedKra in savedAgreement.KRAs)
                    {
                        Console.WriteLine(
                            "KRA ID=" +
                            savedKra.Id +
                            " | AgreementID=" +
                            savedKra.PerformanceAgreementId +
                            " | Name=" +
                            savedKra.Name +
                            " | Weight=" +
                            savedKra.Weight
                        );
                    }
                }

                Console.WriteLine("========================================");

                // ========================================================
                // CHECK THAT ALL KRAs WERE SAVED
                // ========================================================

                int expectedKraCount = krasToSave.Count;
                int actualKraCount = savedAgreement.KRAs?.Count ?? 0;

                if (actualKraCount != expectedKraCount)
                {
                    Console.WriteLine(
                        "WARNING: EXPECTED " +
                        expectedKraCount +
                        " KRAs BUT FOUND " +
                        actualKraCount
                    );

                    ModelState.AddModelError(
                        "",
                        "The Performance Agreement was saved, but not all KRAs were saved."
                    );

                    model.KRAs = krasToSave;

                    return View(model);
                }

                // ========================================================
                // SUCCESS
                // ========================================================

                TempData["Success"] =
                    "Performance Agreement and " +
                    actualKraCount +
                    " KRA(s) saved successfully.";

                // ========================================================
                // REDIRECT TO AGREEMENT PAGE
                // ========================================================

                return RedirectToAction(
                    nameof(Agreement),
                    new
                    {
                        agreementId = savedAgreement.Id
                    }
                );
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("DATABASE ERROR");
                Console.WriteLine("========================================");

                Console.WriteLine(ex.ToString());

                string errorMessage =
                    ex.InnerException?.Message ??
                    ex.Message;

                Console.WriteLine(
                    "DATABASE ERROR MESSAGE: " +
                    errorMessage
                );

                ModelState.AddModelError(
                    "",
                    "DATABASE ERROR: " +
                    errorMessage
                );

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("GENERAL ERROR");
                Console.WriteLine("========================================");

                Console.WriteLine(ex.ToString());

                string errorMessage =
                    ex.InnerException?.Message ??
                    ex.Message;

                ModelState.AddModelError(
                    "",
                    "ERROR: " +
                    errorMessage
                );

                return View(model);
            }
        }

        // ============================================================
        // CREATE - POST
        // ============================================================




        // ============================================================
        // AGREEMENT - GET
        //
        // This works with:
        //
        // /PerformanceAgreement/Agreement?agreementId=1003
        //
        // AND also:
        //
        // /PerformanceAgreement/Agreement
        //
        // If no ID is supplied, the latest agreement is opened.
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> Agreement(int? agreementId)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("AGREEMENT PAGE REQUESTED");
            Console.WriteLine("========================================");

            Console.WriteLine(
                "Agreement ID received: " +
                (agreementId.HasValue
                    ? agreementId.Value.ToString()
                    : "NULL")
            );

            // --------------------------------------------------------
            // If no ID was supplied, get the latest agreement
            // --------------------------------------------------------

            PerformanceAgreement? agreement;

            if (!agreementId.HasValue || agreementId.Value <= 0)
            {
                Console.WriteLine(
                    "No valid Agreement ID supplied."
                );

                agreement = await _context.PerformanceAgreements
                    .Include(p => p.KRAs)
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefaultAsync();

                if (agreement == null)
                {
                    Console.WriteLine(
                        "NO PERFORMANCE AGREEMENTS FOUND."
                    );

                    TempData["Error"] =
                        "No Performance Agreement was found.";

                    return RedirectToAction(nameof(Create));
                }

                Console.WriteLine(
                    "Opening latest Agreement ID: " +
                    agreement.Id
                );
            }
            else
            {
                // ----------------------------------------------------
                // ID was supplied - load that exact agreement
                // ----------------------------------------------------

                agreement = await _context.PerformanceAgreements
                    .Include(p => p.KRAs)
                    .FirstOrDefaultAsync(
                        p => p.Id == agreementId.Value
                    );

                if (agreement == null)
                {
                    Console.WriteLine(
                        "Agreement not found. ID = " +
                        agreementId.Value
                    );

                    TempData["Error"] =
                        "Performance Agreement with ID " +
                        agreementId.Value +
                        " was not found.";

                    return RedirectToAction(nameof(Index));
                }
            }

            // --------------------------------------------------------
            // Current logged-in Identity user
            // --------------------------------------------------------

            string? currentUserId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                );

            Console.WriteLine(
                "Current Identity User ID: " +
                (currentUserId ?? "NULL")
            );

            // --------------------------------------------------------
            // Default values
            // --------------------------------------------------------

            ViewBag.EmployeeNameAndCapacity =
                "Employee";

            ViewBag.SupervisorNameAndCapacity =
                "Supervisor";

            ViewBag.CanSupervisorSign = false;

            ViewBag.ExistingSupervisorSignature =
                "";

            // --------------------------------------------------------
            // Find logged-in employee
            // --------------------------------------------------------

            if (!string.IsNullOrWhiteSpace(currentUserId))
            {
                var currentEmployee =
                    await _context.Employees
                        .FirstOrDefaultAsync(
                            e => e.UserId == currentUserId
                        );

                if (currentEmployee != null)
                {
                    Console.WriteLine(
                        "Employee found: " +
                        currentEmployee.FirstName +
                        " " +
                        currentEmployee.LastName
                    );

                    // ------------------------------------------------
                    // Employee display name
                    // ------------------------------------------------

                    ViewBag.EmployeeNameAndCapacity =
                        (
                            currentEmployee.FirstName +
                            " " +
                           currentEmployee.LastName
                        ).Trim();

                    // ------------------------------------------------
                    // Find assigned supervisor
                    // ------------------------------------------------

                    if (currentEmployee.SupervisorId.HasValue)
                    {
                        var supervisor =
                            await _context.Employees
                                .FirstOrDefaultAsync(
                                    e =>
                                        e.Id ==
                                        currentEmployee.SupervisorId.Value
                                );

                        if (supervisor != null)
                        {
                            string supervisorName =
                                (
                                    supervisor.FirstName +
                                    " " +
                                   supervisor.LastName
                                ).Trim();

                            ViewBag.SupervisorNameAndCapacity =
                                supervisorName;

                            // ----------------------------------------
                            // Determine whether logged-in user is
                            // the assigned supervisor
                            // ----------------------------------------

                            bool isSupervisor =
                                supervisor.UserId ==
                                currentUserId;

                            ViewBag.CanSupervisorSign =
                                isSupervisor;

                            Console.WriteLine(
                                "Assigned Supervisor: " +
                                supervisorName
                            );

                            Console.WriteLine(
                                "Can Supervisor Sign: " +
                                isSupervisor
                            );
                        }
                    }
                }
            }

            // --------------------------------------------------------
            // Existing supervisor signature
            //
            // This uses the property already used by your Agreement
            // view/controller workflow.
            // --------------------------------------------------------

            ViewBag.ExistingSupervisorSignature = "";

            // --------------------------------------------------------
            // Extra useful ViewBags
            // --------------------------------------------------------

            ViewBag.AgreementId = agreement.Id;

            ViewBag.KraCount =
                agreement.KRAs?.Count ?? 0;

            // --------------------------------------------------------
            // Log final information
            // --------------------------------------------------------

            Console.WriteLine("========================================");
            Console.WriteLine("AGREEMENT FOUND");
            Console.WriteLine(
                "Agreement ID: " +
                agreement.Id
            );

            Console.WriteLine(
                "KRA Count: " +
                (agreement.KRAs?.Count ?? 0)
            );

            Console.WriteLine(
                "Can Supervisor Sign: " +
                (ViewBag.CanSupervisorSign ?? false)
            );

            Console.WriteLine("========================================");

            // --------------------------------------------------------
            // Send agreement to Agreement.cshtml
            // --------------------------------------------------------

            return View(agreement);
        }


        // ============================================================
        // INDEX
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var agreements =
                await _context.PerformanceAgreements
                    .Include(p => p.KRAs)
                    .OrderByDescending(p => p.Id)
                    .AsNoTracking()
                    .ToListAsync();

            return View(agreements);
        }
    }
}