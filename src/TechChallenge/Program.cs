using Microsoft.EntityFrameworkCore;
using TechChallenge.Application.Services;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Interfaces.Infra;
using TechChallenge.Infra.Repositories;
using TechChallenge.Infra.Data;

namespace TechChallenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //add connection from mysql
            builder.Services.AddDbContext<ApiDbContext>(options =>
                           options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                                              new MySqlServerVersion(new Version(8, 0)),
                                              mySqlOptions =>
                                              {
                                                  mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                                              }));
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
            builder.Services.AddScoped<IPedidoService, PedidoService>();
            builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}