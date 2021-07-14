using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using League.Models;
using League.Data;
using Microsoft.AspNetCore.Http;

namespace League.Pages.Teams
{
    public class IndexModel : PageModel
    {
        /* Inject the Entity Framework */
        private readonly LeagueContext _context;

        public IndexModel(LeagueContext context)
        {
            _context = context;
        }

        /* Properties for Conferences, Division, and Teams */
        public List<Conference> Conferences { get; set; }
        public List<Division> Divisions { get; set; }
        public List<Team> Teams { get; set; }

        public SelectList AllTeams { get; set; }

        [BindProperty(SupportsGet = true)] 
        public string SearchString { get; set; }

        /* HTTP GET REQUEST */
        public async Task OnGetAsync()
        {
            /* Load records from tables */
            Conferences = await _context.Conferences.ToListAsync();
            Divisions = await _context.Divisions.ToListAsync();
            Teams = await _context.Teams.ToListAsync();

            if (!string.IsNullOrEmpty(SearchString))
            {
                /* Search for all aspects of a team */
                Teams = Teams.Where(t =>
                    (t.Name + ' ' + t.Location).ToUpper().Contains(SearchString.ToUpper()) ||
                    t.Win.ToString().Contains(SearchString) ||
                    t.Loss.ToString().Contains(SearchString) ||
                    t.Division.Name.ToUpper().Contains(SearchString.ToUpper()) ||
                    t.Division.Conference.Name.ToUpper().Contains(SearchString.ToUpper())).ToList();
            }

            Teams.OrderByDescending(t => t.Win).ToList();
        }
    }
}
