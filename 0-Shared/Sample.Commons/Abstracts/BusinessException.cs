namespace Sample.Commons.Abstracts
{
    public abstract class BusinessException : Exception
    {
        protected BusinessException(int code)
        {
            Code = code;
        }

        public int Code { get; set; }
    }
}
