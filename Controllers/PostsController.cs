using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstAPINet;
using FirstAPINet.Models;
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
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .ToListAsync();

            var dataPosts = posts.Select(p => new
            {
                p.Id,
                p.Title,
                p.Content,
                p.CreationDate,
                p.UserId,
                Username = p.User != null ? p.User.Username : "Sin título"
            }).ToList();

            return Ok(dataPosts);
        }

        // GET: api/Posts/5

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            try
            {
                var post = await _context.Posts
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (post == null)
                {
                    return NotFound();
                }

                var dataPost = new
                {
                    post.Id,
                    post.Title,
                    post.Content,
                    post.CreationDate,
                    UserName = post.User != null ? post.User.Username : "Sin título",

                };

                return Ok(dataPost);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, [FromBody] Post post)
        {
            var update_post = await _context.Posts
                       .FirstOrDefaultAsync(p => p.Id == id);

            try
            {
                update_post.Title = post.Title;
                update_post.Content = post.Content;

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

            return Ok(new {message = "Post Updated Success!"});
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

            return Ok(new {message="Post Created Success!"});
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

            return Ok(new {message="Post Deleted!"});
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        //GET: api/user_id/posts
        [HttpGet("{userId}/posts")]
        public async Task<IActionResult> GetPostsByUser(int userId)
        {
            var postusers = await _context.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var data = postusers.Select(p => new 
            {
                p.Id,
                p.Title,
                p.Content,
                p.CreationDate,
                p.UserId,

            }).ToList();

            return Ok(data);

        }
    }
}
