
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
            // Get the invalid model state keys.
            string[] invalidModelStateKeys = modelStateDictionary
                .Where(modelStateEntry => modelStateEntry.Value.Errors.Any())
                .Select(modelStateEntry => modelStateEntry.Key)
                .ToArray();

            // The invalid model state keys contain type names.
            // We are only interested in their properties.
            string[] invalidFields = invalidModelStateKeys
                .Select(invalidModelStateKey => invalidModelStateKey.Split('.').Last())
                .ToArray();

            // Camel case the invalid fields.
            string[] camelCasedInvalidFields = invalidFields
                .Select(invalidField => invalidField.Substring(0, 1).ToLower() + invalidField.Substring(1))
                .ToArray();

            // Return the invalid fields.
            return camelCasedInvalidFields;
        }
    }
}
