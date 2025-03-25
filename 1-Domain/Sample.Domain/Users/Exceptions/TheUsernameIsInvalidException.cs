using Sample.Commons.Abstracts;

namespace Sample.Domain.Users.Exceptions
{
    public class TheUsernameIsInvalidException : BusinessException
    {
        public TheUsernameIsInvalidException() :base(1)
        {
                
        }

        public override string Message => "The username is invalid";
    }
}
