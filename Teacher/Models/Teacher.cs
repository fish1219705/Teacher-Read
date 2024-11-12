namespace Teacher.Models
{
    public class ATeacher
    {
        public int TeacherId { get; set; }
        public string? TeacherFirstName { get; set; }
        public string? TeacherLastName { get; set; }
        public string? EmployeeNumber { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public string? TeacherCourse { get; set; }
    }
}
