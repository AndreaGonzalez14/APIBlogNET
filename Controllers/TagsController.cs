using FirstAPINet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstAPINet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            return await _context.Tags.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetOneTag(int id)
        {
            try
            {
                var tag = await _context.Tags.FindAsync(id);

                if (tag == null)
                {
                    return NotFound();
                }

                return tag;
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            _context.Entry(tag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Tag Creado Actualizado correctamente!" });
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!TagExists(id))
                {
                    return NotFound();

                }else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> CreateTag(Tag tag)
        {
            _context.Tags.Add(tag);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOneTag", new { id = tag.Id }, tag);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{postId}/tags/{tagId}")]
        public async Task<ActionResult> AssignTagToPost(int postId, int tagId)
        {
            var post = await _context.Posts.FindAsync(postId);  // Buscar el post por ID
            var tag = await _context.Tags.FindAsync(tagId);  // Buscar el tag por ID

            if (post == null || tag == null)
            {
                return NotFound();  // Si no se encuentran el post o el tag, retornar 404
            }

            post.Tags.Add(tag);  // Agregar el tag al post
            await _context.SaveChangesAsync();  // Guardar los cambios

            return NoContent();  // Retornar respuesta sin contenido
        }

    }
}
