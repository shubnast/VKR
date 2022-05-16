using DataBaseProvider.Entitys;

namespace DataBaseProvider.Reporsitories
{
    public class CourseRepository : AbstractRepository<Course>
    {
        public CourseRepository(ApplicatonDBContext context)
            : base(context, context.Course)
        {
        }
    }
}
