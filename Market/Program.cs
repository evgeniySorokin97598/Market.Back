
using DataBaseLib;
using DataBaseLib.Interfaces;
using DataBaseLib.Providers;
using IdentityServer4.AccessTokenValidation;
using LoggerLib.Loggers;
using Market.Repositories.Interfaces;
using Market.Repositories.Repositories.PostgresqlRepositories;
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
            Environment.SetEnvironmentVariable("Port", "5433");
            Environment.SetEnvironmentVariable("Username", "postgres");
            Environment.SetEnvironmentVariable("identety_url", "http://localhost:5234");
            Environment.SetEnvironmentVariable("Database", "Market");
#endif



            builder.Services.AddSingleton(new DataBaseConfig()
            {
                Host = Environment.GetEnvironmentVariable("Host"),
                Password = Environment.GetEnvironmentVariable("Password"),
                Port = Environment.GetEnvironmentVariable("Port"),
                Username = Environment.GetEnvironmentVariable("Username"),
                DataBase = Environment.GetEnvironmentVariable("Database"),
            });


            builder.Services.AddTransient<IDbProvider, PostgresqlProvider>();
            builder.Services.AddTransient<ICategoriesRepository, Market.Repositories.Repositories.PostgresqlRepositories.CategoriesRepository.Repository>();
            builder.Services.AddTransient<ICommentsLikesRepository, Market.Repositories.Repositories.PostgresqlRepositories.CommentsLikesRepository.Repository>();
            builder.Services.AddTransient<ICommentsRepository, Market.Repositories.Repositories.PostgresqlRepositories.CommentsRepository.Repository>();
            builder.Services.AddTransient<IProductsRepository, Market.Repositories.Repositories.PostgresqlRepositories.ProductsRepository.Repository>();
            builder.Services.AddTransient<ISubcategoryRepository, Market.Repositories.Repositories.PostgresqlRepositories.SubcategoryRepository.Repository>();
            builder.Services.AddTransient<IType—haracteristicsRepository, Market.Repositories.Repositories.PostgresqlRepositories.Type—haracteristicsRepository.Repository>();
            builder.Services.AddTransient<IUsersRepository, Market.Repositories.Repositories.PostgresqlRepositories.UsersRepository.Repository>();
            builder.Services.AddTransient<I—haracteristicsRepository, Market.Repositories.Repositories.PostgresqlRepositories.—haracteristicsRepository.Repository>();
            builder.Services.AddSingleton<LoggerLib.Interfaces.ILogger>(new ConsoleLogger());


            #region —ÓÁ‰‡ÌËÂ Ë Á‡ÔÓÎÌÂÌËÂ ¡ƒ
            var logger = new ConsoleLogger();
            var configs = new DataBaseConfig()
            {
                Host = Environment.GetEnvironmentVariable("Host"),
                Password = Environment.GetEnvironmentVariable("Password"),
                Port = Environment.GetEnvironmentVariable("Port"),
                Username = Environment.GetEnvironmentVariable("Username"),
                DataBase = Environment.GetEnvironmentVariable("Database"),
            };
            DataBaseCreater creater = new DataBaseCreater(configs, logger);
            creater.Create();
            DataBaseManager manager = new DataBaseManager(new PostgresqlProvider(configs), logger);
            manager.AddTestData().GetAwaiter().GetResult();

            #endregion


            builder.Services.AddTransient<IDataBaseManager, DataBaseManager>();
            IdentityModelEventSource.ShowPII = true;

            builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
    .AddIdentityServerAuthentication(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.Authority = Environment.GetEnvironmentVariable("identety_url");
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