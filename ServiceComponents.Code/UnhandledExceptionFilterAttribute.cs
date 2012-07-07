
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

using SampleHttpApplication.ServiceComponents.Interface;

namespace SampleHttpApplication.ServiceComponents.Code
{
    /// <summary>
    /// Represents the unhandled exception filter attribute.
    /// </summary>
    public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        public override void OnException(HttpActionExecutedContext httpActionExecutedContext)
        {
            // Service exceptions have their own exception filter.
            if (httpActionExecutedContext.Exception is ServiceException)
            {
                return;
            }

            // Build the HTTP response message with the InternalServerError status code.
            HttpResponseMessage httpResponseMessage = httpActionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError);

            // Return the HTTP response message.
            httpActionExecutedContext.Response = httpResponseMessage;
        }
    }
}
