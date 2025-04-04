using System;
using Company.MVC.DAL.Models;
using Company.MVC.PL.DTOS;
using Company.MVC.PL.Helpers;
using Company.MVC03.PL.Controllers;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Company.MVC.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region SignUp

        [HttpGet] // GET : /Account/SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        // P@ssW0rd
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDTO model)
        {
            if (ModelState.IsValid) // Server sid Validation
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        // Register User
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree

                        };

                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            // Sned Email to Confirm Email
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }


                    }
                }
                ModelState.AddModelError("", "Invalid SignUp !!");

            }

            return View(model);
        }


        #endregion

        #region SignIn

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);

                    if(flag)
                    {
                        var rsesult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe ,false);
                        if (rsesult.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");

                        }
                    }
                }

                ModelState.AddModelError("" , "Invalid Login :(");

            }
            return View();
        }

        #endregion

        #region SignOut

        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(SignIn));
        }


        #endregion

        #region  Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    var email = new Helpers.Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body =url 
                    };

                    // Send Email
                    var flag = EmailSettings.SendEmail(email);
                    if (flag)
                    {
                        return RedirectToAction("CheckYourIndex");

                    }
                }

            }

            ModelState.AddModelError("", "Invalid Reset Password Operation :(");
            return View("ForgetPassword", model);
        }

        [HttpGet]
        public IActionResult CheckYourIndex()
        {
            return View();
        }

        #endregion




    }
}
