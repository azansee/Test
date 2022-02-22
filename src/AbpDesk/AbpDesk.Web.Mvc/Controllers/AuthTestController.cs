﻿using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpDesk.Web.Mvc.Controllers
{
    public class AuthTestController : AbpController
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthTestController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [Authorize(Policy = "RequirePhoneNumber")]
        public ContentResult PhoneNumberIsRequired()
        {
            return Content("OK: " + User.Claims.First(c => c.Type == "phone_number")?.Value);
        }
    }
}
