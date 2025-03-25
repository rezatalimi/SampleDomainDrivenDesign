using FluentAssertions;
using Sample.Domain.Builder;
using Sample.Domain.Users.Exceptions;

namespace Sample.Domain.Tests
{
    public class UnitTestUser
    {
        [Theory]
        [InlineData("")]
        [InlineData("aa")]
        [InlineData("qwertyuiopasdfgh")]
        public void Should_Be_Throw_Exception_When_Username_Is_Invalid(string username)
        {
            //arenge
            var userBuilder = new UserBuilder().SetUsername(username);
            //action
            Action act = () => userBuilder.Build();
            //assert
            act.Should().Throw<TheUsernameIsInvalidException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("aa")]
        [InlineData("password")]
        public void Should_Be_Throw_Exception_When_Password_Is_Invalid(string password)
        {
            //arenge
            var userBuilder = new UserBuilder().SetPassword(password);
            //action
            Action act = () => userBuilder.Build();
            //assert
            act.Should().Throw<ThePasswordIsInvalidException>();
        }
    }
}