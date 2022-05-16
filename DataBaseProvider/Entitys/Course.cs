namespace DataBaseProvider.Entitys
{
    public class Course
	{
		public int id { get; set; }
		public string Titlle { get; set; }
		public string Type { get; set; }

        public override string ToString()
        {
            return $"{Titlle} {Type}";
        }
    }
}
