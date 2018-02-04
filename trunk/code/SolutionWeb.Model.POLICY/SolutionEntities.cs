namespace SolutionWeb.Model.POLICY
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

        public virtual DbSet<BASE_PROJECT_CONFIG> BASE_PROJECT_CONFIG { get; set; }
        public virtual DbSet<BASE_PROJECT_INFO> BASE_PROJECT_INFO { get; set; }
        public virtual DbSet<BASE_STATUS_DIC> BASE_STATUS_DIC { get; set; }
        public virtual DbSet<BASE_WORKFLOW_INFO> BASE_WORKFLOW_INFO { get; set; }
        public virtual DbSet<CORPORATION_BASE_INFO> CORPORATION_BASE_INFO { get; set; }
        public virtual DbSet<POLICY_MAIN_INFO> POLICY_MAIN_INFO { get; set; }
        public virtual DbSet<POLICY_STATUS_CHANGE> POLICY_STATUS_CHANGE { get; set; }
        public virtual DbSet<POLICY_APPLY_FILE> POLICY_APPLY_FILE { get; set; }
        public virtual DbSet<POLICY_NOTICE_INFO> POLICY_NOTICE_INFO { get; set; }
        public virtual DbSet<BASE_CONFIG> POLICY_BASE_CONFIG { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BASE_PROJECT_INFO>()
                .HasMany(e => e.BASE_PROJECT_CONFIG)
                .WithOptional(e => e.BASE_PROJECT_INFO)
                .HasForeignKey(e => e.PROJECT_SID);

            modelBuilder.Entity<POLICY_MAIN_INFO>()
                .HasMany(e => e.POLICY_STATUS_CHANGE)
                .WithOptional(e => e.POLICY_MAIN_INFO)
                .HasForeignKey(e => e.POLICY_SID);
        }
    }
}
