    using System.ComponentModel.DataAnnotations;

    namespace BrickOutApi.Models
    {
        public class Users
        {
            [Key]
            public int Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public int Score { get; set; }
            public string RefreshToken { get; set; }
            public DateTime RefreshTokenExpiryTime { get; set; }
        }
    }
