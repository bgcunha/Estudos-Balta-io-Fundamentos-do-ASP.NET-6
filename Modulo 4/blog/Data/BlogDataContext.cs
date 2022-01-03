using Microsoft.EntityFrameworkCore;
using Blog.Models;
using Blog.Data.Mappings;

namespace Blog.Data;

public class BlogDataContext : DbContext
{
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Post>? Posts { get; set; }
    public DbSet<User>? Users { get; set; }

    //public DbSet<CategoryWithCount>? CategoryWithCounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=DESKTOP-BKLEGIN\\SQLEXPRESS2019; Database=Blog; User ID=sa;Password=sa123");
        //options.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryMap());
        modelBuilder.ApplyConfiguration(new PostMap());
        modelBuilder.ApplyConfiguration(new UserMap());

        //modelBuilder.Entity<CategoryWithCount>(x =>
        //{
        //    x.ToSqlQuery(@"
        //                        SELECT 
        //                            COUNT([CategoryId]) AS [Count],   
        //                            [Title]  as Name    
        //                        FROM [Post] AS p
        //                        GROUP BY TITLE"
        //                );
        //});
    }

}

