using MinimalsApi.Models;
using Microsoft.EntityFrameworkCore;
using MinimalsApi.Infra.Database;
using MinimalsApi.Infra.Interfaces;
using MinimalsApi.Domain.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MinimalsContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddScoped<IAdminService, AdminServices>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", ([FromBody]loginDTO loginDTO, IAdminService adminService)=>{
    if(adminService.Login(loginDTO) != null)
        return Results.Ok("Login efetuado com sucesso");
    else
        return Results.Unauthorized();
});

app.Run();
