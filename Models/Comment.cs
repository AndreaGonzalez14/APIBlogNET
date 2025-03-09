using System.ComponentModel.DataAnnotations;

namespace FirstAPINet.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Creator { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public DateTime CreationDate { get; set; }


        public int PostId { get; set; }

        public Post? Post { get; set; }

    }
}
