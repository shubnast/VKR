namespace DataBaseProvider.Entitys
{
    public class Group
	{
		public int id { get; set; }
		public string Title { get; set; }
		public int CourseId { get; set; }
		public int DepartamentId { get; set; }
		public int TrainingDirectionId { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
