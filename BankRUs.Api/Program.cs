using BankRUs.Api.Configuration;
using BankRUs.Application.Abstractions;
using BankRUs.Application.Authentication;
using BankRUs.Application.Authentication.AuthenticateUser;
using BankRUs.Application.Identity;
using BankRUs.Application.UseCases.DepositMoney;
using BankRUs.Application.UseCases.GetTransactions;
using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Application.UseCases.OpenBankAccount;
using BankRUs.Application.UseCases.WithdrawMoney;
using BankRUs.Infrastructure.Configuration;
using BankRUs.Infrastructure.Persistence;
using BankRUs.Intrastructure.Autentication;
using BankRUs.Intrastructure.Email;
using BankRUs.Intrastructure.Identity;
using BankRUs.Intrastructure.Persistance;
using BankRUs.Intrastructure.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
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
builder.Services.AddScoped<OpenBankAccountHandler>();
builder.Services.AddScoped<AuthenticateUserHandler>();
builder.Services.AddScoped<DepositMoneyHandler>();
builder.Services.AddScoped<WithdrawMoneyHandler>();
builder.Services.AddScoped<GetTransactionsHandler>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddHttpClient<TestPersonnummerValidator>();

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
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<QueryParamsOptions>(
    builder.Configuration.GetSection("QueryParams"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.Migrate();

    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("CustomerService"))
        await roleManager.CreateAsync(new IdentityRole("CustomerService"));

    var email = "service@test.com";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = "Service",
            LastName = "User",
            SocialSecurityNumber = "19010101-9999"
        };

        await userManager.CreateAsync(user, "Secret#1");
        await userManager.AddToRoleAsync(user, "CustomerService");
    }

    for (int i = 1; i <= 15; i++)
    {
        var testEmail = $"user{i}@test.com";

        if (await userManager.FindByEmailAsync(testEmail) == null)
        {
            var user = new ApplicationUser
            {
                UserName = testEmail,
                Email = testEmail,
                FirstName = $"User{i}",
                LastName = "Test",
                SocialSecurityNumber = $"19010101-00{i:D2}"
            };

            await userManager.CreateAsync(user, "Secret#1");
        }
    }
}


app.Run();
