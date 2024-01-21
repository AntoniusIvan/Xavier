using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JIRMDataManager.Library.Models;
using JIRMDataManager.Library.DataAccess;
using System.Security.Claims;
using JIRMApi.Data;
using JIRMApi.Models;

namespace JIRMApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize]
  public class UserController : ControllerBase
  {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserData _userData;
    private readonly ILogger<UserController> _logger;

    public UserController(ApplicationDbContext context
      , UserManager<IdentityUser> userManager
      , IUserData userData
      , ILogger<UserController> logger)
    {
      _context = context;
      _userManager = userManager;
      _userData = userData;
      _logger = logger;
    }
    [HttpGet] //Get INformation fo who they are. Just Get information about themself
    public async Task<ActionResult<UserModel>> GetById()
    {
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//RequestContext.Principal.Identity.GetUserId();
            ////UserData data = new UserData(_config);

            //return (await _userData.GetUserById(userId)).First();

            try
            {
                //string userId = User.Claims.FirstOrDefault(c => c.Type == "shttp://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

                //string 
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    // Handle the case where user ID is not available in claims
                    return BadRequest("User ID not found in claims.");
                }

                var users = await _userData.GetUserById(userId);

                if (users == null || !users.Any())
                {
                    // Handle the case where the user with the specified ID is not found
                    return NotFound("User not found");
                }

                return users.First();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [RouteAttribute("Admin/GetAllUsers")]
    public List<ApplicationUserModel> GetAllUsers()
    {
      List<ApplicationUserModel> output = new List<ApplicationUserModel>();

      var users = _context.Users.ToList();
      var userRoles = from ur in _context.UserRoles
                      join r in _context.Roles on ur.RoleId equals r.Id
                      select new { ur.UserId, ur.RoleId, r.Name };

      foreach (var user in users)
      {
        ApplicationUserModel u = new ApplicationUserModel
        {
          Id = user.Id,
          Email = user.Email
        };

        u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(Key => Key.RoleId, val => val.Name);

        output.Add(u);
      }

      return output;
      //}
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    [RouteAttribute("Admin/GetAllRoles")]
    public Dictionary<string, string> GetAllRoles()
    {
      //using (var context = new ApplicationDbContext())
      //{
      var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);

      return roles;
      //}
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [RouteAttribute("Admin/AddRole")]
    public async Task AddARole(UserRolePairModel pairing)
    {
      string loggedInuserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      var user = await _userManager.FindByIdAsync(pairing.UserId);

      _logger.LogInformation("Admin {Admin} added user {User} to role {Role}"
        , loggedInuserId, user.Id, pairing.RoleName); //so we can puull with serilog

      await _userManager.AddToRoleAsync(user, pairing.RoleName);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [RouteAttribute("Admin/RemoveRole")]
    public async Task RemoveARole(UserRolePairModel pairing)
    {
      string loggedInuserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      var user = await _userManager.FindByIdAsync(pairing.UserId);

      _logger.LogInformation("Admin {Admin} remove user {User} from role {Role}"
  , loggedInuserId, user.Id, pairing.RoleName); //so we can puull with serilog

      await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);


    }
  }
}
