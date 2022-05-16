using DataBaseProvider.Entitys;

namespace DataBaseProvider.Reporsitories
{
    public class DepartamentRepository : AbstractRepository<Departament>
    {
        public DepartamentRepository(ApplicatonDBContext context)
            : base(context, context.Departament)
        {
        }
    }
}
