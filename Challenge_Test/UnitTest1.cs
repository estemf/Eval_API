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
            // Arrange : Configuration de la base de données en mémoire
            var options = new DbContextOptionsBuilder<ChallengeDBContext>()
                .Options;

            using var context = new ChallengeDBContext(options);

            // Ajout des données en mémoire
            context.Database.EnsureDeleted(); // Réinitialise la base pour éviter les conflits entre tests
            context.Database.EnsureCreated(); // Crée la base avec l'utilisateur par défaut
            context.SaveChanges();

            // Configuration fictive pour le JWT
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("JwtSettings:TokenPWD", "YourSecretKey12345!")
            }).Build();

            var controller = new AuthController(context, configuration);

            // Identifiants corrects correspondant à l'utilisateur par défaut
            var login = new Login
            {
                Email = "Jean@gmail.com",
                Password = "motdepasse123"
            };

            // Act : Appel de la méthode Login
            var result = await controller.Login(login);

            // Assert : Vérification du résultat
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Connexion réussie.", okResult.Value);
        }
    }
}
