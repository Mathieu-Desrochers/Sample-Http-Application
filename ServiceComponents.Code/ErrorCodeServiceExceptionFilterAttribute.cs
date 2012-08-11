
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
    /// Represents the ErrorCode service exception filter attribute.
    /// </summary>
    public class ErrorCodeServiceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        public override void OnException(HttpActionExecutedContext httpActionExecutedContext)
        {
            // Handle the ErrorCode service exceptions only.
            ErrorCodeServiceException errorCodeServiceException = httpActionExecutedContext.Exception as ErrorCodeServiceException;
            if (errorCodeServiceException == null)
            {
                return;
            }

            // Return an HTTP response message containing the error codes.
            HttpResponseMessage httpResponseMessage = httpActionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorCodeServiceException.ErrorCodes);
            httpActionExecutedContext.Response = httpResponseMessage;
        }
    }
}
