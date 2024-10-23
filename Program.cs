using MinimalsApi.Models;
using Microsoft.EntityFrameworkCore;
using MinimalsApi.Infra.Database;
using MinimalsApi.Infra.Interfaces;
using MinimalsApi.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using MinimalsApi.Domain.Views;
using MinimalsApi.Domain.Entities;
using MinimalsApi.Domain.Interfaces;
using MinimalsApi.Domain.DTO;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi.Models;
using System.Text;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "JwtBearerDefault";
    option.DefaultChallengeScheme = "JwtBearerDefault";
}).AddJwtBearer("JwtBearerDefault", option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
builder.Services.AddDbContext<MinimalsContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddScoped<IAdminService, AdminServices>();
builder.Services.AddScoped<IVehicle, VehicleServices>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT no campo abaixo. Exemplo: Bearer {seu token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddControllers();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();



#endregion

#region Home 
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous();

#endregion

#region Admin

string TokenJWT(Admin admin)
{
    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
    var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(ClaimTypes.Email, admin.Email),
        new Claim(ClaimTypes.Role, admin.Profile)
    };

    var token = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(30),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}


app.MapGet("/admin", (IAdminService adminService) => {
    return Results.Ok(adminService.GetAll());
}).RequireAuthorization();

app.MapPost("/admin/login", ([FromBody]loginDTO loginDTO, IAdminService adminService)=>{
    var admin = adminService.Login(loginDTO);
    if(adminService.Login(loginDTO) != null)
    {
        string token = TokenJWT(admin);
        return Results.Ok(new AdmLogged
        {
            Email = admin.Email,
            Token = token,
            Profile = admin.Profile
        });
    }
    else
        return Results.Unauthorized();
}).AllowAnonymous();

app.MapPost("/admin/register", ([FromBody]AdminDTO adminDTO, IAdminService adminService)=>{
    
    if(string.IsNullOrEmpty(adminDTO.Email))
        return Results.BadRequest("Você deve inserir um e-mail");

    if(string.IsNullOrEmpty(adminDTO.Password))
        return Results.BadRequest(" você deve inserir uma senha");

    
    var admin = new Admin
    {
        Email = adminDTO.Email,
        Password = adminDTO.Password,
        Profile = adminDTO.Profile.ToString() 
    };
    

    adminService.Include(admin);
    return Results.Created("/admin", admin);
}).RequireAuthorization();

#endregion

#region Vehicles

app.MapPost("/vehicles", ([FromBody] VehicleDTO vehicleDTO, IVehicle IVehicle) => 
{   
    
    if(vehicleDTO.Nome == null)
        return Results.BadRequest("Você deve inserir um nome");

    if(vehicleDTO.Marca == null)
        return Results.BadRequest("Você deve inserir a marca do veiculo");

    if(vehicleDTO.Ano == 0)
        return Results.BadRequest("Você deve inserir o ano do veiculo");

    var vehicle = new Vehicle{
        Nome = vehicleDTO.Nome,
        Marca = vehicleDTO.Marca,
        Ano = vehicleDTO.Ano
    };

    IVehicle.Include(vehicle);
    return Results.Created("/vehicles", vehicle);
}).RequireAuthorization();

app.MapGet("/vehicles", (IVehicle IVehicle) => {
    var vehicles = IVehicle.GetALL();

    if(vehicles == null)
        return Results.NotFound("Nenhum veiculo encontrado");

    return Results.Ok(vehicles);
}).RequireAuthorization();

app.MapPatch("/vehicles/{id}", (int id, [FromBody] VehicleDTO vehicleDTO, IVehicle IVehicle) => {
    var vehicle = new Vehicle{
        Id = id,
        Nome = vehicleDTO.Nome,
        Marca = vehicleDTO.Marca,
        Ano = vehicleDTO.Ano
    };
    IVehicle.Update(vehicle);
    return Results.NoContent();
}).RequireAuthorization();

app.MapDelete("/vehicles/{id}", (int id, IVehicle IVehicle) => {
    if(IVehicle.GetById(id) == null)
        return Results.NotFound("Veiculo não encontrado");

    IVehicle.Delete(IVehicle.GetById(id));
    return Results.NoContent();
}).RequireAuthorization();

app.MapGet("/vehicles/{id}", (int id, IVehicle IVehicle) => {
    if(IVehicle.GetById(id) == null)
        return Results.NotFound("Veiculo não encontrado");

    var vehicle = IVehicle.GetById(id);
    return Results.Ok(vehicle);
}).RequireAuthorization();

#endregion

#region App
app.Run();
#endregion
