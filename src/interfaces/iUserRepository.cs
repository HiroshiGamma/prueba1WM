using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.dtos;
using api.src.models;

namespace api.src.interfaces
{
    public interface iUserRepository
    {
        Task<List<User>> GetAll();

        Task<User> CreateUser(User user);

        Task<User?> GetByRut(string rut);

        Task<User?> Delete(int id);

        Task<User?> Put(int id, UpdateUserDto userDto);
        
    }
}