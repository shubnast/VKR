namespace DataBaseProvider.Entitys
{
    public class Departament
	{
		public int id { get; set; }
		public string Title { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
