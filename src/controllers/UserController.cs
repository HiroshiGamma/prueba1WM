using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> GetAll()
        {
            var users = await _userRepository.GetAll();
            var userDto = users.Select(p => p.toUserDto());
            return Ok(userDto);
        }


    }
}