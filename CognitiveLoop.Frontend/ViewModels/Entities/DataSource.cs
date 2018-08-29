namespace FluentTest.ViewModels.Entities
{
    public class DataSource
    {
        public string Name { get; set; }

        public DataSource(string v)
        {
            this.Name = v;
        }

        public DataSource()
        {
        }
    }
}