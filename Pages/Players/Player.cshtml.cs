using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using League.Models;
using League.Data;
using System;

namespace League.Pages.Players
{
    public class PlayerModel : PageModel
    {
        // inject the Entity Framework context

        private readonly LeagueContext _context;

        public PlayerModel(LeagueContext context)
        {
            _context = context;
        }

        /* Load a single player */
        public Player Player { get; set; }

        public string Position { get; private set; }
        public Team Team { get; private set; }
        public string Height { get; private set; }
        public string Drafted { get; private set; }
        public string Birthday { get; private set; }

        public async Task OnGetAsync(string Id)
        {
            Player = await _context.Players.FindAsync(Id);
            Team = await _context.Teams.FindAsync(Player.TeamId);

            switch(Player.Position)
            {
                case "C": Position = "Center"; break;
                case "C-F": Position = "Center-Forward"; break;
                case "F": Position = "Forward"; break;
                case "F-C": Position = "Forward-Center"; break;
                case "G": Position = "Guard"; break;
                case "F-G": Position = "Forward-Guard"; break;
                case "G-F": Position = "Guard-Forward"; break;
            }

            int heightFeet = (int) Player.Height / 12;
            int heightInch = (int) Player.Height % 12;

            Height = $"{heightFeet}'{heightInch}\"";

            if (Player.DraftYear == null || Player.DraftPick == null || Player.DraftRound == null)
            {
                Drafted = "Undrafted";
            }
            else
            {
                Drafted = $"{Player.DraftYear} R{Player.DraftRound} #{Player.DraftPick}";
            }

            Birthday = Player.BirthDate.ToString().Substring(0, 10);
        }
    }
}