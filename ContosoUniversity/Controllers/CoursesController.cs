using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Packaging;
using System.Runtime.Intrinsics.X86;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(int? id, int? instructorId)
        {

            var vm = new InstructorIndexData();
            vm.Courses = await _context.Courses
                .Include(c => c.CourseAssignments)
                .ThenInclude(c => c.Instructor)
                .Include(c => c.Enrollments)
                .ThenInclude(i => i.Student)
                .AsNoTracking()
                .OrderByDescending(c => c.Credits)
                .ToListAsync();

            if (id != null)
            {
                ViewData["CourseID"] = id.Value;
                Course course = vm.Courses
                    .Where(i => i.ID == id.Value).Single();
                vm.Instructors = course.CourseAssignments
                    .Select(i => i.Instructor);
            }

            return _context.Courses != null ?
                          View(vm) :
                          Problem("Entity set 'SchoolContext.Courses'  is null.");
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses
                .Include(c => c.Department)
                .ThenInclude(d => d.Administrator)
                .Include(c => c.CourseAssignments)
                .ThenInclude(c => c.Instructor)
                .ThenInclude(c => c.OfficeAssignment)
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (courses == null)
            {
                return NotFound();
            }

            return View(courses);
        }

        // GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CourseID"] = Convert.ToInt32(_context.Courses.Max(x => x.ID)) + 1;
            //await PopulateCourseData();
            var course = new Course();
            course.CourseAssignments = new List<CourseAssignment>();
            PopulateAssignedCourseData(course);
            return View();
        }



        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Credits,Department")] Course course,
            string[] selectedStudents, string[] selectedInstructors, string selectedDepartment)
        {
            if ((selectedStudents != null && selectedStudents.Length > 0) &&
                (selectedInstructors != null && selectedInstructors.Length > 0))
            {
                course.CourseAssignments = new List<CourseAssignment>();
                foreach (var si in selectedInstructors)
                {
                    var assignment = new CourseAssignment()
                    {
                        CourseID = Convert.ToInt32(course.ID),
                        InstructorID = Convert.ToInt32(si)
                    };
                    course.CourseAssignments.Add(assignment);
                }

                course.Enrollments = new List<Enrollment>();
                foreach (var ss in selectedStudents)
                {
                    var enrollment = new Enrollment()
                    {
                        CourseID = Convert.ToInt32(course.ID),
                        StudentID = Convert.ToInt32(ss)
                    };
                    course.Enrollments.Add(enrollment);
                }
            }

            ModelState.Remove("Department");
            ModelState.Remove("Enrollments");
            ModelState.Remove("CourseAssignments");

            var depId = Convert.ToInt32(selectedDepartment);
            var department = await _context.Departments
                .FindAsync(depId);

            if (department == null)
            {
                return Problem("Entity set 'Course.Department'  is null.");
            }
            course.Department = department;

            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = Convert.ToInt32(_context.Courses.Max(x => x.ID)) + 1;
            PopulateAssignedCourseData(course);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            //var instructor = await _context.Instructors
            //    .Include(i => i.OfficeAssignment)
            //    .Include(i => i.CourseAssignments)
            //    .ThenInclude(i => i.Course)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(m => m.ID == id);
            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(i => i.CourseAssignments)
                .ThenInclude(c => c.Instructor)
                .Include(c => c.Enrollments)
                .ThenInclude(i => i.Student)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (course == null)
            {
                return NotFound();
            }
            //await PopulateCourseData();
            PopulateAssignedCourseData(course);
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Title,Credits")] Course course, string[] selectedStudents, string[] selectedInstructors, string selectedDepartment)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var courseToUpdate = await _context.Courses
            .Include(c => c.Department)
            .Include(i => i.CourseAssignments)
            //.ThenInclude(c => c.Instructor)
            .Include(c => c.Enrollments)
            //.ThenInclude(i => i.Student)
            .FirstOrDefaultAsync(m => m.ID == id);

            courseToUpdate.Title = course.Title;
            courseToUpdate.Credits = course.Credits;

            ModelState.Remove("Department");
            ModelState.Remove("Enrollments");
            ModelState.Remove("CourseAssignments");

            // Department
            var depId = Convert.ToInt32(selectedDepartment);
            var department = await _context.Departments
                .FindAsync(depId);

            if (department == null)
            {
                return Problem("Entity set 'Course.Department'  is null.");
            }
            courseToUpdate.Department = department;

            // Instructor
            //var instructorIds = new HashSet<int>(selectedInstructors.Select(s => Convert.ToInt32(s)));
            //var instructor = await _context.Instructors
            //    .SingleAsync(i => i.ID == instructorId);

            courseToUpdate.CourseAssignments = new List<CourseAssignment>();
            foreach (var si in selectedInstructors)
            {
                var assignment = new CourseAssignment()
                {
                    CourseID = Convert.ToInt32(courseToUpdate.ID),
                    InstructorID = Convert.ToInt32(si)
                };
                courseToUpdate.CourseAssignments.Add(assignment);
            }
            foreach (var sss in selectedStudents)
            {
                await Console.Out.WriteLineAsync($"SelectedStudents: {sss}");

            }

            if (true)
            {
                _context.Update(courseToUpdate);
                try
                {
                    UpdateStudentEnrollments(selectedStudents, courseToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to save changes. "
                    + "Please try again later, and if the problem persists "
                    + "contact your system administrator");
                }
                return RedirectToAction(nameof(Index));
            }
            //UpdateStudentEnrollments(selectedStudents, courseToUpdate);
            PopulateAssignedCourseData(courseToUpdate);
            return View(courseToUpdate);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }
            var course = await _context.Courses
                .Include(c => c.Department)
                .ThenInclude(d => d.Administrator)
                .Include(c => c.CourseAssignments)
                .ThenInclude(c => c.Instructor)
                .ThenInclude(c => c.OfficeAssignment)
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'SchoolContext.Courses'  is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return (_context.Courses?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        private void PopulateCourseData(List<AssignedCourseData> vm, List<AssignedCourseData> vm2)
        {
            //await _context.Instructors.ToListAsync();
            ViewData["Instructors"] = vm;
            ViewData["Students"] = vm2;
            //await _context.Students.ToListAsync();
            //ViewData["Students"] = new SelectList(_context.Students, "ID", "LastName");
            ViewData["Departments"] = new SelectList(_context.Departments, "ID", "Name");
        }

        private async void PopulateAssignedCourseData(Course course)
        {
            var vm = new List<AssignedCourseData>();
            var allInstructors = _context.Instructors;
            var courseInstructors = new HashSet<int>(course.CourseAssignments.Select(c => c.InstructorID));
            foreach (var instructor in allInstructors)
            {
                vm.Add(new AssignedCourseData
                {
                    ID = instructor.ID,
                    Title = instructor.FullName,
                    Assigned = courseInstructors.Contains(instructor.ID)
                });
            }
            var vm2 = new List<AssignedCourseData>();

            //var allEnrollments = _context.Enrollments;
            var allStudents = _context.Students;
            foreach (var student in allStudents)
            {
                var courseData = new AssignedCourseData();
                if (student != null)
                {
                    courseData.ID = student.ID;
                    courseData.Title = student.FullName;
                    courseData.Assigned = false;

                    if (course.Enrollments != null)
                    {
                        var studentEnrollments = new HashSet<int>(course.Enrollments.Select(c => c.StudentID));
                        courseData.Assigned = studentEnrollments.Contains(student.ID);
                    }
                    vm2.Add(courseData);
                }
            }

            PopulateCourseData(vm, vm2);
            //ViewData["Instructors"] = vm;
        }

        private void UpdateStudentEnrollments(string[] selectedStudents, Course courseToUpdate)
        {
            if (selectedStudents == null || selectedStudents.Length <= 0)
            {
                courseToUpdate.Enrollments = new List<Enrollment>();
                return;
            }
            var selectedStudentsHS = new HashSet<string>(selectedStudents);
            //var instructorCourses = new HashSet<int>(courseToUpdate.Enrollments.Select(c => c.CourseID));
            var courseEnrollments = new HashSet<int>(courseToUpdate.Enrollments.Select(c => c.StudentID));
            foreach (var student in _context.Students)
            {
                // selected student exists
                if (selectedStudentsHS.Contains(student.ID.ToString()))
                {
                    // course does not have current student
                    if (!courseEnrollments.Contains(student.ID))
                    {
                        courseToUpdate.Enrollments.Add(new Enrollment
                        {
                            StudentID = student.ID,
                            CourseID = courseToUpdate.ID
                        });
                    }
                } else
                {
                    if (courseEnrollments.Contains(student.ID))
                    {
                        Enrollment enrollmentToRemove = courseToUpdate.Enrollments
                        .FirstOrDefault(c => c.StudentID == student.ID);
                        _context.Remove(enrollmentToRemove);
                    }
                }
                //// course enrollments do not contain student
                //if (!courseEnrollments.Contains(student.ID))
                //{
                //    Enrollment enrollmentToRemove = courseToUpdate.Enrollments
                //        .FirstOrDefault(c => c.StudentID == student.ID);
                //    _context.Remove(enrollmentToRemove);

                //} else
                //{
                //    // selected does not contain current student
                //    if (selectedStudentsHS.Contains(student.ID.ToString()))
                //    {
                //        courseToUpdate.Enrollments.Add(new Enrollment
                //        {
                //            StudentID = student.ID,
                //            CourseID = courseToUpdate.ID
                //        });
                //    }
                //}
            }
        }

    }
}
