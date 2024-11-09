using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Models;
using Todo.Models.DTO;
using Todo.RepositryContracts;

namespace Todo.Controllers;
[Authorize]
public class TodoController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TodoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task <IActionResult> Index()
    {
        IEnumerable<TodoTask> todoTasks = await _unitOfWork.Todo.GetAll();
        todoTasks = todoTasks.Where(u => u.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
        todoTasks = todoTasks.Where(u => u.IsCompleted == false);
        var todo = _mapper.Map<IEnumerable<TodoTaskResponse>>(todoTasks);
        return View(todo);
    }

    [HttpGet]
    public IActionResult AddTodo()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddTodo(TodoTaskAddRequest todoTaskAddRequest)
    {
        todoTaskAddRequest.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ??"" ;
        if (ModelState.IsValid)
        {
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
    public async Task <IActionResult> Details(int id)
    {
       TodoTask? todoTask = await _unitOfWork.Todo.Find(u => u.Id == id);
        if(todoTask == null)
        {
            TempData["error"] = "Task not found";
            return RedirectToAction("Index");
        }
        else
        {
            TodoTaskResponse todo = _mapper.Map<TodoTaskResponse>(todoTask);
            return View(todo);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, TodoTaskUpdateRequest todoTaskUpdateRequest)
    {
        // Step 1: Validate incoming data (usually happens with POST, not GET)
        if (!ModelState.IsValid)
        {
            return View(todoTaskUpdateRequest); // Return with validation errors if invalid
        }

        // Step 2: Find the task
        TodoTask? todoTask = await _unitOfWork.Todo.Find(u => u.Id == id);
        if (todoTask == null)
        {
            return Json("NotFound"); // Return 404 if the task doesn't exist
        }

        // Step 3: Validate user ownership (check if the logged-in user is the owner)
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (todoTask.UserId != currentUserId)
        {
            return Json("Unauthorized"); // Return 403 if the user doesn't own the task
        }

        // Step 4: Check if the task is already completed (business rule)
        if (todoTask.IsCompleted)
        {
            return Json("Task is already completed"); // Return 400 if the task is completed
        }

        // Step 5: Update the task
        _mapper.Map(todoTaskUpdateRequest, todoTask); // Map incoming data onto the existing task
        TodoTask? updatedTask = await _unitOfWork.Todo.Update(todoTask);
        await _unitOfWork.Save();

        // Step 6: Return response or redirect
        TodoTaskResponse response = _mapper.Map<TodoTaskResponse>(updatedTask);
        return RedirectToAction(nameof(TodoController.Index), "Todo"); // Or redirect based on your business flow
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        TodoTask? todoTask = await _unitOfWork.Todo.Find(u => u.Id == id);
        if (todoTask == null)
        {
            return Json("NotFound"); // Return 404 if the task doesn't exist
        }

        // Step 3: Validate user ownership (check if the logged-in user is the owner)
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (todoTask.UserId != currentUserId)
        {
            return Json("Unauthorized"); // Return 403 if the user doesn't own the task
        }

        // Step 4: Check if the task is already completed (business rule)
        if (todoTask.IsCompleted)
        {
            return Json("Task is already completed"); // Return 400 if the task is completed
        }

        // Step 5: Update the task
       TodoTaskUpdateRequest updateRequest = _mapper.Map<TodoTaskUpdateRequest>(todoTask); // Map incoming data onto the existing task
        
        return View(updateRequest);
    }

}
