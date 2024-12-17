using Microsoft.AspNetCore.Mvc;

namespace Challenge_P2.Controllers
{
    public class ChallengeHistoryController : ControllerBase
    {
        private readonly ChallengeHistoryContext _context;

        public ChallengeHistoryController(ChallengeHistoryContext context)
        {
            _context = context;
        }

        [HttpGet("users/{userId}/challenges")]
        public async Task<ActionResult<IEnumerable<ChallengeHistory>>> GetChallengeHistory(int userId)
        {
            var challengeHistory = await _context.ChallengeHistories
                .Where(ch => ch.UserId == userId)
                .Include(ch => ch.Challenge)
                .ToListAsync();

            return challengeHistory;
        }
    }
}
