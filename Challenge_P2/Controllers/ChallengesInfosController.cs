using Challenge_Classe;
using Challenge_P2.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge_P2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ChallengesInfosController : ControllerBase
    {
        private readonly ChallengeDBContext _context;

        public ChallengesInfosController(ChallengeDBContext context)
        {
            _context = context;
        }

        // GET: api/ChallengesInfos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChallMinimized>>> GetAllChallengesMinimized()
        {
            try
            {
                // Récupère tous les challenges de la base de données
                var challenges = await _context.Challenges.ToListAsync();

                if (challenges == null || challenges.Count == 0)
                {
                    return NotFound("Aucun challenge disponible.");
                }

                // Convertir les données en DTO minimisé
                var minimizedChallenges = challenges.Select(challenge => new ChallMinimized
                {
                    Id = challenge.Id,
                    Title = challenge.Title,
                    Image = challenge.ImageB64,
                    Difficulty = challenge.DifficultyLevel,
                    Points = challenge.Points,
                    Category = challenge.Category
                }).ToList();

                return Ok(minimizedChallenges);
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                return StatusCode(500, $"Erreur du serveur : {ex.Message}");
            }
        }

        // GET: api/ChallengesInfos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChallMinimized>> GetChallengeMinimizedById(int id)
        {
            try
            {
                // Récupère le challenge par ID
                var challenge = await _context.Challenges.FindAsync(id);

                if (challenge == null)
                {
                    return NotFound($"Challenge avec l'ID {id} non trouvé.");
                }

                // Convertir en DTO minimisé
                var minimizedChallenge = new ChallMinimized
                {
                    Id = challenge.Id,
                    Title = challenge.Title,
                    Image = challenge.ImageB64,
                    Difficulty = challenge.DifficultyLevel,
                    Points = challenge.Points,
                    Category = challenge.Category
                };

                return Ok(minimizedChallenge);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur du serveur : {ex.Message}");
            }
        }
    }
}
