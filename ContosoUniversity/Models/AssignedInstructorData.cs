namespace ContosoUniversity.Models
{
    public class AssignedInstructorData
    {
        public Instructor? Instructor { get; set; }
        public Student? Student { get; set; }
        public bool Assigned { get; set; }
    }
}
