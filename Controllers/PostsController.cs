using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstAPINet;
using FirstAPINet.Models;
using FirstAPINet.Dtos;
using Microsoft.Extensions.Hosting;

namespace FirstAPINet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .ToListAsync();

            var postDtos = posts.Select(p => new PostDto
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreationDate = p.CreationDate,
                UserId = p.UserId,
                User = new UserDto
                {
                    Id = p.User.Id,
                    Name = p.User.Name,
                    Username = p.User.Username,
                    Active = p.User.Active
                }
            }).ToList();

            return postDtos;
        }

        // GET: api/Posts/5

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            // Convertimos el post a DTO
            var postDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreationDate = post.CreationDate,
                UserId = post.UserId,
                User = new UserDto
                {
                    Id = post.User.Id,
                    Name = post.User.Name,
                    Username = post.User.Username,
                    Active = post.User.Active
                }
            };

            return postDto;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostUpdateDto postUpdateDto)
        {
            var update_post = await _context.Posts
                       .FirstOrDefaultAsync(p => p.Id == id);

            try
            {
                update_post.Title = postUpdateDto.Title;
                update_post.Content = postUpdateDto.Content;

                // Guardar los cambios
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new {message = "Post Actualizado"});
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{userId}/posts")]
        public async Task<ActionResult<Post>> CreatePost(int userId, Post post)
        {
            var user = await _context.Users.FindAsync(userId); // Buscar el usuario

            if (user == null)
            {
                return NotFound(); // Si no se encuentra el usuario
            }

            post.UserId = user.Id; // Asignar el ID del usuario al post

            _context.Posts.Add(post); // Agregar el post al contexto
            await _context.SaveChangesAsync(); // Guardar cambios

            return Ok(new {message="Post Creado!"});
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok(new {message="Post Eliminado!"});
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        [HttpGet("{userId}/posts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsByUser(int userId)
        {
            var postusers = await _context.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var data = postusers.Select(p => new PostDto
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreationDate = p.CreationDate,
                UserId = p.UserId,

            }).ToList();

            return data;

        }
    }
}
