using Microsoft.EntityFrameworkCore;
using TicTatToe.Data.Extensions;
using TicTatToe.Data.Models;

namespace TicTatToe.Data.Storage;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; } = null!;
    public DbSet<GameRoom> GameRooms { get; init; } = null!;
    public DbSet<GameRoomPublic> GameRoomPublics { get; init; } = null!;
    public DbSet<SystemChatMessage> SystemChatMessages { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.PrepareEntities();
        modelBuilder.PrepareData();
    }
}