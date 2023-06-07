namespace RentReadyTechnicalAssessmentFn.src.Logic
{
    public class Consts
    {
        public const string EXPECTED_JSON_SCHEMA = @"{
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
}";
        public const int MSDYN_DURATION_MINUTES = 1440;
        public const string DYNAMICS_365_CONNECTION_STRING_VARIABLE_NAME = "CUSTOMCONNSTR_ConnectToDynamics365";
    }    
}
