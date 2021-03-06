﻿using System.Threading.Tasks;
using FlatMate.Module.Account.Services;
using FlatMate.Web.Areas.Account.Data;
using FlatMate.Web.Common.Base;
using Microsoft.AspNetCore.Mvc;
using prayzzz.Common.Mvc.Filters;

namespace FlatMate.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class RegisterController : MvcController
    {
        private readonly IRegisterService _service;

        public RegisterController(IRegisterService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidationFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            var result = await _service.CreateUserAsync(model.User);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("Error", result.ErrorMessage);
                return View();
            }

            return LocalRedirectPermanent("/Dashboard");
        }
    }
}