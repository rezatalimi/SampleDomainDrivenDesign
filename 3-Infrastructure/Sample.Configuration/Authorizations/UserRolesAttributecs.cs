using Sample.Commons.Enums;

namespace Sample.Configuration.Authorizations
{
    [AttributeUsage(AttributeTargets.All)]
    public abstract class UserRolesAttribute : Attribute
    {
        public UserRolesAttribute()
        {
        }

        public UserRole[] AcceptableRoles { get; set; }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class AdminUserRolesAttribute : UserRolesAttribute
    {
        public AdminUserRolesAttribute()
        {
            AcceptableRoles = new UserRole[]
            {
                UserRole.Admin,
            };
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class AllUserRolesAttribute : UserRolesAttribute
    {
        public AllUserRolesAttribute()
        {
            AcceptableRoles = new UserRole[]
            {
                UserRole.Admin,
                UserRole.Public,
            };
        }
    }
}
