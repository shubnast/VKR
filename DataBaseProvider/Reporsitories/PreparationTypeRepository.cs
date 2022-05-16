using DataBaseProvider.Entitys;

namespace DataBaseProvider.Reporsitories
{
    public class PreparationTypeRepository : AbstractRepository<PreparationType>
    {
        public PreparationTypeRepository(ApplicatonDBContext context)
            : base(context, context.PreparationType)
        {
        }
    }
}
