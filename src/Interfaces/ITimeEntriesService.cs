using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentReadyTechnicalAssessmentFn.src.Interfaces
{
    public interface ITimeEntriesService
    {
        /// <summary>
        /// Adds the entries
        /// </summary>        
        /// <returns>
        /// Count of actually added entries
        /// </returns>
        /// <param name="dates">date entries to be added</param>
        public Task<List<DateTime>> AddEntries(List<DateTime> dates);
    }
}
