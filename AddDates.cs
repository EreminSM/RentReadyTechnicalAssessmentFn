using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using System.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;

namespace RentReadyTechnicalAssessmentFn
{
    public static class AddDates
    {
        [FunctionName("AddDates")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string _clientId = "<some client ID>";
                string _clientSecret = "<some client Secret>";
                string _environment = "<some environment>";

                var _connectionString = @$"Url=https://{_environment}.dynamics.com;AuthType=ClientSecret;ClientId={_clientId}
                ;ClientSecret={_clientSecret};RequireNewInstance=true";

                var service = new ServiceClient(new Uri(@$"https://{_environment}.dynamics.com"), _clientId, _clientSecret, false);

                if (service.IsReady)
                {
                    DateTime dateValue = DateTime.Now.Date;
                    List<DateTime> dateValues = new List<DateTime>() { dateValue, dateValue.AddDays(1), dateValue.AddDays(2), dateValue.AddDays(3) };
                    log.LogInformation("C# HTTP trigger function processed a request.");
                    List<Task<Guid>> tasks = new List<Task<Guid>>();
                    List<string> dateValuesStr = new List<string>();
                    foreach(var item in dateValues)
                    {
                        Entity e = new Entity("msdyn_timeentry");
                        e["msdyn_date"] = item;
                        e["msdyn_duration"] = 5;
                        var task = service.CreateAsync(e);
                        tasks.Add(task);

                        dateValuesStr.Add(item.ToString("s"));
                    }

                    await Task.WhenAll(tasks);
                 
                    QueryExpression solutionQuery = new QueryExpression
                    {
                        EntityName = "msdyn_timeentry",
                        ColumnSet = new ColumnSet(new string[] { "msdyn_date", "msdyn_duration" }),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression("msdyn_date", ConditionOperator.In, dateValues)
                            }
                        }
                    };

                    var retrieveAnswer = service.RetrieveMultiple(solutionQuery);

                    QueryByAttribute queryCheckForSampleSolution = new QueryByAttribute();
                    queryCheckForSampleSolution.AddAttributeValue("msdyn_date", dateValue);

                    queryCheckForSampleSolution.EntityName = "msdyn_timeentry";

                    EntityCollection querySampleSolutionResults = service.RetrieveMultiple(queryCheckForSampleSolution);


                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    dynamic data = JsonConvert.DeserializeObject(requestBody);

                    JSchema schema = JSchema.Parse(@"{
  '$schema': 'http://json-schema.org/draft-04/schema#',
  'type': 'object',
  'properties': {
    'StartOn': {
      'type': 'string',
      'format': 'date'
    },
    'EndOn': {
      'type': 'string',
      'format': 'date'
    }
  },
  'required': [
    'StartOn',
    'EndOn'
  ]
}");

                    JObject user = JObject.Parse(requestBody);

                    bool valid = user.IsValid(schema);

                    string responseMessage = $"Hello. This HTTP triggered function executed successfully." + requestBody + "valid ? " + (valid ? "true" : "false")
                        + " retrieveAnswer: " + retrieveAnswer.Entities.Count.ToString();

                    return new OkObjectResult(responseMessage);
                }
                else
                {
                    return new OkObjectResult("Unable to connect to Dynamics 365");
                }
            }
            catch (Exception ex)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                return new OkObjectResult(ex.Message + "\r\n" + ex.StackTrace+"\r\n"+ requestBody);
            }
        }
    }
}
