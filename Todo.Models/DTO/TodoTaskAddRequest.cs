using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Models.DTO;
public class TodoTaskAddRequest
{
    [Required]
    [MaxLength(50)]
    public string? Title { get; set; }
    [Required]
    [MaxLength(2000)]
    public string? Description { get; set; }
    public string? UserId { get; set; }
    public string? ImageUrl { get; set; }
}
