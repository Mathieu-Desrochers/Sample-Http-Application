
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace SampleHttpApplication.Infrastructure.Code.Http
{
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// Returns the fields marked with at least one error.
        /// </summary>
        public static string[] GetInvalidFields(this ModelStateDictionary modelStateDictionary)
        {
            // Get the invalid fields.
            string[] invalidFields = modelStateDictionary
                .Where(modelStateEntry => modelStateEntry.Value.Errors.Any())
                .Select(modelStateEntry => modelStateEntry.Key)
                .ToArray();

            // Return the invalid fields.
            return invalidFields;
        }
    }
}
