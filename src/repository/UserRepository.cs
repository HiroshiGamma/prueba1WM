using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using api.src.interfaces;
using api.src.models;
using Microsoft.EntityFrameworkCore;

namespace api.src.repository
{
    public class UserRepository : iUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context){

            _context = context;
        }
        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
    }
}