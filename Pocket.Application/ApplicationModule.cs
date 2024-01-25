using Microsoft.Extensions.DependencyInjection;
using Pocket.Application.Interfaces;
using Pocket.Application.Services;

namespace Pocket.Domain;

public class ApplicationModule
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IUserService, UserService>();
    }
}
