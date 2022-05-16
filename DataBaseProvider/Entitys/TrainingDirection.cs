namespace DataBaseProvider.Entitys
{
    public class TrainingDirection
	{
		public int id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }

        public override string ToString()
        {
            return $"{Code} - {Title}";
        }
    }
}
