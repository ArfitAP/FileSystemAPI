using FileSystemAPI.Domain.Common;
using FileSystemAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;


namespace FileSystemAPI.Persistence
{
    public class FileSystemDbContext : DbContext
    {
        public FileSystemDbContext(DbContextOptions<FileSystemDbContext> options)
           : base(options)
        {
        }

        public DbSet<Domain.Entities.File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>(e => {
                
                e.ToTable("Folder");

                e.Property(p => p.FolderName)
                    .HasMaxLength(128)
                    .IsRequired();

                e.HasMany(e => e.SubFolders)
                    .WithOne(e => e.ParentFolder)
                    .HasForeignKey(e => e.ParentFolderId)
                    .IsRequired(false);

                e.HasMany(e => e.Files)
                    .WithOne(e => e.Folder)
                    .HasForeignKey(e => e.FolderId)
                    .IsRequired();

                e.HasData(new Folder
                {
                    Id = -1,
                    FolderName = string.Empty,
                    ParentFolderId = null,
                    Active = true
                });
            });

            modelBuilder.Entity<Domain.Entities.File>(e => {

                e.ToTable("File");

                e.Property(p => p.FileName)
                    .HasMaxLength(128)
                    .IsRequired();

                e.Property(p => p.StoredFileName)
                    .IsRequired();

            });

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
