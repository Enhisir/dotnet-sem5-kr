using TicTacToe.Common.Extensions;
using TicTacToe.Extensions;
using TicTacToe.Helpers;
using TicTacToe.Hubs;
using TicTatToe.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddSwaggerGen()
    .AddEndpointsApiExplorer();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services
    .AddJwtConfigured()
    .AddDbContextConfigured()
    .AddMongoConfigured()
    .AddMassTransitConfigured()
    .AddRepositories()
    .AddServices()
    .AddMediatorConfigured(
        AssemblyReference.Assembly, 
        TicTacToe.Common.Helpers.AssemblyReference.Assembly, 
        TicTatToe.Data.Helpers.AssemblyReference.Assembly)
    .AddHandlers()
    .AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseCors(
    opt => opt
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials());

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapHub<GameRoomHub>("/api/v1/hubs/gameRoom");

app.Run();
