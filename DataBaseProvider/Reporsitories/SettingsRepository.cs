using DataBaseProvider.Entitys;
namespace DataBaseProvider.Reporsitories
{
    public class SettingsRepository : AbstractRepository<Settings>
    {
        public SettingsRepository(ApplicatonDBContext context)
            : base(context, context.Settings)
        {
        }
    }
}
