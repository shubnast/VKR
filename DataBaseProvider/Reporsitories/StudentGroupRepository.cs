using DataBaseProvider.Entitys;

namespace DataBaseProvider.Reporsitories
{
    public class StudentGroupRepository : AbstractRepository<StudentGroup>
    {
        public StudentGroupRepository(ApplicatonDBContext context)
            : base(context, context.StudentGroup)
        {
        }
    }
}
