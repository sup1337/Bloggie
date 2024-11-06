using Bloggie.web.Models.ViewModels;
using Bloggie.web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.web.Controllers;
[Authorize (Roles = "Admin")]
public class AdminUsersController : Controller
{
    private readonly IUserRepository _userRepository;

    public AdminUsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    // GET
    public async Task<IActionResult> List()
    {
        var users = await _userRepository.GetAll();
        
        var usersViewModel = new UserViewModel();
        usersViewModel.Users = new List<User>();
        foreach (var user in users)
        {
            usersViewModel.Users.Add(new User
            {
                Id = Guid.Parse(user.Id),
                UserName = user.UserName,
                Email = user.Email
            });
        }
        
        return View(usersViewModel);
    }
}