using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DZ3_ASOIU.Models;

namespace DZ3_ASOIU.Controllers;
public class ReportsController : Controller
{
    private readonly AppDbContext _context;
    public ReportsController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var model = new ReportsViewModel();
        model.Report1 = await _context.Cities
            .Include(c => c.Country)
            .OrderBy(c => c.Name)
            .Select(c => new ReportCityRow
            {
                Name = c.Name,
                CountryName = c.Country != null ? c.Country.Name : "Не указана",
                PopulationK = c.PopulationK
            })
            .ToListAsync();
        model.Report2 = await _context.Cities
            .GroupBy(c => c.Country != null ? c.Country.Name : "Не указана")
            .Select(g => new ReportCountRow
            {
                CountryName = g.Key,
                CityCount = g.Count()
            })
            .OrderBy(r => r.CountryName)
            .ToListAsync();
        model.Report3 = await _context.Cities
            .GroupBy(c => c.Country != null ? c.Country.Name : "Не указана")
            .Select(g => new ReportAvgRow
            {
                CountryName = g.Key,
                AvgPopulation = Math.Round(g.Average(c => c.PopulationK), 1)
            })
            .OrderByDescending(r => r.AvgPopulation)
            .ToListAsync();

        return View(model);
    }
}
