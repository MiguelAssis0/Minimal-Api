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

#region Builder
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MinimalsContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddScoped<IAdminService, AdminServices>();
builder.Services.AddScoped<IVehicle, VehicleServices>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

#endregion

#region Home 
app.MapGet("/", () => Results.Json(new Home()));

#endregion

#region Admin
app.MapPost("/admin/login", ([FromBody]loginDTO loginDTO, IAdminService adminService)=>{
    if(adminService.Login(loginDTO) != null)
        return Results.Ok("Login efetuado com sucesso");
    else
        return Results.Unauthorized();
});

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
});

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
});

app.MapGet("/vehicles", (IVehicle IVehicle) => {
    var vehicles = IVehicle.GetALL();

    if(vehicles == null)
        return Results.NotFound("Nenhum veiculo encontrado");

    return Results.Ok(vehicles);
});

app.MapPatch("/vehicles/{id}", (int id, [FromBody] VehicleDTO vehicleDTO, IVehicle IVehicle) => {
    var vehicle = new Vehicle{
        Id = id,
        Nome = vehicleDTO.Nome,
        Marca = vehicleDTO.Marca,
        Ano = vehicleDTO.Ano
    };
    IVehicle.Update(vehicle);
    return Results.NoContent();
});

app.MapDelete("/vehicles/{id}", (int id, IVehicle IVehicle) => {
    if(IVehicle.GetById(id) == null)
        return Results.NotFound("Veiculo não encontrado");

    IVehicle.Delete(IVehicle.GetById(id));
    return Results.NoContent();
});

app.MapGet("/vehicles/{id}", (int id, IVehicle IVehicle) => {
    if(IVehicle.GetById(id) == null)
        return Results.NotFound("Veiculo não encontrado");

    var vehicle = IVehicle.GetById(id);
    return Results.Ok(vehicle);
});

#endregion


app.Run();
