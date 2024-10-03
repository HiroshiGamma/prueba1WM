using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.dtos
{
    public class CreateUserRequestDto
    {

        [Required]
        public String Rut {get;set;} = string.Empty;

        [Required]
        public String Nombre {get;set;} = string.Empty;

        [Required]
        public String Correo {get;set;} = string.Empty;

        [Required]
        public String Genero {get;set;} = string.Empty;

        [Required]
        public String FechaNacimiento {get;set;} = string.Empty;
    }
}