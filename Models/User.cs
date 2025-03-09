using System.ComponentModel.DataAnnotations;

namespace FirstAPINet.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public Boolean Active { get; set; } = true;

        public ICollection<Post> Posts { get; set; } = new List<Post>();

    }
}
