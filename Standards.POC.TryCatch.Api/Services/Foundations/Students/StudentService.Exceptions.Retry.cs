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

namespace Standards.POC.TryCatch.Api.Services.Foundations.Students
{
    public partial class StudentService
    {
        private async ValueTask<Student> WithRetry(
            ReturningStudentFunction returningStudentFunction,
            List<Type> retryExceptionTypes)
        {
            var attempts = 0;

            while (true)
            {
                try
                {
                    attempts++;
                    return await returningStudentFunction();
                }
                catch (Exception ex)
                {
                    if (retryExceptionTypes.Any(exception => exception == ex.GetType()))
                    {
                        this.loggingBroker
                            .LogInformation(
                                $"Error found. Retry attempt {attempts}/{this.retryConfig.MaxRetryAttempts}. " +
                                    $"Exception: {ex.Message}");

                        if (attempts == this.retryConfig.MaxRetryAttempts)
                        {
                            throw;
                        }

                        Task.Delay(this.retryConfig.DelayBetweenFailures).Wait();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}