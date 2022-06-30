﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProject.Helpers;
using MyProject.Models;
using MyProject.ViewModels;
using System;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            if (registerVM == null) NotFound();
            AppUser user = new AppUser
            {
                Firstname=registerVM.Firstname,
                Lastname=registerVM.Lastname,
                UserName=registerVM.Username,
                Email=registerVM.Email
            };
            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                    return View();
                }
            }
            await _userManager.AddToRoleAsync(user,UserRole.Admin.ToString());
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser user;
            if (loginVM.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            }
            if (user == null)
            {
                ModelState.AddModelError("","Sifre,Username veya Email yanlisdir!!!");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user,loginVM.Password,loginVM.RememberMe,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Sifre,Username veya Email yanlisdir!!!");
                return View();
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Sizin hesab 3 defe yanlis girildiyi ucun bloklanib,Zehmet olamasa biraz gozleyin!!!");
                return View();
            }
            await _signInManager.SignInAsync(user, loginVM.RememberMe);
            return RedirectToAction("Index", "Home");
        }
        public async Task CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(item.ToString()));
                }
            }
        }
    }
}
