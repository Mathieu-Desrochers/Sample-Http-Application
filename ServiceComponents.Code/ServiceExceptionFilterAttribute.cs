
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
    /// Represents the service exception filter attribute.
    /// </summary>
    public class ServiceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        public override void OnException(HttpActionExecutedContext httpActionExecutedContext)
        {
            // Handle service exceptions only.
            ServiceException serviceException = httpActionExecutedContext.Exception as ServiceException;
            if (serviceException == null)
            {
                return;
            }

            // Build the HTTP response message with the BadRequest status code.
            // Set the content to the service exception details.
            HttpResponseMessage httpResponseMessage = httpActionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, serviceException.Details);

            // Return the HTTP response message.
            httpActionExecutedContext.Response = httpResponseMessage;
        }
    }
}
