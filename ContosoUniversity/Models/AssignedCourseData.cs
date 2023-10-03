namespace ContosoUniversity.Models
{
    public class AssignedCourseData
    {
        public int? ID { get; set; }
        public int? CourseID { get; set; }
        public string Title { get; set; }
        public bool Assigned { get; set; }
    }
}
