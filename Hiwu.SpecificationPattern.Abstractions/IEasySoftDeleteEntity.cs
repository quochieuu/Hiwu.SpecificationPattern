namespace Hiwu.SpecificationPattern.Abstractions
{
    public interface IEasySoftDeleteEntity
    {
        public DateTime? DeletionDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
