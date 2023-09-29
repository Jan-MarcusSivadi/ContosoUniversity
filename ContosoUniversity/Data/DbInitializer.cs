using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace ContosoUniversity.Data;

public static class DbInitializer
{
    public static void Initialize(SchoolContext context)
    {
        context.Database.EnsureCreated();

        if (context.Students.Any())
        {
            return;
        }

        var students = new Student[]
        {
            new Student() {FirstMidName="Kaarel-Martin",LastName="Noole",EnrollmentDate=DateTime.Now},
            new Student() {FirstMidName="Palmi",LastName="Lahe",EnrollmentDate=DateTime.Parse("2021-09-1")},
            new Student() {FirstMidName="Kommi",LastName="Onu",EnrollmentDate=DateTime.Parse("2021-09-01")},
            new Student() {FirstMidName="Risto",LastName="Koort",EnrollmentDate=DateTime.Parse("2021-09-1")},
            new Student() {FirstMidName="Kregor",LastName="Latt",EnrollmentDate=DateTime.Parse("2021-09-1")},
            new Student() {FirstMidName="Joonas",LastName="Õispuu",EnrollmentDate=DateTime.Parse("2021-09-1")},
        };
        foreach (Student s in students)
        {
            context.Students.Add(s);
        }
        context.SaveChanges();

        var instructors = new Instructor[]
        {
            new Instructor() {FirstMidName = "Jõulu", LastName = "Vana", HireDate=DateTime.Parse("1995-03-11")},
            new Instructor() {FirstMidName = "Rootsi", LastName = "Kuningas", HireDate=DateTime.Parse("1995-03-11")},
            new Instructor() {FirstMidName = "Balta", LastName = "Parm", HireDate=DateTime.Parse("1995-03-11")},
            new Instructor() {FirstMidName = "Kinder", LastName = "Surprise", HireDate=DateTime.Parse("1995-03-11")},
        };
        foreach (Instructor i in instructors)
        {
            context.Instructors.Add(i);
        }
        context.SaveChanges();

        var departments = new Department[]
        {
            new Department()
            {
                Name = "Infotehnoloogia",
                Budget = 0,
                StartDate = DateTime.Parse("2007-09-01"),
                InstructorID = instructors.Single(i => i.LastName == "Parm").ID,
            },
            new Department()
            {
                Name = "Suhtlemine",
                Budget = 0,
                StartDate = DateTime.Parse("2007-09-01"),
                InstructorID = instructors.Single(i => i.LastName == "Kuningas").ID,
            },
            new Department()
            {
                Name = "Jõulu-õpetus",
                Budget = 0,
                StartDate = DateTime.Parse("2007-09-01"),
                InstructorID = instructors.Single(i => i.LastName == "Vana").ID,
            },
            new Department()
            {
                Name = "Ettevõtlus",
                Budget = 0,
                StartDate = DateTime.Parse("2007-09-01"),
                InstructorID = instructors.Single(i => i.LastName == "Kuningas").ID,
            },
            new Department()
            {
                Name = "Kokandus",
                Budget = 0,
                StartDate = DateTime.Parse("2007-09-01"),
                InstructorID = instructors.Single(i => i.LastName == "Surprise").ID,
            },
        };
        foreach (Department d in departments)
        {
            context.Departments.Add(d);
        }
        context.SaveChanges();

        var officeAssignments = new OfficeAssignment[]
        {
            new OfficeAssignment() 
            {
                InstructorID = instructors.Single(i => i.LastName == "Vana").ID, 
                Location = "A236"
            },
            new OfficeAssignment()
            {
                InstructorID = instructors.Single(i => i.LastName == "Parm").ID,
                Location = "A230"
            },
            new OfficeAssignment()
            {
                InstructorID = instructors.Single(i => i.LastName == "Surprise").ID,
                Location = "B220"
            },
        };
        foreach (OfficeAssignment o in officeAssignments)
        {
            context.OfficeAssignments.Add(o);
        }
        context.SaveChanges();

        var courses = new Course[]
        {
            new Course() {ID=1050,Title="Programmeerimine",Credits=160},
            new Course() {ID=6900,Title="Keemia",Credits=160},
            new Course() {ID=1420,Title="Matemaatika",Credits=160},
            new Course() {ID=5555,Title="Testimine",Credits=160},
            new Course() {ID=1234,Title="Riigikaitse",Credits=160},
        };
        foreach (Course c in courses)
        {
            context.Courses.Add(c);
        }
        context.SaveChanges();

        var courseAssignments = new CourseAssignment[]
        {
            // Parm
            new CourseAssignment()
            {
                CourseID = courses.Single(c => c.Title == "Keemia").ID,
                InstructorID = instructors.Single(i => i.LastName == "Parm").ID
            },
            new CourseAssignment()
            {
                CourseID = courses.Single(c => c.Title == "Matemaatika").ID,
                InstructorID = instructors.Single(i => i.LastName == "Parm").ID
            },
            new CourseAssignment()
            {
                CourseID = courses.Single(c => c.Title == "Riigikaitse").ID,
                InstructorID = instructors.Single(i => i.LastName == "Parm").ID
            },
            // Vana
            new CourseAssignment()
            {
                CourseID = courses.Single(c => c.Title == "Keemia").ID,
                InstructorID = instructors.Single(i => i.LastName == "Vana").ID
            },
            new CourseAssignment()
            {
                CourseID = courses.Single(c => c.Title == "Programmeerimine").ID,
                InstructorID = instructors.Single(i => i.LastName == "Vana").ID
            },
            new CourseAssignment()
            {
                CourseID = courses.Single(c => c.Title == "Matemaatika").ID,
                InstructorID = instructors.Single(i => i.LastName == "Vana").ID
            },
            // Surprise
            new CourseAssignment()
            {
                CourseID = courses.Single(c => c.Title == "Matemaatika").ID,
                InstructorID = instructors.Single(i => i.LastName == "Surprise").ID
            },
            new CourseAssignment()
            {
                CourseID = courses.Single(c => c.Title == "Riigikaitse").ID,
                InstructorID = instructors.Single(i => i.LastName == "Surprise").ID
            },
        };
        foreach (CourseAssignment ci in courseAssignments)
        {
            context.CourseAssignments.Add(ci);
        }
        context.SaveChanges();

        var enrollments = new Enrollment[]
        {
            // student 1
            new Enrollment() {StudentID=1,CourseID=1050,Grade=Grade.A},
            new Enrollment() {StudentID=1,CourseID=6900,Grade=Grade.B},
            new Enrollment() {StudentID=1,CourseID=1420,Grade=Grade.C},
            new Enrollment() {StudentID=1,CourseID=5555,Grade=Grade.D},
            // student 2
            new Enrollment() {StudentID=2,CourseID=1050,Grade=Grade.B},
            new Enrollment() {StudentID=2,CourseID=6900,Grade=Grade.B},
            new Enrollment() {StudentID=2,CourseID=1420,Grade=Grade.A},
            new Enrollment() {StudentID=2,CourseID=5555,Grade=Grade.C},
            new Enrollment() {StudentID=2,CourseID=1234,Grade=Grade.C},
            new Enrollment() {StudentID=2,CourseID=1234,Grade=Grade.D},
            // student 3
            new Enrollment() {StudentID=3,CourseID=1050,Grade=Grade.C},
            new Enrollment() {StudentID=3,CourseID=6900,Grade=Grade.A},
            new Enrollment() {StudentID=3,CourseID=1420,Grade=Grade.A},
            new Enrollment() {StudentID=3,CourseID=5555,Grade=Grade.B},
            // student 4
            new Enrollment() {StudentID=4,CourseID=1050,Grade=Grade.A},
            new Enrollment() {StudentID=4,CourseID=6900,Grade=Grade.B},
            new Enrollment() {StudentID=4,CourseID=1420,Grade=Grade.A},
            new Enrollment() {StudentID=4,CourseID=5555,Grade=Grade.C},
            // student 5
            new Enrollment() {StudentID=5,CourseID=1050,Grade=Grade.A},
            new Enrollment() {StudentID=5,CourseID=6900,Grade=Grade.A},
            new Enrollment() {StudentID=5,CourseID=1420,Grade=Grade.A},
            new Enrollment() {StudentID=5,CourseID=5555,Grade=Grade.A},
            new Enrollment() {StudentID=5,CourseID=1234,Grade=Grade.B},
            // student 6
            new Enrollment() {StudentID=6,CourseID=1050,Grade=Grade.B},
            new Enrollment() {StudentID=6,CourseID=6900,Grade=Grade.A},
            new Enrollment() {StudentID=6,CourseID=1420,Grade=Grade.C},
            new Enrollment() {StudentID=6,CourseID=5555,Grade=Grade.B},
            new Enrollment() {StudentID=6,CourseID=1234,Grade=Grade.A},
        };
        foreach (Enrollment e in enrollments)
        {
            var enrollmentInDatabase = context.Enrollments.Where(
                s => s.StudentID == e.StudentID &&
                s.CourseID == e.CourseID).SingleOrDefault();
            if (enrollmentInDatabase == null)
            {
                context.Enrollments.Add(e);
            }
        }
        context.SaveChanges();
    }
}
