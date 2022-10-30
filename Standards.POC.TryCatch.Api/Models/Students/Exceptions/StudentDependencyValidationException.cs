using Xeptions;

namespace Standards.POC.TryCatch.Api.Models.Students.Exceptions
{
    public class StudentDependencyValidationException : Xeption
    {
        public StudentDependencyValidationException(Xeption innerException)
            : base(message: "Student dependency validation occurred, please try again.", innerException)
        { }
    }
}