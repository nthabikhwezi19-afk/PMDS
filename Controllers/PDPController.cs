using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using System.Security.Claims;

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
        public async Task<IActionResult> Create(string? persalNo)
        {
            Employee? employee = null;

            // ============================================================
            // 1. FIND EMPLOYEE USING PERSAL NUMBER
            // ============================================================
            if (!string.IsNullOrWhiteSpace(persalNo))
            {
                employee = await _context.Employees
                    .Include(e => e.Supervisor)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e =>
                        e.PersalNumber == persalNo);
            }

            // ============================================================
            // 2. IF NOT FOUND, USE LOGGED-IN USER
            // ============================================================
            if (employee == null)
            {
                var userId =
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier);

                Console.WriteLine("======================================");
                Console.WriteLine("PDP LOOKUP USING LOGGED-IN USER");
                Console.WriteLine("UserId = " + userId);
                Console.WriteLine("======================================");

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    employee = await _context.Employees
                        .Include(e => e.Supervisor)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e =>
                            e.UserId == userId);
                }
            }

            // ============================================================
            // 3. EMPLOYEE NOT FOUND
            // ============================================================
            if (employee == null)
            {
                Console.WriteLine("======================================");
                Console.WriteLine("PDP ERROR");
                Console.WriteLine("EMPLOYEE NOT FOUND");
                Console.WriteLine("======================================");

                TempData["Error"] =
                    "Employee could not be found.";

                return View(new PDPModel());
            }

            // ============================================================
            // 4. DISPLAY EXACT DATABASE VALUES
            // ============================================================
            Console.WriteLine("======================================");
            Console.WriteLine("PDP EMPLOYEE DATABASE VALUES");
            Console.WriteLine("======================================");

            Console.WriteLine(
                "Employee ID: [" +
                employee.Id +
                "]");

            Console.WriteLine(
                "PersalNumber: [" +
                employee.PersalNumber +
                "]");

            Console.WriteLine(
                "FirstName: [" +
                employee.FirstName +
                "]");

            Console.WriteLine(
                "LastName: [" +
                employee.LastName +
                "]");

            Console.WriteLine(
                "IdentificationNumber: [" +
                employee.IdentificationNumber +
                "]");

            Console.WriteLine(
                "AgeGroup: [" +
                employee.AgeGroup +
                "]");

            Console.WriteLine(
                "HasDisability: [" +
                employee.HasDisability +
                "]");

            Console.WriteLine(
                "NatureOfDisability: [" +
                employee.NatureOfDisability +
                "]");

            Console.WriteLine(
                "Gender: [" +
                employee.Gender +
                "]");

            Console.WriteLine(
                "Race: [" +
                employee.Race +
                "]");

            Console.WriteLine("======================================");


            // ============================================================
            // 5. LOAD PMDS FORM
            // ============================================================
            var pmdsForm = await _context.PMDSForms
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.EmployeeId == employee.Id);

            if (pmdsForm != null)
            {
                ViewBag.StartYear = pmdsForm.StartYear;
                ViewBag.EndYear = pmdsForm.EndYear;

                Console.WriteLine(
                    "Performance Cycle: " +
                    pmdsForm.StartYear +
                    "/" +
                    pmdsForm.EndYear);
            }
            else
            {
                ViewBag.StartYear = null;
                ViewBag.EndYear = null;

                Console.WriteLine(
                    "WARNING: PMDS Form was not found.");
            }


            // ============================================================
            // 6. AGE GROUP
            // ============================================================
            string ageGroup =
                employee.AgeGroup ?? "";

            // If AgeGroup is empty, calculate it from ID number
            if (string.IsNullOrWhiteSpace(ageGroup))
            {
                ageGroup =
                    CalculateAgeGroup(
                        employee.IdentificationNumber);
            }


            // ============================================================
            // 7. DISABILITY
            // ============================================================
            //
            // IMPORTANT:
            // HasDisability is bool?, therefore we explicitly compare
            // it to true.
            //
            // true  = Yes
            // false = No
            // null  = No
            // ============================================================

            bool hasDisability =
                employee.HasDisability == true;

            string disabled =
                hasDisability
                    ? "Yes"
                    : "No";


            // ============================================================
            // 8. DISABILITY DETAILS
            // ============================================================
            string disabilityDetails =
                hasDisability
                    ? employee.NatureOfDisability ?? ""
                    : "";


            // ============================================================
            // 9. BUILD PDP MODEL
            // ============================================================
            var model = new PDPModel
            {
                // --------------------------------------------------------
                // EMPLOYEE INFORMATION
                // --------------------------------------------------------

                PersalNo =
                    employee.PersalNumber,

                Surname =
                    string.IsNullOrWhiteSpace(employee.Initials)
                        ? employee.LastName
                        : employee.LastName +
                          " " +
                          employee.Initials,

                Directorate =
                    employee.Department,

                IdNumber =
                    employee.IdentificationNumber,

                Branch =
                    employee.BranchOrRegion,

                SalaryLevel =
                    employee.SalaryLevel,


                // --------------------------------------------------------
                // AUTO-POPULATED AGE GROUP
                // --------------------------------------------------------

                AgeGroup =
                    ageGroup,


                // --------------------------------------------------------
                // AUTO-POPULATED GENDER
                // --------------------------------------------------------

                Gender =
                    employee.Gender,


                // --------------------------------------------------------
                // AUTO-POPULATED RACE
                // --------------------------------------------------------

                Race =
                    employee.Race,


                // --------------------------------------------------------
                // AUTO-POPULATED DISABILITY
                // --------------------------------------------------------

                Disabled =
                    disabled,


                // --------------------------------------------------------
                // AUTO-POPULATED DISABILITY DETAILS
                // --------------------------------------------------------

                DisabilityDetails =
                    disabilityDetails
            };


            // ============================================================
            // 10. SUPERVISOR
            // ============================================================
            if (employee.Supervisor != null)
            {
                model.Supervisor =
                    employee.Supervisor.LastName;

                model.SupervisorPosition =
                    employee.Supervisor.Position;
            }
            else
            {
                model.Supervisor =
                    employee.SupervisorSurnameInitials;

                model.SupervisorPosition =
                    employee.SupervisorRankPostLevel;
            }


            // ============================================================
            // 11. DEBUG - VALUES SENT TO VIEW
            // ============================================================
            Console.WriteLine("======================================");
            Console.WriteLine("PDP MODEL SENT TO VIEW");
            Console.WriteLine("======================================");

            Console.WriteLine(
                "MODEL AgeGroup = [" +
                model.AgeGroup +
                "]");

            Console.WriteLine(
                "MODEL Disabled = [" +
                model.Disabled +
                "]");

            Console.WriteLine(
                "MODEL DisabilityDetails = [" +
                model.DisabilityDetails +
                "]");

            Console.WriteLine(
                "MODEL Gender = [" +
                model.Gender +
                "]");

            Console.WriteLine(
                "MODEL Race = [" +
                model.Race +
                "]");

            Console.WriteLine("======================================");


            // ============================================================
            // 12. RETURN PDP VIEW
            // ============================================================
            return View(model);
        }


        // ============================================================
        // CALCULATE AGE GROUP
        // ============================================================
        private string CalculateAgeGroup(
            string? idNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(idNumber))
                {
                    Console.WriteLine(
                        "AGE GROUP: ID NUMBER IS EMPTY.");

                    return "";
                }

                // Remove spaces and non-numeric characters
                string cleanId =
                    new string(
                        idNumber
                            .Where(char.IsDigit)
                            .ToArray());

                Console.WriteLine(
                    "AGE GROUP: Clean ID = [" +
                    cleanId +
                    "]");


                // South African ID number must have 13 digits
                if (cleanId.Length != 13)
                {
                    Console.WriteLine(
                        "AGE GROUP: ID NUMBER IS NOT 13 DIGITS.");

                    return "";
                }


                // --------------------------------------------------------
                // YEAR
                // --------------------------------------------------------
                int yy =
                    int.Parse(
                        cleanId.Substring(
                            0,
                            2));


                // --------------------------------------------------------
                // MONTH
                // --------------------------------------------------------
                int month =
                    int.Parse(
                        cleanId.Substring(
                            2,
                            2));


                // --------------------------------------------------------
                // DAY
                // --------------------------------------------------------
                int day =
                    int.Parse(
                        cleanId.Substring(
                            4,
                            2));


                // --------------------------------------------------------
                // DETERMINE CENTURY
                // --------------------------------------------------------
                int currentYear =
                    DateTime.Today.Year;

                int currentCentury =
                    currentYear / 100;

                int currentTwoDigitYear =
                    currentYear % 100;

                int year;

                if (yy <= currentTwoDigitYear)
                {
                    year =
                        (currentCentury * 100) +
                        yy;
                }
                else
                {
                    year =
                        ((currentCentury - 1) * 100) +
                        yy;
                }


                // --------------------------------------------------------
                // CREATE DATE OF BIRTH
                // --------------------------------------------------------
                DateTime dateOfBirth;

                try
                {
                    dateOfBirth =
                        new DateTime(
                            year,
                            month,
                            day);
                }
                catch
                {
                    Console.WriteLine(
                        "AGE GROUP: INVALID DATE IN ID NUMBER.");

                    return "";
                }


                // --------------------------------------------------------
                // CALCULATE AGE
                // --------------------------------------------------------
                int age =
                    DateTime.Today.Year -
                    dateOfBirth.Year;

                if (dateOfBirth.Date >
                    DateTime.Today.AddYears(-age))
                {
                    age--;
                }


                Console.WriteLine(
                    "AGE GROUP: Date of Birth = " +
                    dateOfBirth.ToString("yyyy-MM-dd"));

                Console.WriteLine(
                    "AGE GROUP: Age = " +
                    age);


                // --------------------------------------------------------
                // AGE GROUPS
                // --------------------------------------------------------
                if (age < 18)
                    return "Under 18";

                if (age <= 24)
                    return "18 - 24";

                if (age <= 34)
                    return "25 - 34";

                if (age <= 44)
                    return "35 - 44";

                if (age <= 54)
                    return "45 - 54";

                if (age <= 64)
                    return "55 - 64";

                return "65+";
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "AGE GROUP ERROR: " +
                    ex.Message);

                return "";
            }
        }


        // ============================================================
        // SAVE PDP
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePDP(
            PDPModel model)
        {
            // ============================================================
            // FIND EMPLOYEE AGAIN
            // ============================================================
            Employee? employee = null;

            if (!string.IsNullOrWhiteSpace(model.PersalNo))
            {
                employee = await _context.Employees
                    .Include(e => e.Supervisor)
                    .FirstOrDefaultAsync(e =>
                        e.PersalNumber == model.PersalNo);
            }


            // ============================================================
            // IF EMPLOYEE NOT FOUND, USE LOGGED-IN USER
            // ============================================================
            if (employee == null)
            {
                var userId =
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier);

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    employee = await _context.Employees
                        .Include(e => e.Supervisor)
                        .FirstOrDefaultAsync(e =>
                            e.UserId == userId);
                }
            }


            // ============================================================
            // EMPLOYEE NOT FOUND
            // ============================================================
            if (employee == null)
            {
                TempData["Error"] =
                    "Employee record could not be found.";

                return View(
                    "Create",
                    model);
            }


            // ============================================================
            // VALIDATE MODEL
            // ============================================================
            if (!ModelState.IsValid)
            {
                return View(
                    "Create",
                    model);
            }


            // ============================================================
            // AUTO-POPULATE EMPLOYEE INFORMATION
            // ============================================================

            string ageGroup =
                employee.AgeGroup ?? "";

            if (string.IsNullOrWhiteSpace(ageGroup))
            {
                ageGroup =
                    CalculateAgeGroup(
                        employee.IdentificationNumber);
            }


            // ============================================================
            // DISABILITY
            // ============================================================

            bool hasDisability =
                employee.HasDisability == true;

            string disabled =
                hasDisability
                    ? "Yes"
                    : "No";


            string disabilityDetails =
                hasDisability
                    ? employee.NatureOfDisability ?? ""
                    : "";


            // ============================================================
            // CREATE MAIN PDP
            // ============================================================
            var pdp = new PDPModel
            {
                // ========================================================
                // EMPLOYEE INFORMATION
                // ========================================================

                Surname =
                    string.IsNullOrWhiteSpace(employee.Initials)
                        ? employee.LastName
                        : employee.LastName +
                          " " +
                          employee.Initials,

                PersalNo =
                    employee.PersalNumber,

                Directorate =
                    employee.Department,

                IdNumber =
                    employee.IdentificationNumber,

                Branch =
                    employee.BranchOrRegion,

                SalaryLevel =
                    employee.SalaryLevel,


                // ========================================================
                // AUTO-POPULATED INFORMATION
                // ========================================================

                AgeGroup =
                    ageGroup,

                Gender =
                    employee.Gender,

                Race =
                    employee.Race,

                Disabled =
                    disabled,

                DisabilityDetails =
                    disabilityDetails,


                // ========================================================
                // SUPERVISOR
                // ========================================================

                SupervisorPosition =
                    employee.Supervisor != null
                        ? employee.Supervisor.Position
                        : employee.SupervisorRankPostLevel,

                Supervisor =
                    employee.Supervisor != null
                        ? employee.Supervisor.LastName
                        : employee.SupervisorSurnameInitials,


                // ========================================================
                // EDUCATIONAL / DEVELOPMENT INFORMATION
                // ========================================================

                Workshops =
                    model.Workshops,

                CurrentStudies =
                    model.CurrentStudies,

                Bursary =
                    model.Bursary,

                BursaryDuration =
                    model.BursaryDuration,


                // ========================================================
                // DECLARATION / SIGNATURES
                // ========================================================

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


                // ========================================================
                // OFFICE USE ONLY
                // ========================================================

                CapturedOnDatabase =
                    model.CapturedOnDatabase,

                DateCaptured =
                    model.DateCaptured,


                // ========================================================
                // PDP
                // ========================================================

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
                var qualification =
                    Request.Form[
                        $"Qualification{educationNumber}"
                    ].FirstOrDefault();

                var nqf =
                    Request.Form[
                        $"EducationNQFLevel{educationNumber}"
                    ].FirstOrDefault();

                var year =
                    Request.Form[
                        $"YearCompleted{educationNumber}"
                    ].FirstOrDefault();


                // STOP WHEN THERE ARE NO MORE ROWS
                if (qualification == null &&
                    nqf == null &&
                    year == null)
                {
                    break;
                }


                // SAVE ROW IF IT HAS INFORMATION
                if (!string.IsNullOrWhiteSpace(qualification) ||
                    !string.IsNullOrWhiteSpace(nqf) ||
                    !string.IsNullOrWhiteSpace(year))
                {
                    pdp.Education.Add(
                        new PDPEducation
                        {
                            Qualification =
                                qualification,

                            NQF =
                                nqf,

                            Year =
                                year
                        });
                }

                educationNumber++;
            }


            // ============================================================
            // JOB REQUIREMENT ROWS
            // ============================================================

            int jobNumber = 1;

            while (true)
            {
                var task =
                    Request.Form[
                        $"Task{jobNumber}"
                    ].FirstOrDefault();

                var training =
                    Request.Form[
                        $"Training{jobNumber}"
                    ].FirstOrDefault();

                var learningType =
                    Request.Form[
                        $"LearningType{jobNumber}"
                    ].FirstOrDefault();

                var nqfLevel =
                    Request.Form[
                        $"JobNQFLevel{jobNumber}"
                    ].FirstOrDefault();

                var cost =
                    Request.Form[
                        $"Cost{jobNumber}"
                    ].FirstOrDefault();

                var impact =
                    Request.Form[
                        $"Impact{jobNumber}"
                    ].FirstOrDefault();


                // STOP WHEN THERE ARE NO MORE ROWS
                if (task == null &&
                    training == null &&
                    learningType == null &&
                    nqfLevel == null &&
                    cost == null &&
                    impact == null)
                {
                    break;
                }


                // SAVE ROW IF IT HAS INFORMATION
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
                            Task =
                                task,

                            Training =
                                training,

                            LearningType =
                                learningType,

                            NQFLevel =
                                nqfLevel,

                            Cost =
                                cost,

                            Impact =
                                impact
                        });
                }

                jobNumber++;
            }


            // ============================================================
            // SAVE PDP
            // ============================================================

            _context.PDPs.Add(pdp);
      
            await _context.SaveChangesAsync();


            // ============================================================
            // SUCCESS MESSAGE
            // ============================================================

            TempData["Success"] =
                "First Cycle Completed Successfully!";


            // ============================================================
            // RETURN TO PDP
            // ============================================================

            return RedirectToAction(
                "Create",
                new
                {
                    persalNo =
                        employee.PersalNumber
                });
        }
    }
}