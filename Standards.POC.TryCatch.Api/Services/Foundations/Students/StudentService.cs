// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Standards.POC.TryCatch.Api.Brokers.DateTimes;
using Standards.POC.TryCatch.Api.Brokers.Loggings;
using Standards.POC.TryCatch.Api.Brokers.Storages;
using Standards.POC.TryCatch.Api.Models.Configuration.Retries;
using Standards.POC.TryCatch.Api.Models.Configuration.TryCatches;
using Standards.POC.TryCatch.Api.Models.Students;
using Standards.POC.TryCatch.Api.Models.Students.Exceptions;

namespace Standards.POC.TryCatch.Api.Services.Foundations.Students
{
    public partial class StudentService : IStudentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IRetryConfig retryConfig;

        public StudentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            IRetryConfig retryConfig)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.retryConfig = retryConfig;
        }

        public ValueTask<Student> AddStudentAsync(Student student)
        {
            return TryCatch(new TryCatchDefinition<ValueTask<Student>>
            {
                Execution = async () =>
                {
                    ValidateStudentOnAdd(student);

                    return await this.storageBroker.InsertStudentAsync(student);
                },
                WithTracing = new
                {
                    Enabled = true,
                    ActivityName =
                        "Standards.POC.TryCatch.Api.Services.Foundations.Students.StudentService.AddStudentAsync",
                    Tags = new Dictionary<string, string> { { "StudentId", student?.Id.ToString() } },
                    Baggage = new Dictionary<string, string> { { "StudentId", student?.Id.ToString() } },
                },
                WithSecurityRoles = new List<string>() { },
                WithRetryOn = new List<Type>() { typeof(TimeoutException) },
                WithRollbackOn = new List<Type>() { typeof(StudentDependencyValidationException) }
            });
        }

        public IQueryable<Student> RetrieveAllStudents() =>
            TryCatch(() => this.storageBroker.SelectAllStudents());

        public ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId) =>
            TryCatch(async () =>
            {
                ValidateStudentId(studentId);

                Student maybeStudent = await this.storageBroker
                    .SelectStudentByIdAsync(studentId);

                ValidateStorageStudent(maybeStudent, studentId);

                return maybeStudent;
            });

        public ValueTask<Student> ModifyStudentAsync(Student student) =>
            TryCatch(async () =>
            {
                ValidateStudentOnModify(student);

                Student maybeStudent =
                    await this.storageBroker.SelectStudentByIdAsync(student.Id);

                ValidateStorageStudent(maybeStudent, student.Id);
                ValidateAgainstStorageStudentOnModify(inputStudent: student, storageStudent: maybeStudent);

                return await this.storageBroker.UpdateStudentAsync(student);
            });

        public ValueTask<Student> RemoveStudentByIdAsync(Guid studentId) =>
            TryCatch(async () =>
            {
                ValidateStudentId(studentId);

                Student maybeStudent = await this.storageBroker
                    .SelectStudentByIdAsync(studentId);

                ValidateStorageStudent(maybeStudent, studentId);

                return await this.storageBroker.DeleteStudentAsync(maybeStudent);
            });
    }
}