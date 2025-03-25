namespace Sample.Commons.Abstracts
{
    public abstract class EntityDto
    {
        public DateTime CreateAt { get; set; }
        public string CreateAtDisplay { get; set; } = string.Empty;
        public Guid CreateBy { get; set; }
        public string CreateByDisplay { get; set; } = string.Empty;
        public DateTime UpdateAt { get; set; }
        public string UpdateAtDisplay { get; set; } = string.Empty;
        public Guid UpdateBy { get; set; }
        public string UpdateByDisplay { get; set; } = string.Empty;
    }
}
