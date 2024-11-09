using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models.IdentityModels;

namespace Todo.Models;

public class TodoTask
{
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string? Title { get; set; }
    [Required]
    [MaxLength(500)]
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public string? UserId { get; set; } 
    public ApplicationUser? User { get; set; }
}
