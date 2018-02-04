namespace SolutionWeb.Model.SYS
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SolutionEntities : DbContext
    {
        public SolutionEntities()
            : base("name=SolutionEntities")
        {
        }

        public virtual DbSet<SYS_DEPT> SYS_DEPT { get; set; }
        public virtual DbSet<SYS_MEMBER> SYS_MEMBER { get; set; }
        public virtual DbSet<SYS_MENU> SYS_MENU { get; set; }
        public virtual DbSet<SYS_MENUOPT> SYS_MENUOPT { get; set; }
        public virtual DbSet<SYS_ROLE> SYS_ROLE { get; set; }
        public virtual DbSet<SYS_ROLE_MENU_MAP> SYS_ROLE_MENU_MAP { get; set; }
        public virtual DbSet<SYS_ROLE_MENUOPT_MAP> SYS_ROLE_MENUOPT_MAP { get; set; }
        public virtual DbSet<SYS_USER> SYS_USER { get; set; }
        public virtual DbSet<SYS_USER_DEFAULTMENU> SYS_USER_DEFAULTMENU { get; set; }
        public virtual DbSet<SYS_USER_ROLE_MAP> SYS_USER_ROLE_MAP { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SYS_DEPT>()
                .Property(e => e.STATUS_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SYS_DEPT>()
                .Property(e => e.DEL_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SYS_DEPT>()
                .Property(e => e.DEPT_ORDER)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SYS_DEPT>()
                .Property(e => e.DEPT_FLAG)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SYS_MEMBER>()
                .Property(e => e.DEL_FLAG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SYS_MENU>()
                .Property(e => e.MENU_LEVEL)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SYS_MENU>()
                .Property(e => e.MENU_ORDER)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SYS_MENU>()
                .Property(e => e.GIS_ORDER)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SYS_MENU>()
                .Property(e => e.ISSHOW_FLAG)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
