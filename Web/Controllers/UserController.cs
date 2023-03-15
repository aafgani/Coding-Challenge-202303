﻿using Infrastructure.Factory;
using Infrastructure.HttpClientService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Model.AppConfig;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IWebCacheServiceClient serviceClient;
        private readonly WebCacheServiceConfiguration config;

        public UserController(IWebCacheServiceClient serviceClient, IOptions<WebCacheServiceConfiguration> options)
        {
            this.serviceClient = serviceClient;
            this.config = options.Value;
        }

        public IActionResult Index()
        {
            ViewBag.BaseURL = config.BaseURL;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Generate([FromBody] User user)
        {
            List<Task> tasksCreateUser = new List<Task>();
            List<int> sentUser = new List<int>();
            for (int i = 0; i < user.Number; i++)
            {
                var generatedUser = UserFactory.GenerateUser();
                tasksCreateUser.Add(SendUser(generatedUser));
                sentUser.Add(generatedUser.Number);
            }
            Task.WaitAll(tasksCreateUser.ToArray());

            return Json(user);
        }

        private Task SendUser(User user)
        {
            var TCS = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            try
            {
                Random rnd = new Random();
                int min = 1000, max = 5000;
                Thread.Sleep(rnd.Next(min, max));
                serviceClient.CreateUser(UserFactory.GenerateUser());

                TCS.SetResult();
            }
            catch (Exception e)
            {
                TCS.SetException(new ApplicationException(e.Message));
            }
            return TCS.Task;
        }
    }
}
