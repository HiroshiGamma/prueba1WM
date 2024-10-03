using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.dtos
{
    public class UserDto
    {
        public int Id {get;set;}

        public String Rut {get;set;} = string.Empty;

        public String Nombre {get;set;} = string.Empty;

        public String Correo {get;set;} = string.Empty;

        public String Genero {get;set;} = string.Empty;

        public String FechaNacimiento {get;set;} = string.Empty;

        
    }
}