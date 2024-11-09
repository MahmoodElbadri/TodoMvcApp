using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Todo.Filters;
using Todo.Models;
using Todo.Models.DTO;
using Todo.RepositryContracts;

namespace Todo.Controllers;

[Authorize]
public class TodoController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public TodoController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        IEnumerable<TodoTask> todoTasks = await _unitOfWork.Todo.GetAll();
        todoTasks = todoTasks.Where(u => u.UserId == userId && !u.IsCompleted);
        var todo = _mapper.Map<IEnumerable<TodoTaskResponse>>(todoTasks);
        return View(todo);
    }

    [HttpGet]
    public IActionResult AddTodo()
    {
        return View();
    }

    /// <summary>
    /// Add todo to the list of tasks
    /// </summary>
    /// <param name="todoTaskAddRequest"></param>
    /// <returns>Task Response</returns>
    [HttpPost]
    public async Task<IActionResult> AddTodo(TodoTaskAddRequest todoTaskAddRequest, IFormFile? file)
    {
        todoTaskAddRequest.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            // Handle file upload
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images/todos");

                // Delete existing image if updating
                if (!string.IsNullOrEmpty(todoTaskAddRequest.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, todoTaskAddRequest.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save new image file
                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream); // Use async for better performance
                }

                todoTaskAddRequest.ImageUrl = @"/images/todos/" + fileName;
            }

            TodoTask todoTask = _mapper.Map<TodoTask>(todoTaskAddRequest);
            await _unitOfWork.Todo.Add(todoTask);
            await _unitOfWork.Save();
            TempData["Success"] = "Task added successfully";
            return RedirectToAction("Index");
        }

        TempData["Error"] = "Something went wrong";
        return View(todoTaskAddRequest);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        TodoTask? todoTask = await _unitOfWork.Todo.Find(u => u.Id == id);
        if (todoTask == null)
        {
            TempData["error"] = "Task not found";
            return RedirectToAction("Index");
        }

        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (todoTask.UserId != currentUserId)
        {
            TempData["error"] = "Unauthorized Access";
            return RedirectToAction("Index");
        }

        TodoTaskResponse todo = _mapper.Map<TodoTaskResponse>(todoTask);
        return View(todo);
    }

    [HttpPost]
    [ValidatingModel]
    public async Task<IActionResult> Edit(int id, TodoTaskUpdateRequest todoTaskUpdateRequest, IFormFile? file)
    {
        if (!ModelState.IsValid)
        {
            TempData["error"] = "Invalid input data.";
            return View(todoTaskUpdateRequest);
        }

        string wwwRootPath = _webHostEnvironment.WebRootPath;

        // Handle the uploaded file (if any)
        if (file != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath, @"images/todos");

            // Check and delete the old image if updating
            if (!string.IsNullOrEmpty(todoTaskUpdateRequest.ImageUrl))
            {
                var oldImagePath = Path.Combine(wwwRootPath, todoTaskUpdateRequest.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream); // Async file copy
            }

            todoTaskUpdateRequest.ImageUrl = @"/images/todos/" + fileName;
        }

        // Step 3: Find and validate the task
        TodoTask? todoTask = await _unitOfWork.Todo.Find(u => u.Id == id);
        if (todoTask == null)
        {
            TempData["error"] = "Task not found.";
            return RedirectToAction("Index");
        }

        // Validate user ownership
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (todoTask.UserId != currentUserId)
        {
            TempData["error"] = "Unauthorized Access.";
            return Json("Unauthorized");
        }

        // Check if the task is completed
        if (todoTask.IsCompleted)
        {
            TempData["error"] = "Task is already completed and cannot be edited.";
            return View("Index");
        }

        // Step 6: Update the task
        _mapper.Map(todoTaskUpdateRequest, todoTask);
        TodoTask? updatedTask = await _unitOfWork.Todo.Update(todoTask);
        await _unitOfWork.Save();

        // Step 7: Redirect to the updated task or task list
        TodoTaskResponse response = _mapper.Map<TodoTaskResponse>(updatedTask);
        return RedirectToAction(nameof(TodoController.Index), "Todo");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        TodoTask? todoTask = await _unitOfWork.Todo.Find(u => u.Id == id);
        if (todoTask == null)
        {
            return Json("NotFound");
        }

        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (todoTask.UserId != currentUserId)
        {
            return Json("Unauthorized");
        }

        if (todoTask.IsCompleted)
        {
            return Json("Task is already completed");
        }

        TodoTaskUpdateRequest updateRequest = _mapper.Map<TodoTaskUpdateRequest>(todoTask);
        return View(updateRequest);
    }

    [HttpGet]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        TodoTask? todoTask = await _unitOfWork.Todo.Find(u => u.Id == id);
        if (todoTask == null)
        {
            TempData["error"] = "Task not found";
            return RedirectToAction("Index");
        }

        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (todoTask.UserId != currentUserId)
        {
            TempData["error"] = "Unauthorized Access";
            return RedirectToAction("Index");
        }

        TodoTaskResponse todo = _mapper.Map<TodoTaskResponse>(todoTask);
        return View(todo);
    }

    [HttpPost]
    [ActionName("ChangeStatus")]
    public async Task<IActionResult> ChangeTaskStatus(int id, TodoTaskUpdateRequest model)
    {
        TodoTask? todoTask = await _unitOfWork.Todo.Find(u => u.Id == id);
        if (todoTask != null)
        {
            todoTask.IsCompleted = model.IsCompleted;
            await _unitOfWork.Todo.Update(todoTask);
            await _unitOfWork.Save();

            TempData["Success"] = "Task status updated successfully";
            return RedirectToAction("Index");
        }

        TempData["Error"] = "Something went wrong";
        return RedirectToAction("Index");
    }
}
