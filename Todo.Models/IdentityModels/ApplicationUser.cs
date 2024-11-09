using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Todo.Models.IdentityModels;
public class ApplicationUser:IdentityUser
{
    public string? PersonName { get; set; }
}
