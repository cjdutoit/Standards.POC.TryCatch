// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;

namespace Standards.POC.TryCatch.Api.Services.Foundations.Students
{
    public partial class StudentService
    {
        private static void ValidateSecurityRequirement(List<string> securityRoles)
        {
            // Do security check here.
            // throw new ForbiddenStudentException();

            // Entity specific security checks can be done with the locigal validation
            // e.g.  person that submitted the student, can not be same as approver.
        }
    }
}