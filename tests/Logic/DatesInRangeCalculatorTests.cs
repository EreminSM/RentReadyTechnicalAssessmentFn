using NUnit.Framework;
using RentReadyTechnicalAssessmentFn.src.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentReadyTechnicalAssessmentFn.tests.Logic
{
    [TestFixture]
    public class DatesInRangeCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetDates_ThrowsAnExceptionWhenEndDateIsLessThanStartDate()
        {
            var json = @"{
  ""StartOn"": ""2020-07-03"",
  ""EndOn"": ""2020-02-19""
}";

            DatesInRangeCalculator parser = new DatesInRangeCalculator(json);

            Assert.Throws<ArgumentException>(() => parser.GetDates());
        }

        [Test]
        public void GetDates_ThrowsAnExceptionWhenEndDateEqualsToStartDate()
        {
            var json = @"{
  ""StartOn"": ""2020-07-01"",
  ""EndOn"": ""2020-07-01""
}";
            var expected = new List<DateTime>() { DateTime.Parse("2020-07-01") };

            DatesInRangeCalculator parser = new DatesInRangeCalculator(json);

            Assert.Throws<ArgumentException>(() => parser.GetDates());
        }

        [Test]
        public void GetDates_ReturnsExpectedDates_001()
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
            };

            DatesInRangeCalculator parser = new DatesInRangeCalculator(json);
            var actual = parser.GetDates();

            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [Test]
        public void GetDates_ReturnsExpectedDates_002()
        {
            var json = @"{
  ""StartOn"": ""2020-02-28"",
  ""EndOn"": ""2020-03-02""
}";
            var expected = new List<DateTime>()
            {
                DateTime.Parse("2020-02-28"),
                DateTime.Parse("2020-02-29"),
                DateTime.Parse("2020-03-01"),
            };

            DatesInRangeCalculator parser = new DatesInRangeCalculator(json);
            var actual = parser.GetDates();

            Assert.IsTrue(actual.SequenceEqual(expected));
        }
    }
}
