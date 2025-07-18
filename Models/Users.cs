using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginApi.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        public string Username { get; set; }
        
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTime DateRegistered { get; set; }
    }

    public class UserSignUpDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }



    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public DateTime DateRegistered { get; set; }
    }

    public class UserLoginDto 
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DateRegistered { get; set; }
    }

}
