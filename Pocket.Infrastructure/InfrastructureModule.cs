using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pocket.Domain;

namespace Pocket.Infrastructure;

public class InfrastructureModule
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<PocketDbContext>(options =>
            options.UseInMemoryDatabase("PocketDatabase"));
        services.AddTransient<IPocketDbContext, PocketDbContext>();
    }
}
