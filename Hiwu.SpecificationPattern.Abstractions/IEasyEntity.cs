namespace Hiwu.SpecificationPattern.Abstractions
{
    internal interface IEasyEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
