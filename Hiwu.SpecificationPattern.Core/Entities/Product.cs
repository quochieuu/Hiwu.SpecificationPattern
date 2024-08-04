using Hiwu.SpecificationPattern.Abstractions;

namespace Hiwu.SpecificationPattern.Core.Entities
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
