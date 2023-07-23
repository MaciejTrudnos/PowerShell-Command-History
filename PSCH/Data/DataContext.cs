using Microsoft.EntityFrameworkCore;
using PSCH.Model;
namespace PSCH.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dataSource = AppDomain
                .CurrentDomain
                .BaseDirectory;

            optionsBuilder.UseSqlite(@$"DataSource={dataSource}\psch.db;");
        }

        public DbSet<FavouriteCommand> FavouriteCommand { get; set; }

    }
}
