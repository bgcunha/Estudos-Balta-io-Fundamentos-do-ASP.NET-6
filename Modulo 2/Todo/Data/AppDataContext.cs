using Microsoft.EntityFrameworkCore;
using Todo.Model;

namespace Todo.Data;

public class AppDataContext : DbContext
{
    DbSet<TodoModel>? Todos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
       => options.UseSqlite("DataSource=app.db; Cache=Shared");

}