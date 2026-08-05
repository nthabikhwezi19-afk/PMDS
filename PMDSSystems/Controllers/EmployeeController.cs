using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using PMDSSystems.Services;

namespace PMDSSystems.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();

            return View(employees);
        }


        //========================================================
        // CREATE EMPLOYEE - GET
        //========================================================

        [HttpGet]
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

            // =====================================================
            // MODEL VALIDATION
            // =====================================================

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

                currentRank = employee.PostLevel,

                relatedOSDDescription = employee.OSDDescription,

                appointmentDate =
                    employee.AppointmentDateInCurrentRank?
                    .ToString("yyyy-MM-dd"),

                appointmentInDcsDate =
                    employee.AppointmentInDcsDate?
                    .ToString("yyyy-MM-dd"),

                currentRankDate =
                    employee.CurrentRankDate?
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
        public async Task<IActionResult> AssignSupervisor(
            int id,
            int supervisorId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            // Prevent employee from being their own supervisor
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
