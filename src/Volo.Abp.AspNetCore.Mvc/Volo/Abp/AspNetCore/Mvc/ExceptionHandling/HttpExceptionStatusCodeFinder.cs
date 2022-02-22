﻿using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class HttpExceptionStatusCodeFinder : ITransientDependency
    {
        public virtual int GetStatusCode(HttpContext httpContext, Exception exception)
        {
            if (exception is AbpAuthorizationException)
            {
                return httpContext.User.Identity.IsAuthenticated
                    ? (int)HttpStatusCode.Forbidden
                    : (int)HttpStatusCode.Unauthorized;
            }

            if (exception is AbpValidationException)
            {
                return (int)HttpStatusCode.BadRequest;
            }

            if (exception is EntityNotFoundException)
            {
                return (int)HttpStatusCode.NotFound;
            }

            return (int)HttpStatusCode.InternalServerError;
        }
    }
}