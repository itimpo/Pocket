using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pocket.Application;
using Pocket.Domain;
using Pocket.Infrastructure;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Pocket.Api.xml"), true);
});

// adding modules for application
InfrastructureModule.ConfigureServices(builder.Services);
DomainModule.ConfigureServices(builder.Services);
ApplicationModule.ConfigureServices(builder.Services);

builder.Services.AddScoped<UserContext>((IServiceProvider sp) =>
{
    var http = sp.GetRequiredService<IHttpContextAccessor>();
    return http == null 
    ? new UserContext() 
    : new UserContext
    {
        Id = Guid.Parse(http.HttpContext?.User.FindFirstValue(ClaimTypes.Sid) ?? String.Empty),
        Name = http.HttpContext?.User.FindFirstValue(ClaimTypes.Name) ?? String.Empty,
        Email = http.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ?? String.Empty
    };
}); 

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    var secretString = builder.Configuration["JWT:Secret"];
    if(string.IsNullOrEmpty(secretString))
    {
        throw new Exception("JWT:Secret is not configured");
    }
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretString))
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
