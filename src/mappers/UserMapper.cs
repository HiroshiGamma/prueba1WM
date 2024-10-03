using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.dtos;
using api.src.models;

namespace api.src.mappers
{
    public static class UserMapper
    {
        
        public static UserDto toUserDto(this User userModel)
        {
            return new UserDto
            {
                Id = userModel.Id,
                Rut = userModel.Rut,
                Nombre = userModel.Nombre,
                Correo = userModel.Correo,
                Genero = userModel.Genero,
                FechaNacimiento = userModel.FechaNacimiento

            };
        }

    }
}