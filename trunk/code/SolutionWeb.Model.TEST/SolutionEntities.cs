namespace SolutionWeb.Model.TEST
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SolutionEntities : DbContext
    {
        public SolutionEntities()
            : base("name=SolutionEntities1")
        {
        }

        public virtual DbSet<SYS_MEMBER> SYS_MEMBER { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SYS_MEMBER>()
                .Property(e => e.DEL_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SYS_MEMBER>()
                .Property(e => e.LOCATION_FLAG)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
