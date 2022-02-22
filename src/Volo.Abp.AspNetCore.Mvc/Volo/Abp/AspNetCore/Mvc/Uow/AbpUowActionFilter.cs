﻿using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    public class AbpUowActionFilter : IAsyncActionFilter, ITransientDependency
    {
        public const string UnitOfWorkReservationName = "_AbpActionUnitOfWork";

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly UnitOfWorkDefaultOptions _defaultOptions;

        public AbpUowActionFilter(IUnitOfWorkManager unitOfWorkManager, IOptions<UnitOfWorkDefaultOptions> options)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _defaultOptions = options.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                await next();
                return;
            }

            var methodInfo = context.ActionDescriptor.GetMethodInfo();
            var unitOfWorkAttr = UnitOfWorkHelper.GetUnitOfWorkAttributeOrNull(methodInfo);

            SetAbpActionInfoToHttpContext(context.HttpContext, methodInfo);

            if (unitOfWorkAttr?.IsDisabled == true)
            {
                await next();
                return;
            }

            var options = CreateOptions(context, unitOfWorkAttr);

            //Trying to begin a reserved UOW by AbpUnitOfWorkMiddleware
            if (_unitOfWorkManager.TryBeginReserved(UnitOfWorkReservationName, options))
            {
                var result = await next();
                if (!Succeed(result))
                {
                    await RollbackAsync(context);
                }

                return;
            }

            //Begin a new, independent unit of work
            using (var uow = _unitOfWorkManager.Begin(options))
            {
                var result = await next();
                if (Succeed(result))
                {
                    await uow.CompleteAsync(context.HttpContext.RequestAborted);
                }
            }
        }

        private static void SetAbpActionInfoToHttpContext(HttpContext context, MethodInfo methodInfo)
        {
            context.Items["_AbpActionInfo"] = new AbpActionInfoInHttpContext
            {
                IsObjectResult = ActionResultHelper.IsObjectResult(methodInfo.ReturnType)
            };
        }

        private UnitOfWorkOptions CreateOptions(ActionExecutingContext context, UnitOfWorkAttribute unitOfWorkAttribute)
        {
            var options = new UnitOfWorkOptions();

            unitOfWorkAttribute?.SetOptions(options);

            if (unitOfWorkAttribute?.IsTransactional == null)
            {
                options.IsTransactional = _defaultOptions.CalculateIsTransactional(
                    autoValue: !string.Equals(context.HttpContext.Request.Method, HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase)
                );
            }

            return options;
        }

        private async Task RollbackAsync(ActionExecutingContext context)
        {
            var currentUow = _unitOfWorkManager.Current;
            if (currentUow != null)
            {
                await currentUow.RollbackAsync(context.HttpContext.RequestAborted);
            }
        }

        private static bool Succeed(ActionExecutedContext result)
        {
            return result.Exception == null || result.ExceptionHandled;
        }
    }
}
