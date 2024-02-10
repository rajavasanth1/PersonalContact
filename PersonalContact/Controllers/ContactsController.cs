using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using PersonalContact;
using PersonalContact.Models;

namespace PersonalContact.Controllers
{
    public class ContactsController : Controller
    {
        private readonly PersonalDBContext _context;

        public ContactsController(PersonalDBContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var personalDBContext = _context.Contact.Include(c => c.Country).Include(c => c.Person).Include(c => c.State);
            return View(await personalDBContext.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .Include(c => c.Country)
                .Include(c => c.Person)
                .Include(c => c.State)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            var countries = _context.Countries.ToList().OrderBy(x=>x.CountryName);
            var states=new List<Models.State>();

            //countries.Add(new Country()
            //{
            //    Id = 0,
            //    CountryName = "--Select Country--"
            //});

            //states.Add(new Models.State()
            //{
            //    Id = 0,
            //    Name = "--Select State--"
            //});

            ViewData["CountryId"] = new SelectList(countries, "Id", "CountryName");
            ViewData["PersonId"] = new SelectList(_context.Person, "Id", "FirstName");
            ViewData["StateId"] = new SelectList(states, "Id", "Name");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PersonId,Email,Mobile,HomePhone,Address1,Address2,City,CountryId,StateId,PostalCode")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", contact.Country.CountryName);
            ViewData["PersonId"] = new SelectList(_context.Person, "Id", "FirstName", contact.PersonId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", contact.State.Name);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", contact.CountryId);
            ViewData["PersonId"] = new SelectList(_context.Person, "Id", "FirstName", contact.PersonId);
            ViewData["StateId"] = new SelectList(_context.States.Where(x=>x.CountryId==contact.CountryId).ToList() , "Id", "Name", contact.StateId);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PersonId,Email,Mobile,HomePhone,Address1,Address2,City,CountryId,StateId,PostalCode")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Capital", contact.CountryId);
            ViewData["PersonId"] = new SelectList(_context.Person, "Id", "FirstName", contact.PersonId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", contact.StateId);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .Include(c => c.Country)
                .Include(c => c.Person)
                .Include(c => c.State)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contact.FindAsync(id);
            if (contact != null)
            {
                _context.Contact.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contact.Any(e => e.Id == id);
        }

        public JsonResult GetStatesByCountryID(int countryID)
        {
            var states = _context.States.Where(x => x.CountryId == countryID).ToList().OrderBy(x=>x.Name);
            return Json(states);
        }
        public JsonResult GetStateByContactID(int contactID)
        {
            var contact = _context.Contact.FirstOrDefault(x => x.Id == contactID);
            var states = _context.States.Where(x => x.Id== contact.StateId).ToList();
            return Json(states);
        }
    }
}
