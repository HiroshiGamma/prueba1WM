using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.dtos;
using api.src.interfaces;
using api.src.mappers;
using Microsoft.AspNetCore.Mvc;



namespace api.src.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly iUserRepository _userRepository;

        public UserController(iUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] string? sort = null, [FromQuery] string? genero = null)
        {
            var users = await _userRepository.GetAll();

            // Filtrar por género si se ha proporcionado
            if (!string.IsNullOrEmpty(genero))
            {
                if (genero != "masculino" && genero != "femenino" && genero != "otro" && genero != "prefiero no decirlo")
                    return BadRequest("Genero no valido. Utilice masculino, feminino, otro o prefiero no decirlo.");

                users = users.Where(u => u.Genero == genero).ToList();
            }

            // Aplicar la ordenación solo si el parámetro 'sort' está presente
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "asc")
                {
                    users = users.OrderBy(u => u.Nombre).ToList();
                }
                else if (sort == "desc")
                {
                    users = users.OrderByDescending(u => u.Nombre).ToList();
                }
                else
                {
                    return BadRequest("El parametro de ordenación es invalido. Use 'asc' o 'desc'.");
                }
            }

            // Convertir los usuarios a DTO antes de devolverlos
            var userDto = users.Select(u => u.toUserDto());
            return Ok(userDto);
        }


        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserRequestDto userDto)
        {
            

            
            if (userDto.Nombre.Length < 3 || userDto.Nombre.Length > 100)
                return BadRequest("El nombre debe tener entre 3 y 100 caracteres.");

            
            if (!IsValidEmail(userDto.Correo))
                return BadRequest("El correo electronico no es valido.");

            
            if (userDto.Genero != "masculino" && userDto.Genero != "femenino" && userDto.Genero != "otro" && userDto.Genero != "prefiero no decirlo")
                return BadRequest("El genero debe ser 'masculino', 'femenino', 'otro' o 'prefiero no decirlo'.");

            if (!DateTime.TryParseExact(userDto.FechaNacimiento, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime birthDate))
                return BadRequest("El formato de la fecha de nacimiento debe ser 'dd-MM-yyyy'.");

            if (birthDate >= DateTime.Now)
                return BadRequest("La fecha de nacimiento debe ser menor a la fecha actual.");

            var existingUser = await _userRepository.GetByRut(userDto.Rut);
            if (existingUser != null)
                return Conflict("El RUT ya existe.");


            var userModel = userDto.toUserFromUserDto();
            await _userRepository.CreateUser(userModel);

            return StatusCode(201, "Usuario creado exitosamente.");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateUserDto userDto)
        {
            // Validaciones antes de llamar al repositorio

            // Validar si el nombre es mayor a 3 y menor que 100 caracteres
            if (userDto.Nombre.Length < 3 || userDto.Nombre.Length > 100)
            {
                return BadRequest("El nombre debe tener entre 3 y 100 caracteres.");
            }

            // Validar si el correo electrónico es válido
            if (!IsValidEmail(userDto.Correo))
            {
                return BadRequest("El correo electrónico no es válido.");
            }

            // Validar el género
            if (userDto.Genero != "masculino" && userDto.Genero != "femenino" && userDto.Genero != "otro" && userDto.Genero != "prefiero no decirlo")
            {
                return BadRequest("El género debe ser 'masculino', 'femenino', 'otro' o 'prefiero no decirlo'.");
            }

            // Validar la fecha de nacimiento (formato dd-MM-yyyy)
            if (!DateTime.TryParseExact(userDto.FechaNacimiento, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime birthDate))
            {
                return BadRequest("El formato de la fecha de nacimiento debe ser 'dd-MM-yyyy'.");
            }

            if (birthDate >= DateTime.Now)
            {
                return BadRequest("La fecha de nacimiento debe ser menor a la fecha actual.");
            }

            // Actualizar el usuario usando el repositorio
            var updatedUser = await _userRepository.Put(id, userDto);
            
            if (updatedUser == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(updatedUser.toUserDto());
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
                var user = await _userRepository.Delete(id);
                if(user == null)
                {
                    return NotFound();
                }

                
                return NoContent();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }








    }
}