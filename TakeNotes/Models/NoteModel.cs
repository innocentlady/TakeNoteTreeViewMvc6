using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TakeNotes.Models
{
    public class NoteModel
    {
        [Key]
        public int Id { get; set; }

        
        [MaxLength(100)]
        public string Title { get; set; }

        
        public string Body { get; set; }

        public int? ParentId { get; set; }

        
        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        
        public bool IsDeleted { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ParentId))]
        public virtual NoteModel? ParentNote { get; set; }

        [JsonIgnore]
        public virtual ICollection<NoteModel>? ChildNotes { get; set; }
    }
}
