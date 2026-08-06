using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PMDSSystems.Controllers
{
    public class SupervisorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupervisorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Supervisor Dashboard
        public async Task<IActionResult> Index()
        {
            // Get logged-in user ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Find supervisor in Employee table
            var supervisor = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (supervisor == null)
            {
                return Content("Supervisor not found.");
            }

            // Get only team members
            var employees = await _context.Employees
                .Where(e => e.SupervisorId == supervisor.Id)
                .ToListAsync();

            return View(employees);
        }

        // ✅ PDP Form Page
        public IActionResult PersonalDevelopmentPlan()
        {
            return View();
        }

        // ✅ Save PDP (FIXES 405 ERROR)
        [HttpPost]
        public IActionResult SavePDP(PDPModel model)
        {
            if (ModelState.IsValid)
            {
                // Example save (you can create a PDP table later)
                // _context.PDPs.Add(model);
                // _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("PersonalDevelopmentPlan", model);
        }
    }
}