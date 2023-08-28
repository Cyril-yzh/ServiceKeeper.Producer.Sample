using Microsoft.EntityFrameworkCore;
using ServiceKeeper.Core.Entity;
using Pomelo.EntityFrameworkCore.MySql;

namespace ServiceKeeper.Producer.Sample.Domain.EFCore
{
    public class TaskDbContext : DbContext
    {
        public DbSet<TaskEntity> TaskEntity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql("Data Source=192.168.23.4;Initial Catalog=GTA;User Id=root;Password=Aa111111;", ServerVersion.Parse("8.0.32-mysql"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>()
                .Property(e => e.Id)
                .ValueGeneratedNever(); // 指定主键不是自增
        }
    }
}
