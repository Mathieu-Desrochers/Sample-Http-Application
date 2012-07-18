
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
            // Handle the InvalidFields service exceptions.
            InvalidFieldsServiceException invalidFieldsServiceException = httpActionExecutedContext.Exception as InvalidFieldsServiceException;
            if (invalidFieldsServiceException != null)
            {
                // Return an HTTP response message containing the invalid fields.
                HttpResponseMessage httpResponseMessage = httpActionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, invalidFieldsServiceException.Details);
                httpActionExecutedContext.Response = httpResponseMessage;
            }

            // Handle the ErrorCode service exceptions.
            ErrorCodeServiceException errorCodeServiceException = httpActionExecutedContext.Exception as ErrorCodeServiceException;
            if (errorCodeServiceException != null)
            {
                // Return an HTTP response message containing the error code.
                HttpResponseMessage httpResponseMessage = httpActionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorCodeServiceException.Details);
                httpActionExecutedContext.Response = httpResponseMessage;
            }
        }
    }
}
