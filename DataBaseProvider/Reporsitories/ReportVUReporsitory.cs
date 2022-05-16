using DataBaseProvider.Entitys;

namespace DataBaseProvider.Reporsitories
{
    public class ReportVUReporsitory : AbstractRepository<ReportVU>
    {
        public ReportVUReporsitory(ApplicatonDBContext context)
            : base(context, context.ReportVU)
        {
        }
    }
}
