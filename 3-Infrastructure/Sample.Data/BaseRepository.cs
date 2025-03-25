namespace Sample.Data
{
    public abstract class BaseRepository
    {
        public SampleDataBase DB { get; private set; }

        public BaseRepository(SampleDataBase db)
        {
            DB = db;
        }
    }
}
