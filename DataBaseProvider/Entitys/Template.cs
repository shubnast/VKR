namespace DataBaseProvider.Entitys
{
    public class Template
	{
		public int id { get; set; }
		public string Path { get; set; }

        public override string ToString()
        {
            return Path;
        }
    }
}
