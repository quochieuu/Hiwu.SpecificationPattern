namespace Hiwu.SpecificationPattern.Domain.Common
{
    internal interface IEasyEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
