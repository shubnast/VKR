using DataBaseProvider.Entitys;
namespace DataBaseProvider.Reporsitories
{
    public class LecturerGroupRepository : AbstractRepository<LecturerGroup>
    {
        public LecturerGroupRepository(ApplicatonDBContext context)
            : base(context, context.LecturerGroup)
        {
        }
    }
}
