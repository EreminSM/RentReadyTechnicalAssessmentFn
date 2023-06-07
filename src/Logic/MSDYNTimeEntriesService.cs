using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentReadyTechnicalAssessmentFn.src.Logic
{
    internal class MSDYNTimeEntriesService
    {
        public MSDYNTimeEntriesService()
        {

        }
        public async Task<List<DateTime>> AddEntries(List<DateTime> dates)
        {
            var service = new ServiceClient(Environment.GetEnvironmentVariable("CUSTOMCONNSTR_ConnectToDynamics365"));

            if (!service.IsReady)
            {
                throw new Exception("Unable to connect to Dynamics 365");
            }

            var existedEntries = GetAlreadyExistedEntries(service, dates);            

            var result = new List<DateTime>();

            List<Task<Guid>> tasks = new();
            foreach (var item in dates.Except(existedEntries).ToList())
            {
                var entity = new Entity("msdyn_timeentry");
                entity["msdyn_date"] = item;
                entity["msdyn_duration"] = Consts.MSDYN_DURATION_HOURS;
                var task = service.CreateAsync(entity);
                tasks.Add(task);
                
                result.Add(item);
            }
            await Task.WhenAll(tasks);

            return result;
        }

        private static List<DateTime> GetAlreadyExistedEntries(ServiceClient service, List<DateTime> dateValues)
        {
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
            var result = new List<DateTime>();
            foreach (var entity in retrieveAnswer.Entities)
            {
                result.Add((DateTime)entity.Attributes["msdyn_date"]);
            }

            return result;
        }
    }
}
