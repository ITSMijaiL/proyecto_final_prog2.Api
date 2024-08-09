using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_final_prog2.Application.Services;
using proyecto_final_prog2.Infrastructure.Models;
using proyecto_final_prog2.Application.Dtos.Tasks;
using proyecto_final_prog2.Application.Dtos.Tags;

namespace proyecto_final_prog2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly TaskService _service;
        public TasksController(ILogger<TasksController> logger, TaskService service)
        {
            _logger = logger;
            _service = service;
        }
        
        [HttpGet(Name = "GetTasks")]
        public async Task<IEnumerable<Domain.Entities.Task>> Get()
        {
            return await _service.GetTasks();
        }

        [HttpGet("ID/{task_title}",Name = "GetTaskIDUsingTitle")]
        public async Task<int?> Get(string task_title)
        {
            return (await _service.GetTaskID(task_title))??-1;
        }
        //GetTaskID
        [HttpGet("{id}",Name = "GetTasksFromColumn")]
        //public async Task<IndexTaskDto?> Get(int id)
        public async Task<IEnumerable<Domain.Entities.Task>> Get(int id)
        {
            return await _service.GetTasksFromColumn(id);
        }

        [HttpPost("{column_name}", Name = "CreateTask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel taskModel, string column_name) {
            if (taskModel == null)
            {
                return BadRequest("Task's data is invalid.");
            }
            if (ModelState.IsValid)
            {
                Domain.Entities.Task tsk = await _service.CreateTask(new CreateTaskDto { text = taskModel.text, title = taskModel.title}, column_name);

                return CreatedAtRoute("CreateTask", new {id = tsk.ID}, tsk);
                //return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}",Name = "UpdateTask")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskModel taskModel)
        {
            if (taskModel == null) 
            {
                return BadRequest("Task's data is invalid.");
            }

            if (ModelState.IsValid)
            {
                Domain.Entities.Task? task = await _service.UpdateTask(id, new UpdateTaskDto { text = taskModel.text, title = taskModel.title });
                if (task == null)
                {
                    return NotFound("Task not found!");
                }
                return Ok(task);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}/{column_id}", Name = "UpdateTaskColumn")]
        public async Task<IActionResult> UpdateTask(int id, int column_id)
        {

            if (ModelState.IsValid)
            {
                Domain.Entities.Task? task = await _service.UpdateTaskColumn(id, column_id);
                if (task == null)
                {
                    return NotFound("Task not found!");
                }
                return Ok(task);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}", Name = "DeleteTask")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            Domain.Entities.Task? task = await _service.DeleteTask(id);
            return Ok("Task deleted.");
        }
        
    }
}
