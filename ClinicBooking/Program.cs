using ClinicBooking.Entities;
using ClinicBooking.Services;
using ClinicBooking_Data.Repositories.Implementations;
using ClinicBooking_Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt");

var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing."));
var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is missing.");
var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience is missing.");

// ✅ Cấu hình DbContext
builder.Services.AddDbContext<ClinicBookingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClinicBookingDB")
    ?? throw new InvalidOperationException("Connection string is missing.")));

//   Repository and Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EmailService>();



builder.Services.AddControllersWithViews();

//  Authentication with JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSession();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ✅ Middleware xử lý HTTP
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); // Nếu có dùng session

app.UseRouting();

// ✅ Kích hoạt Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// ✅ Định tuyến
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
