using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MagasinCentral.Data;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Supprime le vrai DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<MagasinDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Ajoute le InMemory DB
            services.AddDbContext<MagasinDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Effectue la migration InMemory
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MagasinDbContext>();
                db.Database.EnsureCreated();
            }
        });
    }
}
