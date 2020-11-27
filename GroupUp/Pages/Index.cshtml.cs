using GroupUp.Data;
using GroupUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroupUp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly GroupUpContext _context;

        [BindProperty]
        public List<Group> GroupsList { get; set; } = new List<Group>();

        public IndexModel(ILogger<IndexModel> logger, GroupUpContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            var name = Request.Query["Name"];
            if (!string.IsNullOrEmpty(name))
            {
                GroupsList = _context.Groups.Where(g => EF.Functions.Like(g.Name, $"%{name}%")).ToList();
                _logger.LogInformation($"name: {name}, groups: {GroupsList.Count}");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var groupId = new Guid(Request.Form["group.Id"]);
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return RedirectToPage("/Index");
            }
            var currentUser = await GetCurrentUser();
            group.Users.Add(currentUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }

        public async Task<User> GetCurrentUser()
        {
            var id = new Guid(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            return await _context.Users.FindAsync(id);
        }
    }
}
