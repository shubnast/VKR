using DataBaseProvider.Entitys;

namespace DataBaseProvider.Reporsitories
{
    public class SubjectRepository : AbstractRepository<Subject>
    {
        public SubjectRepository(ApplicatonDBContext context)
            : base(context, context.Subject)
        {
        }
    }
}
