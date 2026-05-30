using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DZ3_ASOIU.Models;

namespace DZ3_ASOIU.Controllers;
public class CitiesController : Controller
{
    private readonly AppDbContext _context;
    public CitiesController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var cities = await _context.Cities
            .Include(c => c.Country)
            .OrderBy(c => c.Name)
            .ToListAsync();
        return View(cities);
    }
    public async Task<IActionResult> Create()
    {
        var countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
        ViewBag.Countries = new SelectList(countries, "Id", "Name");
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CountryId,Name,PopulationK")] City city)
    {
        if (string.IsNullOrWhiteSpace(city.Name))
        {
            ModelState.AddModelError("Name", "Название города не может быть пустым.");
        }

        if (city.PopulationK < 0)
        {
            ModelState.AddModelError("PopulationK", "Население не может быть отрицательным.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Cities.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при сохранении: {ex.Message}");
            }
        }

        var countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
        ViewBag.Countries = new SelectList(countries, "Id", "Name", city.CountryId);
        return View(city);
    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var city = await _context.Cities.FindAsync(id);
        if (city == null) return NotFound();

        var countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
        ViewBag.Countries = new SelectList(countries, "Id", "Name", city.CountryId);
        return View(city);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CountryId,Name,PopulationK")] City city)
    {
        if (id != city.Id) return NotFound();

        if (string.IsNullOrWhiteSpace(city.Name))
        {
            ModelState.AddModelError("Name", "Название города не может быть пустым.");
        }

        if (city.PopulationK < 0)
        {
            ModelState.AddModelError("PopulationK", "Население не может быть отрицательным.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Cities.Update(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CityExists(city.Id)) return NotFound();
                else throw;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при сохранении: {ex.Message}");
            }
        }

        var countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
        ViewBag.Countries = new SelectList(countries, "Id", "Name", city.CountryId);
        return View(city);
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var city = await _context.Cities
            .Include(c => c.Country)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (city == null) return NotFound();

        return View(city);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city == null) return NotFound();

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CityExists(int id)
    {
        return await _context.Cities.AnyAsync(e => e.Id == id);
    }
}
