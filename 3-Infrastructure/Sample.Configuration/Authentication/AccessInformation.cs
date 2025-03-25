using Sample.Data.Tokens;
using Sample.Domain.Users;

namespace Sample.Configuration.Authentication
{
    public class AccessInformation
    {
        private AccessInformation()
        {

        }

        public AccessInformation(Token token, User user)
        {
            Token = token;
            User = user;
        }

        public Token Token { get; set; } 
        public User User { get; set; }
    }
}
