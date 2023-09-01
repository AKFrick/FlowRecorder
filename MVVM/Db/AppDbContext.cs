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
            Database.EnsureCreated();
        }

        public DbSet<MeterNode> MeterNode { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=Flowrecorder;Username=postgres;Password=79579022");        
    }
}
