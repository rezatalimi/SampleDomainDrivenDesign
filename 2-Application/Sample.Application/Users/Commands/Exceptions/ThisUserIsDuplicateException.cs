using Sample.Commons.Abstracts;

namespace Sample.Application.Users.Commands.Exceptions
{
    public class ThisUserIsDuplicateException : BusinessException
    {
        public ThisUserIsDuplicateException() : base(3)
        {
        }

        public override string Message => "This Username Is duplicate.";
    }
}
