
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.DataAccessComponents.Interface.Session
{
    /// <summary>
    /// Represents a Session data row.
    /// </summary>
    public class SessionDataRow
    {
        public int SessionID;
        public string SessionCode;
        public string Name;
        public DateTime StartDate;
    }
}
