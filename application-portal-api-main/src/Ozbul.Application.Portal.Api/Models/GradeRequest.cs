namespace Ozbul.Application.Portal.Api.Models
{
    public class GradeRequest
    {
        public int StudentId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public double GradeValue { get; set; }
       
        public int Credit { get; set; }
        // Add other grade details
    }
}
