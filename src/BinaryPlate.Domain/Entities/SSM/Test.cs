namespace BinaryPlate.Domain.Entities.SSM
{
    public class Test : IAuditable, ISoftDeletable, IMayHaveTenant, IConcurrencyStamp
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<TestMaterial> TestMaterials { get; set; }
        public ICollection<TestQuestion> TestQuestions { get; set; }
        public ICollection<TestResult> TestResults { get; set; }

        #region Inherited Properties
        public string ConcurrencyStamp { get; set; }
        public Guid? TenantId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        #endregion Inherited Properties
    }
}
