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
#endregion

#region Vehicles

app.MapPost("/vehicles", ([FromBody] VehicleDTO vehicleDTO, IVehicle IVehicle) => {
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
    return Results.Ok(vehicles);
});

app.MapPut("/vehicles/{id}", (int id, [FromBody] VehicleDTO vehicleDTO, IVehicle IVehicle) => {
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
    IVehicle.Delete(IVehicle.GetById(id));
    return Results.NoContent();
});

app.MapGet("/vehicles/{id}", (int id, IVehicle IVehicle) => {
    var vehicle = IVehicle.GetById(id);
    return Results.Ok(vehicle);
});

#endregion


app.Run();
