using DataBaseProvider.Entitys;

namespace DataBaseProvider.Reporsitories
{
    public class ReportRepository : AbstractRepository<Report>
    {
        public ReportRepository(ApplicatonDBContext context)
            : base(context, context.Report)
        {
        }
    }
}
