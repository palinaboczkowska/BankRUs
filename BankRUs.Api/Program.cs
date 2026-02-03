using BankRUs.Api.Configuration;
using BankRUs.Application.Abstractions;
using BankRUs.Application.Identity;
using BankRUs.Application.UseCases.OpenAccount;
using BankRUs.Application.UseCases.OpenBankAccount;
using BankRUs.Intrastructure.Email;
using BankRUs.Intrastructure.Identity;
using BankRUs.Intrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddHttpClient<TestPersonnummerValidator>();

// 3 typer av livslängder på objekt
// - singleton = ett och samma objekt delas mellan alla andra under hela applikations livslängd
// - scoped = varje HTTP-reqeust får sin egen isntans som sen delas av alla objekt inom denna request
// - transitent = varje objekt får alltid sin egna instans av typen

builder.Services
  .AddIdentity<ApplicationUser, IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();

builder.Services.Configure<QueryParamsOptions>(builder.Configuration.GetSection("QueryParamsOptions"));

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
}


app.Run();
