using NUnit.Framework;
using RentReadyTechnicalAssessmentFn.src.Logic;

namespace RentReadyTechnicalAssessmentFn.tests.Logic
{
    [TestFixture]
    internal class DatesRangeJSONValidatorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TryParse_ReturnsFalseForEmptyJSON()
        {
            var json = "";
            var expected = false;

            var validator = new DatesRangeJSONValidator(json);
            var actual = validator.IsValid();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TryParse_ReturnsTrueForValidJSON()
        {
            var json = @"{
  ""StartOn"": ""2020-07-03"",
  ""EndOn"": ""2020-02-19""
}";
            var expected = true;

            var validator = new DatesRangeJSONValidator(json);
            var actual = validator.IsValid();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TryParse_ReturnsFalseForInvalidJSON()
        {
            var json = @"{
  ""Start"": ""2020-07-03"",
  ""End"": ""2020-02-19""
}";
            var expected = false;

            var validator = new DatesRangeJSONValidator(json);
            var actual = validator.IsValid();

            Assert.AreEqual(expected, actual);
        }
    }
}
