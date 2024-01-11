using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRecorder.MVVM.Db
{
    public class AppDbContext : DbContext
    {            
        public AppDbContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Database.EnsureCreated();
        }

        public DbSet<MeterNode> MeterNode { get; set; }
        public DbSet<DataForHLS> DataForHLS { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=Flowrecorder;Username=postgres;Password=postgres");        
    }
}