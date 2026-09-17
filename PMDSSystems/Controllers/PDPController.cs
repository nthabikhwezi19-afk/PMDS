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

            // ========================================================
            // IF NO PERSAL NUMBER WAS SUPPLIED
            // ========================================================
            if (string.IsNullOrWhiteSpace(persalNo))
            {
                return View(model);
            }

            // ========================================================
            // FIND EMPLOYEE USING PERSAL NUMBER
            // ========================================================
            var employee = await _context.Employees
                .Include(e => e.Supervisor)
                .FirstOrDefaultAsync(e => e.PersalNumber == persalNo);

            // ========================================================
            // EMPLOYEE NOT FOUND
            // ========================================================
            if (employee == null)
            {
                TempData["Error"] =
                    "Employee with PERSAL number " +
                    persalNo +
                    " was not found.";

                model.PersalNo = persalNo;

                return View(model);
            }

            // ========================================================
            // MAP EMPLOYEE INFORMATION TO PDP MODEL
            // ========================================================

            // --------------------------------------------------------
            // PERSAL NUMBER
            // --------------------------------------------------------
            model.PersalNo = employee.PersalNumber;

            // --------------------------------------------------------
            // SURNAME & INITIALS
            // --------------------------------------------------------
            model.Surname = string.IsNullOrWhiteSpace(employee.Initials)
                ? employee.LastName
                : employee.LastName + " " + employee.Initials;

            // --------------------------------------------------------
            // DIRECTORATE / DEPARTMENT
            // --------------------------------------------------------
            model.Directorate = employee.Department;

            // --------------------------------------------------------
            // ID NUMBER
            // --------------------------------------------------------
            model.IdNumber = employee.IdentificationNumber;

            // --------------------------------------------------------
            // BRANCH / REGION
            // --------------------------------------------------------
            model.Branch = employee.BranchOrRegion;

            // --------------------------------------------------------
            // SALARY LEVEL
            // --------------------------------------------------------
            model.SalaryLevel = employee.SalaryLevel;

            // --------------------------------------------------------
            // GENDER
            // --------------------------------------------------------
            model.Gender = employee.Gender;

            // --------------------------------------------------------
            // RACE
            // --------------------------------------------------------
            model.Race = employee.Race;

            // ========================================================
            // AGE GROUP - AUTO POPULATE
            // ========================================================
            model.AgeGroup = employee.AgeGroup;

            // ========================================================
            // DISABILITY - AUTO POPULATE
            // ========================================================

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

            // ========================================================
            // NATURE OF DISABILITY
            //
            // ONLY POPULATE THIS FIELD IF DISABILITY = YES
            // ========================================================

            if (employee.HasDisability == true)
            {
                model.DisabilityDetails =
                    employee.NatureOfDisability;
            }
            else
            {
                model.DisabilityDetails = null;
            }

            // ========================================================
            // DEBUG INFORMATION
            // ========================================================

            Console.WriteLine("======================================");
            Console.WriteLine("PDP EMPLOYEE LOOKUP");
            Console.WriteLine("======================================");

            Console.WriteLine(
                "PERSAL: " +
                employee.PersalNumber
            );

            Console.WriteLine(
                "AGE GROUP: " +
                employee.AgeGroup
            );

            Console.WriteLine(
                "HAS DISABILITY: " +
                employee.HasDisability
            );

            Console.WriteLine(
                "DISABILITY DISPLAY: " +
                model.Disabled
            );

            Console.WriteLine(
                "NATURE OF DISABILITY: " +
                (
                    employee.HasDisability == true
                        ? employee.NatureOfDisability
                        : "Hidden / N/A"
                )
            );

            Console.WriteLine("======================================");

            // ========================================================
            // SUPERVISOR
            // ========================================================

            if (employee.Supervisor != null)
            {
                // Supervisor surname
                model.Supervisor =
                    employee.Supervisor.LastName;

                // Supervisor position
                model.SupervisorPosition =
                    employee.Supervisor.Position;
            }
            else
            {
                // Fallback to manually captured supervisor information
                model.Supervisor =
                    employee.SupervisorSurnameInitials;

                model.SupervisorPosition =
                    employee.SupervisorRankPostLevel;
            }

            // ========================================================
            // RETURN PDP VIEW
            // ========================================================

            return View(model);
        }


        // ============================================================
        // SAVE PDP
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePDP(PDPModel model)
        {
            // ========================================================
            // VALIDATE MODEL
            // ========================================================

            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            // ========================================================
            // CREATE MAIN PDP
            // ========================================================

            var pdp = new PDPModel
            {
                // ====================================================
                // EMPLOYEE INFORMATION
                // ====================================================

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


                // ====================================================
                // SUPERVISOR
                // ====================================================

                SupervisorPosition =
                    model.SupervisorPosition,

                Supervisor =
                    model.Supervisor,


                // ====================================================
                // EDUCATIONAL / DEVELOPMENT INFORMATION
                // ====================================================

                Workshops =
                    model.Workshops,

                CurrentStudies =
                    model.CurrentStudies,

                Bursary =
                    model.Bursary,

                BursaryDuration =
                    model.BursaryDuration,


                // ====================================================
                // DECLARATION / SIGNATURES
                // ====================================================

                EmployeeSignature =
                    model.EmployeeSignature,

                EmployeeDate =
                    model.EmployeeDate,

                SupervisorName =
                    model.SupervisorName,

                SupervisorSignature =
                    model.SupervisorSignature,

                SupervisorDate =
                    model.SupervisorDate,


                // ====================================================
                // OFFICE USE ONLY
                // ====================================================

                CapturedOnDatabase =
                    model.CapturedOnDatabase,

                DateCaptured =
                    model.DateCaptured,


                // ====================================================
                // PDP
                // ====================================================

                Goal =
                    model.Goal,

                ActionPlan =
                    model.ActionPlan
            };


            // ============================================================
            // EDUCATION ROWS
            // ============================================================

            int educationNumber = 1;

            while (true)
            {
                // --------------------------------------------------------
                // QUALIFICATION
                // --------------------------------------------------------
                var qualification =
                    Request.Form[
                        $"Qualification{educationNumber}"
                    ].FirstOrDefault();

                // --------------------------------------------------------
                // NQF LEVEL
                //
                // IMPORTANT:
                // The PDP view uses NQFLevel1, NQFLevel2, etc.
                // --------------------------------------------------------
                var nqf =
                    Request.Form[
                        $"NQFLevel{educationNumber}"
                    ].FirstOrDefault();

                // --------------------------------------------------------
                // YEAR COMPLETED
                //
                // IMPORTANT:
                // The PDP view uses YearCompleted1, YearCompleted2, etc.
                // --------------------------------------------------------
                var year =
                    Request.Form[
                        $"YearCompleted{educationNumber}"
                    ].FirstOrDefault();

                // --------------------------------------------------------
                // STOP WHEN THERE ARE NO MORE EDUCATION ROWS
                // --------------------------------------------------------
                if (qualification == null &&
                    nqf == null &&
                    year == null)
                {
                    break;
                }

                // --------------------------------------------------------
                // SAVE ROW ONLY IF IT HAS INFORMATION
                // --------------------------------------------------------
                if (!string.IsNullOrWhiteSpace(qualification) ||
                    !string.IsNullOrWhiteSpace(nqf) ||
                    !string.IsNullOrWhiteSpace(year))
                {
                    pdp.Education.Add(
                        new PDPEducation
                        {
                            Qualification = qualification,

                            NQF = nqf,

                            Year = year
                        }
                    );
                }

                educationNumber++;
            }


            // ============================================================
            // JOB REQUIREMENT ROWS
            // ============================================================

            int jobNumber = 1;

            while (true)
            {
                // --------------------------------------------------------
                // TASK
                // --------------------------------------------------------
                var task =
                    Request.Form[
                        $"Task{jobNumber}"
                    ].FirstOrDefault();

                // --------------------------------------------------------
                // TRAINING
                // --------------------------------------------------------
                var training =
                    Request.Form[
                        $"Training{jobNumber}"
                    ].FirstOrDefault();

                // --------------------------------------------------------
                // LEARNING TYPE
                // --------------------------------------------------------
                var learningType =
                    Request.Form[
                        $"LearningType{jobNumber}"
                    ].FirstOrDefault();

                // --------------------------------------------------------
                // NQF LEVEL
                // --------------------------------------------------------
                var nqfLevel =
                    Request.Form[
                        $"NQFLevel{jobNumber}"
                    ].FirstOrDefault();

                // --------------------------------------------------------
                // COST
                // --------------------------------------------------------
                var cost =
                    Request.Form[
                        $"Cost{jobNumber}"
                    ].FirstOrDefault();

                // --------------------------------------------------------
                // IMPACT
                // --------------------------------------------------------
                var impact =
                    Request.Form[
                        $"Impact{jobNumber}"
                    ].FirstOrDefault();

                // --------------------------------------------------------
                // STOP WHEN THERE ARE NO MORE JOB REQUIREMENT ROWS
                // --------------------------------------------------------
                if (task == null &&
                    training == null &&
                    learningType == null &&
                    nqfLevel == null &&
                    cost == null &&
                    impact == null)
                {
                    break;
                }

                // --------------------------------------------------------
                // SAVE ROW IF AT LEAST ONE FIELD HAS INFORMATION
                // --------------------------------------------------------
                if (!string.IsNullOrWhiteSpace(task) ||
                    !string.IsNullOrWhiteSpace(training) ||
                    !string.IsNullOrWhiteSpace(learningType) ||
                    !string.IsNullOrWhiteSpace(nqfLevel) ||
                    !string.IsNullOrWhiteSpace(cost) ||
                    !string.IsNullOrWhiteSpace(impact))
                {
                    pdp.JobRequirements.Add(
                        new PDPJobRequirement
                        {
                            Task = task,

                            Training = training,

                            LearningType = learningType,

                            NQFLevel = nqfLevel,

                            Cost = cost,

                            Impact = impact
                        }
                    );
                }

                jobNumber++;
            }


            // ============================================================
            // SAVE EVERYTHING TO DATABASE
            // ============================================================

            _context.PDPs.Add(pdp);

            await _context.SaveChangesAsync();


            // ============================================================
            // SUCCESS MESSAGE
            // ============================================================

            TempData["Success"] =
                "First Cycle Completed Successfully!";


            // ============================================================
            // RETURN TO PDP PAGE
            // ============================================================

            return RedirectToAction(
                "Create",
                new
                {
                    persalNo = model.PersalNo
                }
            );
        }
    }
}
