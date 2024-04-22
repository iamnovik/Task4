using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task4_1.Models;

namespace task4_1.Controllers;
[TypeFilter(typeof(CheckBlockedUserFilter))]
[Authorize]
public class UsersController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private SignInManager<AppUser> _signInManager;

    public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }
    
    [HttpPost]
    public IActionResult BlockUsers(List<string> userIds)
    {

        foreach (var userId in userIds)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user != null)
            {
                user.IsBlocked = true;
                _userManager.UpdateAsync(user).Wait();
            }
        }

        return Ok();
       
    }
    
    [HttpPost]
    public IActionResult UnblockUsers(List<string> userIds)
    {

        foreach (var userId in userIds)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user != null)
            {
                user.IsBlocked = false;
                _userManager.UpdateAsync(user).Wait();
            }
        }

        return Ok();
       
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUsers(List<string> userIds)
    {
        bool isDeletedHimself = false;

        foreach (var userId in userIds)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user != null)
            {
                _userManager.DeleteAsync(user).Wait();
                if (User.Identity.Name == user.UserName)
                {
                    isDeletedHimself = true;
                }
            }
        }

        if (isDeletedHimself)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        return Ok();
        
        
    }
}
