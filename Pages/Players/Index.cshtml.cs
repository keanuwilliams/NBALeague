using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using League.Models;
using League.Data;

namespace League.Pages.Players
{
    public class IndexModel : PageModel
    {
        /* Inject the Entity Framework */
        private readonly LeagueContext _context;

        public IndexModel(LeagueContext context)
        {
            _context = context;
        }

        public List<Player> Players { get; set; }
        public List<Team> Teams { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortField { get; set; } = "Name";

        /* HTTP GET REQUEST */
        public async Task OnGetAsync()
        {
            /* Load records from tables */
            Players = await _context.Players.ToListAsync();

            if (!string.IsNullOrEmpty(SearchString))
            {
                /* Search aspects of a player */
                Players = Players.Where(p =>
                    p.Name.ToUpper().Contains(SearchString.ToUpper()) ||
                    p.Number.ToString().Contains(SearchString)).ToList();
            }

            switch (SortField)
            {
                case "Team": Players = Players.OrderBy(p => p.TeamId).ToList(); break;
                case "Number": Players = Players.OrderBy(p => p.Number).ThenBy(p => p.TeamId).ToList(); break;
                case "Name": Players = Players.OrderBy(p => p.Name).ThenBy(p => p.TeamId).ToList(); break;
                case "Position": Players = Players.OrderBy(p => p.Position).ThenBy(p => p.TeamId).ToList(); break;
                case "PPG": Players = Players.OrderByDescending(p => p.PPG).ThenBy(p => p.TeamId).ToList(); break;
                case "APG": Players = Players.OrderByDescending(p => p.APG).ThenBy(p => p.TeamId).ToList(); break;
                case "RPG": Players = Players.OrderByDescending(p => p.RPG).ThenBy(p => p.TeamId).ToList(); break;
            }
        }
    }
}
