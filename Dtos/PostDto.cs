using static FirstAPINet.Controllers.PostsController;

namespace FirstAPINet.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public UserDto? User { get; set; }  // Usamos un DTO para el User

    }

    public class PostUpdateDto
    {
        
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
  

    }
}
