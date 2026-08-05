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

        // =========================
        // GET: Login Page
        // =========================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // =========================
        // POST: Login
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string email,
            string password)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(
                    "",
                    "Email and Password are required");

                return View();
            }

            var user =
                await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid email or password");

                return View();
            }

            var result =
                await _signInManager.PasswordSignInAsync(
                    user.UserName!,
                    password,
                    isPersistent: false,
                    lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Find linked employee
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.UserId == user.Id);

                // First login? Force password change
                if (employee != null && employee.MustChangePassword)
                {
                    return RedirectToAction(
                        "ChangePassword",
                        new { userId = user.Id });
                }

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin") ||
                    roles.Contains("Supervisor"))
                {
                    return RedirectToAction(
                        "Index",
                        "Dashboard",
                        new { area = "Admin" });
                }

                

                if (employee == null)
                {
                    await _signInManager.SignOutAsync();

                    ModelState.AddModelError("", "Your employee profile was not found.");

                    return View();
                }

                // Force password change
                if (employee.MustChangePassword)
                {
                    return RedirectToAction(
                        "ChangePassword",
                        new { userId = user.Id });
                }

                // Continue normally
                return RedirectToAction(
                    "Cycles",
                    "PMDS"); 
            }
        
            ModelState.AddModelError(
                "",
                "Invalid email or password");

            return View();
        }

        // =========================
        // GET: Register
        // =========================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // =========================
        // POST: Register
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Console.WriteLine("====================================");
            Console.WriteLine("REGISTER POST RECEIVED");
            Console.WriteLine("====================================");

            Console.WriteLine($"Persal Number : {model.PersalNumber}");
            Console.WriteLine($"Email         : {model.Email}");
            Console.WriteLine($"Password      : {(string.IsNullOrEmpty(model.Password) ? "EMPTY" : "ENTERED")}");
            Console.WriteLine($"Confirm Pass  : {(string.IsNullOrEmpty(model.ConfirmPassword) ? "EMPTY" : "ENTERED")}");

            //---------------------------------------------------
            // MODEL VALIDATION
            //---------------------------------------------------

            if (!ModelState.IsValid)
            {
                Console.WriteLine("====================================");
                Console.WriteLine("MODELSTATE INVALID");
                Console.WriteLine("====================================");

                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"FIELD: {item.Key}");
                        Console.WriteLine($"ERROR: {error.ErrorMessage}");
                    }
                }

                return View(model);
            }

            //---------------------------------------------------
            // CHECK IF EMPLOYEE EXISTS
            //---------------------------------------------------

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e =>
                    e.PersalNumber == model.PersalNumber);

            if (employee == null)
            {
                Console.WriteLine("EMPLOYEE NOT FOUND");

                ModelState.AddModelError("", "Employee record not found.");

                return View(model);
            }

            Console.WriteLine("EMPLOYEE FOUND");
            Console.WriteLine($"Employee Id : {employee.Id}");
            Console.WriteLine($"Employee Email : {employee.Email}");

            //---------------------------------------------------
            // CHECK EMAIL MATCHES
            //---------------------------------------------------

            if (!string.Equals(employee.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("EMAIL DOES NOT MATCH EMPLOYEE RECORD");

                ModelState.AddModelError("", "Email does not match the employee record.");

                return View(model);
            }

            //---------------------------------------------------
            // CHECK EXISTING ACCOUNT
            //---------------------------------------------------

            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                Console.WriteLine("IDENTITY ACCOUNT ALREADY EXISTS");

                ModelState.AddModelError("", "An account with this email already exists.");

                return View(model);
            }

            //---------------------------------------------------
            // CREATE IDENTITY USER
            //---------------------------------------------------

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            Console.WriteLine("CREATING IDENTITY USER...");

            var result = await _userManager.CreateAsync(user, model.Password);

            Console.WriteLine($"Create Result: {result.Succeeded}");

            if (!result.Succeeded)
            {
                Console.WriteLine("IDENTITY CREATION FAILED");

                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"{error.Code} : {error.Description}");
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            Console.WriteLine("IDENTITY USER CREATED");

            //---------------------------------------------------
            // ASSIGN ROLE
            //---------------------------------------------------

            var roleResult = await _userManager.AddToRoleAsync(user, "Employee");

            Console.WriteLine($"Role Added: {roleResult.Succeeded}");

            //---------------------------------------------------
            // LINK EMPLOYEE
            //---------------------------------------------------

            employee.UserId = user.Id;

            _context.Update(employee);

            await _context.SaveChangesAsync();

            Console.WriteLine("EMPLOYEE LINKED");

            Console.WriteLine("====================================");
            Console.WriteLine("REGISTER SUCCESS");
            Console.WriteLine("====================================");

            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult ChangePassword(string userId)
        {
            var model = new ChangePasswordViewModel
            {
                UserId = userId
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            string userId,
            string currentPassword,
            string newPassword,
            string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                ModelState.AddModelError("", "All fields are required.");

                return View(new ChangePasswordViewModel
                {
                    UserId = userId
                });
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");

                return View(new ChangePasswordViewModel
                {
                    UserId = userId
                });
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                currentPassword,
                newPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(new ChangePasswordViewModel
                {
                    UserId = userId
                });
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == user.Id);

            if (employee != null)
            {
                employee.MustChangePassword = false;
                await _context.SaveChangesAsync();
            }

            await _signInManager.SignOutAsync();

            TempData["Success"] =
                "Password changed successfully. Please log in using your new password.";

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "Please enter your email address.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            // Don't reveal whether the email exists
            if (user == null)
            {
                TempData["Success"] =
                    "If an account exists for this email address, a password reset email has been sent.";

                return RedirectToAction(nameof(Login));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Action(
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

        <p>We received a request to reset your PMDS password.</p>

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

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                TempData["Success"] =
                    "Your password has been reset successfully.";

                return RedirectToAction(nameof(Login));
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                model.Token,
                model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            // Update employee record
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == user.Id);

            if (employee != null)
            {
                employee.MustChangePassword = false;

                await _context.SaveChangesAsync();
            }

            TempData["Success"] =
                "Password reset successfully. You can now log in using your new password.";

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        // =========================
        // LOGOUT
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}