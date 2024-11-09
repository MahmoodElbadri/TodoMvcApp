using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.RepositryContracts;
public interface IUnitOfWork
{
    ITodoTaskRepository Todo {  get; }
    Task Save();
}
