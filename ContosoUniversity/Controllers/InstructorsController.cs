﻿using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            return _context.Instructors != null ?
                        View(await _context.Instructors
                            .ToListAsync()) : 
                        Problem("Entity set 'SchoolContext.Instructors' is null.");
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName","FirstMidName","HireDate")] Instructor instructor)
        {
            ModelState.Remove("CourseAssignments");
            ModelState.Remove("OfficeAssignment");
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(instructor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. "
                    + "Please try again later, and if the problem persists "
                    + "contact your system administrator");
            }
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if(instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName","FirstMidName,HireDate")] Instructor instructor)
        {
            if (id != instructor.ID)
            {
                return NotFound();
            }

            ModelState.Remove("CourseAssignments");
            ModelState.Remove("OfficeAssignment");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.ID))
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
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }
            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Instructors == null)
            {
                return Problem("Entity set 'SchoolContext.Instructors' is null.");
            }
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return (_context.Instructors?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}