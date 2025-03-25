using Sample.Commons.Abstracts;
using Sample.Commons.Enums;
using Sample.Commons.Extensions;
using Sample.Domain.Users.Exceptions;

namespace Sample.Domain.Users
{
    public class User : Entity
    {
        private User()
        {
                
        }

        public User(Guid id, UserRole role,string username, string password, string fullname)
        {
            GuardForUsername(username);
            GuardForPassword(password);

            Id = id;
            Role = role;
            Username = username;
            Password = password;
            Fullname = fullname;
        }

        public string Fullname { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }

        public static void GuardForUsername(string username)
        {
            if (username.IsNullOrEmpty())
                throw new TheUsernameIsInvalidException();

            if (username.Length < 5 || username.Length > 15)
                throw new TheUsernameIsInvalidException();
        }

        public void GuardForPassword(string password)
        {
            if (password.IsNullOrEmpty())
                throw new ThePasswordIsInvalidException();

            if (password.Length < 6 || password.Length > 500)
                throw new ThePasswordIsInvalidException();

            if (password.IsHash() == false)
                throw new ThePasswordIsInvalidException();
        }
    }
}
