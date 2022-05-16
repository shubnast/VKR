using DataBaseProvider.Entitys;
namespace DataBaseProvider.Reporsitories
{
    public class TemplateRepository : AbstractRepository<Template>
    {
        public TemplateRepository(ApplicatonDBContext context)
            : base(context, context.Template)
        {
        }
    }
}
