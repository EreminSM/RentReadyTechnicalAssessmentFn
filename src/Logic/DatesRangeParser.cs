using Newtonsoft.Json;
using RentReadyTechnicalAssessmentFn.src.Models;
using System;
using System.Collections.Generic;

namespace RentReadyTechnicalAssessmentFn.src.Logic
{
    public class DatesRangeParser
    {
        private readonly string _json = "";
        public DatesRangeParser(string json)
        {
            _json = json;
        }

        public List<DateTime> TryParse()
        {
            var datesRange = JsonConvert.DeserializeObject<DatesRangeDto>(_json);
            var startDate = DateTime.Parse(datesRange.StartOn).Date;
            var endDate = DateTime.Parse(datesRange.EndOn).Date;
            if (endDate < startDate)
            {
                throw new ArgumentException();
            }

            var result = new List<DateTime>();
            for (var day = startDate.Date; day <= endDate; day = day.AddDays(1))
            {
                result.Add(day);
            }

            return result;
        }
    }
}
