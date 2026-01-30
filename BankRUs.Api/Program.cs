using BankRUs.Application.Abstractions;
using BankRUs.Application.Authentication;
using BankRUs.Application.Authentication.AuthenticateUser;
using BankRUs.Application.Identity;
using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Application.UseCases.OpenBankAccount;
using BankRUs.Infrastructure.Configuration;
using BankRUs.Intrastructure.Autentication;
using BankRUs.Intrastructure.Email;
using BankRUs.Intrastructure.Identity;
using BankRUs.Intrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//// Samma instans av CustomerService ska vara tillgänglig
//// för samtliga klasser inom ett anrop.
//// Varje request får sin egna instans av CustomerService
//builder.Services.AddScoped<CustomerService>();

// Det finns enbart en instans av CustomerService som delas
// av alla komponenter i applikationen, över applikations livstid.

//// Varje enskild komponent som begär en CustomerService får sin egna
//// instans av denna.
//builder.Services.AddTransient<CustomerService>();

// Registrera ApplicationDbContext i DI-containern
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddControllers();

builder.Services.AddScoped<OpenAccountHandler>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<OpenBankAccountHandler>();
builder.Services.AddScoped<AuthenticateUserHandler>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddHttpClient<TestPersonnummerValidator>();

// 3 typer av livslängder på objekt
// - singleton = ett och samma objekt delas mellan alla andra under hela applikations livslängd
// - scoped = varje HTTP-reqeust får sin egen isntans som sen delas av alla objekt inom denna request
// - transitent = varje objekt får alltid sin egna instans av typen

builder.Services
  .AddIdentity<ApplicationUser, IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services
  .AddAuthentication(options =>
  {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  })
  .AddJwtBearer(options =>
  {
      var jwt = builder.Configuration
        .GetSection(JwtOptions.SectionName)
        .Get<JwtOptions>()!;

      options.RequireHttpsMetadata = false; // false endast i dev
      options.SaveToken = true;

      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidIssuer = jwt.Issuer,

          ValidateAudience = true,
          ValidAudience = jwt.Audience,

          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(jwt.Secret)
        ),

          ValidateLifetime = true,
          ClockSkew = TimeSpan.FromSeconds(30),

          NameClaimType = JwtRegisteredClaimNames.Name,
          RoleClaimType = ClaimTypes.Role
      };
  });

builder.Services.AddAuthorization();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.Migrate();

    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
