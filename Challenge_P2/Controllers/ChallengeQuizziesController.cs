using Challenge_Classe.VM;
using Challenge_P2.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Challenge_P2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ChallengeQuizziesController : ControllerBase
    {
        private readonly ChallengeDBContext _context;

        public ChallengeQuizziesController(ChallengeDBContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChallengeWithQuestions(int id)
        {
            // Récupérer le challenge avec ses relations
            var challenge = await _context.Challenges
                .Include(c => c.ChallengeQuestions)
                    .ThenInclude(cq => cq.Question)
                        .ThenInclude(q => q.Options)
                .Include(c => c.ChallengeQuestions)
                    .ThenInclude(cq => cq.Question)
                        .ThenInclude(q => q.Type)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (challenge == null)
            {
                return NotFound(new { Message = $"Challenge with ID {id} not found." });
            }

            var challengeVM = new ChallengeVM
            {
                Id = challenge.Id,
                Title = challenge.Title,
                Questions = challenge.ChallengeQuestions.Select(cq => new QuestionVM
                {
                    Id = cq.Question.Id,
                    Title = cq.Question.Title,
                    Text = cq.Question.Text,
                    IdType = cq.Question.IdType,
                    TypeName = cq.Question.Type.Name,
                    Options = cq.Question.Options.Select(o => new QuestionOptionVM
                    {
                        Id = o.Id,
                        Text = o.Text
                    }).ToList()
                }).ToList()
            };

            return Ok(challengeVM);
        }

    }
}
