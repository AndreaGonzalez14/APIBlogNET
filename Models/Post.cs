using System.ComponentModel.DataAnnotations;

namespace FirstAPINet.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; } = DateTime.Now;


        public int UserId { get; set; }
        public User? User { get; set; }


        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    }
}
