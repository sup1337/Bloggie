﻿using Bloggie.web.Models.ViewModels;
using Bloggie.web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.web.Controllers;
   [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller

    {

        private readonly IUserRepository userRepository;

        private readonly UserManager<IdentityUser> userManager;



        public AdminUsersController(IUserRepository userRepository,

            UserManager<IdentityUser> userManager)

        {

            this.userRepository = userRepository;

            this.userManager = userManager;

        }



        [HttpGet]

        public async Task<IActionResult> List()

        {

            var users = await userRepository.GetAll();



            var usersViewModel = new UserViewModel();

            usersViewModel.Users = new List<User>();



            foreach (var user in users)

            {

                usersViewModel.Users.Add(new Models.ViewModels.User

                {

                    Id = Guid.Parse(user.Id),

                    UserName = user.UserName,

                    Email = user.Email

                });

            }



            return View(usersViewModel);

        }



   



        [HttpPost]

        public async Task<IActionResult> List(UserViewModel request)

        {

            var identityUser1 = new IdentityUser

            {

                UserName = request.Username,

                Email = request.Email

            };





            var identityResult =

                await userManager.CreateAsync(identityUser1, request.Password);



            System.Diagnostics.Debug.WriteLine("This will be displayed in output window");



            if (identityResult is not null)

            {

                if (identityResult.Succeeded)

                {

                    // assign roles to this user

                    var roles = new List<string> { "User" };



                    if (request.AdminRoleCheckbox)

                    {

                        roles.Add("Admin");

                    }



                    identityResult =

                        await userManager.AddToRolesAsync(identityUser1, roles);



                    if (identityResult is not null && identityResult.Succeeded)

                    {

                        return RedirectToAction("List", "AdminUsers");

                    }



                }

            }



            return View();

        }



        [HttpPost]

        public async Task<IActionResult> Delete(Guid id)

        {

            var user = await userManager.FindByIdAsync(id.ToString());



            if (user is not null)

            {

                var identityResult = await userManager.DeleteAsync(user);



                if (identityResult is not null && identityResult.Succeeded)

                {

                    return RedirectToAction("List", "AdminUsers");

                }

            }



            return View();

        }

    }