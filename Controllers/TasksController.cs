using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_final_prog2.Domain.Entities;
using proyecto_final_prog2.Infrastructure;
using proyecto_final_prog2.Infrastructure.Models;

namespace proyecto_final_prog2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly AppDBContext _context;
        public TasksController(ILogger<TasksController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        [HttpGet(Name = "GetTasks")]
        public IEnumerable<Domain.Entities.Task> Get()
        {
            return _context.tasks.ToList();
        }

        [HttpPost(Name = "CreateTask")]
        public async Task<IActionResult> CreateTask([FromBody] Domain.Entities.Task taskModel) {
            if (taskModel == null)
            {
                return BadRequest("Task's data is invalid.");
            }
            if (ModelState.IsValid)
            {
                Domain.Entities.Task tsk = new Domain.Entities.Task
                {
                    text = taskModel.text,
                    title = taskModel.title
                };

                _context.tasks.Add(t);
                await _context.SaveChangesAsync();
                return CreatedAtRoute("CreateTask", new {id = t.ID}, t);
                //return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}",Name = "UpdateTask")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Domain.Entities.Task taskModel)
        {
            if (taskModel == null) 
            {
                return BadRequest("Task's data is invalid.");
            }
            Domain.Entities.Task? task = await _context.FindAsync<Domain.Entities.Task>(id);
            if (task == null)
            {
                return NotFound("Task not found!");
            }

            if (ModelState.IsValid)
            {
                task.text = taskModel.text;
                task.title = taskModel.title;
                task.tags = taskModel.tags;

                _context.tasks.Update(task);
                await _context.SaveChangesAsync();
                return Ok(task);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}", Name = "DeleteTask")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            Domain.Entities.Task? task = await _context.FindAsync<Domain.Entities.Task>(id);
            if (task == null)
            {
                return NotFound("Task not found!");
            }

            _context.tasks.Remove(task);
            await _context.SaveChangesAsync();
            return Ok("Task deleted.");
        }

    }
}
