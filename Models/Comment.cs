using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisionShare.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "The UserName is reqired")]
        [MaxLength(100, ErrorMessage = "The UserName cannot exceed 100 characters")]
        public int UserName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCommented { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The content is reqired")]
        public string Content { get; set; }
        //public Comment() { }

        [ForeignKey("Idea")]
        public int IdeaId { get; set; }

        public Idea Idea { get; set; }
    }
}
