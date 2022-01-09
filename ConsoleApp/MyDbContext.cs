using Microsoft.EntityFrameworkCore;

namespace ConsoleApp
{
    public class MyDbContext : DbContext
    {
        public DbSet<BehaviourDto> Behaviours { set; get; }

        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BehaviourDto>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<BehaviourDto>().HasKey(b => b.Name);
            modelBuilder.Entity<BehaviourDto>().Property(b => b.Behaviour).IsRequired();
        }
    }
}