namespace Sample.Commons.Abstracts
{
    public abstract class Command
    {
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public MetaData MetaData { get; set; } = new MetaData();
    }
}
