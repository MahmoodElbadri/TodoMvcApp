using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Models.DTO;
public class TodoTaskUpdateRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}
