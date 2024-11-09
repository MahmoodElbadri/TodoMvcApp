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
    [Display(Name = "Task Title")]
    public string? Title { get; set; }
    [Required]
    [MaxLength(500)]
    [Display(Name = "Task Description")]
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public string? UserId { get; set; } 
    public ApplicationUser? User { get; set; }
    public string? ImageUrl { get; set; }
}
