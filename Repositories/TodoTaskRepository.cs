using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;
using Todo.RepositryContracts;

namespace Todo.Repositories;
public class TodoTaskRepository : Repository<TodoTask>, ITodoTaskRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TodoTaskRepository(ApplicationDbContext dbContext):base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TodoTask?> Update(TodoTask obj)
    {
        _dbContext.TodoTasks.Update(obj);
        await Task.CompletedTask;
        return obj;
    }
}
