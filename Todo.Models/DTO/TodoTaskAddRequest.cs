using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Models.DTO;
public class TodoTaskAddRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? UserId { get; set; }
}
