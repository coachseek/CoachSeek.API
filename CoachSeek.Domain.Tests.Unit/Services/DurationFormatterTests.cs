using CoachSeek.Domain.Services;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Services
{
    [TestFixture]
    public class DurationFormatterTests
    {
        [Test]
        public void FormatDurationTests()
        {
            AssertDurationFormat(5, "5 minutes");
            AssertDurationFormat(15, "15 minutes");
            AssertDurationFormat(30, "30 minutes");
            AssertDurationFormat(45, "45 minutes");
            AssertDurationFormat(60, "1 hour");
            AssertDurationFormat(65, "1 hour and 5 minutes");
            AssertDurationFormat(75, "1 hour and 15 minutes");
            AssertDurationFormat(90, "1 hour and 30 minutes");
            AssertDurationFormat(105, "1 hour and 45 minutes");
            AssertDurationFormat(120, "2 hours");
            AssertDurationFormat(135, "2 hours and 15 minutes");
            AssertDurationFormat(150, "2 hours and 30 minutes");
            AssertDurationFormat(165, "2 hours and 45 minutes");
            AssertDurationFormat(180, "3 hours");
            AssertDurationFormat(195, "3 hours and 15 minutes");
            AssertDurationFormat(210, "3 hours and 30 minutes");
            AssertDurationFormat(225, "3 hours and 45 minutes");
            AssertDurationFormat(240, "4 hours");
            AssertDurationFormat(360, "6 hours");
            AssertDurationFormat(1440, "24 hours");
        }


        private void AssertDurationFormat(int duration, string expectedFormat)
        {
            Assert.That(DurationFormatter.Format(duration), Is.EqualTo(expectedFormat));
        }
    }
}
