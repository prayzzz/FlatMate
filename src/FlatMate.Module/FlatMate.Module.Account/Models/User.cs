using System.ComponentModel.DataAnnotations;

namespace FlatMate.Module.Account.Models
{
    public class User
    {
        public string Email { get; set; }
        public int Id { get; set; }

        public string LastName { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }
    }

    public class LoginUser
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }

    public class RegisterUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }

        [Required]
        public string UserName { get; set; }
    }

    public class UserDbo
    {
        public string Email { get; set; }

        [Key]
        public int Id { get; set; }

        public string LastName { get; set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public string UserName { get; set; }
    }
}
