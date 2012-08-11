
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
    /// Represents the BadFormat service exception filter attribute.
    /// </summary>
    public class BadFormatServiceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        public override void OnException(HttpActionExecutedContext httpActionExecutedContext)
        {
            // Handle the BadFormat service exceptions only.
            BadFormatServiceException badFormatServiceException = httpActionExecutedContext.Exception as BadFormatServiceException;
            if (badFormatServiceException == null)
            {
                return;
            }

            // Return an HTTP response message containing the bad formats.
            HttpResponseMessage httpResponseMessage = httpActionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, badFormatServiceException.BadFormats);
            httpActionExecutedContext.Response = httpResponseMessage;
        }
    }
}
