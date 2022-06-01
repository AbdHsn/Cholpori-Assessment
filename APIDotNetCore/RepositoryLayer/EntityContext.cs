using DataLayer.Models.Entities;
using DataLayer.Models.Global;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer
{
    public partial class EntityContext : DbContext
    {
        public EntityContext()
        {
        }

        public EntityContext(DbContextOptions<EntityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        #region TableEntity
        public virtual DbSet<Tasks> Tasks { get; set; } = null!;
        #endregion TableEntity

        #region ViewEntity
        //public DbSet<UsersView> UsersView { get; set; } = null!;
       
        #endregion ViewEntity

        #region  RawSQL Entity
        public DbSet<TotalRecordCountGLB> TotalRecordCountGLB { get; set; }
        #endregion RawSQL Entity

    }

}
