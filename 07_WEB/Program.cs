using Microsoft.EntityFrameworkCore;
using _07_WEB.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => 
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnections"))
);

builder.Services.AddIdentityApiEndpoints<IdentityUser>(opt =>
{
    opt.Password.RequiredLength = 6;
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireDigit = false;
    opt.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization(c =>
{
    c.AddPolicy("Admin", p => p.RequireRole("Admin"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGroup("/api").MapCustomIdentityApi<IdentityUser>();

app.Run();
