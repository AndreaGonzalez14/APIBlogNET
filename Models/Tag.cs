using System.ComponentModel.DataAnnotations;

namespace FirstAPINet.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<Post> Posts { get; set; } = new List<Post>();


    }
}
