using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace PMDSSystems.Controllers
{
    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("PERFORMANCE AGREEMENT CREATE GET");
            Console.WriteLine("========================================");

            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            Console.WriteLine(
                "Logged-in UserId: " +
                (userId ?? "NULL")
            );

            // --------------------------------------------------------
            // CHECK USER
            // --------------------------------------------------------

            if (string.IsNullOrWhiteSpace(userId))
            {
                Console.WriteLine(
                    "ERROR: No logged-in Identity user."
                );

                return Challenge();
            }

            // --------------------------------------------------------
            // FIND EMPLOYEE
            // --------------------------------------------------------

            var employee = await _context.Employees
                .Include(e => e.Supervisor)
                .FirstOrDefaultAsync(
                    e => e.UserId == userId
                );

            if (employee == null)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("ERROR: EMPLOYEE NOT FOUND");
                Console.WriteLine(
                    "UserId searched: " + userId
                );
                Console.WriteLine("========================================");

                return Content(
                    "Performance Agreement cannot open because your Employee record " +
                    "does not have the correct UserId linked to your logged-in account."
                );
            }

            Console.WriteLine("Employee found:");
            Console.WriteLine(
                "Employee ID: " + employee.Id
            );
            Console.WriteLine(
                "Persal Number: " + employee.PersalNumber
            );
            Console.WriteLine(
                "Name: " +
                employee.FirstName +
                " " +
                employee.LastName
            );
            Console.WriteLine(
                "Salary Level: " +
                employee.SalaryLevel
            );

            // --------------------------------------------------------
            // FIND PART A
            // --------------------------------------------------------

            var pmdsForm = await _context.PMDSForms
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.EmployeeId == employee.Id
                );

            if (pmdsForm == null)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("ERROR: PART A NOT FOUND");
                Console.WriteLine(
                    "Employee ID: " + employee.Id
                );
                Console.WriteLine("========================================");

                return Content(
                    "Performance Agreement cannot open because no Part A record " +
                    "was found for this employee."
                );
            }

            // --------------------------------------------------------
            // CHECK PART A PERFORMANCE CYCLE
            // --------------------------------------------------------

            if (!pmdsForm.StartYear.HasValue ||
                !pmdsForm.EndYear.HasValue)
            {
                Console.WriteLine("========================================");
                Console.WriteLine(
                    "ERROR: PART A HAS NO PERFORMANCE CYCLE"
                );
                Console.WriteLine("========================================");

                return Content(
                    "Performance Agreement cannot open because the Start Year " +
                    "and End Year have not been saved in Part A."
                );
            }

            int startYear = pmdsForm.StartYear.Value;
            int endYear = pmdsForm.EndYear.Value;

            Console.WriteLine(
                $"Performance Cycle: {startYear}/{endYear}"
            );

            // --------------------------------------------------------
            // CREATE MODEL
            // --------------------------------------------------------

            var model = new PerformanceAgreement
            {
                StartYear = startYear,
                EndYear = endYear,

                // Always take salary level from Employee.
                SalaryLevel = employee.SalaryLevel,

                // Level 2-4 starts with exactly 4.
                // Other levels start with 1 and can be increased
                // according to the rules in the Create page.
                NumberOfKRAs =
                    GetInitialKraCount(
                        employee.SalaryLevel
                    ),

                KRAs = new List<KRA>()
            };

            // --------------------------------------------------------
            // EMPLOYEE INFORMATION
            // --------------------------------------------------------

            ViewBag.EmployeeNameAndCapacity =
                $"{employee.FirstName} " +
                $"{employee.LastName} - " +
                $"{employee.Position}";

            // --------------------------------------------------------
            // SUPERVISOR INFORMATION
            // --------------------------------------------------------

            if (employee.Supervisor != null)
            {
                ViewBag.SupervisorNameAndCapacity =
                    $"{employee.Supervisor.FirstName} " +
                    $"{employee.Supervisor.LastName} - " +
                    $"{employee.Supervisor.Position}";
            }
            else
            {
                ViewBag.SupervisorNameAndCapacity =
                    "Supervisor Not Assigned";
            }

            // --------------------------------------------------------
            // PERFORMANCE CYCLE
            // --------------------------------------------------------

            ViewBag.StartYear = startYear;
            ViewBag.EndYear = endYear;

            ViewBag.PerformanceCycle =
                $"{startYear}/{endYear}";

            // --------------------------------------------------------
            // TOTAL WEIGHT
            // --------------------------------------------------------

            ViewBag.TotalWeight = 0m;

            // --------------------------------------------------------
            // SUPERVISOR SIGNATURE
            // --------------------------------------------------------

            ViewBag.CanSupervisorSign = false;
            ViewBag.ExistingSupervisorSignature = "";

            Console.WriteLine("========================================");
            Console.WriteLine("CREATE PAGE READY");
            Console.WriteLine(
                $"Cycle: {startYear}/{endYear}"
            );
            Console.WriteLine(
                $"Salary Level: {employee.SalaryLevel}"
            );
            Console.WriteLine(
                $"Initial KRA Count: {model.NumberOfKRAs}"
            );
            Console.WriteLine("========================================");

            return View("Create", model);
        }


        // ============================================================
        // CREATE - POST
        // ============================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            PerformanceAgreement model)
        {
            Console.WriteLine("========================================");
            Console.WriteLine(
                "PERFORMANCE AGREEMENT CREATE POST"
            );
            Console.WriteLine("========================================");

            // --------------------------------------------------------
            // GET CURRENT USER
            // --------------------------------------------------------

            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            // --------------------------------------------------------
            // FIND EMPLOYEE
            // --------------------------------------------------------

            var employee = await _context.Employees
                .Include(e => e.Supervisor)
                .FirstOrDefaultAsync(
                    e => e.UserId == userId
                );

            if (employee == null)
            {
                TempData["Error"] =
                    "Your employee profile could not be found.";

                return RedirectToAction(
                    "Cycles",
                    "PMDS"
                );
            }

            // --------------------------------------------------------
            // GET PART A
            // --------------------------------------------------------

            var pmdsForm = await _context.PMDSForms
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.EmployeeId == employee.Id
                );

            if (pmdsForm == null ||
                !pmdsForm.StartYear.HasValue ||
                !pmdsForm.EndYear.HasValue)
            {
                TempData["Error"] =
                    "Please complete Part A before saving the Performance Agreement.";

                return RedirectToAction(
                    "Create",
                    "PMDS"
                );
            }

            // --------------------------------------------------------
            // FORCE CYCLE FROM PART A
            // --------------------------------------------------------

            model.StartYear =
                pmdsForm.StartYear.Value;

            model.EndYear =
                pmdsForm.EndYear.Value;

            // --------------------------------------------------------
            // FORCE SALARY LEVEL FROM EMPLOYEE
            // --------------------------------------------------------
            //
            // Do NOT trust the salary level posted by the browser.
            //

            model.SalaryLevel =
                employee.SalaryLevel;

            Console.WriteLine(
                "Employee Salary Level: " +
                employee.SalaryLevel
            );

            Console.WriteLine(
                "Received KRA Count: " +
                (model.KRAs?.Count ?? 0)
            );

            // --------------------------------------------------------
            // REMOVE OPTIONAL VALIDATION
            // --------------------------------------------------------

            ModelState.Remove("JobPurpose");
            ModelState.Remove("JobKnowledge");
            ModelState.Remove("Responsibility");
            ModelState.Remove("QualityOfWork");
            ModelState.Remove("TechnicalSkills");
            ModelState.Remove("Reliability");
            ModelState.Remove("Communication");
            ModelState.Remove("TeamWork");
            ModelState.Remove("Leadership");
            ModelState.Remove("Id");

            // --------------------------------------------------------
            // MAKE SURE KRA COLLECTION EXISTS
            // --------------------------------------------------------

            if (model.KRAs == null)
            {
                model.KRAs = new List<KRA>();
            }

            // --------------------------------------------------------
            // REMOVE COMPLETELY EMPTY KRA ROWS
            // --------------------------------------------------------

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

            // --------------------------------------------------------
            // ACTUAL KRA COUNT
            // --------------------------------------------------------

            int actualKraCount =
                model.KRAs.Count;

            model.NumberOfKRAs =
                actualKraCount;

            Console.WriteLine(
                "FINAL KRA COUNT: " +
                actualKraCount
            );

            // ========================================================
            // KRA COUNT VALIDATION
            // ========================================================

            ValidateKraCount(
                employee.SalaryLevel,
                actualKraCount
            );

            // ========================================================
            // VALIDATE EACH KRA
            // ========================================================

            decimal totalWeight = 0m;

            for (int i = 0; i < model.KRAs.Count; i++)
            {
                var kra = model.KRAs[i];

                // ----------------------------------------------------
                // KRA NAME
                // ----------------------------------------------------

                if (string.IsNullOrWhiteSpace(kra.Name))
                {
                    ModelState.AddModelError(
                        $"KRAs[{i}].Name",
                        $"KRA {i + 1}: Please enter the KRA name."
                    );
                }

                // ----------------------------------------------------
                // ACTIVITIES
                // ----------------------------------------------------

                if (string.IsNullOrWhiteSpace(kra.Activities))
                {
                    ModelState.AddModelError(
                        $"KRAs[{i}].Activities",
                        $"KRA {i + 1}: Please enter the activities."
                    );
                }

                // ----------------------------------------------------
                // STANDARDS
                // ----------------------------------------------------

                if (string.IsNullOrWhiteSpace(kra.Standards))
                {
                    ModelState.AddModelError(
                        $"KRAs[{i}].Standards",
                        $"KRA {i + 1}: Please enter the standards."
                    );
                }

                // ====================================================
                // WEIGHT
                // ====================================================

                if (string.IsNullOrWhiteSpace(kra.Weight))
                {
                    ModelState.AddModelError(
                        $"KRAs[{i}].Weight",
                        $"KRA {i + 1}: Please enter a weight."
                    );
                }
                else
                {
                    if (TryParseWeight(
                        kra.Weight,
                        out decimal parsedWeight))
                    {
                        if (parsedWeight <= 0)
                        {
                            ModelState.AddModelError(
                                $"KRAs[{i}].Weight",
                                $"KRA {i + 1}: Weight must be greater than 0%."
                            );
                        }
                        else
                        {
                            totalWeight += parsedWeight;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(
                            $"KRAs[{i}].Weight",
                            $"KRA {i + 1}: Weight must be a valid number."
                        );
                    }
                }

                // ====================================================
                // BATHO PELE
                // ====================================================

                if (string.IsNullOrWhiteSpace(kra.BathoPele))
                {
                    ModelState.AddModelError(
                        $"KRAs[{i}].BathoPele",
                        $"KRA {i + 1}: Please select at least one Batho Pele principle."
                    );
                }

                // ====================================================
                // GAF
                // ====================================================

                if (string.IsNullOrWhiteSpace(kra.GAFs))
                {
                    ModelState.AddModelError(
                        $"KRAs[{i}].GAFs",
                        $"KRA {i + 1}: Please select the applicable GAFs."
                    );
                }
                else
                {
                    // ------------------------------------------------
                    // JOB KNOWLEDGE
                    // ------------------------------------------------

                    if (!ContainsSelection(
                        kra.GAFs,
                        "Job Knowledge"))
                    {
                        ModelState.AddModelError(
                            $"KRAs[{i}].GAFs",
                            $"KRA {i + 1}: Job Knowledge is compulsory."
                        );
                    }

                    // ------------------------------------------------
                    // ACCEPTANCE OF RESPONSIBILITY
                    // ------------------------------------------------

                    if (!ContainsSelection(
                        kra.GAFs,
                        "Acceptance of Responsibility"))
                    {
                        ModelState.AddModelError(
                            $"KRAs[{i}].GAFs",
                            $"KRA {i + 1}: Acceptance of Responsibility is compulsory."
                        );
                    }
                }
            }

            // ========================================================
            // TOTAL WEIGHT
            // ========================================================

            totalWeight =
                decimal.Round(
                    totalWeight,
                    2,
                    MidpointRounding.AwayFromZero
                );

            Console.WriteLine(
                "TOTAL KRA WEIGHT: " +
                totalWeight +
                "%"
            );

            // --------------------------------------------------------
            // MUST BE EXACTLY 100
            // --------------------------------------------------------

            if (totalWeight != 100m)
            {
                ModelState.AddModelError(
                    "",
                    $"The total KRA weight must be exactly 100%. " +
                    $"Current total is {totalWeight}%."
                );
            }

            ViewBag.TotalWeight =
                totalWeight;

            // ========================================================
            // VALIDATE MODEL STATE
            // ========================================================

            if (!ModelState.IsValid)
            {
                Console.WriteLine("========================================");
                Console.WriteLine(
                    "PERFORMANCE AGREEMENT VALIDATION FAILED"
                );
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

                await PrepareCreateView(
                    employee,
                    model
                );

                return View(
                    "Create",
                    model
                );
            }

            // ========================================================
            // DATABASE SAVE
            // ========================================================

            try
            {
                Console.WriteLine(
                    "STARTING DATABASE SAVE"
                );

                var krasToSave =
                    model.KRAs.ToList();

                // ----------------------------------------------------
                // RESET AGREEMENT ID
                // ----------------------------------------------------

                model.Id = 0;

                // ----------------------------------------------------
                // RESET KRA IDs
                // ----------------------------------------------------

                foreach (var kra in krasToSave)
                {
                    kra.Id = 0;

                    kra.PerformanceAgreementId = 0;

                    kra.PerformanceAgreement =
                        model;
                }

                model.KRAs =
                    krasToSave;

                // ----------------------------------------------------
                // SAVE
                // ----------------------------------------------------

                _context.PerformanceAgreements.Add(
                    model
                );

                await _context.SaveChangesAsync();

                Console.WriteLine(
                    "PERFORMANCE AGREEMENT SAVED. ID = " +
                    model.Id
                );

                Console.WriteLine(
                    "KRAs SAVED = " +
                    model.KRAs.Count
                );

                // ====================================================
                // VERIFY AGREEMENT
                // ====================================================

                var savedAgreement =
                    await _context.PerformanceAgreements
                        .Include(p => p.KRAs)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(
                            p => p.Id == model.Id
                        );

                if (savedAgreement == null)
                {
                    ModelState.AddModelError(
                        "",
                        "The Performance Agreement could not be found after saving."
                    );

                    model.KRAs =
                        krasToSave;

                    await PrepareCreateView(
                        employee,
                        model
                    );

                    return View(
                        "Create",
                        model
                    );
                }

                // ====================================================
                // VERIFY KRAs
                // ====================================================

                int savedKraCount =
                    savedAgreement.KRAs?.Count ?? 0;

                Console.WriteLine(
                    "Expected KRAs: " +
                    krasToSave.Count
                );

                Console.WriteLine(
                    "Saved KRAs: " +
                    savedKraCount
                );

                if (savedKraCount != krasToSave.Count)
                {
                    ModelState.AddModelError(
                        "",
                        "The Performance Agreement was saved, " +
                        "but not all KRAs were saved."
                    );

                    model.KRAs =
                        krasToSave;

                    await PrepareCreateView(
                        employee,
                        model
                    );

                    return View(
                        "Create",
                        model
                    );
                }
                // ====================================================
                // SUCCESS
                // ====================================================

                TempData["Success"] =
                    "Performance Agreement and " +
                    savedKraCount +
                    " KRA(s) saved successfully.";

                Console.WriteLine(
                    "PERFORMANCE AGREEMENT SAVED SUCCESSFULLY."
                );

                Console.WriteLine(
                    "Agreement ID: " +
                    savedAgreement.Id
                );

                // ----------------------------------------------------
                // REDIRECT TO PMDS CYCLES PAGE
                // ----------------------------------------------------

                return RedirectToAction(
                    "Cycles",
                    "PMDS"
                );
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("DATABASE ERROR");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("========================================");

                string errorMessage =
                    ex.InnerException?.Message ??
                    ex.Message;

                ModelState.AddModelError(
                    "",
                    "DATABASE ERROR: " +
                    errorMessage
                );

                model.KRAs =
                    model.KRAs ??
                    new List<KRA>();

                await PrepareCreateView(
                    employee,
                    model
                );

                return View(
                    "Create",
                    model
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine("GENERAL ERROR");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("========================================");

                ModelState.AddModelError(
                    "",
                    "ERROR: " +
                    (
                        ex.InnerException?.Message ??
                        ex.Message
                    )
                );

                model.KRAs =
                    model.KRAs ??
                    new List<KRA>();

                await PrepareCreateView(
                    employee,
                    model
                );

                return View(
                    "Create",
                    model
                );
            }
        }


        // ============================================================
        // VALIDATE KRA COUNT
        // ============================================================

        private void ValidateKraCount(
            string? salaryLevel,
            int kraCount)
        {
            int level =
                ExtractSalaryLevel(
                    salaryLevel
                );

            Console.WriteLine(
                "========================================"
            );

            Console.WriteLine(
                "KRA COUNT VALIDATION"
            );

            Console.WriteLine(
                "Salary Level: " +
                salaryLevel
            );

            Console.WriteLine(
                "Extracted Level: " +
                level
            );

            Console.WriteLine(
                "KRA Count: " +
                kraCount
            );

            Console.WriteLine(
                "========================================");

            // --------------------------------------------------------
            // LEVEL 2-4
            // EXACTLY 4
            // --------------------------------------------------------

            if (level >= 2 && level <= 4)
            {
                if (kraCount != 4)
                {
                    ModelState.AddModelError(
                        "NumberOfKRAs",
                        "Salary Levels 2–4 require exactly 4 KRAs."
                    );
                }

                return;
            }

            // --------------------------------------------------------
            // LEVEL 5-7
            // MAXIMUM 6
            // --------------------------------------------------------

            if (level >= 5 && level <= 7)
            {
                if (kraCount < 1)
                {
                    ModelState.AddModelError(
                        "NumberOfKRAs",
                        "At least 1 KRA is required."
                    );
                }

                if (kraCount > 6)
                {
                    ModelState.AddModelError(
                        "NumberOfKRAs",
                        "Salary Levels 5–7 allow a maximum of 6 KRAs."
                    );
                }

                return;
            }

            // --------------------------------------------------------
            // LEVEL 8-16
            // MAXIMUM 6
            // --------------------------------------------------------

            if (level >= 8 && level <= 16)
            {
                if (kraCount < 1)
                {
                    ModelState.AddModelError(
                        "NumberOfKRAs",
                        "At least 1 KRA is required."
                    );
                }

                if (kraCount > 6)
                {
                    ModelState.AddModelError(
                        "NumberOfKRAs",
                        "Salary Levels 8–16 allow a maximum of 6 KRAs."
                    );
                }

                return;
            }

            // --------------------------------------------------------
            // INVALID SALARY LEVEL
            // --------------------------------------------------------

            if (string.IsNullOrWhiteSpace(salaryLevel))
            {
                ModelState.AddModelError(
                    "SalaryLevel",
                    "Salary Level could not be determined from your employee profile."
                );

                return;
            }

            if (level == 0)
            {
                ModelState.AddModelError(
                    "SalaryLevel",
                    "The employee salary level is invalid or could not be determined."
                );
            }
        }


        // ============================================================
        // EXTRACT NUMERIC SALARY LEVEL
        // ============================================================

        private int ExtractSalaryLevel(
            string? salaryLevel)
        {
            if (string.IsNullOrWhiteSpace(salaryLevel))
            {
                return 0;
            }

            // --------------------------------------------------------
            // Examples:
            //
            // Production (Level 2-4)
            // Production (Level 5-7)
            // Supervisor / Junior Management (Level 8-10)
            // Middle Management / Chief (Level 11-12)
            // Senior Management (Level 13-16)
            //
            // Returns the first number.
            // --------------------------------------------------------

            Match match =
                Regex.Match(
                    salaryLevel,
                    @"Level\s*(\d+)",
                    RegexOptions.IgnoreCase
                );

            if (match.Success &&
                int.TryParse(
                    match.Groups[1].Value,
                    out int level
                ))
            {
                return level;
            }

            // --------------------------------------------------------
            // If database contains simply:
            //
            // "2"
            // "8"
            // "12"
            //
            // --------------------------------------------------------

            Match numberMatch =
                Regex.Match(
                    salaryLevel,
                    @"\b(\d{1,2})\b"
                );

            if (numberMatch.Success &&
                int.TryParse(
                    numberMatch.Groups[1].Value,
                    out int numericLevel
                ))
            {
                return numericLevel;
            }

            return 0;
        }


        // ============================================================
        // INITIAL KRA COUNT
        // ============================================================

        private int GetInitialKraCount(
            string? salaryLevel)
        {
            int level =
                ExtractSalaryLevel(
                    salaryLevel
                );

            // Level 2-4 MUST have exactly 4.
            if (level >= 2 && level <= 4)
            {
                return 4;
            }

            // Other levels start with 1.
            return 1;
        }


        // ============================================================
        // PARSE WEIGHT
        // ============================================================

        private bool TryParseWeight(
            string? value,
            out decimal weight)
        {
            weight = 0m;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            // --------------------------------------------------------
            // First try invariant culture.
            // --------------------------------------------------------

            if (decimal.TryParse(
                value.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out weight))
            {
                return true;
            }

            // --------------------------------------------------------
            // Then try current culture.
            // --------------------------------------------------------

            if (decimal.TryParse(
                value.Trim(),
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out weight))
            {
                return true;
            }

            // --------------------------------------------------------
            // Finally try South African culture.
            // --------------------------------------------------------

            var southAfricanCulture =
                CultureInfo.GetCultureInfo("en-ZA");

            return decimal.TryParse(
                value.Trim(),
                NumberStyles.Number,
                southAfricanCulture,
                out weight
            );
        }


        // ============================================================
        // CHECK GAF SELECTION
        // ============================================================

        private bool ContainsSelection(
            string? selections,
            string requiredSelection)
        {
            if (string.IsNullOrWhiteSpace(selections))
            {
                return false;
            }

            // --------------------------------------------------------
            // Remove common JSON-style characters.
            // --------------------------------------------------------

            string cleaned =
                selections
                    .Replace("[", "")
                    .Replace("]", "")
                    .Replace("\"", "");

            // --------------------------------------------------------
            // Split possible multi-select formats.
            // --------------------------------------------------------

            var values =
                cleaned
                    .Split(
                        new[]
                        {
                            ',',
                            ';',
                            '|'
                        },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                    .Select(
                        x => x.Trim()
                    )
                    .Where(
                        x =>
                            !string.IsNullOrWhiteSpace(x)
                    )
                    .ToList();

            return values.Any(
                x =>
                    string.Equals(
                        x,
                        requiredSelection,
                        StringComparison.OrdinalIgnoreCase
                    )
            );
        }


        // ============================================================
        // PREPARE CREATE VIEW
        // ============================================================

        private async Task PrepareCreateView(
            Employee employee,
            PerformanceAgreement model)
        {
            // --------------------------------------------------------
            // EMPLOYEE
            // --------------------------------------------------------

            ViewBag.EmployeeNameAndCapacity =
                $"{employee.FirstName} " +
                $"{employee.LastName} - " +
                $"{employee.Position}";

            // --------------------------------------------------------
            // SUPERVISOR
            // --------------------------------------------------------

            if (employee.Supervisor != null)
            {
                ViewBag.SupervisorNameAndCapacity =
                    $"{employee.Supervisor.FirstName} " +
                    $"{employee.Supervisor.LastName} - " +
                    $"{employee.Supervisor.Position}";
            }
            else
            {
                ViewBag.SupervisorNameAndCapacity =
                    "Supervisor Not Assigned";
            }

            // --------------------------------------------------------
            // PART A CYCLE
            // --------------------------------------------------------

            var pmdsForm =
                await _context.PMDSForms
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x =>
                            x.EmployeeId ==
                            employee.Id
                    );

            if (pmdsForm != null &&
                pmdsForm.StartYear.HasValue &&
                pmdsForm.EndYear.HasValue)
            {
                // PMDSForm fields are nullable int?.
                ViewBag.StartYear =
                    pmdsForm.StartYear.Value;

                ViewBag.EndYear =
                    pmdsForm.EndYear.Value;

                ViewBag.PerformanceCycle =
                    $"{pmdsForm.StartYear.Value}/" +
                    $"{pmdsForm.EndYear.Value}";
            }
            else
            {
                // PerformanceAgreement fields are normal int.
                ViewBag.StartYear =
                    model.StartYear;

                ViewBag.EndYear =
                    model.EndYear;

                ViewBag.PerformanceCycle =
                    $"{model.StartYear}/" +
                    $"{model.EndYear}";
            }

            // --------------------------------------------------------
            // SALARY LEVEL
            // --------------------------------------------------------

            ViewBag.SalaryLevel =
                employee.SalaryLevel;

            // --------------------------------------------------------
            // KRA LIMIT
            // --------------------------------------------------------

            int level =
                ExtractSalaryLevel(
                    employee.SalaryLevel
                );

            if (level >= 2 && level <= 4)
            {
                ViewBag.RequiredKraCount = 4;
                ViewBag.MaxKraCount = 4;
            }
            else
            {
                ViewBag.RequiredKraCount = 1;
                ViewBag.MaxKraCount = 6;
            }

            // --------------------------------------------------------
            // TOTAL WEIGHT
            // --------------------------------------------------------

            decimal totalWeight = 0m;

            if (model.KRAs != null)
            {
                foreach (var kra in model.KRAs)
                {
                    if (TryParseWeight(
                        kra.Weight,
                        out decimal weight))
                    {
                        totalWeight += weight;
                    }
                }
            }

            ViewBag.TotalWeight =
                decimal.Round(
                    totalWeight,
                    2,
                    MidpointRounding.AwayFromZero
                );

            // --------------------------------------------------------
            // SUPERVISOR SIGNATURE
            // --------------------------------------------------------

            ViewBag.CanSupervisorSign =
                false;

            ViewBag.ExistingSupervisorSignature =
                "";
        }


        // ============================================================
        // AGREEMENT - GET
        // ============================================================

        [HttpGet]
        public async Task<IActionResult> Agreement(
            int? agreementId)
        {
            Console.WriteLine(
                "========================================"
            );

            Console.WriteLine(
                "AGREEMENT PAGE REQUESTED"
            );

            Console.WriteLine(
                "Agreement ID: " +
                (
                    agreementId?.ToString() ??
                    "NULL"
                )
            );

            // --------------------------------------------------------
            // LOAD AGREEMENT
            // --------------------------------------------------------

            PerformanceAgreement? agreement;

            if (!agreementId.HasValue ||
                agreementId.Value <= 0)
            {
                agreement =
                    await _context.PerformanceAgreements
                        .Include(p => p.KRAs)
                        .OrderByDescending(
                            p => p.Id
                        )
                        .FirstOrDefaultAsync();

                if (agreement == null)
                {
                    TempData["Error"] =
                        "No Performance Agreement was found.";

                    return RedirectToAction(
                        nameof(Create)
                    );
                }
            }
            else
            {
                agreement =
                    await _context.PerformanceAgreements
                        .Include(p => p.KRAs)
                        .FirstOrDefaultAsync(
                            p =>
                                p.Id ==
                                agreementId.Value
                        );

                if (agreement == null)
                {
                    TempData["Error"] =
                        "Performance Agreement with ID " +
                        agreementId.Value +
                        " was not found.";

                    return RedirectToAction(
                        nameof(Index)
                    );
                }
            }

            // --------------------------------------------------------
            // CURRENT USER
            // --------------------------------------------------------

            var currentUserId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                );

            ViewBag.EmployeeNameAndCapacity =
                "Employee";

            ViewBag.SupervisorNameAndCapacity =
                "Supervisor";

            ViewBag.CanSupervisorSign =
                false;

            ViewBag.ExistingSupervisorSignature =
                "";

            // --------------------------------------------------------
            // CURRENT EMPLOYEE
            // --------------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                currentUserId))
            {
                var currentEmployee =
                    await _context.Employees
                        .Include(e => e.Supervisor)
                        .FirstOrDefaultAsync(
                            e =>
                                e.UserId ==
                                currentUserId
                        );

                if (currentEmployee != null)
                {
                    ViewBag.EmployeeNameAndCapacity =
                        $"{currentEmployee.FirstName} " +
                        $"{currentEmployee.LastName}";

                    // ------------------------------------------------
                    // PART A CYCLE
                    // ------------------------------------------------

                    var pmdsForm =
                        await _context.PMDSForms
                            .AsNoTracking()
                            .FirstOrDefaultAsync(
                                x =>
                                    x.EmployeeId ==
                                    currentEmployee.Id
                            );

                    if (pmdsForm != null &&
                        pmdsForm.StartYear.HasValue &&
                        pmdsForm.EndYear.HasValue)
                    {
                        // PMDSForm = nullable int?.
                        ViewBag.StartYear =
                            pmdsForm.StartYear.Value;

                        ViewBag.EndYear =
                            pmdsForm.EndYear.Value;
                    }
                    else
                    {
                        // PerformanceAgreement = int.
                        ViewBag.StartYear =
                            agreement.StartYear;

                        ViewBag.EndYear =
                            agreement.EndYear;
                    }

                    // ------------------------------------------------
                    // SUPERVISOR
                    // ------------------------------------------------

                    if (currentEmployee.Supervisor != null)
                    {
                        var supervisor =
                            currentEmployee.Supervisor;

                        ViewBag.SupervisorNameAndCapacity =
                            $"{supervisor.FirstName} " +
                            $"{supervisor.LastName}";

                        bool isSupervisor =
                            supervisor.UserId ==
                            currentUserId;

                        ViewBag.CanSupervisorSign =
                            isSupervisor;
                    }
                }
            }

            // --------------------------------------------------------
            // EXTRA VIEWBAGS
            // --------------------------------------------------------

            ViewBag.AgreementId =
                agreement.Id;

            ViewBag.KraCount =
                agreement.KRAs?.Count ?? 0;

            // --------------------------------------------------------
            // RETURN VIEW
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
                    .OrderByDescending(
                        p => p.Id
                    )
                    .AsNoTracking()
                    .ToListAsync();

            return View(agreements);
        }
    }
}