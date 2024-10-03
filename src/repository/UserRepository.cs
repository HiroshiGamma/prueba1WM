using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using api.src.dtos;
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

        public async Task<User> CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

         public async Task<User?> GetByRut(string rut)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Rut == rut);
        }

        public async Task<User?> Delete(int id)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync( x => x.Id == id);
            if(userModel == null)
            {
                throw new Exception("Product not found");
            }

            _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();
            return userModel;

        }

                
        public async Task<User?> Put(int id, UpdateUserDto userDto)
        {
            // Buscar el usuario por ID
            var userModel = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userModel == null)
            {
                return null; // Si el usuario no se encuentra, devolver null
            }

            // Actualizar los campos del usuario
            userModel.Rut = userDto.Rut;
            userModel.Nombre = userDto.Nombre;
            userModel.Correo = userDto.Correo;
            userModel.Genero = userDto.Genero;
            
            // Convertir la fecha de nacimiento (debe estar validada antes)
            userModel.FechaNacimiento = DateTime.ParseExact(userDto.FechaNacimiento, "dd-MM-yyyy", null).ToString();

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();

            return userModel; // Retornar el modelo actualizado
        }



    }
}