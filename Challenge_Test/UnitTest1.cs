using System.Threading.Tasks;
using Challenge_P2.Controllers;
using Challenge_Classe;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Challenge_P2.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Challenge_Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreCorrect()
        {
            // Arrange : Configuration de la base de donn�es en m�moire
            var options = new DbContextOptionsBuilder<ChallengeDBContext>()
                .Options;

            using var context = new ChallengeDBContext(options);

            // Ajout des donn�es en m�moire
            context.Database.EnsureDeleted(); // R�initialise la base pour �viter les conflits entre tests
            context.Database.EnsureCreated(); // Cr�e la base avec l'utilisateur par d�faut
            context.SaveChanges();

            // Configuration fictive pour le JWT
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("JwtSettings:TokenPWD", "YourSecretKey12345!")
            }).Build();

            var controller = new AuthController(context, configuration);

            // Identifiants corrects correspondant � l'utilisateur par d�faut
            var login = new Login
            {
                Email = "Jean@gmail.com",
                Password = "motdepasse123"
            };

            // Act : Appel de la m�thode Login
            var result = await controller.Login(login);

            // Assert : V�rification du r�sultat
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Connexion r�ussie.", okResult.Value);
        }
    }
}
