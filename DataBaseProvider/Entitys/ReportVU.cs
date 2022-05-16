using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseProvider.Entitys
{
    public class ReportVU
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public string Template { get; set; }
        public int StudentId { get; set; }
        public string Student { get; set; }
        public int LecturerId { get; set; }
        public string Lecturer { get; set; }
        public int GroupId { get; set; }
        public string Group { get; set; }
        public int CourceId { get; set; }
        public string Course { get; set; }
        public string CouseType { get; set; }
        public int DepartamentId { get; set; }
        public string Departament { get; set; }
        public int TrainingDirectionId { get; set; }
        public string TrainingDirectionCode { get; set; }
        public string TrainingDirection { get; set; }
        public int PreparationTypeId { get; set; }
        public string PreparationType { get; set; }
        public string PreparationTypeReport { get; set; }
        public DateTime PreparationTypeFrom { get; set; }
        public DateTime PreparationTypeTo { get; set; }
        public string PreparationPeriod
        {
            get
            {
                string from = PreparationTypeFrom.ToString("dd.MM.yyyy");
                string to = PreparationTypeTo.ToString("dd.MM.yyyy");
                return $"{from} - {to}";
            }
        }

        [NotMapped]
        public bool SomeItemSelected { get; set; }
    }
}
