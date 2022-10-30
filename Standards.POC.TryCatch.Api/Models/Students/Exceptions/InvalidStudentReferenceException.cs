using System;
using Xeptions;

namespace Standards.POC.TryCatch.Api.Models.Students.Exceptions
{
    public class InvalidStudentReferenceException : Xeption
    {
        public InvalidStudentReferenceException(Exception innerException)
            : base(message: "Invalid student reference error occurred.", innerException) { }
    }
}