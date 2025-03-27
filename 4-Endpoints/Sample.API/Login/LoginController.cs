using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Application.Contracts.Users;
using Sample.Application.Contracts.Users.Queries;
using Sample.Commons;
using Sample.Commons.Extensions;
using Sample.Configuration;
using Sample.Configuration.Authentication;
using Sample.Domain.Users;

namespace Sample.API.Login
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GeneralSettings _generalSettings;
        private readonly AccessManagementInMemory _accessManagementInMemory;
        private readonly IUserRepository _userRepository;

        public LoginController(IDistributor distributor,
            IHttpContextAccessor httpContextAccessor,
            GeneralSettings generalSettings,
            AccessManagementInMemory accessManagementInMemory,
            IUserRepository userRepository) : base(distributor)
        {
            _httpContextAccessor = httpContextAccessor;
            _generalSettings = generalSettings;
            _accessManagementInMemory = accessManagementInMemory;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<TokenModel> SignIn([FromBody]LoginDto loginDto)
        {
            var filter = new GetUserByUsernameFilterQuery { Username = loginDto.Username };

            Domain.Users.User.GuardForUsername(loginDto.Username);

            var user  = await _userRepository.GetByUsername(loginDto.Username);

            if (user == null) throw new Exception("Login failed!");

            var password = loginDto.Password.GetHashPassword(_generalSettings.Salt);

            if (user.Password != password) throw new Exception("Login failed!");

            var userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];

               var acc = _accessManagementInMemory.RegisterAccessInformation(user, "local", userAgent);

            var tokenModel = new TokenModel { Token = acc.Token.Key, FullName = acc.User.Fullname };

            return tokenModel;
        }
    }
}
