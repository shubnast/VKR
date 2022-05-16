using DataBaseProvider.Entitys;
using Microsoft.EntityFrameworkCore;

namespace DataBaseProvider
{
    public class ApplicatonDBContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Subject> Subject { get; internal set; }

        public DbSet<Course> Course { get; internal set; }

        public DbSet<Departament> Departament { get; internal set; }

        public DbSet<TrainingDirection> TrainingDirection { get; internal set; }

        public DbSet<Group> Group { get; internal set; }

        public DbSet<PreparationType> PreparationType { get; internal set; }

        public DbSet<LecturerGroup> LecturerGroup { get; internal set; }

        public DbSet<StudentGroup> StudentGroup { get; internal set; }

        public DbSet<Template> Template { get; internal set; }

        public DbSet<Report> Report { get; internal set; }

        public DbSet<Settings> Settings { get; internal set; }
        public DbSet<ReportVU> ReportVU { get; internal set; }

        public ApplicatonDBContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        public ApplicatonDBContext(DbContextOptions options)
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseMySql(_connectionString, ServerVersion.Parse(_connectionString));
            }
        }

        //Для переопределения ключей
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
