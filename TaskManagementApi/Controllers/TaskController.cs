// Controllers/TaskController.cs
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET /tasks
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        // GET /tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound(new { message = "Task not found" });
            return Ok(task);
        }

        // POST /tasks
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Task task)
        {
            if (task == null || string.IsNullOrEmpty(task.Title) || task.DueDate == DateTime.MinValue)
                return BadRequest(new { message = "Invalid task data" });

            var createdTask = await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }

        // PUT /tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Task updatedTask)
        {
            var task = await _taskService.UpdateTaskAsync(id, updatedTask);
            if (task == null) return NotFound(new { message = "Task not found" });

            return Ok(task);
        }

        // DELETE /tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var success = await _taskService.DeleteTaskAsync(id);
            if (!success) return NotFound(new { message = "Task not found" });

            return NoContent();
        }
    }
}
