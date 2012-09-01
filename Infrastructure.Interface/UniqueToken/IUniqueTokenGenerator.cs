
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.Infrastructure.Interface.UniqueToken
{
    /// <summary>
    /// Represents the unique token generator.
    /// </summary>
    public interface IUniqueTokenGenerator
    {
        /// <summary>
        /// Generates a unique token.
        /// </summary>
        string GenerateUniqueToken();
    }
}
