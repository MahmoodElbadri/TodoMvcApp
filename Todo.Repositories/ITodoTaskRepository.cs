using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;
using Todo.Repositories;

namespace Todo.RepositryContracts;
public interface ITodoTaskRepository : IRepository<TodoTask>
{
    Task<TodoTask?> Update(TodoTask obj);
}
