using ContosoUniversity.Data;
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
        public async Task<IActionResult> Index(int? id, int? courseId)
        {
            var vm = new InstructorIndexData();
            vm.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Enrollments)
                .ThenInclude(i => i.Student)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Department)
                .AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();
            if (id != null)
            {
                ViewData["InstructorID"] = id.Value;
                Instructor instructor = vm.Instructors
                    .Where(i => i.ID == id.Value).Single();
                vm.Courses = instructor.CourseAssignments
                    .Select(i => i.Course);
            }
            if (courseId != null)
            {
                ViewData["CourseID"] = courseId.Value;
                vm.Enrollments = vm.Courses
                    .Where(x => x.ID == courseId)
                    .Single().Enrollments;
            }
            return View(vm);
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Enrollments)
                .ThenInclude(i => i.Student)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Department)
                .AsNoTracking()
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
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();
            PopulateAssignedCourseData(instructor);
            return View();
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName","FirstMidName","HireDate,OfficeAssignments")] Instructor instructor,
            string selectedCourses)
        {
            //ModelState.Remove("CourseAssignments");
            //ModelState.Remove("OfficeAssignment");
            if (selectedCourses != null)
            {

            }
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

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if(instructor == null)
            {
                return NotFound();
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstMidName,HireDate")] Instructor instructor) //string[] selectedCourses
        {
            if (id != instructor.ID)
            {
                return NotFound();
            }
            ModelState.Remove("OfficeAssignment");
            ModelState.Remove("CourseAssignments");
            //var instructorToUpdate = await _context.Instructors
            //    .Include(i => i.OfficeAssignment)
            //    .Include(i => i.CourseAssignments)
            //    .ThenInclude(i => i.Course)
            //    .FirstOrDefaultAsync(s => s.ID == id);
            var instructorToUpdate = await _context.Instructors
                .FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Instructor>(instructorToUpdate, "", studentToUpdate => studentToUpdate.FirstMidName,
                s => s.LastName))
            {
                try
                {
                    PopulateAssignedCourseData(instructorToUpdate);
                    await _context.SaveChangesAsync();

                    //PopulateAssignedCourseData(instructorToUpdate);
                    return View(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. "
                    + "Please try again later, and if the problem persists "
                    + "contact your system administrator");

                }
                finally {
                    TempData["message"] = "test";
                }
            }
            return View(instructorToUpdate);
            //await TryUpdateModelAsync<Instructor>(instructorToUpdate, "",
            //    i => i.FirstMidName,
            //    i => i.LastName,
            //    i => i.HireDate,
            //    i => i.OfficeAssignment)
            //ModelState.Remove("");
            //if (true)
            //{
            //    if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
            //    {
            //        instructorToUpdate.OfficeAssignment = null;
            //    }

            //    if (await TryUpdateModelAsync<Instructor>(instructorToUpdate, "", instructorToUpdate => instructorToUpdate.FirstMidName,
            //    s => s.LastName, s => s.HireDate, s => s.OfficeAssignment))
            //    {
            //        try
            //        {
            //            await _context.SaveChangesAsync();
            //            return RedirectToAction(nameof(Index));
            //        }
            //        catch (DbUpdateException)
            //        {
            //            ModelState.AddModelError("", "Unable to save changes. "
            //            + "Please try again later, and if the problem persists "
            //            + "contact your system administrator");
            //        }
            //    }

            //    //UpdateInstructorCourses(selectedCourses, instructorToUpdate);
            //    //try
            //    //{
            //    //    _context.Update(instructorToUpdate);
            //    //    await _context.SaveChangesAsync();
            //    //    UpdateInstructorCourses(selectedCourses, instructorToUpdate);
            //    //    PopulateAssignedCourseData(instructorToUpdate);
            //    //    return RedirectToAction(nameof(Index));
            //    //}
            //    //catch (DbUpdateException)
            //    //{
            //    //    ModelState.AddModelError("", "Unable to save changes. "
            //    //    + "Please try again later, and if the problem persists "
            //    //    + "contact your system administrator");
            //    //}
            //}
            //return View(instructorToUpdate);
            //return View(instructor);
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }
            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.CourseAssignments.Select(c => c.CourseID));
            foreach (var course in _context.Courses)
            {
                if (!selectedCoursesHS.Contains(course.ID.ToString()))
                {
                    if (!instructorCourses.Contains(course.ID))
                    {
                        instructorToUpdate.CourseAssignments.Add(new CourseAssignment
                        {
                            InstructorID = instructorToUpdate.ID,
                            CourseID = course.ID
                        });
                    }
                    else
                    {
                        if (instructorCourses.Contains(course.ID))
                        {
                            CourseAssignment courseToRemove = instructorToUpdate.CourseAssignments
                                .FirstOrDefault(c => c.CourseID == course.ID);
                            _context.Remove(courseToRemove);
                        }
                    }
                }
            }
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }
            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Enrollments)
                .ThenInclude(i => i.Student)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            //var instructor = await _context.Instructors
            //    .FirstOrDefaultAsync(m => m.ID == id);
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
            Instructor instructor = await _context.Instructors
                .Include(i => i.CourseAssignments)
                .SingleAsync(i => i.ID == id);
            var departments = await _context.Departments
                .Where(d => d.InstructorID == id)
                .ToListAsync();
            departments.ForEach(d => d.InstructorID = null);

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

        private async void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = await _context.Courses
                .ToListAsync();
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            var vm = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                vm.Add(new AssignedCourseData
                {
                    CourseID = course.ID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.ID)
                });
            }
            //ViewBag.Courses = vm;
            ViewData["Courses"] = vm;
        }
    }
}
