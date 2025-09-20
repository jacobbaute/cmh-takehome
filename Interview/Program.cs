var builder = WebApplication.CreateBuilder(args);

var OpenPolicy = "_openPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: OpenPolicy,
    policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(OpenPolicy);

app.MapControllers();

app.Run();
