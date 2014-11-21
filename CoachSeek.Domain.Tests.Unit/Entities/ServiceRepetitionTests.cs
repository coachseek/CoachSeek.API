﻿using System;
using CoachSeek.Data.Model;
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
            ServiceRepetitionCreationFailure("x", 8, new[,] { { "The repeatFrequency field is not valid.", "service.repetition.repeatFrequency" } });
            ServiceRepetitionCreationFailure("d", -2, new[,] { { "The sessionCount field is not valid.", "service.repetition.sessionCount" } });
            ServiceRepetitionCreationFailure("z", -6, new[,] { { "The sessionCount field is not valid.", "service.repetition.sessionCount" },
                                                               { "The repeatFrequency field is not valid.", "service.repetition.repeatFrequency" } });
            ServiceRepetitionCreationFailure("d", 1, new[,] { { "For a single session the repeatFrequency must not be set.", "service.repetition.repeatFrequency" } });
            ServiceRepetitionCreationFailure(null, 12, new[,] { { "For a repeated session the repeatFrequency must be set.", "service.repetition.repeatFrequency" } });
            ServiceRepetitionCreationFailure(null, -1, new[,] { { "For a repeated session the repeatFrequency must be set.", "service.repetition.repeatFrequency" } });


            ServiceRepetitionCreationSuccess(null, 1, false);   // Single session
            ServiceRepetitionCreationSuccess("w", -1, true);    // SessionCount of -1 is for open-ended course.
            ServiceRepetitionCreationSuccess("d", 5, false);
        }


        private void ServiceRepetitionCreationSuccess(string repeatFrequnecy, int sessionCount, bool isOpenEnded)
        {
            var repetition = new ServiceRepetition(new RepetitionData { RepeatFrequency = repeatFrequnecy, SessionCount = sessionCount });
            Assert.That(repetition, Is.Not.Null);
            Assert.That(repetition.RepeatFrequency, Is.EqualTo(repeatFrequnecy));
            Assert.That(repetition.SessionCount, Is.EqualTo(sessionCount));
            Assert.That(repetition.IsOpenEnded, Is.EqualTo(isOpenEnded));
        }

        private void ServiceRepetitionCreationFailure(string repeatFrequnecy, int sessionCount, string[,] expectedErrors)
        {
            try
            {
                var repetition = new ServiceRepetition(new RepetitionData { RepeatFrequency = repeatFrequnecy, SessionCount = sessionCount });
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<ValidationException>());
                var errors = ((ValidationException) ex).Errors;
                Assert.That(errors.Count, Is.EqualTo(expectedErrors.GetLength(0)));

                var i = 0;
                foreach (var error in errors)
                {
                    Assert.That(error.Field, Is.EqualTo(expectedErrors[i, 1]));
                    Assert.That(error.Message, Is.EqualTo(expectedErrors[i, 0]));
                    i++;
                }
            }
        }
    }
}
