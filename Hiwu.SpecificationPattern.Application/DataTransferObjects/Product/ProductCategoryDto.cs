namespace Hiwu.SpecificationPattern.Application.DataTransferObjects.Product
{
    public class ProductCategoryDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string? Content { get; set; }
        public string? UrlImage { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
