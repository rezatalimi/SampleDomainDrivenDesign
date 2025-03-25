using Sample.Commons.Abstracts;

namespace Sample.Domain.Users.Exceptions
{
    public class ThePasswordIsInvalidException : BusinessException
    {
        public ThePasswordIsInvalidException() : base(1)
        {

        }

        public override string Message => "The password is invalid";
    }
}
