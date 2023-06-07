using NUnit.Framework;
using RentReadyTechnicalAssessmentFn.src.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentReadyTechnicalAssessmentFn.tests.Logic
{
    [TestFixture]
    public class DatesRangeParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TryParse_ThrowsAnExceptionWhenEndDateIsLessThanStartDate()
        {
            var json = @"{
  ""StartOn"": ""2020-07-03"",
  ""EndOn"": ""2020-02-19""
}";

            DatesRangeParser parser = new DatesRangeParser(json);

            Assert.Throws<ArgumentException>(() => parser.TryParse());
        }

        [Test]
        public void TryParse_ReturnsExpectedDates_001()
        {
            var json = @"{
  ""StartOn"": ""2020-07-01"",
  ""EndOn"": ""2020-07-01""
}";
            var expected = new List<DateTime>() { DateTime.Parse("2020-07-01") };

            DatesRangeParser parser = new DatesRangeParser(json);
            var actual = parser.TryParse();

            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [Test]
        public void TryParse_ReturnsExpectedDates_002()
        {
            var json = @"{
  ""StartOn"": ""2020-07-01"",
  ""EndOn"": ""2020-07-10""
}";
            var expected = new List<DateTime>() 
            { 
                DateTime.Parse("2020-07-01"),
                DateTime.Parse("2020-07-02"),
                DateTime.Parse("2020-07-03"),
                DateTime.Parse("2020-07-04"),
                DateTime.Parse("2020-07-05"),
                DateTime.Parse("2020-07-06"),
                DateTime.Parse("2020-07-07"),
                DateTime.Parse("2020-07-08"),
                DateTime.Parse("2020-07-09"),
                DateTime.Parse("2020-07-10"),
            };

            DatesRangeParser parser = new DatesRangeParser(json);
            var actual = parser.TryParse();

            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [Test]
        public void TryParse_ReturnsExpectedDates_003()
        {
            var json = @"{
  ""StartOn"": ""2020-02-28"",
  ""EndOn"": ""2020-03-01""
}";
            var expected = new List<DateTime>()
            {
                DateTime.Parse("2020-02-28"),
                DateTime.Parse("2020-02-29"),
                DateTime.Parse("2020-03-01"),
            };

            DatesRangeParser parser = new DatesRangeParser(json);
            var actual = parser.TryParse();

            Assert.IsTrue(actual.SequenceEqual(expected));
        }
    }
}
