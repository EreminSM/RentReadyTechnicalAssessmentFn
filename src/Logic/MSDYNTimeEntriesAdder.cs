using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentReadyTechnicalAssessmentFn.src.Logic
{
    internal class MSDYNTimeEntriesAdder
    {
        private const string MSDYN_ENTITY_NAME = "msdyn_timeentry";
        private const string MSDYN_START_COLUMN = "msdyn_start";
        private const string MSDYN_DURATION_COLUMN = "msdyn_duration";
        public async Task<List<DateTime>> AddEntries(List<DateTime> dates)
        {
            var service = new ServiceClient(Environment.GetEnvironmentVariable(Consts.DYNAMICS_365_CONNECTION_STRING_VARIABLE_NAME));

            if (!service.IsReady)
            {
                throw new Exception("Unable to connect to Dynamics 365");
            }

            var existedEntries = GetAlreadyExistedEntries(service, dates);            

            var result = new List<DateTime>();

            List<Task<Guid>> tasks = new();
            foreach (var item in dates.Except(existedEntries).ToList())
            {
                var entity = new Entity(MSDYN_ENTITY_NAME);
                entity[MSDYN_START_COLUMN] = item.Date;
                entity[MSDYN_DURATION_COLUMN] = Consts.MSDYN_DURATION_MINUTES;
                var task = service.CreateAsync(entity);
                tasks.Add(task);
                
                result.Add(item);
            }
            await Task.WhenAll(tasks);

            return result;
        }

        private static List<DateTime> GetAlreadyExistedEntries(ServiceClient service, List<DateTime> dateValues)
        {
            QueryExpression solutionQuery = new()
            {
                EntityName = MSDYN_ENTITY_NAME,
                ColumnSet = new ColumnSet(new string[] { MSDYN_START_COLUMN }),
                Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(MSDYN_START_COLUMN, ConditionOperator.In, dateValues)
                            }
                        }
            };

            var retrieveAnswer = service.RetrieveMultiple(solutionQuery);
            var result = new List<DateTime>();
            foreach (var entity in retrieveAnswer.Entities)
            {
                result.Add((DateTime)entity.Attributes[MSDYN_START_COLUMN]);
            }

            return result;
        }
    }
}
