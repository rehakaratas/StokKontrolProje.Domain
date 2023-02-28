using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StokKontrolProje.Repository.Abstract;
using StokKontrolProje.Repository.Concrete;
using StokKontrolProje.Repository.Context;
using StokKontrolProje.Service.Abstract;
using StokKontrolProje.Service.Concrete;

namespace StokKontrolProje.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(option=>option.SerializerSettings.ReferenceLoopHandling=ReferenceLoopHandling.Ignore);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StokKontrolContext>(option =>
            {
                option.UseSqlServer("Server=DESKTOP-GQND9IH;Database=StokKontrolDB;Uid=sa;Pwd=1234;");
            });

            builder.Services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
            builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}