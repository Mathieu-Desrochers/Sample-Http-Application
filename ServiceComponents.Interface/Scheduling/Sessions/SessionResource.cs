﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.ServiceComponents.Interface.Scheduling.Sessions
{
    /// <summary>
    /// Represents the Session resource.
    /// </summary>
    public class SessionResource
    {
        /// <summary>
        /// Gets or sets the SessionID.
        /// </summary>
        public int SessionID { get; set; }

        /// <summary>
        /// Gets or sets the SessionCode.
        /// </summary>
        public string SessionCode { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the StartDate.
        /// </summary>
        public DateTime StartDate { get; set; }
    }
}
