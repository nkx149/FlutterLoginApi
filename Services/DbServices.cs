using LoginApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace LoginApi.Services
{
    public class DbServices
    {
        private readonly UserDbContext _context;
        private readonly PasswordHasher<Users> _passwordHasher = new PasswordHasher<Users>();
        private readonly JwtTokenService _jwtTokenService;

        public DbServices(UserDbContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<UserResponseDto> SignupUserAsync(UserSignUpDto dto)
        {

            var _usernameExists = await _context.Users.AnyAsync(u => u.Username == dto.Username);
            var _emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);

            if (_usernameExists) {
                throw new ApplicationException("Username is already taken");
            }
            if (_emailExists) {
                throw new ApplicationException("Email is already registered");
            }

            var newUser = new Users
            {
                Username = dto.Username,
                Email = dto.Email,
       
            };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                DateRegistered = newUser.DateRegistered
            };
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                DateRegistered = user.DateRegistered
            };
        }

        public async Task<LoginResponseDto> LoginUserAsync(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null) {
                throw new UnauthorizedAccessException("Username incorrect or doesn't exist");
            }

            var hashCheck = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (hashCheck != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException("Password incorrect");
            }
            var token = _jwtTokenService.GenerateJwtToken(dto.Username);

            return new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Id = user.Id,
                DateRegistered = user.DateRegistered

            };
        }
        
    }
}
