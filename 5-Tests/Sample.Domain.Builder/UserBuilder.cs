using Sample.Commons.Enums;
using Sample.Domain.Users;

namespace Sample.Domain.Builder
{
    public class UserBuilder
    {
        private Guid _id = Guid.Empty;
        private string _fullName = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private UserRole _role;

        public UserBuilder()
        {
            _id = Guid.NewGuid();
            _fullName = "FullName";
            _username = "Username";
            _password = "P@ssword123";
            _role = UserRole.Public;
        }

        public UserBuilder SetfullName(string fullName)
        {
            _fullName = fullName;
            return this;
        }

        public UserBuilder SetUsername(string username)
        {
            _username = username;
            return this;
        }

        public UserBuilder SetPassword(string password)
        {
            _password = password;
            return this;
        }

        public User Build()
        {
            return new User(_id, _role, _username, _password, _fullName);
        }
    }
}
