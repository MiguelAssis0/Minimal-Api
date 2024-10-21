using MinimalsApi.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (loginDTO loginDTO)=>{
    if(loginDTO.Email == "adm@teste.com" && loginDTO.Password == "123456")
        return Results.Ok("Login efetuado com sucesso");
    else
        return Results.Unauthorized();
});

app.Run();
