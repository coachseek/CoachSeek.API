using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class ColourTests
    {
        [Test]
        public void DefaultColourTest()
        {
            Assert.That(Colour.Default, Is.EqualTo("green"));
        }

        [Test]
        public void TestColours()
        {
            ColourSuccess("orange", "orange");
            ColourSuccess("yellow", "yellow");
            ColourSuccess("red", "red");
            ColourSuccess("dark-red", "dark-red");
            ColourSuccess("blue", "blue");
            ColourSuccess("mid-blue", "mid-blue");
            ColourSuccess("dark-blue", "dark-blue");
            ColourSuccess("green", "green");
            ColourSuccess("mid-green", "mid-green");
            ColourSuccess("dark-green", "dark-green");

            ColourSuccess("OranGe", "orange");
            ColourSuccess("Dark-Red", "dark-red");
            ColourSuccess("   mid-Blue    ", "mid-blue");

            ColourFailure(null);
            ColourFailure("");
            ColourFailure("darkred");
            ColourFailure("midgreen");
            ColourFailure("aquamarine");
            ColourFailure("magenta");
            ColourFailure("prussian-blue");
        }


        private void ColourSuccess(string colourInput, string expectedOutput)
        {
            var colour = new Colour(colourInput);
            Assert.That(colour.Colouration, Is.EqualTo(expectedOutput));
        }

        private void ColourFailure(string colourInput)
        {
            object response;

            try
            {
                response = new Colour(colourInput);
            }
            catch (InvalidColour ex)
            {
                response = ex;
            }

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<InvalidColour>());
        }
    }
}
