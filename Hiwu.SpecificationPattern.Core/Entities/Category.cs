using Hiwu.SpecificationPattern.Abstractions;

namespace Hiwu.SpecificationPattern.Core.Entities
{
    public class Category : EasyBaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
