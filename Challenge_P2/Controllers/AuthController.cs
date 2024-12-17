using Microsoft.AspNetCore.Mvc;
using Challenge_P2.Context;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Challenge_Classe;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Challenge_P2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ChallengeDBContext context;
        private readonly string jwtKey;

        public AuthController(ChallengeDBContext context, IConfiguration configuration)
        {
            this.context = context;
            jwtKey = configuration.GetValue<string>("JwtSettings:TokenPWD")!;

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("un problème est survenue.");
            }
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Login et mot de passe doivent être fournis.");
            }

            var user = await context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == login.Email);

            if (user == null)
            {
                return Unauthorized("Login ou mot de passe incorrect.");
            }

            if (!Regex.IsMatch(login.Email, @"^[a-zA-Z0-9](?!.*\.\.)[a-zA-Z0-9.!#$&'*\/=?^_`{|}~-]*@[a-zA-Z0-9-]+(\.[a-zA-Z]{2,})+$"))
            {
                return BadRequest("Login ou mot de passe incorrect.");
            }

            var passwordValid = BCrypt.Net.BCrypt.EnhancedVerify(login.Password, user.Password);

            if (!passwordValid)
            {
                return Unauthorized("Login ou mot de passe incorrect.");
            }
            else
            {
                if (string.IsNullOrEmpty(jwtKey))
                {
                    return StatusCode(500, "un problème est survenue."); // La clé JWT n'est pas configurée.
                }

                try
                {
                    // Création du token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(jwtKey);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim("UserId", user.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddHours(1), // Durée de vie du token
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    // Retourner un message de succès
                    return Ok(tokenString);
                }
                catch (Exception)
                {
                    return StatusCode(500, "un problème est survenue.");
                }

            }
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] Register login)
        {
            // Vérifier si les champs requis sont fournis
            if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Login et mot de passe doivent être fournis.");
            }

            if (!Regex.IsMatch(login.Email, @"^[a-zA-Z0-9](?!.*\.\.)[a-zA-Z0-9.!#$&'*\/=?^_`{|}~-]*@[a-zA-Z0-9-]+(\.[a-zA-Z]{2,})+$"))
            {
                return BadRequest("L'adresse email n'est pas valide.");
            }

            var user = await context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == login.Email);
            if (user != null)
            {
                return BadRequest("Un utilisateur avec cette Email existe déjà.");
            }

            if (login.Password != login.PasswordConfirmatiion)
            {
                return BadRequest("Les mots de passe ne correspondent pas.");
            }

            if (!Regex.IsMatch(login.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$"))
            {
                return BadRequest("Le mot de passe doit contenir au moins 8 caractères, une lettre majuscule, une lettre minuscule, un chiffre et un caractère spécial.");
            }

            try
            {
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = login.Username,
                    Email = login.Email,
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword(login.Password, 12),
                    CreatedAt = DateTime.Now,
                    ProfilePicture = "https://pixabay.com/vectors/blank-profile-picture-mystery-man-973460/",
                };

                context.Utilisateurs.Add(newUser);
                await context.SaveChangesAsync();

                return Ok("Compte créer");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "un problème est survenue." + ex);
            }
        }

        // POST: api/Auth/logout
        [HttpPost("logout")]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok("Déconnexion réussie.");
        }
    }
}
