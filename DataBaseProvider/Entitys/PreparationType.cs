using System;

namespace DataBaseProvider.Entitys
{
    public class PreparationType
	{
		public int id { get; set; }
		public string Title { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public string ReportTitle { get; set; }

		public string PreparationPeriod 
		{ 
			get 
			{
				string from = From.ToString("dd.MM.yyyy");
				string to = To.ToString("dd.MM.yyyy");
				return $"{from} - {to}"; 
			} 
		}

        public override string ToString()
        {
            return $"{Title} {PreparationPeriod}";
        }
    }
}
