using Challenge_Classe;
using Challenge_P2.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Challenge_P2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AnswerVerificationController : ControllerBase
    {
        private readonly ChallengeDBContext _context;

        public AnswerVerificationController(ChallengeDBContext context)
        {
            _context = context;
        }

        [HttpPost("verify")]
        public async Task<ActionResult<AnswerVerificationResponse>> VerifyAnswer([FromBody] AnswerVerificationRequest request)
        {
            // Valider que la question existe
            var question = await _context.Questions
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == request.QuestionId);

            if (question == null)
            {
                return NotFound("Question not found.");
            }

            // Trouver l'option choisie par l'utilisateur
            var selectedOption = question.Options.FirstOrDefault(o => o.Id == request.SelectedOptionId);

            if (selectedOption == null)
            {
                return NotFound("Selected option not found.");
            }

            // Vérifier si l'option est correcte
            var isCorrect = selectedOption.IsCorrect;

            // Récupérer la bonne réponse
            var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);

            if (isCorrect != correctOption.IsCorrect)
            {
                return BadRequest(false);
            }

            return Ok(isCorrect);
        }
    }
}
