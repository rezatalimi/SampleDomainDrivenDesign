namespace Sample.Commons.Abstracts
{
    public abstract class FilterQuery
    {
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public MetaData MetaData { get; set; } = new MetaData();
    }

    public abstract class ResultQuery
    {

    }
}
