using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniSteam.Abstractions;
using MiniSteam.Application;
using MiniSteam.DataAccess;
using MiniSteam.Entities.MicrosoftIdentity;
using MiniSteam.Exceptions;
using MiniSteam.Repository;
using MiniSteam.Services;
using MiniSteam.Services.AuthServices;
using System.Text;

try
{
    // 🪵 Configuración inicial del logger (antes del builder)
    Log.Information("🚀 Starting MiniSteam API...");

    var builder = WebApplication.CreateBuilder(args);

    // Configurar Serilog leyendo desde appsettings.json
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

    // Integrar Serilog con el host
    builder.Host.UseSerilog();

    // --- Servicios de la aplicación ---
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // --- Swagger + JWT ---
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiniSteam.WebApi", Version = "v1" });

        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Put your Token below",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { jwtSecurityScheme, Array.Empty<string>() }
        });
    });

    // --- Base de datos ---
    builder.Services.AddDbContext<DbDataAccess>(options =>
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            o => o.MigrationsAssembly("MiniSteam.WebApi"));
        options.UseLazyLoadingProxies();
    });
    builder.Services.AddHealthChecks()
    .AddDbContextCheck<DbDataAccess>("Database");


    // --- JWT ---
    builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwt =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true
        };
    });

    // --- Identity ---
    builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<DbDataAccess>()
        .AddSignInManager<SignInManager<User>>()
        .AddRoleManager<RoleManager<Role>>()
        .AddUserManager<UserManager<User>>();

    // --- AutoMapper y dependencias ---
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.AddScoped(typeof(IStringServices), typeof(StringServices));
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped(typeof(IApplication<>), typeof(Application<>));
    builder.Services.AddScoped(typeof(IDbContext<>), typeof(DbContext<>));
    builder.Services.AddScoped(typeof(ITokenHandlerService), typeof(TokenHandlerService));

    var app = builder.Build();

    // 🧩 Middleware global de excepciones
    app.UseMiddleware<ExceptionMiddleware>();

    // Aplicar migraciones automáticas (opcional)
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DbDataAccess>();
        context.Database.Migrate();
    }

    // Swagger solo en desarrollo
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapHealthChecks("/health");
    app.MapControllers();

    Log.Information("✅ MiniSteam API started successfully.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ MiniSteam API terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
