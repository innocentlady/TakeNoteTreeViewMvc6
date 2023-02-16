using Microsoft.EntityFrameworkCore;
using System.Data.Common;

using TakeNotes.Models;

namespace TakeNotes.Data
{
    public class NoteContext:DbContext
    {
        public NoteContext(DbContextOptions<NoteContext> options)
        : base(options)
        {
        }

        public virtual DbSet<NoteModel> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NoteModel>()
                .HasOne(n => n.ParentNote)
                .WithMany(n => n.ChildNotes)
                .HasForeignKey(n => n.ParentId);
        }

    }
}
