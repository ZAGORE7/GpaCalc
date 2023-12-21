namespace Ozbul.Application.Portal.Api.Models
{
    public class StudentRequest
    {
        public string Name { get; set; }
        public int StudentId { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public string Email { get; set; }
        public string MothersName { get; set; }
        public string FathersName { get; set; }
       
        // Add other student properties as needed
    }
}
