using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginApi.Models;
using LoginApi.Services;
using System.Drawing.Text;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace LoginApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class LoginController : ControllerBase
    {
        private readonly DbServices _dbservice;

        public LoginController(DbServices services) { 
            _dbservice = services;

        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<UserResponseDto>> PostUserSignup([FromBody] UserSignUpDto dto)
        {
            try
            {
                var user = await _dbservice.SignupUserAsync(dto);

                var response = new UserResponseDto
                { 
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    DateRegistered = user.DateRegistered
                };

                return CreatedAtAction(nameof(GetUserById), new {id = response.Id}, response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginResponseDto>> PostUserLogin([FromBody] UserLoginDto dto)
        {
            try{
                var result = await _dbservice.LoginUserAsync(dto);
                return Ok(result);
            }catch (UnauthorizedAccessException ex) {
                return Unauthorized(new {message = ex.Message});
            }
        }

        [HttpGet]
        public Task<ActionResult<Users>> GetUsers()
        {
            return default;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            try
            {
                var user = await _dbservice.GetUserByIdAsync(id);
                var response = new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    DateRegistered = user.DateRegistered
                };

                return Ok(response);
            }
            catch (KeyNotFoundException ex) {
                return NotFound();
            }
        }

        [HttpPut]
        public Task<ActionResult<Users>> UpdateUserDetails()
        {
            return default;
        }

        [HttpDelete]
        public Task<ActionResult<Users>> DeleteUser (int id)
        {
            return default;
        }
    }
}
