namespace BinaryPlate.Domain.Entities.SSM
{
    public class Material : IAuditable, ISoftDeletable, IMayHaveTenant, IConcurrencyStamp
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
        public int? MaterialCategoryId { get; set; }
        public MaterialCategory MaterialCategory { get; set; }
        public ICollection<TestMaterial> TestMaterials { get; set; }

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
