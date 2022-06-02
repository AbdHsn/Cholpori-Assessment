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

            modelBuilder.Entity<TbDirectoryName>(entity =>
            {
                entity.ToTable("tb_DirectoryNames");
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
            });

        }

        #region TableEntity
        public virtual DbSet<Tasks> Tasks { get; set; } = null!;
        public virtual DbSet<TbDirectoryName> TbDirectoryNames { get; set; } = null!;

        #endregion TableEntity

        #region ViewEntity
        //public DbSet<UsersView> UsersView { get; set; } = null!;

        #endregion ViewEntity

        #region  RawSQL Entity
        public DbSet<TotalRecordCountGLB> TotalRecordCountGLB { get; set; }
        #endregion RawSQL Entity

    }

}
