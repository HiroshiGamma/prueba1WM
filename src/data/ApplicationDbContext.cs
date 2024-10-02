using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.src.models;

namespace api.src.data
{
    public class ApplicationDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions) 
    {
        public DbSet<User> Users {get;set;} = null!;
        
    }
}