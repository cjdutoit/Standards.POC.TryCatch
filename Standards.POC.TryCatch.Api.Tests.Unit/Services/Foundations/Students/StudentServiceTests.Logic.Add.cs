using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Standards.POC.TryCatch.Api.Models.Students;
using Xunit;

namespace Standards.POC.TryCatch.Api.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        [Fact]
        public async Task ShouldAddStudentAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            Student randomStudent = CreateRandomStudent(randomDateTimeOffset);
            Student inputStudent = randomStudent;
            Student storageStudent = inputStudent;
            Student expectedStudent = storageStudent.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAsync(inputStudent))
                    .ReturnsAsync(storageStudent);

            // when
            Student actualStudent = await this.studentService
                .AddStudentAsync(inputStudent);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(inputStudent),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}