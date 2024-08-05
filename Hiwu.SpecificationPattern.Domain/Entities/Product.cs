using Hiwu.SpecificationPattern.Domain.Common;

namespace Hiwu.SpecificationPattern.Domain.Entities
{
    public class Product : EasyBaseEntity<Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Content { get; set; }
        public string? UrlImage { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
