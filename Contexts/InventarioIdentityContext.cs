using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using InventarioAPI.Models;
using Microsoft.EntityFrameworkCore;
using InventarioAPI.Controllers;

namespace InventarioAPI.Contexts 
{
    public class InventarioIdentityContext : IdentityDbContext<ApplicationUser>
    {
        public InventarioIdentityContext(DbContextOptions<InventarioIdentityContext> options) 
            : base(options)
        {

        }
    }
}
