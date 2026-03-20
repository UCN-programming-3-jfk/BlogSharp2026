using BlogSharp2026.DataAccess.Interfaces;
using BlogSharp2026.DataAccess.SqlClient;

namespace BlogSharp2026.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        //registrér, at hvis en controllerklasse skal bruge en IAuthorDao
        //i sin constructor, så lav en AuthorDao

        builder.Services.AddControllers();

        var app = builder.Build();
        
        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
