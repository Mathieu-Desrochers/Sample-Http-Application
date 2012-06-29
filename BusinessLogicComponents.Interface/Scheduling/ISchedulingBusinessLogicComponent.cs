
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.DataAccessComponents.Interface;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling
{
    /// <summary>
    /// Represents a Scheduling business logic component.
    /// </summary>
    public interface ISchedulingBusinessLogicComponent
    {
        /// <summary>
        /// Creates a new session.
        /// </summary>
        Task<NewSessionBusinessResponse> NewSession(IDatabaseConnection databaseConnection, NewSessionBusinessRequest businessRequest);
    }
}
