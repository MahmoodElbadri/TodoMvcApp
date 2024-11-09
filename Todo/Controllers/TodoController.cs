using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Todo.Models;
using Todo.Models.DTO;
using Todo.RepositryContracts;

namespace Todo.Controllers;
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
        return View(todoTasks);
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
            return Json("NotFound");
        }
        else
        {
            TodoTaskResponse todo = _mapper.Map<TodoTaskResponse>(todoTask);
            return View(todo);
        }
    }

}
