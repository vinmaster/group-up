using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GroupUp.Data;
using GroupUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupUp.Areas.My.Pages
{
    public class GroupsModel : PageModel
    {
        private readonly ILogger<AccountModel> _logger;
        private readonly GroupUpContext _context;

        [BindProperty]
        public List<Group> GroupsList { get; set; } = new List<Group>();

        public GroupsModel(
            ILogger<AccountModel> logger,
            GroupUpContext context
        )
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = GetCurrentUserId();
            var user = await _context.Users.Where(u => u.Id == userId).Include(u => u.Groups).FirstOrDefaultAsync();
            GroupsList = user.Groups;
            // Need UseLazyLoadingProxies and installing package to work
            //GroupsList = (await _context.Users.FindAsync(userId)).Groups;

            return Page();
        }

        public Guid GetCurrentUserId()
        {
            return new Guid(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
        }
    }
}
