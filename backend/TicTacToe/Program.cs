using TicTacToe.Common.Extensions;
using TicTacToe.Extensions;
using TicTacToe.Helpers;
using TicTatToe.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddSwaggerGen()
    .AddEndpointsApiExplorer();

builder.Services
    .AddJwtConfigured()
    .AddDbContextConfigured()
    .AddMongoConfigured()
    .AddRepositories()
    .AddServices()
    .AddMediatorConfigured(
        AssemblyReference.Assembly, 
        TicTacToe.Common.Helpers.AssemblyReference.Assembly, 
        TicTatToe.Data.Helpers.AssemblyReference.Assembly)
    .AddHandlers()
    .AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.Run();
