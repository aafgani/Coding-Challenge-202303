using Infrastructure.Factory;
using Infrastructure.HttpClientService;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IWebCacheServiceClient serviceClient;

        public UserController(IWebCacheServiceClient serviceClient)
        {
            this.serviceClient = serviceClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            var t = await serviceClient.CreateUser(user);
            List<Task> tasksCreateUser = new List<Task>();
            for (int i = 0; i < user.Number; i++)
            {
                tasksCreateUser.Add(serviceClient.CreateUser(UserFactory.GenerateUser()));
            }
            Task.WaitAll(tasksCreateUser.ToArray());

            return RedirectToAction("Index");
        }
    }
}
