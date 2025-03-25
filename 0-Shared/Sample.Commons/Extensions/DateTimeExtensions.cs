namespace Sample.Commons.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsExpired(this DateTime value)
        {
            return value < DateTime.Now;
        }
    }
}
