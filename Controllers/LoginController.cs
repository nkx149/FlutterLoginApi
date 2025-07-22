using LoginApi.Models;
using LoginApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Drawing.Text;
using System.Security.Claims;

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

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<ActionResult<Users>> UpdateUserDetails(int id, [FromBody] UserUpdateDto dto) 
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var response = await _dbservice.UpdateUserDetailsAsync(dto);

            return response.Type switch
            {
                ServiceResultType.Ok => NoContent(),
                ServiceResultType.NotFound => NotFound(response.Error),
                ServiceResultType.BadRequest => BadRequest(response.Error),
                _ => StatusCode(500, "Unknown Error")
            };

        
        }

        [HttpDelete]
        public Task<ActionResult<Users>> DeleteUser (int id)
        {
            return default;
        }
    }
}
