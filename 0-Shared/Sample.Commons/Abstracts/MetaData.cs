using Sample.Commons.Enums;

namespace Sample.Commons.Abstracts
{
    public class MetaData
    {
        public Guid CurentUserId { get; set; }
        public string CurentUserFullname { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
