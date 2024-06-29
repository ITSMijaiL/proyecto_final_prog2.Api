using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_final_prog2.Domain;
using proyecto_final_prog2.Domain.Entities;
using proyecto_final_prog2.Infrastructure;

namespace proyecto_final_prog2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        private readonly ILogger<ColumnsController> _logger;
        private readonly AppDBContext _context;
        public ColumnsController(ILogger<ColumnsController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetColumns")]
        public IEnumerable<Column> Get()
        {
            return _context.columns.ToList();
        }

        [HttpPost(Name = "CreateColumn")]
        public async Task<IActionResult> CreateColumn([FromBody] Column columnModel)
        {
            if (columnModel == null)
            {
                return BadRequest("Column's data is invalid.");
            }
            if (ModelState.IsValid)
            {
                Column column = new Column
                {
                    column_title = columnModel.column_title
                };

                if (await _context.columns.AnyAsync(t => t.column_title == columnModel.column_title))
                {
                    return BadRequest("This column already exists!");
                }

                _context.columns.Add(column);
                await _context.SaveChangesAsync();
                return CreatedAtRoute("CreateColumn", new { id = column.ID }, column);
                //return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}", Name = "UpdateColumn")]
        public async Task<IActionResult> UpdateColumn(int id, [FromBody] Column columnModel)
        {
            if (columnModel == null)
            {
                return BadRequest("Column's data is invalid.");
            }
            Column? column = await _context.FindAsync<Column>(id);
            if (column == null)
            {
                return NotFound("Column not found!");
            }

            if (ModelState.IsValid)
            {
                column.column_title= columnModel.column_title;

                _context.columns.Update(column);
                await _context.SaveChangesAsync();
                return Ok(column);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}", Name = "DeleteColumn")]
        public async Task<IActionResult> DeleteColumn(int id)
        {
            Column? column = await _context.FindAsync<Column>(id);
            if (column == null)
            {
                return NotFound("Column not found!");
            }

            _context.columns.Remove(column);
            await _context.SaveChangesAsync();
            return Ok("Column deleted.");
        }
    }
}
