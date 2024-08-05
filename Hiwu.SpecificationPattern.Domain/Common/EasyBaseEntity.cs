namespace Hiwu.SpecificationPattern.Domain.Common
{
    public abstract class EasyBaseEntity<TPrimaryKey> : IEasyEntity<TPrimaryKey>, IEasyCreateDateEntity, IEasyUpdateDateEntity, IEasySoftDeleteEntity
    {
        public virtual DateTime CreationDate { get; set; }
        public virtual TPrimaryKey Id { get; set; }
        public virtual DateTime? ModificationDate { get; set; }
        public virtual DateTime? DeletionDate { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
