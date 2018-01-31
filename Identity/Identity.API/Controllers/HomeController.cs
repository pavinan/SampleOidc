using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using SampleOidc.Identity.API.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace SampleOidc.Identity.API.Controllers
{

    public class HomeController : Controller
    {

        private readonly IIdentityServerInteractionService _interaction;

        public HomeController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Error", vm);
        }
    }

}