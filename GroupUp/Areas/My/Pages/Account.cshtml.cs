using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using GroupUp.Data;
using GroupUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace GroupUp.Areas.My.Pages
{
    public class AccountModel : PageModel
    {
        private readonly ILogger<AccountModel> _logger;
        private readonly GroupUpContext _context;
        private readonly IMapper _mapper;

        public AccountModel(
            ILogger<AccountModel> logger,
            GroupUpContext context,
            IMapper mapper
        )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await GetCurrentUser();

            userDTO = _mapper.Map<UserDTO>(currentUser);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Invalid";
                return Page();
            }

            var jsonString = JsonSerializer.Serialize(userDTO, new JsonSerializerOptions { WriteIndented = true });
            _logger.LogInformation("Account: {user}", jsonString);

            var user = await _context.Users.FindAsync(userDTO.Id);
            _mapper.Map<UserDTO, User>(userDTO, user);
            await _context.SaveChangesAsync();

            userDTO = _mapper.Map<UserDTO>(user);

            return RedirectToPage();
        }

        public async Task<User> GetCurrentUser()
        {
            var id = new Guid(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            return await _context.Users.FindAsync(id);
        }

        [BindProperty]
        public UserDTO userDTO { get; set; }

        public class UserDTO
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            [Required]
            public string Email { get; set; }
            [Required]
            public string UserName { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public DateTime LastSignInAt { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<User, UserDTO>();

                CreateMap<UserDTO, User>()
                    .ForMember(destination => destination.Id, options => options.Ignore());
            }
        }
    }
}
