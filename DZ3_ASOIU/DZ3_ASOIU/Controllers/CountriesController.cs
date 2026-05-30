using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DZ3_ASOIU.Models;

namespace DZ3_ASOIU.Controllers;
public class CountriesController : Controller
{
    private readonly AppDbContext _context;
    public CountriesController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
        return View(countries);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] Country country)
    {
        if (string.IsNullOrWhiteSpace(country.Name))
        {
            ModelState.AddModelError("Name", "Название страны не может быть пустым.");
        }

        if (ModelState.IsValid)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(country);
    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var country = await _context.Countries.FindAsync(id);
        if (country == null) return NotFound();

        return View(country);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Country country)
    {
        if (id != country.Id) return NotFound();

        if (string.IsNullOrWhiteSpace(country.Name))
        {
            ModelState.AddModelError("Name", "Название страны не может быть пустым.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Countries.Update(country);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(country.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(country);
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var country = await _context.Countries
            .FirstOrDefaultAsync(m => m.Id == id);
        if (country == null) return NotFound();
        bool hasCities = await _context.Cities.AnyAsync(c => c.CountryId == id);
        ViewBag.HasCities = hasCities;

        return View(country);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null) return NotFound();
        bool hasCities = await _context.Cities.AnyAsync(c => c.CountryId == id);
        if (hasCities)
        {
            ModelState.AddModelError("", "Невозможно удалить страну, так как в ней есть связанные города.");
            ViewBag.HasCities = true;
            return View(country);
        }

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CountryExists(int id)
    {
        return await _context.Countries.AnyAsync(e => e.Id == id);
    }
}
