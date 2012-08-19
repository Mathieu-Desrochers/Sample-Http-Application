
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
    /// Represents the service exception filter attribute.
    /// </summary>
    public class ServiceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        public override void OnException(HttpActionExecutedContext httpActionExecutedContext)
        {
            // Handle the service exceptions only.
            ServiceException serviceException = httpActionExecutedContext.Exception as ServiceException;
            if (serviceException == null)
            {
                return;
            }

            // Return an HTTP response message containing the errors.
            HttpResponseMessage httpResponseMessage = httpActionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, serviceException.Errors);
            httpActionExecutedContext.Response = httpResponseMessage;
        }
    }
}
