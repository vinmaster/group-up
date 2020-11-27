using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GroupUp.Data;
using GroupUp.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GroupUp.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly GroupUpContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            GroupUpContext context,
            IMapper mapper,
            UserManager<User> userManager,
            ILogger<RegisterModel> logger
        )
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserDTO userDTO { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new User
            {
                Email = userDTO.Email,
                UserName = userDTO.Username,
                LastSignInAt = null,
            };
            var password = userDTO.Password;
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                ViewData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                return Page();
            }
            _logger.LogInformation("User created: {@user}", user);

            return RedirectToPage("./Index");
        }

        public class UserDTO
        {
            [Required]
            public string Email { get; set; }
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }
        }
    }
}
