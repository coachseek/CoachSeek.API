using System;
using CoachSeek.Data.Model;
using NUnit.Framework;

namespace CoachSeek.Data.Tests.Unit
{
    [TestFixture]
    public class ErrorDataTests
    {
        [Test]
        public void GivenFieldIsNull_WhenConstruct_ThenFieldIsNull()
        {
            var properties = GivenFieldIsNull();
            var error = WhenConstruct(properties);
            ThenFieldIsNull(error);
        }

        [Test]
        public void GivenSimpleCamelCaseProperty_WhenConstruct_ThenFieldIsSimpleCamelCaseProperty()
        {
            var properties = GivenSimpleCamelCaseProperty();
            var error = WhenConstruct(properties);
            ThenFieldIsSimpleCamelCaseProperty(error);
        }

        [Test]
        public void GivenSimplePascalCaseProperty_WhenConstruct_ThenFieldIsSimpleCamelCaseProperty()
        {
            var properties = GivenSimplePascalCaseProperty();
            var error = WhenConstruct(properties);
            ThenFieldIsSimpleCamelCaseProperty(error);
        }

        [Test]
        public void GivenComplexPascalCaseProperty_WhenConstruct_ThenFieldIsComplexCamelCaseProperty()
        {
            var properties = GivenComplexPascalCaseProperty();
            var error = WhenConstruct(properties);
            ThenFieldIsComplexCamelCaseProperty(error);
        }

        [Test]
        public void GivenMessageIsNull_WhenConstruct_ThenMessageIsNull()
        {
            var properties = GivenMessageIsNull();
            var error = WhenConstruct(properties);
            ThenMessageIsNull(error);
        }

        [Test]
        public void GivenMessageDoesNotContainField_WhenConstruct_ThenMessageNotAltered()
        {
            var properties = GivenMessageDoesNotContainField();
            var error = WhenConstruct(properties);
            ThenMessageNotAltered(error);
        }

        [Test]
        public void GivenMessageContainsCamelCaseField_WhenConstruct_ThenMessageIsNotAltered()
        {
            var properties = GivenMessageContainsCamelCaseField();
            var error = WhenConstruct(properties);
            ThenMessageIsNotAltered(error);
        }

        [Test]
        public void GivenMessageContainsPascalCaseField_WhenConstruct_ThenFieldInMessageIsChangedToCamelCase()
        {
            var properties = GivenMessageContainsPascalCaseField();
            var error = WhenConstruct(properties);
            ThenFieldInMessageIsChangedToCamelCase(error);
        }

        [Test]
        public void GivenMessageContainsPascalCaseSubField_WhenConstruct_ThenSubFieldInMessageIsChangedToCamelCase()
        {
            var properties = GivenMessageContainsPascalCaseSubField();
            var error = WhenConstruct(properties);
            ThenSubFieldInMessageIsChangedToCamelCase(error);
        }


        private Tuple<string, string> GivenFieldIsNull()
        {
            return new Tuple<string, string>(null, "some message");
        }

        private Tuple<string, string> GivenSimpleCamelCaseProperty()
        {
            return new Tuple<string, string>("businessName", "some message");
        }

        private Tuple<string, string> GivenSimplePascalCaseProperty()
        {
            return new Tuple<string, string>("BusinessName", "some message");
        }

        private Tuple<string, string> GivenComplexPascalCaseProperty()
        {
            return new Tuple<string, string>("BusinessRegistration.BusinessObject.Registrant.FirstName", "some message");
        }

        private Tuple<string, string> GivenMessageIsNull()
        {
            return new Tuple<string, string>("businessName", null);
        }

        private Tuple<string, string> GivenMessageDoesNotContainField()
        {
            return new Tuple<string, string>("businessName", "Some error has occurred!");
        }

        private Tuple<string, string> GivenMessageContainsCamelCaseField()
        {
            return new Tuple<string, string>("businessName", "The businessName is required.");
        }

        private Tuple<string, string> GivenMessageContainsPascalCaseField()
        {
            return new Tuple<string, string>("FirstName", "The FirstName is required.");
        }

        private Tuple<string, string> GivenMessageContainsPascalCaseSubField()
        {
            return new Tuple<string, string>("businessRegistration.Registrant.LastName", "The LastName is required.");
        }


        private ErrorData WhenConstruct(Tuple<string, string> properties)
        {
            return new ErrorData(properties.Item1, properties.Item2);
        }


        private void ThenFieldIsNull(ErrorData error)
        {
            Assert.That(error.Field, Is.Null);
            // .. but the message was not null so should retain it's original value.
            Assert.That(error.Message, Is.EqualTo("some message"));
        }

        private void ThenFieldIsSimpleCamelCaseProperty(ErrorData error)
        {
            Assert.That(error.Field, Is.EqualTo("businessName"));
        }

        private void ThenFieldIsComplexCamelCaseProperty(ErrorData error)
        {
            Assert.That(error.Field, Is.EqualTo("businessRegistration.businessObject.registrant.firstName"));
        }

        private void ThenMessageIsNull(ErrorData error)
        {
            Assert.That(error.Message, Is.Null);
        }

        private void ThenMessageNotAltered(ErrorData error)
        {
            Assert.That(error.Message, Is.EqualTo("Some error has occurred!"));
        }

        private void ThenMessageIsNotAltered(ErrorData error)
        {
            Assert.That(error.Message, Is.EqualTo("The businessName is required."));
        }

        private void ThenFieldInMessageIsChangedToCamelCase(ErrorData error)
        {
            Assert.That(error.Message, Is.EqualTo("The firstName is required."));
        }

        private void ThenSubFieldInMessageIsChangedToCamelCase(ErrorData error)
        {
            Assert.That(error.Field, Is.EqualTo("businessRegistration.registrant.lastName"));
            Assert.That(error.Message, Is.EqualTo("The lastName is required."));
        }
    }
}
