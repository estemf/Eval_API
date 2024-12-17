using Challenge_P2.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge_Classe.VM;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Challenge_P2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    //[ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly ChallengeDBContext _context;

        public UtilisateursController(ChallengeDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserVM>>> GetUsers()
        {
            return await _context.Utilisateurs
                .Include(u => u.Role)
                .Include(u => u.UserTeams)
                .ThenInclude(ut => ut.Team)
                .Select(u => new UserVM
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    ProfilePicture = u.ProfilePicture,
                    CreatedAt = u.CreatedAt,
                    Role = u.Role != null ? u.Role.Name : "Aucun rôle",
                    Teams = u.UserTeams.Select(ut => ut.Team.Name).ToList()
                })
                .ToListAsync();
        }



        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserVM>> GetUserById(Guid id)
        {
            try
            {
                var user = await _context.Utilisateurs
                .Where(u => u.Id == id)
                .Include(u => u.Role)
                .Include(u => u.UserTeams)
                .ThenInclude(ut => ut.Team)
                .Select(u => new UserVM
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    ProfilePicture = u.ProfilePicture,
                    CreatedAt = u.CreatedAt,
                    Role = u.Role != null ? u.Role.Name : "Aucun rôle",
                    Teams = u.UserTeams.Select(ut => ut.Team.Name).ToList()
                })
                .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "un problème est survenue.");
            }
        }

        // GET: api/Users/search?email=email
        [HttpGet("search")]
        public async Task<ActionResult<UserVM>> GetUserByEmail(string username)
        {
            try
            {
                var user = await _context.Utilisateurs
                .Where(u => u.Username == username)
                .Include(u => u.Role)
                .Include(u => u.UserTeams)
                .ThenInclude(ut => ut.Team)
                .Select(u => new UserVM
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    ProfilePicture = u.ProfilePicture,
                    CreatedAt = u.CreatedAt,
                    Role = u.Role != null ? u.Role.Name : "Aucun rôle",
                    Teams = u.UserTeams.Select(ut => ut.Team.Name).ToList()
                })
                .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "un problème est survenue.");
            }
        }

        // GET: api/Users/search?email=email //Challenge history
        [HttpGet("search")]
        public async Task<ActionResult<UserVM>> ChallengeHistory(string username)
        {
            try
            {
                var user = await _context.UserTeams
                    .Include(ut => ut.Team)
                    .ThenInclude(t => t.TeamChallenges)
                    .ThenInclude(tc => tc.Challenge)
                .Where(ut => ut.UserId == id)
                .ToListAsync();
                

                if (userTeams.Any)
                {
                    return NotFound("Aucun challenge effectué");
                }

            }
        }

        // PUT: api/Users/ProfilePicture/{id}
        [HttpPut("ProfilePicture/{id}")]
        public async Task<IActionResult> PutProfilePicture(Guid id, [FromBody] string profilePicture)
        {
            try
            {
                var user = await _context.Utilisateurs.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                user.ProfilePicture = profilePicture;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "un problème est survenue.");
            }
        }

        // PUT: api/Users/Username/{id}
        [HttpPut("Username/{id}")]
        public async Task<IActionResult> PutUsername(Guid id, [FromBody] string username)
        {
            try
            {
                var user = await _context.Utilisateurs.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                user.Username = username;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "un problème est survenue.");
            }
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Utilisateurs.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Utilisateurs.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
