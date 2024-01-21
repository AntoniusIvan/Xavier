using JIRMApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JIRMApi.Controllers
{
  public class TokenController : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public TokenController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    [Route("/token")]
    [HttpPost]
    public async Task<IActionResult> Create(string username, string password, string grant_type)
    {
      if (await IsValidUsernameAndPassword(username, password))
      {
        return new ObjectResult(await GenerateToken(username));
      }
      else
      {
        return BadRequest();
      }
    }
    private async Task<bool> IsValidUsernameAndPassword(string username, string password)
    {
      var user = await _userManager.FindByEmailAsync(username);
      return await _userManager.CheckPasswordAsync(user, password);
    }
    private async Task<dynamic> GenerateToken(string username)
    {
      var user = await _userManager.FindByEmailAsync(username);
      var roles = from ur in _context.UserRoles
                  join r in _context.Roles on ur.RoleId equals r.Id
                  where ur.UserId == user.Id
                  select new { ur.UserId, ur.RoleId, r.Name };

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()), //NBF is not before.
        new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()), //Exp is Expired
      };

      foreach (var role in roles)
      {
        claims.Add(new Claim(ClaimTypes.Role, role.Name));
      }

      var token = new JwtSecurityToken(     //Creating New Javascript Web Token
        new JwtHeader(                      //Header
          new SigningCredentials(           //Signing Credential we use Algorithm for signing it. (HMACSha256)
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aVerySecureAndLongKeyString1234567890")), //The Key we use using for sign it this encoder right here, it takes the string and convert it to UTF)
            SecurityAlgorithms.HmacSha256)),
        new JwtPayload(claims));

      var output = new
      {
        Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
        UserName = username                                                 //Email Address of User
      };

      return output;
    }
  }
}
