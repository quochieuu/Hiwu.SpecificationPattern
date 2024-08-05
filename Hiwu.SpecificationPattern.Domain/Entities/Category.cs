using Hiwu.SpecificationPattern.Domain.Common;

namespace Hiwu.SpecificationPattern.Domain.Entities
{
    public class Category : EasyBaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
