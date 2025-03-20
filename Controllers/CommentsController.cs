using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstAPINet;
using FirstAPINet.Models;

namespace FirstAPINet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<IActionResult> GetComment()
        {
            try
            {
                var listComment = await _context.Comments
                    .Include(p => p.Post)
                    .ToListAsync();//consult LINQ 

                var dataComment = listComment.Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Creator,
                    c.Text,
                    c.CreationDate,
                    c.PostId
                });

                return Ok(dataComment);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Comments/filter
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetLongComments()
        {
            var longComments = await _context.Comments
                .Where(c => c.Text.Length > 100)  // Filtra por contenido largo
                .ToListAsync();  // Ejecuta la consulta y obtiene la lista de comentarios

            return longComments;
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            var dataComment = new
            {
                comment.Id,
                comment.Title,
                comment.Creator,
                comment.Text,
                comment.CreationDate,
                comment.PostId
            };

            return Ok(dataComment);
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            var commentToUpdate = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == id);

            /*_context.Entry(comment).State = EntityState.Modified;
            _context.Entry(comment).Property(c => c.Text).IsModified = true;
            _context.Update(comment);*/

            commentToUpdate.Title = comment.Title;
            commentToUpdate.Text = comment.Text;



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Comment Updated Success!" });
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{postId}/comment")]
        public async Task<ActionResult<Comment>> PostComment(int postId,Comment comment)
        {
            var post = await _context.Posts.FindAsync(postId); // Buscar el post

            if (post == null)
            {
                return NotFound();
            }

            comment.PostId = post.Id;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetComment", new { id = comment.Id }, comment); //return created object HTTP 201 Created and link to get new resource

            return Ok(new { message = "Comment Created!" });
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Comentario Eliminado con éxito!" });
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
