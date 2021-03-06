﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.GetSessions;
using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewCourseSchedule;
using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewSession;
using SampleHttpApplication.DataAccessComponents.Interface;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling
{
    /// <summary>
    /// Represents the Scheduling business logic component.
    /// </summary>
    public interface ISchedulingBusinessLogicComponent
    {
        /// <summary>
        /// Returns all the sessions.
        /// </summary>
        Task<GetSessionsBusinessResponse> GetSessions(IDatabaseConnection databaseConnection, GetSessionsBusinessRequest businessRequest);

        /// <summary>
        /// Creates a new course schedule.
        /// </summary>
        Task<NewCourseScheduleBusinessResponse> NewCourseSchedule(IDatabaseConnection databaseConnection, NewCourseScheduleBusinessRequest businessRequest);

        /// <summary>
        /// Creates a new session.
        /// </summary>
        Task<NewSessionBusinessResponse> NewSession(IDatabaseConnection databaseConnection, NewSessionBusinessRequest businessRequest);
    }
}
