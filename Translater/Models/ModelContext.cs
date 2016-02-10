#region Using

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

#endregion Using

namespace Translater.Models
{
    public class ModelContext : DbContext
    {
        public ModelContext()
            : base("name=translater")
        {
        }

        public DbSet<SearchHistory> SearchHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}