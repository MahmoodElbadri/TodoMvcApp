using Todo.Models;
using Todo.Repositories;
using Todo.RepositryContracts;

namespace Repositories;

public class UnitOfWork:IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public ITodoTaskRepository Todo { get; private set; }
    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Todo = new TodoTaskRepository(_db);
    }
    public async Task Save()
    {
      await  _db.SaveChangesAsync();
    }
}
