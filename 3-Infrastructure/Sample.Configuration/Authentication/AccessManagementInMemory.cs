using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.Commons;
using Sample.Commons.Extensions;
using Sample.Data;
using Sample.Data.Tokens;
using Sample.Domain.Users;

namespace Sample.Configuration.Authentication
{
    public class AccessManagementInMemory
    {
        List<AccessInformation> _listOfAccessInformation;

        readonly IServiceScopeFactory _serviceScopeFactory;

        readonly GeneralSettings _generalSettings;

        public AccessManagementInMemory(IServiceScopeFactory scopeFactory,
            GeneralSettings generalSettings)
        {
            _listOfAccessInformation = new List<AccessInformation>();

            _serviceScopeFactory = scopeFactory;

            _generalSettings = generalSettings;
        }

        public async Task LoadTokens()
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<SampleDataBase>();

            var tokens = await db.Tokens.Where(x => x.ExpiryDateTime > DateTime.Now).ToListAsync();

            var userIds = tokens.Select(x => x.UserId).ToList();

            var users = await db.Users.Where(x => userIds.Contains(x.Id) == true).ToListAsync();

            foreach (var token in tokens)
            {
                var user = users.First(x => x.Id == token.UserId);

                var accessInformation = new AccessInformation(token, user);

                _listOfAccessInformation.Add(accessInformation);
            }

            var removeTokens = await db.Tokens.Where(x => x.ExpiryDateTime < DateTime.Now).ToListAsync();

            db.Tokens.RemoveRange(removeTokens);

            await db.SaveChangesAsync();
        }

        public AccessInformation GetAccessInformationByToken(string tokenKey)
        {
            var accessInformation = _listOfAccessInformation.
                FirstOrDefault(x => x.Token.Key == tokenKey);

            if (accessInformation == null) return null;

            var token = accessInformation.Token;

            if (token.ExpiryDateTime.IsExpired() == true)
            {
                DeleteAccessInformation(accessInformation);

                return null;
            }

            UpdateToken(accessInformation, tokenKey);

            return accessInformation;
        }

        public AccessInformation GetAccessInformationByTokenAndSystemInformation(string tokenKey, string systemInformation)
        {
            var accessInformation = _listOfAccessInformation.
                FirstOrDefault(x => x.Token.Key == tokenKey);

            if (accessInformation == null) return null;

            if (accessInformation.Token.ExpiryDateTime.IsExpired() == true)
            {
                DeleteAccessInformation(accessInformation);

                return null;
            }

            UpdateToken(accessInformation, tokenKey);

            return accessInformation;
        }

        void UpdateToken(AccessInformation accessInformation, string tokenKey)
        {
            var token = accessInformation.Token;

            if (token.ExpiryDateTime.AddMinutes(-1) >= DateTime.Now) return;

            token.UpdateExpiryDateTime(_generalSettings.TokenExpireMinute);

            using var scope = _serviceScopeFactory.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<SampleDataBase>();

            var found = db.Tokens.AsNoTracking().FirstOrDefault(x => x.Id == token.Id);

            if (found != null)
            {
                found.UpdateExpiryDateTime(_generalSettings.TokenExpireMinute);
                db.Tokens.Update(token);
                db.SaveChanges();
            }
        }

        void DeleteAccessInformation(AccessInformation accessInformation)
        {
            if (accessInformation != null)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<SampleDataBase>();

                    var found = db.Tokens.FirstOrDefault(x => x.Id == accessInformation.Token.Id);

                    if (found != null)
                    {
                        db.Tokens.Remove(found);
                        db.SaveChanges();
                    }
                }
                _listOfAccessInformation.Remove(accessInformation);
            }
        }

        public async Task DeleteAccessInformationByToken(string token)
        {
            if (string.IsNullOrEmpty(token) == false)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<SampleDataBase>();

                    var found = await db.Tokens.FirstOrDefaultAsync(x => x.Key == token);

                    if (found != null)
                    {
                        db.Tokens.Remove(found);

                        db.SaveChanges();

                        var acc = _listOfAccessInformation.First(x => x.Token.Key == token);

                        _listOfAccessInformation.Remove(acc);
                    }
                }
            }
        }

        public AccessInformation RegisterAccessInformation(User user, string issuer, string systemInformation)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var dbToken = scope.ServiceProvider.GetRequiredService<SampleDataBase>();

            var token = dbToken.Tokens.FirstOrDefault(x => x.UserId == user.Id);

            var foundAccessInformation = _listOfAccessInformation.FirstOrDefault(x => x.User.Id == user.Id);

            if (foundAccessInformation != null)
            {
                _listOfAccessInformation.Remove(foundAccessInformation);
            }

            string key = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();

            var tokenExpireDatetime = DateTime.Now.AddMinutes(_generalSettings.TokenExpireMinute);

            token = new Token(user.Id, key, tokenExpireDatetime);

            dbToken.Tokens.Add(token);

            dbToken.SaveChanges();

            var accessInformation = new AccessInformation(token, user);

            _listOfAccessInformation.Add(accessInformation);

            return accessInformation;

        }

        public void ReloadUser(string tokenKey)
        {
            var accessInformation = _listOfAccessInformation.FirstOrDefault(x => x.Token.Key == tokenKey);

            using var scope = _serviceScopeFactory.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<SampleDataBase>();

            var user = db.Users.First(x => x.Id == accessInformation.User.Id);

             accessInformation.User = user;
        }
    }
}
