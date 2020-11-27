using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GroupUp.Data;
using GroupUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace GroupUp.Areas.Groups.Pages
{
    public class NewModel : PageModel
    {
        private readonly ILogger<NewModel> _logger;
        private readonly GroupUpContext _context;
        private readonly IMapper _mapper;

        public NewModel(
            ILogger<NewModel> logger,
            GroupUpContext context,
            IMapper mapper
        )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Invalid";
                return Page();
            }
            var group = _mapper.Map<Group>(groupDTO);
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        [BindProperty]
        public GroupDTO groupDTO { get; set; }

        public class GroupDTO
        {
            [Required]
            public string Name { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Group, GroupDTO>();

                CreateMap<GroupDTO, Group>()
                    .ForMember(destination => destination.Id, options => options.Ignore());
            }
        }
    }
}
