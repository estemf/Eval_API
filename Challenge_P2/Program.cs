using Challenge_P2.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.RateLimiting;

namespace Challenge.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Initialisation de l'application");

                var builder = WebApplication.CreateBuilder(args);

                // Configuration JWT
                var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                var key = Encoding.ASCII.GetBytes(jwtSettings["TokenPWD"]!);

                builder.Services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Cookies["jwt"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

                builder.Services.AddDbContext<ChallengeDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(
                    options =>
                    {
                        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                            Scheme = "Bearer",
                            BearerFormat = "JWT",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                            Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
                        });
                        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                        {
                            {
                                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                                {
                                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                                    {
                                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] {}
                            }
                        });
                    }
                );

                builder.Services.AddRateLimiter(options =>
                {
                    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    {
                        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                        if (clientIp == "Unknown")
                        {
                            options.RejectionStatusCode = 400;
                        }

                        return RateLimitPartition.GetFixedWindowLimiter(clientIp, partitionOptions =>
                        {
                            return new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = int.Parse(builder.Configuration["RateLimiter:PermitLimit"]!),
                                Window = TimeSpan.FromMinutes(int.Parse(builder.Configuration["RateLimiter:WaitMinutes"]!)),
                                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                QueueLimit = int.Parse(builder.Configuration["RateLimiter:QueueLimit"]!)
                            };
                        });
                    });

                    options.RejectionStatusCode = 429;
                });

                var logger_test = LogManager.GetCurrentClassLogger();
                logger_test.Info("Test de log NLog à l'initialisation.");
                logger_test.Warn("Ceci est un avertissement pour tester NLog.");
                logger_test.Error("Erreur de test pour vérifier NLog.");


                // Configuration NLog comme système de log
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                //app.UseAuthentication();
                app.UseAuthorization();

                app.UseRateLimiter();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Une erreur est survenue pendant l'initialisation de l'application");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}
