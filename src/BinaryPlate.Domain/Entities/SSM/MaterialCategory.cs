namespace BinaryPlate.Domain.Entities.SSM
{
    public class MaterialCategory : IAuditable, ISoftDeletable, IMayHaveTenant, IConcurrencyStamp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Material> Materials { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? TenantId { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
