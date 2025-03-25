using Sample.Commons.Abstracts;

namespace Sample.Commons.Contracts
{
    public interface ICommandHandler<C> where C : Command
    {
        Task ExecuteAsync(C command);
    }
}
