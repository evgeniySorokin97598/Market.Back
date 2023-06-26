
using IdentityServer4.AccessTokenValidation;
using LoggerLib.Loggers;
using Market.Entities.Configs;
using Market.Repositories.Interfaces;
using Market.Repositories.Repositories.PostgresqlRepositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace Market
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Connection = new NpgsqlConnection($"Host=192.168.133.128;Port=5432;Database = Market; Username=postgres;Password=123qwe45asd");
#if DEBUG
            Environment.SetEnvironmentVariable("Host", "192.168.73.128");
            Environment.SetEnvironmentVariable("Password", "1");
            Environment.SetEnvironmentVariable("Port", "5432");
            Environment.SetEnvironmentVariable("Username", "postgres");
            Environment.SetEnvironmentVariable("identety_url", "http://localhost:5234");
#endif



            Configs configs = new Configs()
            {
                DataBaseConfig = new DataBaseConfig()
                {
                    Host = Environment.GetEnvironmentVariable("Host"),
                    Password = Environment.GetEnvironmentVariable("Password"),
                    Port = Environment.GetEnvironmentVariable("Port"),
                    Username = Environment.GetEnvironmentVariable("Username"),
                    DataBase = Environment.GetEnvironmentVariable("Database"),
                },
                IdentetyServiceUrl = Environment.GetEnvironmentVariable("identety_url")

            };
            if (string.IsNullOrEmpty(configs.DataBaseConfig.DataBase)) configs.DataBaseConfig.DataBase = "Market3";

            LoggerLib.Interfaces.ILogger logger = new ConsoleLogger();

            builder.Services.AddSingleton(logger);


            #region Создание и заполнение БД
            DataBaseCreater creater = new DataBaseCreater(configs, logger);
            creater.Create();



            DataBaseManager manager = new DataBaseManager(configs, logger);
            manager.AddTestData().GetAwaiter().GetResult();

            #endregion
            builder.Services.AddSingleton(configs);
            builder.Services.AddTransient<IDataBaseManager, DataBaseManager>();
            IdentityModelEventSource.ShowPII = true;

            builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
    .AddIdentityServerAuthentication(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.Authority = configs.IdentetyServiceUrl;
    });
            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.WithOrigins("*")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("MyPolicy");
            app.UseCors(x => x
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin => true) // allow any origin
                                                      //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
                  .AllowCredentials()); // allow credentials
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}