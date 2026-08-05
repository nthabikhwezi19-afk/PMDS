using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PMDSSystems.Data;

namespace PMDSSystems.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }



        public IActionResult SupervisorDashboard()
        {
            var userId = _userManager.GetUserId(User);

            var supervisor = _context.Employees
                .FirstOrDefault(e => e.UserId == userId);

            if (supervisor == null)
                return Content("Supervisor profile not linked to employee record");

            var team = _context.Employees
                .Where(e => e.SupervisorId == supervisor.Id)
                .ToList();

            return View(team);
        }

    }


}