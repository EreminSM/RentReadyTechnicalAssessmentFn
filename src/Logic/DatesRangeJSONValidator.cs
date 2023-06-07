using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace RentReadyTechnicalAssessmentFn.src.Logic
{
    public class DatesRangeJSONValidator
    {
        private string _json = "";
        private JSchema _expectedSchema = JSchema.Parse(Consts.EXPECTED_JSON_SCHEMA);
        public DatesRangeJSONValidator(string json)
        {
            _json = json;
        }

        /// <summary>Checks if JSON schema provided at constructor is valid</summary>
        public bool IsValid()
        {
            try
            {
                return JObject.Parse(_json).IsValid(_expectedSchema);
            } catch
            {
                return false;
            }
        }
    }
}
