using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using PMDSSystems.Services;

namespace PMDSSystems.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
        }


        // =========================================================
        // GET: LOGIN
        // =========================================================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        // =========================================================
        // POST: LOGIN
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string email,
            string password)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("LOGIN POST METHOD WAS CALLED");
            Console.WriteLine("========================================");

            Console.WriteLine($"Email: {email}");
            Console.WriteLine(
                $"Password supplied: {!string.IsNullOrWhiteSpace(password)}");


            // =====================================================
            // CHECK REQUIRED FIELDS
            // =====================================================

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine(
                    "LOGIN FAILED: Email or Password is empty.");

                ModelState.AddModelError(
                    "",
                    "Email and Password are required");

                return View();
            }


            // =====================================================
            // FIND IDENTITY USER
            // =====================================================

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                Console.WriteLine(
                    "LOGIN FAILED: Identity user NOT FOUND.");

                Console.WriteLine(
                    $"Email searched: {email}");

                ModelState.AddModelError(
                    "",
                    "Invalid email or password");

                return View();
            }


            Console.WriteLine("----------------------------------------");
            Console.WriteLine("IDENTITY USER FOUND");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"User ID: {user.Id}");
            Console.WriteLine($"UserName: {user.UserName}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"EmailConfirmed: {user.EmailConfirmed}");
            Console.WriteLine($"LockoutEnd: {user.LockoutEnd}");
            Console.WriteLine("----------------------------------------");


            // =====================================================
            // ATTEMPT LOGIN
            // =====================================================

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                password,
                isPersistent: false,
                lockoutOnFailure: false);


            // =====================================================
            // LOGIN DIAGNOSTICS
            // =====================================================

            Console.WriteLine("----------------------------------------");
            Console.WriteLine("PASSWORD SIGN-IN RESULT");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Succeeded: {result.Succeeded}");
            Console.WriteLine($"IsLockedOut: {result.IsLockedOut}");
            Console.WriteLine($"IsNotAllowed: {result.IsNotAllowed}");
            Console.WriteLine(
                $"RequiresTwoFactor: {result.RequiresTwoFactor}");
            Console.WriteLine("----------------------------------------");


            // =====================================================
            // LOGIN FAILED
            // =====================================================

            if (!result.Succeeded)
            {
                Console.WriteLine("LOGIN FAILED.");

                if (result.IsLockedOut)
                {
                    Console.WriteLine(
                        "REASON: Account is locked out.");
                }

                if (result.IsNotAllowed)
                {
                    Console.WriteLine(
                        "REASON: Login is not allowed.");

                    Console.WriteLine(
                        $"EmailConfirmed: {user.EmailConfirmed}");
                }

                if (!result.IsLockedOut &&
                    !result.IsNotAllowed &&
                    !result.RequiresTwoFactor)
                {
                    Console.WriteLine(
                        "REASON: Password is probably incorrect.");
                }

                ModelState.AddModelError(
                    "",
                    "Invalid email or password");

                return View();
            }


            Console.WriteLine(
                "LOGIN SUCCESSFUL - PASSWORD CORRECT");


            // =====================================================
            // FIND LINKED EMPLOYEE
            // =====================================================

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == user.Id);


            if (employee == null)
            {
                Console.WriteLine(
                    "WARNING: No Employee record linked to this user.");
            }
            else
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("LINKED EMPLOYEE");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine(
                    $"Employee ID: {employee.Id}");
                Console.WriteLine(
                    $"Persal Number: {employee.PersalNumber}");
                Console.WriteLine(
                    $"First Name: {employee.FirstName}");
                Console.WriteLine(
                    $"Last Name: {employee.LastName}");
                Console.WriteLine(
                    $"Employee Email: {employee.Email}");
                Console.WriteLine(
                    $"Employee UserId: {employee.UserId}");
                Console.WriteLine(
                    $"MustChangePassword: {employee.MustChangePassword}");
                Console.WriteLine("----------------------------------------");
            }


            // =====================================================
            // GET USER ROLES
            // =====================================================

            var roles = await _userManager.GetRolesAsync(user);

            Console.WriteLine("USER ROLES:");

            if (roles.Any())
            {
                foreach (var role in roles)
                {
                    Console.WriteLine($"  - {role}");
                }
            }
            else
            {
                Console.WriteLine("  NO ROLES FOUND");
            }


            // =====================================================
            // ADMIN LOGIN
            // =====================================================

            if (roles.Contains("Admin"))
            {
                Console.WriteLine("USER IS ADMIN");
                Console.WriteLine(
                    "REDIRECTING TO ADMIN DASHBOARD");

                return RedirectToAction(
                    "Index",
                    "Dashboard",
                    new { area = "Admin" });
            }


            // =====================================================
            // EMPLOYEE MUST HAVE EMPLOYEE RECORD
            // =====================================================

            if (employee == null)
            {
                Console.WriteLine(
                    "LOGIN STOPPED: Employee record not found.");

                await _signInManager.SignOutAsync();

                ModelState.AddModelError(
                    "",
                    "Your employee profile was not found.");

                return View();
            }


            // =====================================================
            // FIRST LOGIN - FORCE PASSWORD CHANGE
            // =====================================================

            if (employee.MustChangePassword)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("FIRST LOGIN DETECTED");
                Console.WriteLine("MustChangePassword = TRUE");
                Console.WriteLine(
                    "REDIRECTING TO CHANGE PASSWORD");
                Console.WriteLine("----------------------------------------");

                return RedirectToAction(
                    "ChangePassword",
                    new { userId = user.Id });
            }


            // =====================================================
            // NORMAL EMPLOYEE / SUPERVISOR LOGIN
            // =====================================================

            Console.WriteLine(
                "PASSWORD CHANGE NOT REQUIRED.");

            Console.WriteLine(
                "REDIRECTING TO PMDS CYCLES.");

            return RedirectToAction(
                "Cycles",
                "PMDS");
        }


        // =========================================================
        // CHANGE PASSWORD - GET
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> ChangePassword(
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Login");
            }

            var user =
                await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ChangePasswordViewModel
            {
                UserId = user.Id,
                Email = user.Email
            };

            return View(model);
        }


        // =========================================================
        // CHANGE PASSWORD - POST
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordViewModel model)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("CHANGE PASSWORD POST STARTED");
            Console.WriteLine("========================================");

            Console.WriteLine(
                $"UserId: {model.UserId}");

            Console.WriteLine(
                $"Email: {model.Email}");

            Console.WriteLine(
                $"Current password supplied: " +
                $"{!string.IsNullOrWhiteSpace(model.CurrentPassword)}");

            Console.WriteLine(
                $"New password supplied: " +
                $"{!string.IsNullOrWhiteSpace(model.NewPassword)}");

            Console.WriteLine(
                $"Confirm password supplied: " +
                $"{!string.IsNullOrWhiteSpace(model.ConfirmPassword)}");


            // =====================================================
            // MODEL VALIDATION
            // =====================================================

            if (!ModelState.IsValid)
            {
                Console.WriteLine(
                    "CHANGE PASSWORD FAILED: ModelState invalid.");

                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine(
                            $"FIELD: {item.Key}");

                        Console.WriteLine(
                            $"ERROR: {error.ErrorMessage}");
                    }
                }

                return View(model);
            }


            // =====================================================
            // CHECK USER ID
            // =====================================================

            if (string.IsNullOrWhiteSpace(model.UserId))
            {
                Console.WriteLine(
                    "ERROR: UserId is empty.");

                ModelState.AddModelError(
                    "",
                    "User account could not be identified.");

                return View(model);
            }


            // =====================================================
            // FIND IDENTITY USER
            // =====================================================

            var user =
                await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                Console.WriteLine(
                    "ERROR: Identity user NOT FOUND.");

                ModelState.AddModelError(
                    "",
                    "User account could not be found.");

                return RedirectToAction("Login");
            }


            Console.WriteLine("----------------------------------------");
            Console.WriteLine("IDENTITY USER FOUND");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"User ID: {user.Id}");
            Console.WriteLine($"Username: {user.UserName}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine("----------------------------------------");


            // =====================================================
            // CHECK NEW PASSWORD MATCH
            // =====================================================

            if (model.NewPassword != model.ConfirmPassword)
            {
                Console.WriteLine(
                    "ERROR: New passwords do not match.");

                ModelState.AddModelError(
                    "",
                    "The new password and confirmation password do not match.");

                return View(model);
            }


            // =====================================================
            // CHECK CURRENT / TEMPORARY PASSWORD
            // =====================================================

            Console.WriteLine(
                "Checking current temporary password...");

            var passwordCorrect =
                await _userManager.CheckPasswordAsync(
                    user,
                    model.CurrentPassword);


            Console.WriteLine("----------------------------------------");
            Console.WriteLine("CURRENT PASSWORD CHECK");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(
                $"Current password correct: {passwordCorrect}");
            Console.WriteLine("----------------------------------------");


            if (!passwordCorrect)
            {
                Console.WriteLine(
                    "ERROR: Current password is incorrect.");

                ModelState.AddModelError(
                    "",
                    "The current temporary password is incorrect. Please enter the exact temporary password supplied by the administrator.");

                return View(model);
            }


            // =====================================================
            // CHANGE PASSWORD
            // =====================================================

            Console.WriteLine(
                "Current password is correct.");

            Console.WriteLine(
                "Attempting to change password...");


            var result =
                await _userManager.ChangePasswordAsync(
                    user,
                    model.CurrentPassword,
                    model.NewPassword);


            Console.WriteLine("----------------------------------------");
            Console.WriteLine("CHANGE PASSWORD RESULT");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(
                $"Succeeded: {result.Succeeded}");
            Console.WriteLine("----------------------------------------");


            // =====================================================
            // PASSWORD CHANGE FAILED
            // =====================================================

            if (!result.Succeeded)
            {
                Console.WriteLine(
                    "PASSWORD CHANGE FAILED.");

                foreach (var error in result.Errors)
                {
                    Console.WriteLine(
                        $"ERROR CODE: {error.Code}");

                    Console.WriteLine(
                        $"ERROR DESCRIPTION: {error.Description}");

                    ModelState.AddModelError(
                        "",
                        error.Description);
                }

                return View(model);
            }


            // =====================================================
            // PASSWORD CHANGED SUCCESSFULLY
            // =====================================================

            Console.WriteLine(
                "PASSWORD CHANGED SUCCESSFULLY.");


            // =====================================================
            // UPDATE EMPLOYEE
            // =====================================================

            var employee =
                await _context.Employees
                    .FirstOrDefaultAsync(
                        e => e.UserId == user.Id);


            if (employee != null)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("EMPLOYEE FOUND");
                Console.WriteLine("----------------------------------------");

                Console.WriteLine(
                    $"Employee ID: {employee.Id}");

                Console.WriteLine(
                    $"Employee Name: " +
                    $"{employee.FirstName} {employee.LastName}");

                Console.WriteLine(
                    $"MustChangePassword BEFORE: " +
                    $"{employee.MustChangePassword}");


                employee.MustChangePassword = false;

                await _context.SaveChangesAsync();


                Console.WriteLine(
                    $"MustChangePassword AFTER: " +
                    $"{employee.MustChangePassword}");
            }
            else
            {
                Console.WriteLine(
                    "WARNING: Employee record not found.");
            }


            // =====================================================
            // SIGN OUT
            // =====================================================

            await _signInManager.SignOutAsync();


            Console.WriteLine("----------------------------------------");
            Console.WriteLine("USER SIGNED OUT");
            Console.WriteLine("PASSWORD CHANGE COMPLETED");
            Console.WriteLine("REDIRECTING TO LOGIN");
            Console.WriteLine("----------------------------------------");


            TempData["Success"] =
                "Password changed successfully. Please log in using your new password.";


            return RedirectToAction("Login");
        }


        // =========================================================
        // POST: REGISTER
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            Console.WriteLine("====================================");
            Console.WriteLine("REGISTER POST RECEIVED");
            Console.WriteLine("====================================");

            Console.WriteLine(
                $"Persal Number : {model.PersalNumber}");

            Console.WriteLine(
                $"Email         : {model.Email}");

            Console.WriteLine(
                $"Password      : " +
                $"{(string.IsNullOrEmpty(model.Password)
                    ? "EMPTY"
                    : "ENTERED")}");

            Console.WriteLine(
                $"Confirm Pass  : " +
                $"{(string.IsNullOrEmpty(model.ConfirmPassword)
                    ? "EMPTY"
                    : "ENTERED")}");


            // =====================================================
            // MODEL VALIDATION
            // =====================================================

            if (!ModelState.IsValid)
            {
                Console.WriteLine(
                    "MODELSTATE INVALID");

                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine(
                            $"FIELD: {item.Key}");

                        Console.WriteLine(
                            $"ERROR: {error.ErrorMessage}");
                    }
                }

                return View(model);
            }


            // =====================================================
            // CHECK EMPLOYEE
            // =====================================================

            var employee =
                await _context.Employees
                    .FirstOrDefaultAsync(
                        e => e.PersalNumber ==
                             model.PersalNumber);


            if (employee == null)
            {
                Console.WriteLine(
                    "EMPLOYEE NOT FOUND");

                ModelState.AddModelError(
                    "",
                    "Employee record not found.");

                return View(model);
            }


            Console.WriteLine(
                $"Employee Id : {employee.Id}");

            Console.WriteLine(
                $"Employee Email : {employee.Email}");


            // =====================================================
            // CHECK EMAIL MATCHES
            // =====================================================

            if (!string.Equals(
                    employee.Email,
                    model.Email,
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(
                    "EMAIL DOES NOT MATCH EMPLOYEE RECORD");

                ModelState.AddModelError(
                    "",
                    "Email does not match the employee record.");

                return View(model);
            }


            // =====================================================
            // CHECK EXISTING ACCOUNT
            // =====================================================

            var existingUser =
                await _userManager.FindByEmailAsync(
                    model.Email);


            if (existingUser != null)
            {
                Console.WriteLine(
                    "IDENTITY ACCOUNT ALREADY EXISTS");

                ModelState.AddModelError(
                    "",
                    "An account with this email already exists.");

                return View(model);
            }


            // =====================================================
            // CREATE IDENTITY USER
            // =====================================================

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };


            Console.WriteLine(
                "CREATING IDENTITY USER...");


            var result =
                await _userManager.CreateAsync(
                    user,
                    model.Password);


            Console.WriteLine(
                $"Create Result: {result.Succeeded}");


            if (!result.Succeeded)
            {
                Console.WriteLine(
                    "IDENTITY CREATION FAILED");

                foreach (var error in result.Errors)
                {
                    Console.WriteLine(
                        $"{error.Code} : {error.Description}");

                    ModelState.AddModelError(
                        "",
                        error.Description);
                }

                return View(model);
            }


            Console.WriteLine(
                "IDENTITY USER CREATED");


            // =====================================================
            // ASSIGN EMPLOYEE ROLE
            // =====================================================

            var roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    "Employee");


            Console.WriteLine(
                $"Role Added: {roleResult.Succeeded}");


            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        error.Description);
                }

                return View(model);
            }


            // =====================================================
            // LINK EMPLOYEE TO IDENTITY USER
            // =====================================================

            employee.UserId = user.Id;

            // Registration uses a password chosen by the employee,
            // so they do not need a forced password change.
            employee.MustChangePassword = false;

            _context.Update(employee);

            await _context.SaveChangesAsync();


            Console.WriteLine(
                "EMPLOYEE LINKED");


            Console.WriteLine("====================================");
            Console.WriteLine("REGISTER SUCCESS");
            Console.WriteLine("====================================");


            return RedirectToAction("Login");
        }


        // =========================================================
        // POST: FORGOT PASSWORD
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(
            string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(
                    "",
                    "Please enter your email address.");

                return View();
            }


            var user =
                await _userManager.FindByEmailAsync(email);


            // Don't reveal whether email exists
            if (user == null)
            {
                TempData["Success"] =
                    "If an account exists for this email address, a password reset email has been sent.";

                return RedirectToAction(nameof(Login));
            }


            var token =
                await _userManager.GeneratePasswordResetTokenAsync(
                    user);


            var resetLink =
                Url.Action(
                    "ResetPassword",
                    "Account",
                    new
                    {
                        token,
                        email = user.Email
                    },
                    Request.Scheme);


            var body = $@"
                <h2>PMDS Password Reset</h2>

                <p>Hello,</p>

                <p>
                    We received a request to reset your PMDS password.
                </p>

                <p>
                    <a href='{resetLink}'>
                        Click here to reset your password
                    </a>
                </p>

                <p>
                    If you did not request this, simply ignore this email.
                </p>

                <br/>

                <p>
                    Department of Correctional Services<br/>
                    PMDS
                </p>";


            await _emailService.SendEmailAsync(
                user.Email!,
                "PMDS Password Reset",
                body);


            TempData["Success"] =
                "If an account exists for this email address, a password reset email has been sent.";


            return RedirectToAction(nameof(Login));
        }


        // =========================================================
        // GET: RESET PASSWORD
        // =========================================================
        [HttpGet]
        public IActionResult ResetPassword(
            string token,
            string email)
        {
            if (string.IsNullOrEmpty(token) ||
                string.IsNullOrEmpty(email))
            {
                return RedirectToAction(nameof(Login));
            }


            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };


            return View(model);
        }


        // =========================================================
        // POST: RESET PASSWORD
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var user =
                await _userManager.FindByEmailAsync(
                    model.Email);


            if (user == null)
            {
                TempData["Success"] =
                    "Your password has been reset successfully.";

                return RedirectToAction(nameof(Login));
            }


            var result =
                await _userManager.ResetPasswordAsync(
                    user,
                    model.Token,
                    model.NewPassword);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        error.Description);
                }

                return View(model);
            }


            // =====================================================
            // UPDATE EMPLOYEE
            // =====================================================

            var employee =
                await _context.Employees
                    .FirstOrDefaultAsync(
                        e => e.UserId == user.Id);


            if (employee != null)
            {
                employee.MustChangePassword = false;

                await _context.SaveChangesAsync();
            }


            TempData["Success"] =
                "Password reset successfully. You can now log in using your new password.";


            return RedirectToAction(nameof(Login));
        }


        // =========================================================
        // GET: FORGOT PASSWORD
        // =========================================================
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        // =========================================================
        // POST: LOGOUT
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}