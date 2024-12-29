using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
