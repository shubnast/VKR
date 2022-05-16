using DataBaseProvider.Entitys;

namespace DataBaseProvider.Reporsitories
{
    public class TrainingDirectionRepository : AbstractRepository<TrainingDirection>
    {
        public TrainingDirectionRepository(ApplicatonDBContext context)
            : base(context, context.TrainingDirection)
        {
        }
    }
}
