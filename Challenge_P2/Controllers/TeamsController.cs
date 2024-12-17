using Challenge_Classe;
using Challenge_Classe.Relation;
using Challenge_P2.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Challenge_P2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TeamsController : ControllerBase
    {
        private readonly ChallengeDBContext _context;

        public TeamsController(ChallengeDBContext context)
        {
            _context = context;
        }

        // GET: api/Teams/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeamById(Guid id)
        {
            try
            {
                var team = await _context.Teams
                .Include(t => t.UserTeams)
                .ThenInclude(ut => ut.User)
                .FirstOrDefaultAsync(t => t.Id == id);

                if (team == null)
                {
                    return NotFound("Équipe non trouvée.");
                }

                return Ok(new
                {
                    team.Id,
                    team.Name,
                    Users = team.UserTeams.Select(ut => new
                    {
                        ut.User.Id,
                        ut.User.Username,
                        ut.User.Email
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue");
            }
        }

        // POST: api/Teams
        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] TeamRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    return BadRequest("Le nom de l'équipe est obligatoire.");
                }

                if (request.UserIds == null || request.UserIds.Count == 0)
                {
                    return BadRequest("Au moins un utilisateur doit être spécifié.");
                }

                // Création d'une nouvelle équipe
                var team = new Team
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    UserTeams = new List<UserTeam>()
                };

                // Vérifier si tous les utilisateurs existent
                var users = await _context.Utilisateurs
                    .Where(u => request.UserIds.Contains(u.Id))
                    .ToListAsync();

                if (users.Count != request.UserIds.Count)
                {
                    var missingIds = request.UserIds.Except(users.Select(u => u.Id));
                    return BadRequest("Un ou plusieurs utilisateurs n'existent pas. GUID manquants : " + string.Join(", ", missingIds));
                }

                // Ajouter les utilisateurs à l'équipe
                foreach (var user in users)
                {
                    team.UserTeams.Add(new UserTeam
                    {
                        UserId = user.Id,
                        TeamId = team.Id
                    });
                }

                _context.Teams.Add(team);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    team.Id,
                    team.Name,
                    Users = team.UserTeams.Select(ut => new
                    {
                        ut.User.Id,
                        ut.User.Username,
                        ut.User.Email
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue : " + ex.Message);
            }
        }

        // PUT: api/Teams/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(Guid id, [FromBody] TeamRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    return BadRequest("Le nom de l'équipe est obligatoire.");
                }

                if (request.UserIds == null || request.UserIds.Count == 0)
                {
                    return BadRequest("Au moins un utilisateur doit être spécifié.");
                }

                // Vérifier si l'équipe existe
                var team = await _context.Teams
                    .Include(t => t.UserTeams)
                    .ThenInclude(ut => ut.User) // Inclure les utilisateurs pour les réponses
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (team == null)
                {
                    return NotFound("L'équipe spécifiée n'existe pas.");
                }

                // Modifier le nom de l'équipe
                team.Name = request.Name;

                // Vérifier si tous les utilisateurs existent
                var users = await _context.Utilisateurs
                    .Where(u => request.UserIds.Contains(u.Id))
                    .ToListAsync();

                if (users.Count != request.UserIds.Count)
                {
                    var missingIds = request.UserIds.Except(users.Select(u => u.Id));
                    return BadRequest("Un ou plusieurs utilisateurs n'existent pas. GUID manquants : " + string.Join(", ", missingIds));
                }

                // Supprimer les utilisateurs qui ne sont plus associés
                var toRemove = team.UserTeams
                    .Where(ut => !request.UserIds.Contains(ut.UserId))
                    .ToList();

                foreach (var item in toRemove)
                {
                    team.UserTeams.Remove(item);
                }

                // Ajouter les nouveaux utilisateurs
                foreach (var user in users)
                {
                    if (!team.UserTeams.Any(ut => ut.UserId == user.Id))
                    {
                        team.UserTeams.Add(new UserTeam
                        {
                            UserId = user.Id,
                            TeamId = team.Id
                        });
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    team.Id,
                    team.Name,
                    Users = team.UserTeams.Select(ut => new
                    {
                        ut.User.Id,
                        ut.User.Username,
                        ut.User.Email
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue : " + ex.Message);
            }
        }

        // DELETE: api/Teams
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            try
            {
                var team = await _context.Teams
                    .Include(t => t.UserTeams)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (team == null)
                {
                    return NotFound("Équipe non trouvée.");
                }

                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();

                return Ok("Équipe supprimée.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue : " + ex.Message);
            }
        }   
    }
}
