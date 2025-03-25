namespace Sample.Commons.Abstracts
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime CreateAt { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid UpdateBy { get; set; }
    }
}
