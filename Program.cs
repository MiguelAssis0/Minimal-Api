using MinimalsApi.Models;
using Microsoft.EntityFrameworkCore;
using MinimalsApi.Infra.Database;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MinimalsContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (loginDTO loginDTO)=>{
    if(loginDTO.Email == "adm@teste.com" && loginDTO.Password == "123456")
        return Results.Ok("Login efetuado com sucesso");
    else
        return Results.Unauthorized();
});

app.Run();
