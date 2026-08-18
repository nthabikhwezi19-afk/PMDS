using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;

namespace PMDSSystems.Controllers
{
    public class PDPController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PDPController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // CREATE PDP
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Create(string persalNo)
        {
            var model = new PDPModel();

            // If no PERSAL number was supplied
            if (string.IsNullOrWhiteSpace(persalNo))
            {
                return View(model);
            }

            // Find employee using PERSAL number
            var employee = await _context.Employees
                .Include(e => e.Supervisor)
                .FirstOrDefaultAsync(e => e.PersalNumber == persalNo);

            if (employee == null)
            {
                TempData["Error"] = "Employee with PERSAL number " + persalNo + " was not found.";

                model.PersalNo = persalNo;

                return View(model);
            }

            // ========================================================
            // MAP EMPLOYEE INFORMATION TO PDP MODEL
            // ========================================================

            model.PersalNo = employee.PersalNumber;

            // Surname & Initials
            model.Surname = string.IsNullOrWhiteSpace(employee.Initials)
                ? employee.LastName
                : employee.LastName + " " + employee.Initials;

            // Directorate / Section
            model.Directorate = employee.Department;

            // ID Number
            model.IdNumber = employee.IdentificationNumber;

            // Branch / Region
            model.Branch = employee.BranchOrRegion;

            // Salary Level
            model.SalaryLevel = employee.SalaryLevel;

            // Gender
            model.Gender = employee.Gender;

            // Race
            model.Race = employee.Race;

            // Age Group
            model.AgeGroup = employee.AgeGroup;

            // Disability
            if (employee.HasDisability.HasValue)
            {
                model.Disabled = employee.HasDisability.Value
                    ? "Yes"
                    : "No";
            }
            else
            {
                model.Disabled = "No";
            }

            // Nature of Disability
            // Nature of Disability
            model.DisabilityDetails = employee.NatureOfDisability;

            // ==========================================
            // DEBUG - CHECK EMPLOYEE VALUES
            // ==========================================
            Console.WriteLine("======================================");
            Console.WriteLine("PDP EMPLOYEE LOOKUP");
            Console.WriteLine("PERSAL: " + employee.PersalNumber);
            Console.WriteLine("AGE GROUP: " + employee.AgeGroup);
            Console.WriteLine("HAS DISABILITY: " + employee.HasDisability);
            Console.WriteLine("NATURE OF DISABILITY: " + employee.NatureOfDisability);
            Console.WriteLine("======================================");

            // Supervisor
            if (employee.Supervisor != null)
            {
                model.Supervisor = employee.Supervisor.LastName;

                model.SupervisorPosition =
                    employee.Supervisor.Position;
            }
            else
            {
                model.Supervisor = employee.SupervisorSurnameInitials;
                model.SupervisorPosition = employee.SupervisorRankPostLevel;
            }

            return View(model);
            // Supervisor
            if (employee.Supervisor != null)
            {
                model.Supervisor = employee.Supervisor.LastName;

                model.SupervisorPosition =
                    employee.Supervisor.Position;
            }
            else
            {
                model.Supervisor = employee.SupervisorSurnameInitials;
                model.SupervisorPosition = employee.SupervisorRankPostLevel;
            }

            return View(model);
        }


        // ============================================================
        // SAVE PDP
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePDP(PDPModel model)
        {
            // ==========================================
            // REMOVE VALIDATION ERRORS THAT MAY COME
            // FROM OPTIONAL FIELDS
            // ==========================================

            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            // ==========================================
            // CREATE MAIN PDP
            // ==========================================

            var pdp = new PDPModel
            {
                // Employee information
                Surname = model.Surname,
                PersalNo = model.PersalNo,
                Directorate = model.Directorate,
                IdNumber = model.IdNumber,
                Branch = model.Branch,
                SalaryLevel = model.SalaryLevel,
                AgeGroup = model.AgeGroup,
                Gender = model.Gender,
                Race = model.Race,
                Disabled = model.Disabled,
                DisabilityDetails = model.DisabilityDetails,

                // Supervisor
                SupervisorPosition = model.SupervisorPosition,
                Supervisor = model.Supervisor,

                // Educational information
                Workshops = model.Workshops,
                CurrentStudies = model.CurrentStudies,
                Bursary = model.Bursary,
                BursaryDuration = model.BursaryDuration,

                // Declaration
                EmployeeSignature = model.EmployeeSignature,
                EmployeeDate = model.EmployeeDate,
                SupervisorName = model.SupervisorName,

                // Office use
                CapturedOnDatabase = model.CapturedOnDatabase,
                DateCaptured = model.DateCaptured,

                // PDP
                Goal = model.Goal,
                ActionPlan = model.ActionPlan
            };

            // ==========================================
            // EDUCATION ROWS
            // ==========================================

            int educationNumber = 1;

            while (true)
            {
                var qualification =
                    Request.Form[$"Qualification{educationNumber}"].FirstOrDefault();

                var nqf =
                    Request.Form[$"NQF{educationNumber}"].FirstOrDefault();

                var year =
                    Request.Form[$"Year{educationNumber}"].FirstOrDefault();

                // Stop when there are no more rows
                if (qualification == null &&
                    nqf == null &&
                    year == null)
                {
                    break;
                }

                // Only save rows that contain something
                if (!string.IsNullOrWhiteSpace(qualification) ||
                    !string.IsNullOrWhiteSpace(nqf) ||
                    !string.IsNullOrWhiteSpace(year))
                {
                    pdp.Education.Add(new PDPEducation
                    {
                        Qualification = qualification,
                        NQF = nqf,
                        Year = year
                    });
                }

                educationNumber++;
            }

            // ==========================================
            // JOB REQUIREMENT ROWS
            // ==========================================

            int jobNumber = 1;

            while (true)
            {
                var task =
                    Request.Form[$"Task{jobNumber}"].FirstOrDefault();

                var training =
                    Request.Form[$"Training{jobNumber}"].FirstOrDefault();

                var learningType =
                    Request.Form[$"LearningType{jobNumber}"].FirstOrDefault();

                var nqfLevel =
                    Request.Form[$"NQFLevel{jobNumber}"].FirstOrDefault();

                var cost =
                    Request.Form[$"Cost{jobNumber}"].FirstOrDefault();

                var impact =
                    Request.Form[$"Impact{jobNumber}"].FirstOrDefault();

                // Stop when there are no more rows
                if (task == null &&
                    training == null &&
                    learningType == null &&
                    nqfLevel == null &&
                    cost == null &&
                    impact == null)
                {
                    break;
                }

                // Save row if at least one field contains information
                if (!string.IsNullOrWhiteSpace(task) ||
                    !string.IsNullOrWhiteSpace(training) ||
                    !string.IsNullOrWhiteSpace(learningType) ||
                    !string.IsNullOrWhiteSpace(nqfLevel) ||
                    !string.IsNullOrWhiteSpace(cost) ||
                    !string.IsNullOrWhiteSpace(impact))
                {
                    pdp.JobRequirements.Add(new PDPJobRequirement
                    {
                        Task = task,
                        Training = training,
                        LearningType = learningType,
                        NQFLevel = nqfLevel,
                        Cost = cost,
                        Impact = impact
                    });
                }

                jobNumber++;
            }

            // ==========================================
            // SAVE EVERYTHING
            // ==========================================

            _context.PDPs.Add(pdp);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Personal Development Plan saved successfully.";

            return RedirectToAction(
                "Create",
                new { persalNo = model.PersalNo }
            );
        }
    }
}
