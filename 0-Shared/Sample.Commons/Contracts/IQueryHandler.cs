using Sample.Commons.Abstracts;

namespace Sample.Commons.Contracts
{
    public interface IQueryHandler<F,R> where F : FilterQuery where R : ResultQuery
    {
        Task<R> ExecuteAsync(F filter);
    }
}
