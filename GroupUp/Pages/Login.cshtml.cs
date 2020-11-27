using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GroupUp.Data;
using GroupUp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace GroupUp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly GroupUpContext _context;
        private readonly UserManager<User> _userManager;

        public LoginModel(
            GroupUpContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<LoginModel> logger
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByNameAsync(userDTO.Username);

            if (user == null)
            {
                ViewData["Error"] = "No such username";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(user, userDTO.Password, false, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut) ViewData["Error"] = "Account locked out";
                else ViewData["Error"] = "Invalid login";
                return Page();
            }

            //var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //var identity = new ClaimsIdentity(new[]
            //{
            //    new Claim(ClaimTypes.Name, user.Email)
            //}, scheme);
            //var principal = new ClaimsPrincipal(identity);
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            user.LastSignInAt = DateTime.Now;
            _context.SaveChanges();

            _logger.LogInformation("User login: {user}", user);

            //return SignIn(principal, scheme);
            return RedirectToPage("./Index");
        }

        [BindProperty]
        public UserDTO userDTO { get; set; }

        public class UserDTO
        {
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }
        }
    }
}
