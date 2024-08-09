using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_final_prog2.Application.Dtos.Columns;
using proyecto_final_prog2.Application.Dtos.Tags;
using proyecto_final_prog2.Application.Services;
using proyecto_final_prog2.Domain;
using proyecto_final_prog2.Domain.Entities;
using proyecto_final_prog2.Infrastructure;
using proyecto_final_prog2.Infrastructure.Models;

namespace proyecto_final_prog2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        private readonly ILogger<ColumnsController> _logger;
        //private readonly AppDBContext _context;
        private readonly ColumnService _service;
        public ColumnsController(ILogger<ColumnsController> logger, ColumnService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet(Name = "GetColumns")]
        public async Task<IEnumerable<Column>> Get()
        {
            return await _service.GetColumns();
        }
        [HttpGet("{id}", Name = "GetColumn")]
        public async Task<IndexColumnDto?> Get(int id)
        {
            return await _service.GetColumn(id);
        }

        [HttpGet("ID/{title}", Name = "GetColumnIDUsingTitle")]
        public async Task<int?> Get(string title)
        {
            return await _service.GetColumnID(title);
        }

        //GetColumnID

        [HttpPost(Name = "CreateColumn")]
        public async Task<IActionResult> CreateColumn([FromBody] ColumnModel columnModel)
        {
            if (columnModel == null)
            {
                return BadRequest("Column's data is invalid.");
            }
            if (ModelState.IsValid)
            {
                Column col_created = await _service.CreateColumn(new CreateColumnDto { column_title = columnModel.column_title });
                return CreatedAtRoute("CreateColumn", new { id = col_created.ID }, col_created);
                //return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}", Name = "UpdateColumn")]
        public async Task<IActionResult> UpdateColumn(int id, [FromBody] ColumnModel columnModel)
        {
            if (columnModel == null)
            {
                return BadRequest("Column's data is invalid.");
            }

            if (ModelState.IsValid)
            {
                Column? column = await _service.UpdateColumn(id, new Column { column_title = columnModel.column_title, ID = id });
                if (column == null)
                {
                    return NotFound("Column not found!");
                }
                return Ok(column);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}", Name = "DeleteColumn")]
        public async Task<IActionResult> DeleteColumn(int id)
        {
            Column? column = await _service.DeleteColumn(id);
            return Ok("Column deleted.");
        }
    }
}
