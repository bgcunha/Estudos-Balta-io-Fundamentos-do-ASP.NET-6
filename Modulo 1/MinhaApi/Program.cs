var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    return Results.Ok("Hello World");
});

app.MapGet("/{nome}", (string nome) =>
{
    return Results.Ok($"Hello {nome}");
});

app.MapGet("/name/{nome}", (string nome) =>
{
    return Results.Ok($"Hello {nome}");
});

app.MapPost("/", (User user) =>
{
    return Results.Ok($"ID: {user.Id} e o Nome: {user.Username}");
});

app.Run();

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
}