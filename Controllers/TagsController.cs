using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyecto_final_prog2.Infrastructure;
using proyecto_final_prog2.Domain;
using proyecto_final_prog2.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace proyecto_final_prog2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ILogger<TagsController> _logger;
        private readonly AppDBContext _context;
        public TagsController(ILogger<TagsController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetTags")]
        public IEnumerable<Domain.Entities.Tag> Get()
        {
            return _context.tags.ToList();
        }

        [HttpPost(Name = "CreateTag")]
        public async Task<IActionResult> CreateTag([FromBody] Tag tagModel)
        {
            if (tagModel == null)
            {
                return BadRequest("Tag's data is invalid.");
            }
            if (ModelState.IsValid)
            {
                Tag tag = new Tag
                {
                    tag_name = tagModel.tag_name,
                };

                if (await _context.tags.AnyAsync(t => t.tag_name == tagModel.tag_name))
                {
                    return BadRequest("This tag already exists!");
                }

                _context.tags.Add(tag);
                await _context.SaveChangesAsync();
                return CreatedAtRoute("CreateTag", new { id = tag.ID }, tag);
                //return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}", Name = "UpdateTag")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] Tag tagModel)
        {
            if (tagModel == null)
            {
                return BadRequest("Tag's data is invalid.");
            }
            Tag? tag = await _context.FindAsync<Tag>(id);
            if (tag == null)
            {
                return NotFound("Tag not found!");
            }

            if (ModelState.IsValid)
            {
                tag.tag_name = tagModel.tag_name;
                tag.tasks = tagModel.tasks;

                _context.tags.Update(tag);
                await _context.SaveChangesAsync();
                return Ok(tag);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}", Name = "DeleteTag")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            Tag? tag = await _context.FindAsync<Tag>(id);
            if (tag == null)
            {
                return NotFound("Tag not found!");
            }

            _context.tags.Remove(tag);
            await _context.SaveChangesAsync();
            return Ok("Tag deleted.");
        }
    }
}
