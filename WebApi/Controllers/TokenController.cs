using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        //[HttpPost]
        //public async Task<IActionResult> GetToken([FromBody] GetUsersByLoginAndPassword model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.Username);

        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        //    if (result.Succeeded)
        //    {
        //        // Tutaj możesz sprawdzić, czy użytkownik jest administratorem
        //        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        //        // Generuj token JWT
        //        var token = GenerateJwtToken(user, isAdmin);

        //        return Ok(new { Token = token });
        //    }

        //    return Unauthorized();
        //}

        //private string GenerateJwtToken(ApplicationUser user, bool isAdmin)
        //{
        //    // Tu implementuj logikę generowania JWT tokena
        //    // Możesz skorzystać z System.IdentityModel.Tokens.Jwt lub Microsoft.IdentityModel.Tokens
        //    // Na przykład, utworzenie SecurityTokenDescriptor, który opisuje, jak ma być zbudowany token
        //    // i użyć JwtSecurityTokenHandler do jego utworzenia.
        //    // Zwróć gotowy token jako string.
        //}
    }
}
