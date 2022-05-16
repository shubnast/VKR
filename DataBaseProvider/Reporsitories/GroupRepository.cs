using DataBaseProvider.Entitys;

namespace DataBaseProvider.Reporsitories
{
    public class GroupRepository : AbstractRepository<Group>
    {
        public GroupRepository(ApplicatonDBContext context)
            : base(context, context.Group)
        {
        }
    }
}
