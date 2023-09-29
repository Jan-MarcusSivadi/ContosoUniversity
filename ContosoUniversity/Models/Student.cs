using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models;

public class Student
{
    public int ID { get; set; }
    public string LastName { get; set; }
    public string FirstMidName { get; set; }
    [Display(Name = "Full Name")]
    public string FullName { get { return LastName + ", " + FirstMidName; } }
    //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTime EnrollmentDate { get; set; }
    public ICollection<Enrollment>? Enrollments { get; set; }
}