// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Standards.POC.TryCatch.Api.Models.Students;
using Standards.POC.TryCatch.Api.Models.Students.Exceptions;

namespace Standards.POC.TryCatch.Api.Services.Foundations.Students
{
    public partial class StudentService
    {
        private async ValueTask<Student> WithRollback(
            ReturningStudentFunction returningStudentFunction,
            List<Type> rollbackExceptions)
        {
            try
            {
                return await returningStudentFunction();
            }
            catch (Exception ex)
            {
                if (rollbackExceptions != null && rollbackExceptions.Any(exception => exception == ex.GetType()))
                {
                    // Do rollback action here...

                    var studentRollbackException = new StudentRollbackException(ex);

                    // Throw rollback exception for upstream handling / distributed rollback
                    throw studentRollbackException;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}