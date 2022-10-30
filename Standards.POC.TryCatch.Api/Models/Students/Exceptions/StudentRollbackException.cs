// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standards.POC.TryCatch.Api.Models.Students.Exceptions
{
    public class StudentRollbackException : Xeption
    {
        public StudentRollbackException(Exception innerException)
            : base(message: "Student rollback error occurred, contact support.", innerException)
        { }
    }
}