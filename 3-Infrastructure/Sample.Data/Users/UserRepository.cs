using Microsoft.EntityFrameworkCore;
using Sample.Application.Contracts.Users;
using Sample.Commons.Extensions;
using Sample.Domain.Users;

namespace Sample.Data.Users
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(SampleDataBase db) : base(db)
        {
        }

        public async Task Add(User user)
        {
            user.CreateAt = DateTime.Now;
            DB.Users.Add(user);
            await DB.SaveChangesAsync();
            DB.ChangeTracker.Clear();
        }

        public async Task Update(User user)
        {
            user.UpdateAt = DateTime.Now;
            DB.Users.Update(user);
            await DB.SaveChangesAsync();
            DB.ChangeTracker.Clear();
        }

        public async Task Delete(User user)
        {
            user.UpdateAt = DateTime.Now;
            DB.Users.Update(user);
            await DB.SaveChangesAsync();
            DB.ChangeTracker.Clear();
        }

        public Task<User> GetById(Guid userID)
        {
            return DB.Users.FirstOrDefaultAsync(x => x.Id == userID);
        }

        public Task<User> GetByUsername(string username)
        {
           return DB.Users.FirstOrDefaultAsync(x=> x.Username == username); 
        }

        public Task<List<User>> GetUsersByFilters(UsersFilters usersFilters)
        {
            IQueryable<User> query = DB.Users;

            if(usersFilters.Username.IsNullOrEmpty() == false)
            {
                query = query.Where(x => x.Username.Contains(usersFilters.Username) == true);
            }

            if (usersFilters.Role.HasValue)
            {
                query = query.Where(x => x.Role == usersFilters.Role.Value);
            }

            return query.OrderByDescending(x => x.CreateAt).AsNoTracking().ToListAsync();
        }
    }
}
