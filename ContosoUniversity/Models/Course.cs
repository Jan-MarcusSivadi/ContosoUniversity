using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models;

public class Course
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ID { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }
    public int? DepartmentID { get; set; }
    public Department Department { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
    public ICollection<CourseAssignment> CourseAssignments { get; set; }
}