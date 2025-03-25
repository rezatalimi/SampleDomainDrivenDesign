using Sample.Commons.Abstracts;

namespace Sample.Application.Users.Commands.Exceptions
{
    public class ThePasswordIsInvalidException : BusinessException
    {
        public ThePasswordIsInvalidException() : base(1)
        {

        }

        public override string Message => "The password is invalid";
    }
}
