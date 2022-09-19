using Microsoft.EntityFrameworkCore;
using BigBazzar.Data;
using BigBazzar.Repository;
using BigBazzar.Services.EmailService;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BigBazzarContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BigBazzarContext") ?? throw new InvalidOperationException("Connection string 'BigBazzarContext' not found.")));
//-----------------------------------------------------------
//for JWT Authentication injection
ConfigurationManager configuration = builder.Configuration;


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})



// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});
//-------------------------------------------------------------

builder.Services.AddScoped<IAdminRepo, AdminRepo>();

builder.Services.AddScoped<ITraderRepo, TraderRepo>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepo, CartRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddControllers().AddJsonOptions(options => //exception on cycles-500 error.
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//-----------------------------------------------------------------------
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" }); 
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { In = ParameterLocation.Header, 
        Description = "Please enter a valid token", 
        Name = "Authorization", Type = SecuritySchemeType.Http, BearerFormat = "JWT", Scheme = "Bearer" }); 
    option.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference 
    { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } } });
});
//-----------------------------------------------------------------------


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

app.Run();
