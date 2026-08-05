using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Data;
using PMDSSystems.Models;
using System.Linq;
using System.Threading.Tasks;
using PMDSSystems.Models;

namespace PMDSSystems.Controllers
{
    public class SupervisorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupervisorController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult PersonalDevelopmentPlan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SavePDP(PDPModel model)
        {
            // Save to database
            return RedirectToAction("Success");
        }
        // GET: Supervisor Dashboard
        public async Task<IActionResult> Index()
        {
            // 🔹 Get logged-in supervisor (optional if using Identity later)
            // var userId = User.Identity.Name;

            // 🔹 Get employees (you can filter later by SupervisorId)
            var employees = await _context.Employees
                //.Where(e => e.SupervisorId == userId) // optional filtering
                .ToListAsync();

            return View(employees);
        }
    }
}