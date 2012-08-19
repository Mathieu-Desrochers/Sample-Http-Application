
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

using SampleHttpApplication.ServiceFacadeComponents.Interface;

namespace SampleHttpApplication.ServiceFacadeComponents.Code
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
            // Service exceptions are handled by their own filter.
            ServiceException serviceException = httpActionExecutedContext.Exception as ServiceException;
            if (serviceException != null)
            {
                return;
            }

            // Return an HTTP response message with the InternalServerError status code.
            HttpResponseMessage httpResponseMessage = httpActionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError);
            httpActionExecutedContext.Response = httpResponseMessage;
        }
    }
}
