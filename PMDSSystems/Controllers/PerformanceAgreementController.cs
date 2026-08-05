using Microsoft.AspNetCore.Mvc;
using PMDSSystems.Data;
using PMDSSystems.Models; // ✅ ADD THIS

public class PerformanceAgreementController : Controller
{
    private readonly ApplicationDbContext _context;

    public PerformanceAgreementController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(PerformanceAgreement model)
    {
        if (ModelState.IsValid)
        {
            _context.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(model);
    }
    [HttpPost]
    public IActionResult SavePDP(PDPModel model)
    {
        // You can test first
        if (ModelState.IsValid)
        {
            // Save logic here later
        }

        return View("PersonalDevelopmentPlan");
    }
    public IActionResult Agreement()
    {
        return View();
    }
}