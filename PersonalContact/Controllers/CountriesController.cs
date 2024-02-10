using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalContact;
using PersonalContact.Models;

namespace PersonalContact.Controllers
{
    public class CountriesController : Controller
    {
        private readonly PersonalDBContext _context;

        public CountriesController(PersonalDBContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index(string sortField, string currentSortField, string currentSortOrder, string searchText = "", int pg = 1)
        {
            //return View(await _context.Countries.ToListAsync());
            const int pageSize = 14;
            List<Country> countries;

            if (searchText != "" && searchText != null)
            {
                countries = _context.Countries.Where(x => x.CountryName.Contains(searchText)).ToList();
            }
            else
                countries = await _context.Countries.ToListAsync();

            int recsCount = countries.Count();

            Pager pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            countries = SortCountryData(countries, sortField, currentSortField, currentSortOrder);
            countries = countries.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(this.SortCountryData(countries, sortField, currentSortField, currentSortOrder));
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CountryName,Capital")] Country country)
        {          
            if (ModelState.IsValid)
            {
                if (_context.Countries.Any(c => c.CountryName == country.CountryName))
                {
                    ModelState.AddModelError(nameof(country.CountryName), country.CountryName + " already exists");
                    return View();
                }
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
            
    
        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CountryName,Capital")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                _context.Countries.Remove(country);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }

        private List<Country> SortCountryData(List<Country> countries, string sortField, string currentSortField, string currentSortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                ViewBag.SortField = "CountryName";
                ViewBag.SortOrder = "Asc";
            }
            else
            {
                if (currentSortField == sortField)
                {
                    ViewBag.SortOrder = currentSortOrder == "Asc" ? "Desc" : "Asc";
                }
                else
                {
                    ViewBag.SortOrder = "Asc";
                }
                ViewBag.SortField = sortField;
            }

            var propertyInfo = typeof(Country).GetProperty(ViewBag.SortField);
            if (ViewBag.SortOrder == "Asc")
            {
                countries = countries.OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
            }
            else
            {
                countries = countries.OrderByDescending(s => propertyInfo.GetValue(s, null)).ToList();
            }
            return countries;
        }
    }
}
