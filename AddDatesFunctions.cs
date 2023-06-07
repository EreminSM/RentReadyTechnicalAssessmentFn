using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using RentReadyTechnicalAssessmentFn.src.Logic;

namespace RentReadyTechnicalAssessmentFn
{
    public static class AddDatesFunctions
    {
        [FunctionName("AddDates")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                if (!new DatesRangeJSONValidator(requestBody).IsValid())
                {
                    log.LogError($"Invalid JSON body: {requestBody}");
                    return new BadRequestObjectResult("Invalid JSON body");
                }                

                List<DateTime> dateValues = new DatesInRangeCalculator(requestBody).GetDates();

                var addedEntries = await new MSDYNTimeEntriesAdder().AddEntries(dateValues);

                string responseMessage = $"This HTTP triggered function executed successfully. Added entries count: {addedEntries.Count}";
                log.LogInformation("C# HTTP trigger function processed a request.");

                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogCritical($"Exception occured: {ex.Message}, {ex.StackTrace}, {requestBody}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
