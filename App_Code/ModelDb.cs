using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CodingRep.App_Code
{
    public partial class ModelDb : DbContext
    {
        public ModelDb()
            : base("name=ModelDb")
        {
        }

        public virtual DbSet<branches> branches { get; set; }
        public virtual DbSet<commits> commits { get; set; }
        public virtual DbSet<fileSnapshots> fileSnapshots { get; set; }
        public virtual DbSet<repositories> repositories { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<commits>()
                .HasMany(e => e.branches)
                .WithOptional(e => e.commits)
                .HasForeignKey(e => e.commitId);

            modelBuilder.Entity<commits>()
                .HasMany(e => e.commits1)
                .WithOptional(e => e.commits2)
                .HasForeignKey(e => e.parentId);

            modelBuilder.Entity<commits>()
                .HasMany(e => e.fileSnapshots)
                .WithRequired(e => e.commits)
                .HasForeignKey(e => e.commitId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<repositories>()
                .HasMany(e => e.branches)
                .WithRequired(e => e.repositories)
                .HasForeignKey(e => e.repoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<repositories>()
                .HasMany(e => e.commits)
                .WithRequired(e => e.repositories)
                .HasForeignKey(e => e.repoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<users>()
                .HasMany(e => e.commits)
                .WithRequired(e => e.users)
                .HasForeignKey(e => e.userId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<users>()
                .HasMany(e => e.repositories)
                .WithRequired(e => e.users)
                .HasForeignKey(e => e.userId)
                .WillCascadeOnDelete(false);
        }
    }
}
