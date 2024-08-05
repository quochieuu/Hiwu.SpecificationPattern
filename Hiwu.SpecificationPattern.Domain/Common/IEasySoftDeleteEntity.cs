namespace Hiwu.SpecificationPattern.Domain.Common
{
    public interface IEasySoftDeleteEntity
    {
        public DateTime? DeletionDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
