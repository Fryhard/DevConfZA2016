using Fryhard.DevConfZA2016.Host.Migrations;
using Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Fryhard.DevConfZA2016.Host
{
    public class DevConfVoteContext : DbContext
    {
        public DbSet<Vote> Votes { get; set; }

        public DevConfVoteContext()
            : base("DevConfVoteContext")
        {

#if DEBUG
            this.Database.Log = s => { System.Diagnostics.Debug.WriteLine(s); };
#endif

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DevConfVoteContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Properties<string>()
                .Where(p => p.GetCustomAttributes(false).OfType<IndexAttribute>().Any())
                .Configure(c => c.HasMaxLength(255));
        }
    }
}
