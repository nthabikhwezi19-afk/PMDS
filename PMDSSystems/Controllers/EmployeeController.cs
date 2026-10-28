using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using PMDSSystems.Services;
using System.Security.Claims;

namespace PMDSSystems.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;


        public EmployeeController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }


        //========================================================
        // EMPLOYEE LIST
        //========================================================

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _context.Employees
                .Include(e => e.Supervisor)
                .ToList();

            return View(employees);
        }


        //========================================================
        // CREATE EMPLOYEE - GET
        //========================================================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            LoadSupervisors();

            return View();
        }

        //========================================================
        // CREATE EMPLOYEE - POST
        //========================================================

        //========================================================
        // CREATE EMPLOYEE - POST
        //========================================================


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("CREATE EMPLOYEE POST RECEIVED");
            Console.WriteLine("======================================");

            Console.WriteLine($"PersalNumber: {employee.PersalNumber}");
            Console.WriteLine($"FirstName: {employee.FirstName}");
            Console.WriteLine($"LastName: {employee.LastName}");
            Console.WriteLine($"Email: {employee.Email}");
            Console.WriteLine($"Position: {employee.Position}");
            Console.WriteLine($"Department: {employee.Department}");
            Console.WriteLine($"Department: {employee.SalaryLevel}");
            Console.WriteLine($"Department: {employee.PostDesignation}");
            Console.WriteLine($"Department: {employee.AppointmentDateInCurrentRank}");
            Console.WriteLine($"Department: {employee.AppointmentInDcsDate}");
            Console.WriteLine($"Department: {employee.AppointmentInDcsDate}");
            Console.WriteLine($"Department: {employee.SupervisorSurnameInitials}");
            Console.WriteLine($"Department: {employee.SupervisorRankPostLevel}");
            Console.WriteLine($"Department: {employee.OSDDescription}");
            Console.WriteLine($"Department: {employee.PostLevel}");
            Console.WriteLine($"Department: {employee.BranchOrRegion}");
            Console.WriteLine($"Department: {employee.PreviousStation}");
            Console.WriteLine($"Department: {employee.TransferDate}");
            Console.WriteLine($"Department: {employee.PreviousStation}");
            Console.WriteLine($"Department: {employee.PreviousCyclePerformance}");


            //=====================================================
            //MODEL VALIDATION
            //=====================================================

            if (!ModelState.IsValid)
            {
                Console.WriteLine("MODELSTATE IS INVALID");

                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine(
                            $"FIELD: {modelState.Key} | ERROR: {error.ErrorMessage}");
                    }
                }

                LoadSupervisors();

                return View(employee);
            }

            try
            {
                // =====================================================
                // CHECK DUPLICATE PERSAL NUMBER
                // =====================================================

                var existingEmployee = await _context.Employees
                    .FirstOrDefaultAsync(e =>
                        e.PersalNumber == employee.PersalNumber);

                if (existingEmployee != null)
                {
                    ModelState.AddModelError(
                        "PersalNumber",
                        "An employee with this Persal Number already exists.");

                    LoadSupervisors();

                    return View(employee);
                }

                // =====================================================
                // EMAIL IS REQUIRED FOR AUTOMATIC ACCOUNT CREATION
                // =====================================================

                if (string.IsNullOrWhiteSpace(employee.Email))
                {
                    ModelState.AddModelError(
                        "Email",
                        "An email address is required to create the employee account.");

                    LoadSupervisors();

                    return View(employee);
                }

                // =====================================================
                // CHECK IF IDENTITY ACCOUNT ALREADY EXISTS
                // =====================================================

                var existingUser = await _userManager
                    .FindByEmailAsync(employee.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError(
                        "Email",
                        "An account already exists for this email address.");

                    LoadSupervisors();

                    return View(employee);
                }

                // =====================================================
                // GENERATE TEMPORARY PASSWORD
                // =====================================================

                var temporaryPassword =
                    GenerateTemporaryPassword();

                Console.WriteLine("TEMPORARY PASSWORD GENERATED");

                // =====================================================
                // CREATE IDENTITY USER
                // =====================================================

                var user = new IdentityUser
                {
                    UserName = employee.Email,
                    Email = employee.Email,
                    EmailConfirmed = true
                };

                Console.WriteLine("CREATING IDENTITY ACCOUNT...");

                var identityResult =
                    await _userManager.CreateAsync(
                        user,
                        temporaryPassword);

                if (!identityResult.Succeeded)
                {
                    Console.WriteLine("IDENTITY ACCOUNT CREATION FAILED");

                    foreach (var error in identityResult.Errors)
                    {
                        Console.WriteLine(
                            $"{error.Code}: {error.Description}");

                        ModelState.AddModelError(
                            "",
                            error.Description);
                    }

                    LoadSupervisors();

                    return View(employee);
                }

                Console.WriteLine(
                    $"IDENTITY ACCOUNT CREATED: {user.Id}");

                // =====================================================
                // ASSIGN EMPLOYEE ROLE
                // =====================================================

                var roleResult =
                    await _userManager.AddToRoleAsync(
                        user,
                        "Employee");

                if (!roleResult.Succeeded)
                {
                    Console.WriteLine("EMPLOYEE ROLE ASSIGNMENT FAILED");

                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine(
                            $"{error.Code}: {error.Description}");
                    }

                    await _userManager.DeleteAsync(user);

                    ModelState.AddModelError(
                        "",
                        "The employee account could not be assigned the Employee role.");

                    LoadSupervisors();

                    return View(employee);
                }

                Console.WriteLine("EMPLOYEE ROLE ASSIGNED");

                // ============================================================
                // EDIT EMPLOYEE - GET
                // ============================================================


                // =====================================================
                // LINK EMPLOYEE TO IDENTITY USER
                // =====================================================

                employee.UserId = user.Id;
                employee.MustChangePassword = true;

                // =====================================================
                // SAVE EMPLOYEE
                // =====================================================

                _context.Employees.Add(employee);



                await _context.SaveChangesAsync();

                Console.WriteLine("EMPLOYEE SAVED");
                Console.WriteLine(
                    $"Employee UserId: {employee.UserId}");

                // =====================================================
                // SEND WELCOME EMAIL
                // =====================================================

                var loginUrl =
                    $"{Request.Scheme}://{Request.Host}/Account/Login";

                var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>

<body style='font-family: Arial, sans-serif;'>

    <h2>Welcome to PMDS</h2>

    <p>Dear <strong>{employee.FirstName} {employee.LastName}</strong>,</p>

    <p>
        Your PMDS account has been created by the
        Department of Correctional Services.
    </p>

    <h3>Your login details</h3>

    <p>
        <strong>Username:</strong> {employee.Email}
    </p>

    <p>
        <strong>Temporary Password:</strong>
        {temporaryPassword}
    </p>

    <p>
        Please use the temporary password to log in to PMDS.
    </p>

    <p>
        Login here:
        <a href='{loginUrl}'>{loginUrl}</a>
    </p>

    <p>
        For security reasons, you will be required to change
        your temporary password after logging in.
    </p>

    <br>

    <p>
        Regards,<br>
        <strong>PMDS</strong><br>
        Department of Correctional Services
    </p>

</body>
</html>";

                try
                {
                    await _emailService.SendEmailAsync(
                        employee.Email,
                        "PMDS Account Created",
                        emailBody);

                    Console.WriteLine("WELCOME EMAIL SENT");
                }
                catch (Exception emailException)
                {
                    Console.WriteLine("==================================");
                    Console.WriteLine("EMAIL FAILED");
                    Console.WriteLine("==================================");

                    Console.WriteLine(emailException.Message);

                    if (emailException.InnerException != null)
                    {
                        Console.WriteLine(emailException.InnerException.Message);
                    }

                    Console.WriteLine(emailException.ToString());

                    throw;
                }
                // =====================================================
                // SUCCESS
                // =====================================================

                TempData["Success"] =
                    "Employee created successfully. " +
                    "The employee's PMDS account has been created.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("======================================");
                Console.WriteLine("DATABASE UPDATE ERROR");
                Console.WriteLine("======================================");

                Console.WriteLine(ex.ToString());

                ModelState.AddModelError(
                    "",
                    "DATABASE ERROR: " +
                    (ex.InnerException?.Message ?? ex.Message));

                LoadSupervisors();

                return View(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine("======================================");
                Console.WriteLine("GENERAL ERROR");
                Console.WriteLine("======================================");

                Console.WriteLine(ex.ToString());

                ModelState.AddModelError(
                    "",
                    "ERROR: " + ex.Message);

                LoadSupervisors();

                return View(employee);
            }
        }


        // =========================================================
        // TEMPORARY PASSWORD GENERATOR
        // =========================================================

        private string GenerateTemporaryPassword()
        {
            return "PMDS@" +
                   Random.Shared.Next(100000, 999999) +
                   "!";
        }



        //========================================================
        // Auto Populate PMDS Form
        //========================================================
        [HttpGet]
        public IActionResult GetEmployeeByPersal(string persalNumber)
        {
            if (string.IsNullOrWhiteSpace(persalNumber))
            {
                return Json(null);
            }

            var employee = _context.Employees
                .FirstOrDefault(e => e.PersalNumber == persalNumber);

            if (employee == null)
            {
                return Json(null);
            }

            var supervisor = _context.Employees
                .FirstOrDefault(e => e.Id == employee.SupervisorId);

            return Json(new
            {
                persalNumber = employee.PersalNumber,

                firstName = employee.FirstName,

                lastName = employee.LastName,

                department = employee.Department,

                position = employee.Position,

                previousStation = employee.PreviousStation,

                transferDate = employee.TransferDate,

                previousCycleperformance = employee.PreviousCyclePerformance,

                currentRank = employee.PostLevel,

                relatedOSDDescription = employee.OSDDescription,

                appointmentDate =
                    employee.AppointmentDateInCurrentRank?
                    .ToString("yyyy-MM-dd"),

                appointmentInDcsDate =
                    employee.AppointmentInDcsDate?
                    .ToString("yyyy-MM-dd"),

                supervisor = supervisor == null
                    ? ""
                    : supervisor.FirstName + " " + supervisor.LastName,

                supervisorRank = supervisor?.PostLevel
            });
        }


        //========================================================
        // ASSIGN SUPERVISOR - GET
        //========================================================

        [HttpGet]
        public async Task<IActionResult> AssignSupervisor(int id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            var supervisors = await _context.Employees
                .Where(e => e.Id != id)
                .Select(e => new
                {
                    Id = e.Id,

                    FullName =
                        e.PersalNumber
                        + " - "
                        + e.FirstName
                        + " "
                        + e.LastName
                })
                .ToListAsync();

            ViewBag.Supervisors = new SelectList(
                supervisors,
                "Id",
                "FullName",
                employee.SupervisorId);

            return View(employee);
        }


        //========================================================
        // ASSIGN SUPERVISOR - POST
        //========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignSupervisor(int id, int supervisorId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            if (id == supervisorId)
            {
                TempData["Error"] =
                    "An employee cannot be their own supervisor.";

                return RedirectToAction(nameof(Index));
            }

            var supervisor = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == supervisorId);

            if (supervisor == null)
            {
                TempData["Error"] =
                    "Selected supervisor does not exist.";

                return RedirectToAction(nameof(Index));
            }

            employee.SupervisorId = supervisorId;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Supervisor assigned successfully.";

            return RedirectToAction(nameof(Index));
        }
        // =========================================================
        // SUPERVISOR - VIEW ASSIGNED EMPLOYEES
        // =========================================================

        [HttpGet]

        public async Task<IActionResult> MyEmployees()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Content("ERROR: No logged-in User ID found.");
            }

            var supervisor = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (supervisor == null)
            {
                return Content(
                    $"ERROR: The logged-in supervisor is not linked to an Employee record. User ID: {userId}");
            }

            var employees = await _context.Employees
                .Where(e => e.SupervisorId == supervisor.Id)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();

            return View(employees);
        }

        //========================================================
        // DEBUG LOGGED-IN USER
        //========================================================

        [HttpGet]
        public async Task<IActionResult> DebugUser()
        {
            var userId = _userManager.GetUserId(User);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e =>
                    e.UserId == userId);

            if (employee == null)
            {
                return Content(
                    "NO EMPLOYEE LINKED TO THIS USER");
            }

            return Content(
                $"Linked Employee: "
                + $"{employee.FirstName} "
                + $"{employee.LastName}");
        }
        // ============================================================
        // EDIT EMPLOYEE - GET
        // ============================================================
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            // Employees editing their own details must always load the
            // Employee record linked to their logged-in Identity account.
            Employee? employee;

            if (User.IsInRole("Employee"))
            {
                employee = await _context.Employees
                    .Include(e => e.Supervisor)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.UserId == userId);
            }
            else
            {
                if (!id.HasValue || id.Value <= 0)
                {
                    return BadRequest("A valid employee ID is required.");
                }

                employee = await _context.Employees
                    .Include(e => e.Supervisor)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id.Value);
            }

            if (employee == null)
            {
                return NotFound(
                    "No employee record is linked to the logged-in account.");
            }

            // ========================================================
            // POPULATE SUPERVISOR DETAILS
            // ========================================================

            if (employee.Supervisor != null)
            {
                if (string.IsNullOrWhiteSpace(
                        employee.SupervisorSurnameInitials))
                {
                    var supervisorInitials = string.Empty;

                    if (!string.IsNullOrWhiteSpace(
                            employee.Supervisor.Initials))
                    {
                        supervisorInitials =
                            employee.Supervisor.Initials.Trim();
                    }
                    else if (!string.IsNullOrWhiteSpace(
                                 employee.Supervisor.FirstName))
                    {
                        supervisorInitials =
                            string.Join(
                                ".",
                                employee.Supervisor.FirstName
                                    .Split(
                                        ' ',
                                        StringSplitOptions.RemoveEmptyEntries)
                                    .Select(name => name[0])) + ".";
                    }

                    employee.SupervisorSurnameInitials =
                        $"{employee.Supervisor.LastName} {supervisorInitials}"
                            .Trim();
                }

                if (string.IsNullOrWhiteSpace(
                        employee.SupervisorRankPostLevel))
                {
                    employee.SupervisorRankPostLevel =
                        !string.IsNullOrWhiteSpace(
                            employee.Supervisor.PostLevel)
                            ? employee.Supervisor.PostLevel
                            : employee.Supervisor.Position;
                }
            }

            // ========================================================
            // GET THE MOST RECENT PART A RECORD
            // ========================================================

            var existingPartA = await _context.PMDSForms
                .AsNoTracking()
                .Where(p => p.EmployeeId == employee.Id)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            if (existingPartA != null)
            {
                // Department / Directorate
                if (string.IsNullOrWhiteSpace(employee.Department))
                {
                    employee.Department = existingPartA.Directorate;
                }

                // Post Designation
                if (string.IsNullOrWhiteSpace(employee.PostDesignation))
                {
                    employee.PostDesignation =
                        existingPartA.PostDesignation;
                }

                // Position
                if (string.IsNullOrWhiteSpace(employee.Position))
                {
                    employee.Position =
                        !string.IsNullOrWhiteSpace(
                            existingPartA.PostDesignation)
                            ? existingPartA.PostDesignation
                            : existingPartA.CurrentRank;
                }

                // Post Level
                if (string.IsNullOrWhiteSpace(employee.PostLevel))
                {
                    employee.PostLevel =
                        !string.IsNullOrWhiteSpace(existingPartA.PostLevel)
                            ? existingPartA.PostLevel
                            : existingPartA.CurrentRank;
                }

                // Appointment date in DCS
                if (!employee.AppointmentInDcsDate.HasValue)
                {
                    employee.AppointmentInDcsDate =
                        existingPartA.AppointmentDate;
                }

                // Appointment date in current rank
                if (!employee.AppointmentDateInCurrentRank.HasValue)
                {
                    employee.AppointmentDateInCurrentRank =
                        existingPartA.AppointmentDateInCurrentRank;
                }

                // OSD
                if (string.IsNullOrWhiteSpace(employee.OSDDescription))
                {
                    employee.OSDDescription =
                        !string.IsNullOrWhiteSpace(
                            existingPartA.OSDDescription)
                            ? existingPartA.OSDDescription
                            : existingPartA.RelatedOSDDescription;
                }

                // Previous station
                if (string.IsNullOrWhiteSpace(employee.PreviousStation))
                {
                    employee.PreviousStation =
                        existingPartA.PreviousStation;
                }

                // Transfer date
                if (!employee.TransferDate.HasValue)
                {
                    employee.TransferDate =
                        existingPartA.TransferDate;
                }

                // Previous performance
                if (!employee.PreviousCyclePerformance.HasValue)
                {
                    employee.PreviousCyclePerformance =
                        existingPartA.PreviousCyclePerformance;
                }

                // Supervisor fallback from Part A
                if (string.IsNullOrWhiteSpace(
                        employee.SupervisorSurnameInitials))
                {
                    employee.SupervisorSurnameInitials =
                        existingPartA.SupervisorSurnameInitials;
                }

                if (string.IsNullOrWhiteSpace(
                        employee.SupervisorRankPostLevel))
                {
                    employee.SupervisorRankPostLevel =
                        existingPartA.SupervisorRankPostLevel;
                }
            }

            LoadSupervisors();

            // ========================================================
            // DEBUG VALUES SENT TO THE VIEW
            // ========================================================

            Console.WriteLine("======================================");
            Console.WriteLine("EDIT MODEL SENT TO VIEW");
            Console.WriteLine($"Employee ID: {employee.Id}");
            Console.WriteLine($"User ID: {employee.UserId}");
            Console.WriteLine($"Persal: {employee.PersalNumber}");
            Console.WriteLine($"Department: {employee.Department}");
            Console.WriteLine($"Position: {employee.Position}");
            Console.WriteLine(
                $"Post Designation: {employee.PostDesignation}");
            Console.WriteLine($"Post Level: {employee.PostLevel}");
            Console.WriteLine($"Supervisor ID: {employee.SupervisorId}");
            Console.WriteLine(
                $"Supervisor: {employee.SupervisorSurnameInitials}");
            Console.WriteLine(
                $"Supervisor Rank: {employee.SupervisorRankPostLevel}");
            Console.WriteLine($"Part A found: {existingPartA != null}");
            Console.WriteLine("======================================");

            return View(employee);
        }
        // ============================================================
        // EDIT EMPLOYEE - POST
        // ============================================================
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            // ========================================================
            // LOAD ORIGINAL DATABASE RECORD
            // ========================================================

            Employee? existingEmployee;

            if (User.IsInRole("Employee"))
            {
                existingEmployee = await _context.Employees
                    .Include(e => e.Supervisor)
                    .FirstOrDefaultAsync(e => e.UserId == userId);
            }
            else
            {
                existingEmployee = await _context.Employees
                    .Include(e => e.Supervisor)
                    .FirstOrDefaultAsync(e => e.Id == id);
            }

            if (existingEmployee == null)
            {
                return NotFound("Employee record was not found.");
            }

            // ========================================================
            // SECURITY
            // ========================================================

            if (User.IsInRole("Employee") &&
                existingEmployee.UserId != userId)
            {
                return Forbid();
            }


            // ========================================================
            // RESTORE MISSING WORK DETAILS FROM EXISTING PART A
            // ========================================================

            var existingPartA = await _context.PMDSForms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.EmployeeId == existingEmployee.Id);

            if (existingPartA != null)
            {
                // Department
                if (string.IsNullOrWhiteSpace(existingEmployee.Department))
                {
                    existingEmployee.Department =
                        existingPartA.Directorate;
                }

                // Post Designation
                if (string.IsNullOrWhiteSpace(existingEmployee.PostDesignation))
                {
                    existingEmployee.PostDesignation =
                        existingPartA.PostDesignation;
                }

                // Position
                if (string.IsNullOrWhiteSpace(existingEmployee.Position))
                {
                    existingEmployee.Position =
                        existingPartA.PostDesignation;
                }

                // Post Level / Current Rank
                if (string.IsNullOrWhiteSpace(existingEmployee.PostLevel))
                {
                    existingEmployee.PostLevel =
                        !string.IsNullOrWhiteSpace(existingPartA.PostLevel)
                            ? existingPartA.PostLevel
                            : existingPartA.CurrentRank;
                }

                // Appointment in DCS
                if (!existingEmployee.AppointmentInDcsDate.HasValue)
                {
                    existingEmployee.AppointmentInDcsDate =
                        existingPartA.AppointmentDate;
                }

                // Appointment in Current Rank
                if (!existingEmployee.AppointmentDateInCurrentRank.HasValue)
                {
                    existingEmployee.AppointmentDateInCurrentRank =
                        existingPartA.AppointmentDateInCurrentRank;
                }

                // OSD Description
                if (string.IsNullOrWhiteSpace(existingEmployee.OSDDescription))
                {
                    existingEmployee.OSDDescription =
                        !string.IsNullOrWhiteSpace(existingPartA.OSDDescription)
                            ? existingPartA.OSDDescription
                            : existingPartA.RelatedOSDDescription;
                }

                // Previous Station
                if (string.IsNullOrWhiteSpace(existingEmployee.PreviousStation))
                {
                    existingEmployee.PreviousStation =
                        existingPartA.PreviousStation;
                }

                // Transfer Date
                if (!existingEmployee.TransferDate.HasValue)
                {
                    existingEmployee.TransferDate =
                        existingPartA.TransferDate;
                }

                // Previous Cycle Performance
                if (!existingEmployee.PreviousCyclePerformance.HasValue)
                {
                    existingEmployee.PreviousCyclePerformance =
                        existingPartA.PreviousCyclePerformance;
                }
            }

            // ========================================================
            // VALIDATION
            // ========================================================
            if (!ModelState.IsValid)
            {
                // Return the original database values instead of the blank
                // values received from fields that were not posted.
                existingEmployee.Supervisor = await _context.Employees
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == existingEmployee.SupervisorId);

                LoadSupervisors();

                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine(
                            $"EDIT ERROR - FIELD: {modelState.Key} | ERROR: {error.ErrorMessage}");
                    }
                }

                return View(existingEmployee);
            }

            // ========================================================
            // DO NOT UPDATE THESE:
            //
            // Id
            // PersalNumber
            // UserId
            // SupervisorId
            // MustChangePassword
            // ========================================================

            // ========================================================
            // PERSONAL INFORMATION
            // ========================================================

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Initials = employee.Initials;
            existingEmployee.Email = employee.Email;
            existingEmployee.IdentificationNumber = employee.IdentificationNumber;


            // ========================================================
            // WORK INFORMATION
            // Keep existing database values when the form submits blanks.
            // ========================================================

            if (!string.IsNullOrWhiteSpace(employee.Department))
            {
                existingEmployee.Department = employee.Department.Trim();
            }

            if (!string.IsNullOrWhiteSpace(employee.Position))
            {
                existingEmployee.Position = employee.Position.Trim();
            }

            if (!string.IsNullOrWhiteSpace(employee.SalaryLevel))
            {
                existingEmployee.SalaryLevel = employee.SalaryLevel.Trim();
            }

            if (!string.IsNullOrWhiteSpace(employee.PostDesignation))
            {
                existingEmployee.PostDesignation = employee.PostDesignation.Trim();
            }

            if (!string.IsNullOrWhiteSpace(employee.PostLevel))
            {
                existingEmployee.PostLevel = employee.PostLevel.Trim();
            }

            if (employee.AppointmentDateInCurrentRank.HasValue)
            {
                existingEmployee.AppointmentDateInCurrentRank =
                    employee.AppointmentDateInCurrentRank.Value.Date;
            }

            if (employee.AppointmentInDcsDate.HasValue)
            {
                existingEmployee.AppointmentInDcsDate =
                    employee.AppointmentInDcsDate.Value.Date;
            }
            // ========================================================
            // SUPERVISOR INFORMATION
            // Do not overwrite supervisor information with blank values.
            // SupervisorId is intentionally not changed here.
            // ========================================================

            if (!string.IsNullOrWhiteSpace(employee.SupervisorSurnameInitials))
            {
                existingEmployee.SupervisorSurnameInitials =
                    employee.SupervisorSurnameInitials.Trim();
            }

            if (!string.IsNullOrWhiteSpace(employee.SupervisorRankPostLevel))
            {
                existingEmployee.SupervisorRankPostLevel =
                    employee.SupervisorRankPostLevel.Trim();
            }

            // ========================================================
            // STATION INFORMATION
            // Do not overwrite existing values with blank values.
            // ========================================================

            if (!string.IsNullOrWhiteSpace(employee.OSDDescription))
            {
                existingEmployee.OSDDescription =
                    employee.OSDDescription.Trim();
            }

            if (!string.IsNullOrWhiteSpace(employee.BranchOrRegion))
            {
                existingEmployee.BranchOrRegion =
                    employee.BranchOrRegion.Trim();
            }

            if (!string.IsNullOrWhiteSpace(employee.PreviousStation))
            {
                existingEmployee.PreviousStation =
                    employee.PreviousStation.Trim();
            }

            if (!string.IsNullOrWhiteSpace(employee.OtherPreviousStation))
            {
                existingEmployee.OtherPreviousStation =
                    employee.OtherPreviousStation.Trim();
            }

            if (employee.TransferDate.HasValue)
            {
                existingEmployee.TransferDate =
                    employee.TransferDate.Value.Date;
            }

            // ========================================================
            // DEMOGRAPHICS
            // ========================================================

            existingEmployee.Gender =
                employee.Gender;

            existingEmployee.AgeGroup =
                employee.AgeGroup;

            existingEmployee.Race =
                employee.Race;

            existingEmployee.HasDisability =
                employee.HasDisability;

            existingEmployee.NatureOfDisability =
                employee.NatureOfDisability;


            // ========================================================
            // PREVIOUS CYCLE PERFORMANCE
            // ========================================================

            existingEmployee.PreviousCyclePerformance =
                employee.PreviousCyclePerformance;


            // ========================================================
            // DECLARATION
            // ========================================================

            existingEmployee.EmployeeSignature =
                employee.EmployeeSignature;

            existingEmployee.SignatureDate =
                employee.SignatureDate;

            existingEmployee.DeclarationDate =
                employee.DeclarationDate;

            // ========================================================
            // DEBUG - CHECK VALUES BEFORE SAVING
            // ========================================================

            Console.WriteLine("==========================================");
            Console.WriteLine("EDIT EMPLOYEE VALUES BEFORE SAVE");
            Console.WriteLine($"Persal: {existingEmployee.PersalNumber}");
            Console.WriteLine($"Department: {existingEmployee.Department}");
            Console.WriteLine($"Position: {existingEmployee.Position}");
            Console.WriteLine($"PostDesignation: {existingEmployee.PostDesignation}");
            Console.WriteLine($"SupervisorId: {existingEmployee.SupervisorId}");
            Console.WriteLine($"SupervisorSurnameInitials: {existingEmployee.SupervisorSurnameInitials}");
            Console.WriteLine("==========================================");

            // ========================================================
            // SAVE
            // ========================================================

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Your employee details have been updated successfully.";

            return RedirectToAction("Cycles", "PMDS");
        }
        //========================================================
        // LOAD SUPERVISORS AND CENTERS
        //========================================================

        private void LoadSupervisors()
        {
            var supervisors = _context.Employees
                .Select(e => new
                {
                    Id = e.Id,

                    FullName =
                        e.PersalNumber
                        + " - "
                        + e.FirstName
                        + " "
                        + e.LastName
                })
                .OrderBy(e => e.FullName)
                .ToList();

            ViewBag.Supervisors = new SelectList(
                supervisors,
                "Id",
                "FullName");


            ViewBag.Centers = new List<string>
        {
            // ==================== EASTERN CAPE REGION ====================

            "FORT BEAUFORT CORRCENT",
            "GRAHAMSTOWN CORRCENT",
            "KING WILLIAMS TOWN REMAND DETENTION FACILITY",
            "MIDDLEDRIFT CORRCENT",
            "STUTTERHEIM CORRCENT",
            "EAST LONDON MED. A (MAXIMUM) CORRCENT",
            "EAST LONDON MED. C (FEMALE) CORRCENT",
            "EAST LONDON REMAND DETENTION FACILITY",
            "MDANTSANE CORRCENT",
            "GRAAFF-REINET REMAND DETENTION FACILITY",
            "JANSENVILLE CORRCENT",
            "KIRKWOOD CORRCENT",
            "SOMERSET EAST CORRCENT",
            "BIZANA REMAND DETENTION FACILITY",
            "ELLIOTDALE CORRCENT",
            "FLAGSTAFF CORRCENT",
            "LUSIKISIKI CORRCENT",
            "MOUNT AYLIFF CORRCENT",
            "MOUNT FLETCHER CORRCENT",
            "MOUNT FRERE CORRCENT",
            "MQANDULI CORRCENT",
            "MTHATHA MAXIMUM CORRCENT",
            "MTHATHA REMAND DETENTION FACILITY",
            "NGQELENI CORRCENT",
            "NTABANKULU CORRCENT",
            "BARKLY EAST CORRCENT",
            "BURGERSDORP CORRCENT",
            "BUTTERWORTH REMAND DETENTION FACILITY",
            "COFIMVABA CORRCENT",
            "CRADOCK CORRCENT",
            "DORDRECHT CORRCENT",
            "ENGCOBO CORRCENT",
            "IDUTYWA CORRCENT",
            "LADY FRERE CORRCENT",
            "MIDDELBURG CORRCENT (E-C)",
            "NQAMAKWE CORRCENT",
            "QUEENSTOWN REMAND DETENTION FACILITY",
            "SADA CORRCENT",
            "STERKSPRUIT CORRCENT",
            "WILLOWVALE CORRCENT",
            "PATENSIE CORRCENT",
            "PORT ELIZABETH CORRCENT",
            "ST. ALBANS MAX. CORRCENT",
            "ST. ALBANS MED. B CORRCENT",
            "ST. ALBANS REMAND DETENTION FACILITY",

            // ==================== WESTERN CAPE REGION ====================

            "ALLANDALE CORRCENT",
            "HAWEQUA CORRCENT",
            "OBIQUA CORRCENT",
            "STAART VAN PAARDEBERG CORRCENT",
            "BRANDVLEI MAX. CORRCENT",
            "BRANDVLEI MED. A CORRCENT NG",
            "BRANDVLEI MED. CORRCENT",
            "BRANDVLEI YOUTH CORRCENT",
            "DWARSRIVIER CORRCENT",
            "ROBERTSON CORRCENT",
            "WARMBOKVELD CORRCENT NG",
            "WORCESTER FEMALE CORRCENT",
            "WORCESTER MALE CORRCENT",
            "DRAKENSTEIN MAX. CORRCENT",
            "DRAKENSTEIN MED. A CORRCENT",
            "DRAKENSTEIN MED. B JUV. CORRCENT",
            "STELLENBOSCH CORRCENT",
            "GOODWOOD REMAND DETENTION FACILITY",
            "BUFFELJAGSRIVIER CORRCENT",
            "CALEDON REMAND DETENTION FACILITY",
            "HELDERSTROOM MAX. CORRCENT",
            "HELDERSTROOM MED. CORRCENT",
            "SWELLENDAM CORRCENT",
            "POLLSMOOR FEMALE CORRCENT",
            "POLLSMOOR MED. B CORRCENT",
            "POLLSMOOR MED. C CORRCENT",
            "POLLSMOOR REMAND DETENTION FACILITY A",
            "POLLSMOOR REMAND DETENTION FACILITY B",
            "BEAUFORT WEST CORRCENT",
            "GEORGE CORRCENT",
            "KNYSNA CORRCENT",
            "LADISMITH CORRCENT",
            "MOSSELBAAI CORRCENT",
            "OUDTSHOORN MED. A CORRCENT",
            "OUDTSHOORN MED. B CORRCENT",
            "PRINCE ALBERT CORRCENT",
            "UNIONDALE CORRCENT",
            "CALVINIA CORRCENT",
            "VANRHYNSDORP CORRCENT NG",
            "VOORBERG MED. A CORRCENT",
            "VOORBERG MED. B CORRCENT",
            "MALMESBURY MED. A CORRCENT NG",
            "MALMESBURY REMAND DETENTION FACILITY",
            "RIEBEECK WEST CORRCENT",

            // ==================== FREE STATE / NORTHERN CAPE ====================

            "BETHLEHEM CORRCENT",
            "FICKSBURG CORRCENT",
            "HARRISMITH CORRCENT",
            "HENNENMAN CORRCENT",
            "HOOPSTAD CORRCENT",
            "KROONSTAD MED. A CORRCENT",
            "KROONSTAD MED. B CORRCENT",
            "KROONSTAD MED. C (FEMALE) CORRCENT",
            "KROONSTAD YOUTH CORRCENT",
            "LINDLEY CORRCENT",
            "ODENDAALSRUS REMAND DETENTION FACILITY",
            "SENEKAL CORRCENT",
            "VENTERSBURG CORRCENT",
            "VIRGINIA CORRCENT",
            "COLESBERG CORRCENT",
            "DE AAR CORRCENT",
            "HOPETOWN CORRCENT",
            "RICHMOND CORRCENT",
            "VICTORIA WEST CORRCENT",
            "BETHULIE CORRCENT",
            "EDENBURG CORRCENT",
            "FAURESMITH CORRCENT",
            "GOEDEMOED MED. A CORRCENT",
            "GOEDEMOED MED. B CORRCENT",
            "ZASTRON CORRCENT",
            "FRANKFORT CORRCENT",
            "GROENPUNT JUVENILE CORRCENT",
            "GROENPUNT MAX. CORRCENT",
            "GROENPUNT MED. CORRCENT",
            "HEILBRON CORRCENT",
            "PARYS CORRCENT",
            "SASOLBURG CORRCENT",
            "VEREENIGING CORRCENT",
            "BOSHOF CORRCENT",
            "BRANDFORT CORRCENT",
            "GROOTVLEI MAX. CORRCENT",
            "GROOTVLEI MED. CORRCENT",
            "LADYBRAND CORRCENT",
            "MANGAUNG APOPS CORRCENT",
            "WEPENER CORRCENT",
            "WINBURG CORRCENT",
            "BARKLY WEST CORRCENT",
            "DOUGLAS CORRCENT",
            "KIMBERLEY CORRCENT",
            "TSWELOPELE CORRCENT NG",
            "KURUMAN CORRCENT",
            "SPRINGBOK CORRCENT",
            "UPINGTON CORRCENT",

            // ==================== GAUTENG REGION ====================

            "BAVIAANSPOORT MAX. CORRCENT",
            "BAVIAANSPOORT MED. CORRCENT",
            "EMTHONJENI JUVINILE CORRCENT",
            "BOKSBURG MAXIMUM JUVENILE CORRCENT",
            "BOKSBURG MEDIUM CORRCENT",
            "HEIDELBERG MEDIUM CORRCENT",
            "JOHANNESBURG FEMALE CORRCENT",
            "JOHANNESBURG MED. B CORRCENT",
            "JOHANNESBURG MED. C CORRCENT",
            "JOHANNESBURG REMAND DETENTION FACILITY",
            "ATTERIDGEVILLE FEMALE CORRCENT",
            "KGOSI MAMPURU II MALE MAXIMUM CORRCENT",
            "KGOSI MAMPURU II REMAND DETENTION FACILITY",
            "ODI CORRCENT",
            "HEAD OFFICE",
            "PRETORIA C-MAX. CORRCENT",
            "PRETORIA CENTRAL CORRCENT",
            "KRUGERSDORP CORRCENT",
            "LEEUWKOP MAX. CORRCENT",
            "LEEUWKOP MED. A CORRCENT",
            "LEEUWKOP MED. B CORRCENT",
            "LEEUWKOP MED. C CORRCENT",
            "DEVON CORRCENT",
            "MODDERBEE CORRCENT",
            "NIGEL MALE CORRCENT",
            "ZONDERWATER MED. A CORRCENT",
            "ZONDERWATER MED. B CORRCENT",

            // ==================== KWA-ZULU NATAL REGION ====================

            "DURBAN FEMALE CORRCENT",
            "DURBAN MED. B CORRCENT",
            "DURBAN MED. C CORRCENT",
            "DURBAN REMAND DETENTION FACILITY",
            "DURBAN YOUTH CORRCENT",
            "UMZINTO CORRCENT",
            "EMPANGENI MEDIUM CORRCENT",
            "ESHOWE CORRCENT",
            "MAPUMULO CORRCENT",
            "MTUNZINI CORRCENT",
            "NGWAVUMA CORRCENT",
            "QALAKABUSHA CORRCENT (EMPANGEN)",
            "STANGER CORRCENT",
            "BERGVILLE CORRCENT",
            "DUNDEE CORRCENT",
            "ESTCOURT CORRCENT NG",
            "GLENCOE CORRCENT",
            "GREYTOWN CORRCENT",
            "KRANSKOP CORRCENT",
            "LADYSMITH REMAND DETENTION FACILITY",
            "POMEROY CORRCENT",
            "EBONGWENI MAX. CORRCENT",
            "KOKSTAD MED. CORRCENT",
            "MATATIELE CORRCENT",
            "PORT SHEPSTONE CORRCENT",
            "UMZIMKULU CORRCENT",
            "MELMOTH CORRCENT",
            "NCOME MED. A CORRCENT",
            "NCOME MED. B CORRCENT",
            "NKANDLA CORRCENT",
            "NONGOMA CORRCENT",
            "VRYHEID CORRCENT",
            "IXOPO CORRCENT",
            "NEW HANOVER CORRCENT",
            "PIETERMARITZBURG MED. A CORRCENT",
            "PIETERMARITZBURG MED. B CORRCENT",
            "SEVONTEIN CORRCENT",
            "EKUSENI YOUTH DEV CORRCENT",
            "NEWCASTLE CORRCENT",
            "UTRECHT CORRCENT",
            "WATERVAL MED. A CORRCENT",
            "WATERVAL MED. B CORRCENT",

            // ==================== LIMPOPO / MPUMALANGA / NORTH-WEST ====================

            "BARBERTON FARM MAX. CORRCENT",
            "BARBERTON FARM MED. A CORRCENT",
            "BARBERTON FARM MED. B CORRCENT",
            "BARBERTON YOUTH CORRCENT",
            "LYDENBURG CORRCENT",
            "NELSPRUIT CORRCENT",
            "BETHAL CORRCENT",
            "ERMELO CORRCENT",
            "GELUK CORRCENT",
            "PIET RETIEF CORRCENT",
            "STANDERTON CORRCENT NG",
            "VOLKSRUST CORRCENT",
            "CHRISTIANA CORRCENT",
            "KLERKSDORP CORRCENT",
            "POTCHEFSTROOM REMAND DETENTION FACILITY",
            "WOLMARANSSTAD CORRCENT",
            "MODIMOLLE CORRCENT",
            "POLOKWANE CORRCENT",
            "TZANEEN CORRCENT NG",
            "LICHTENBURG CORRCENT",
            "MAFIKENG CORRCENT",
            "ROOIGROND MAXIMUM CORRCENT",
            "ROOIGROND MED. B CORRCENT",
            "ZEERUST CORRCENT",
            "BRITS CORRCENT",
            "LOSPERFONTEIN CORRCENT",
            "MOGWASE CORRCENT",
            "RUSTENBURG MED. B JUVENILE CORRCENT",
            "RUSTENBURG YOUTH CORRCENT",
            "KUTAMA-SINTHUMULE CORRCENT (AP)",
            "MAKHADO CORRCENT",
            "THOHOYANDOU FEMALE CORRCENT",
            "THOHOYANDOU MED. A CORRCENT",
            "THOHOYANDOU MED. B CORRCENT",
            "BELFAST CORRCENT",
            "CAROLINA CORRCENT",
            "MIDDELBURG CORRCENT (MP)",
            "WITBANK CORRCENT"
        };
        }
    }


}
