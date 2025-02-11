using Microsoft.EntityFrameworkCore;
using TicTatToe.Data.Models;

namespace TicTatToe.Data.Extensions;

public static class ModelBuilderExtension
{
    public static void PrepareEntities(this ModelBuilder modelBuilder)
    {
        // modelBuilder.HasPostgresExtension("pg_trgm");
        modelBuilder
            .Entity<User>()
            .HasKey(u => u.UserName);
        
        modelBuilder
            .Entity<User>()
            .HasOne<GameRoomPublic>()
            .WithOne()
            .HasForeignKey<GameRoomPublic>(grp => grp.UserName);
        modelBuilder
            .Entity<GameRoom>()
            .HasMany<GameRoomPublic>()
            .WithOne()
            .HasForeignKey(grp => grp.GameRoomId);
        modelBuilder
            .Entity<GameRoomPublic>()
            .HasKey(grp => new { grp.GameRoomId, grp.UserName });

        modelBuilder
            .Entity<SystemChatMessage>()
            .HasOne<GameRoom>()
            .WithMany()
            .HasForeignKey(msg => msg.GameRoomId);
    }

    public static void PrepareData(this ModelBuilder modelBuilder)
    {
        throw new NotImplementedException();
        // var topics = new List<Topic>
        // {
        //     new() { Id = 1, Name = "Разработка" },
        //     new() { Id = 2, Name = "Администрирование" },
        //     new() { Id = 3, Name = "Дизайн" },
        //     new() { Id = 4, Name = "Менеждмент" },
        //     new() { Id = 5, Name = "Маркетинг" },
        //     new() { Id = 6, Name = "Разное" },
        // };
        // modelBuilder.Entity<Topic>().HasData(topics);
        //
        // var articlesAuthor = new User
        // {
        //     Id = 1,
        //     UserName = "DeadWeight47",
        //     FirstName = "Nikolay",
        //     LastName = "Rerich",
        //     AvatarLink =
        //         "https://sun9-79.userapi.com/impg/yqlWcDEY_zY_G76v_qtDQNxmT4xelEj7PD4eVQ/UNb379wiAtM.jpg"
        //         + "?size=1920x1080&quality=96&sign=6fef3a6e0f180bf645c4596c8d8ae2dd",
        //     PasswordHashed = "DefaultPassword",
        //     Role = Role.User
        // };
        //
        // modelBuilder.Entity<User>()
        //     .HasData(articlesAuthor);
        //
        // var tags = new List<ArticleTag>()
        // {
        //     new() { Id = 1, Name = "Different 1" },
        //     new() { Id = 2, Name = "Different 2" },
        //     new() { Id = 3, Name = "Different 3" },
        // };
        // modelBuilder.Entity<ArticleTag>()
        //     .HasData(tags);
        //
        // var defaultDateTime = new DateTime(2024, 5, 2, 0, 0, 0, DateTimeKind.Utc);
        // var articles = new List<Article>()
        // {
        //     new()
        //     {
        //         Id = 1,
        //         AuthorId = articlesAuthor.Id,
        //         TopicId = topics[0].Id,
        //         TimePosted = defaultDateTime,
        //         Karma = 15,
        //         Title = "Text example",
        //         Text = "example example example example example"
        //                + " example example example example example"
        //                + " example example example example example"
        //                + " example example example example example"
        //                + " example example example example example"
        //     },
        //     new()
        //     {
        //         Id = 2,
        //         AuthorId = articlesAuthor.Id,
        //         TopicId = topics[0].Id,
        //         TimePosted = defaultDateTime,
        //         Karma = 15,
        //         Title = "Text example 2",
        //         Text = "example example example example example"
        //                + " example example example example example"
        //                + " example example example example example"
        //                + " example example example example example"
        //                + " example example example example example"
        //     },
        //     new()
        //     {
        //         Id = 3,
        //         AuthorId = articlesAuthor.Id,
        //         TopicId = topics[0].Id,
        //         TimePosted = defaultDateTime.AddDays(-1),
        //         Karma = 15,
        //         Title = "Text example 3",
        //         Text = "example example example example example"
        //                + " example example example example example"
        //                + " example example example example example"
        //                + " example example example example example"
        //                + " example example example example example"
        //     }
        // };
        // modelBuilder.Entity<Article>().HasData(articles);
        //
        // modelBuilder.Entity<Commentary>().HasData(new List<Commentary>()
        // {
        //     new()
        //     {
        //         Id = 1,
        //         ArticleId = 1,
        //         AuthorId = 1,
        //         Text = "Как дела? Пока не родила",
        //         TimePosted = defaultDateTime
        //     },
        //     new()
        //     {
        //         Id = 2,
        //         ParentId = 1,
        //         ArticleId = 1,
        //         AuthorId = 1,
        //         Text = "Как дела? Пока не родила",
        //         TimePosted = defaultDateTime
        //     },
        //     new()
        //     {
        //         Id = 3,
        //         ParentId = 2,
        //         ArticleId = 1,
        //         AuthorId = 1,
        //         Text = "Как дела? Пока не родила",
        //         TimePosted = defaultDateTime.AddMinutes(1)
        //     },
        //     new()
        //     {
        //         Id = 4,
        //         ParentId = 1,
        //         ArticleId = 1,
        //         AuthorId = 1,
        //         Text = "Как дела? Пока не родила",
        //         TimePosted = defaultDateTime
        //     },
        //     new()
        //     {
        //         Id = 5,
        //         ParentId = 2,
        //         ArticleId = 1,
        //         AuthorId = 1,
        //         Text = "Как дела? Пока не родила",
        //         TimePosted = defaultDateTime
        //     }
        // });
        //
        // modelBuilder.Entity<Resume>()
        //     .HasData(new Resume()
        //     {
        //         Id = 1,
        //         UserId = 1,
        //         Profession = "Малолетний дебил",
        //         Grade = Grade.Intern,
        //         LookingForWork = true,
        //         LeftSalaryBorder = 100_000,
        //         RightSalaryBorder = 150_000,
        //         About = "ddfsdgdfdsf dsfdsfsdfs dfdsfsdfsd fdsfdsfsdfsd"
        //                 + "ddfsdgdfdsf dsfdsfsdfs dfdsfsdfsd fdsfdsfsdfsd"
        //                 + "ddfsdgdfdsf dsfdsfsdfs dfdsfsdfsd fdsfdsfsdfsd"
        //                 + "ddfsdgdfdsf dsfdsfsdfs dfdsfsdfsd fdsfdsfsdfsd",
        //     });
        //
        // modelBuilder.Entity<WorkPlace>()
        //     .HasData(new List<WorkPlace>()
        //     {
        //         new()
        //         {
        //             Id = 1,
        //             ResumeId = 1,
        //             CompanyName = "Министрество не твоих собачьих дел",
        //             Profession = "Какой-то мужик",
        //             Grade = Grade.Middle,
        //             DateBegin = new DateTime(2024, 5, 2, 21, 0, 0, 0, DateTimeKind.Utc).ToUniversalTime(),
        //             DateEnd = null,
        //             Description = "См. название"
        //         },
        //         new()
        //         {
        //             Id = 2,
        //             ResumeId = 1,
        //             CompanyName = "ИП Макима",
        //             Profession = "Собачка",
        //             Grade = Grade.Senior,
        //             DateBegin = new DateTime(2022, 5, 2, 21, 0, 0, 0, DateTimeKind.Utc).ToUniversalTime(),
        //             DateEnd = new DateTime(2023, 5, 2, 21, 0, 0, 0, DateTimeKind.Utc).ToUniversalTime(),
        //             Description = "Кусал за ногу"
        //         },
        //     });
        //
        // var resumeTags = new List<ResumeTag>()
        // {
        //     new() { Id = 1, Name = "Backend" },
        //     new() { Id = 2, Name = "Frontend" },
        //     new() { Id = 3, Name = "Full-Stack" },
        // };
        // modelBuilder.Entity<ResumeTag>()
        //     .HasData(resumeTags);
    }
}