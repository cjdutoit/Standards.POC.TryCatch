// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standards.POC.TryCatch.Api.Models.Students.Exceptions
{
    public class ForbiddenStudentException : Xeption
    {
        public ForbiddenStudentException(Exception innerException)
            : base(message: "Forbidden student access error occurred, contact support.", innerException)
        { }
    }
}