using Sample.Commons.Abstracts;

namespace Sample.Data.Tokens
{
    public class Token : Entity
    {
        private Token()
        {
                
        }

        public Token(Guid userId,string key, DateTime expiryDateTime)
        {
            UserId = userId;
            Key = key;
            ExpiryDateTime = expiryDateTime;
        }

        public Guid UserId { get;private set; }
        public string Key { get;private set; }
        public DateTime ExpiryDateTime { get;private set; }

        public void UpdateExpiryDateTime(int tokenExpireMinute)
        {
            ExpiryDateTime = ExpiryDateTime.AddMinutes(tokenExpireMinute);
        }
    }
}
