using Blazor_App_With_Auth.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Blazor_App_With_Auth.Api.Controllers
{
    // TODO [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("User/AddRole")]
        public async Task<IActionResult> AddRole([FromBody] NewRoleForm newRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _roleManager.RoleExistsAsync(newRole.NewRole))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = newRole.NewRole });
            }
            /* Debug */
            foreach (var x in _roleManager.Roles)
                Console.WriteLine($"Role: {x.Id} {x.Name}");
            foreach (var x in User.Claims)
                Console.WriteLine($"Claim: {x.Type} {x.Value} {x.ValueType}");

            /*
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(id);
            */
            var user = await _userManager.FindByNameAsync(newRole.UserName);
            await _userManager.AddToRoleAsync(user, newRole.NewRole);

            return Ok();
        }
    }

    public class NewRoleForm
    {
        public string NewRole { get; set; }
        public string UserName { get; set; }
    }
}
