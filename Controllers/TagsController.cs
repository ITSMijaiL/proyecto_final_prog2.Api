using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyecto_final_prog2.Infrastructure;
using Microsoft.EntityFrameworkCore;
using proyecto_final_prog2.Application.Services;
using proyecto_final_prog2.Domain.Entities;
using proyecto_final_prog2.Application.Dtos.Tags;
using proyecto_final_prog2.Infrastructure.Models;

namespace proyecto_final_prog2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ILogger<TagsController> _logger;
        private readonly TagService _service;

        public TagsController(ILogger<TagsController> logger, TagService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet(Name = "GetTags")]
        public async Task<IEnumerable<Tag>> Get()
        {
            return await _service.GetTags();
        }

        /*[HttpGet("{id}", Name = "GetTag")]
        public async Task<IndexTagDto?> Get(int id)
        {
            return await _service.GetTag(id);
        }*/

        [HttpGet("{id}", Name = "GetTagsFromTask")]
        public async Task<List<Tag>> Get(int id)
        {
            return await _service.GetTagsFromTask(id);
        }

        [HttpPost("{task_id}",Name = "CreateTag")]
        public async Task<IActionResult> CreateTag([FromBody] TagModel tagModel, int task_id)
        {
            if (tagModel == null)
            {
                return BadRequest("Tag's data is invalid.");
            }
            if (ModelState.IsValid)
            {

                if (await _service.TagExistsByName(tagModel.tag_name))
                {
                    return BadRequest("This tag already exists!");
                }

                Tag tag = await _service.CreateTag(new CreateTagDto { tag_name = tagModel.tag_name}, task_id);
                return CreatedAtRoute("CreateTag", new { id = tag.ID }, tag);
                //return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}", Name = "UpdateTag")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] TagModel tagModel)
        {
            if (tagModel == null)
            {
                return BadRequest("Tag's data is invalid.");
            }
            IndexTagDto? tagdto = await _service.GetTag(id);
            if (tagdto == null)
            {
                return NotFound("Tag not found!");
            }

            if (ModelState.IsValid)
            {
                Tag? tag = await _service.UpdateTag(id, new UpdateTagDto { tag_name=tagModel.tag_name });
                return Ok(tag);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}", Name = "DeleteTag")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            IndexTagDto? tagdto = await _service.GetTag(id);
            if (tagdto == null)
            {
                return NotFound("Tag not found!");
            }

            await _service.DeleteTag(id);
            return Ok("Tag deleted.");
        }
    }
}
