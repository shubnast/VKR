namespace DataBaseProvider.Entitys
{
    public class Subject
	{
		public int id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Patronym { get; set; }
		public int IsLecturer { get; set; }

		public string IsLecturerString
        {
			get { return IsLecturer == 1 ? "Да" : "Нет"; }
        }

		public bool IsLecturerBool { get { return IsLecturer == 1; } }

        public override string ToString()
        {
			return $"{Surname} {Name} {Patronym}";
        }
    }
}
