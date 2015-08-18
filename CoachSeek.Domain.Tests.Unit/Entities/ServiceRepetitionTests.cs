using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class ServiceRepetitionTests
    {
        [Test]
        public void ServiceRepetitionCreationTests()
        {
            ServiceRepetitionCreationFailure("x", 8, new[,] { { ErrorCodes.RepeatFrequencyInvalid, "The RepeatFrequency field is not valid.", "x", null } });
            ServiceRepetitionCreationFailure("w", -1, new[,] { { ErrorCodes.SessionCountInvalid, "The SessionCount field is not valid.", "-1", null } });
            ServiceRepetitionCreationFailure(null, -1, new[,] { { ErrorCodes.SessionCountInvalid, "The SessionCount field is not valid.", "-1", null } });
            ServiceRepetitionCreationFailure("d", -2, new[,] { { ErrorCodes.SessionCountInvalid, "The SessionCount field is not valid.", "-2", null } });
            ServiceRepetitionCreationFailure("z", -6, new[,] { { ErrorCodes.SessionCountInvalid, "The SessionCount field is not valid.", "-6", null },
                                                               { ErrorCodes.RepeatFrequencyInvalid, "The RepeatFrequency field is not valid.", "z", null } });
            ServiceRepetitionCreationFailure("d", 1, new[,] { { ErrorCodes.StandaloneSessionMustHaveNoRepeatFrequency, "Standalone sessions must not have the RepeatFrequency set.", null, null } });
            ServiceRepetitionCreationFailure(null, 12, new[,] { { ErrorCodes.CourseMustHaveRepeatFrequency, "Courses must have the RepeatFrequency set.", null, null } });

            ServiceRepetitionCreationFailure("d", 40, new[,] { { ErrorCodes.CourseExceedsMaximumNumberOfDailySessions, 
                                                                 "40 exceeds the maximum number of daily sessions in a course of 30.", 
                                                                 "Maximum Allowed Daily Session Count: 30; Specified Session Count: 40", null } });
            ServiceRepetitionCreationFailure("w", 30, new[,] { { ErrorCodes.CourseExceedsMaximumNumberOfWeeklySessions, 
                                                                 "30 exceeds the maximum number of weekly sessions in a course of 26.", 
                                                                 "Maximum Allowed Weekly Session Count: 26; Specified Session Count: 30", null } });

            ServiceRepetitionCreationSuccess(null, 1);   // Single session
            ServiceRepetitionCreationSuccess("w", 26);    // The maximum number of weekly sessions is 26.
            ServiceRepetitionCreationSuccess("d", 5);
            ServiceRepetitionCreationSuccess("d", 30);    // The maximum number of daily sessions is 30.
        }


        private void ServiceRepetitionCreationSuccess(string repeatFrequnecy, int sessionCount)
        {
            var repetition = new ServiceRepetition(new RepetitionData { RepeatFrequency = repeatFrequnecy, SessionCount = sessionCount });
            Assert.That(repetition, Is.Not.Null);
            Assert.That(repetition.RepeatFrequency, Is.EqualTo(repeatFrequnecy));
            Assert.That(repetition.SessionCount, Is.EqualTo(sessionCount));
        }

        private void ServiceRepetitionCreationFailure(string repeatFrequnecy, int sessionCount, string[,] expectedErrors)
        {
            try
            {
                var repetition = new ServiceRepetition(new RepetitionCommand(sessionCount, repeatFrequnecy));
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.InstanceOf<CoachseekException>());
                var errors = ((CoachseekException)ex).Errors;
                Assert.That(errors.Count, Is.EqualTo(expectedErrors.GetLength(0)));

                var i = 0;
                foreach (var error in errors)
                {
                    Assert.That(error.Code, Is.EqualTo(expectedErrors[i, 0]));
                    Assert.That(error.Message, Is.EqualTo(expectedErrors[i, 1]));
                    Assert.That(error.Data, Is.EqualTo(expectedErrors[i, 2]));
                    Assert.That(error.Field, Is.EqualTo(expectedErrors[i, 3]));
                    i++;
                }
            }
        }
    }
}
